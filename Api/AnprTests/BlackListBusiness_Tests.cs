using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Implementations;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using FluentAssertions;
using Moq;

namespace AnprTests
{
    public class BlackListBusiness_Tests
    {
        private static IMapper CreateMapper()
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<BlackListDto, BlackList>();
                c.CreateMap<BlackList, BlackListDto>();
            });
            return cfg.CreateMapper();
        }

        private static BlackListBusiness BuildSut(
            out Mock<IBlackListData> data,
            out Mock<IVehicleBusiness> vehicleBusiness,
            IMapper mapper = null)
        {
            data = new Mock<IBlackListData>(MockBehavior.Strict);
            vehicleBusiness = new Mock<IVehicleBusiness>(MockBehavior.Strict);
            mapper ??= CreateMapper();

            return new BlackListBusiness(data.Object, mapper, vehicleBusiness.Object);
        }

        [Fact]
        public async Task Save_CasoValido_GuardaYRetornaDto()
        {
            // Arrange
            var sut = BuildSut(out var data, out var vehicles);
            var dto = new BlackListDto
            {
                VehicleId = 100,
                Reason = "Intento de fraude en acceso"
                // RestrictionDate lo pone el negocio
            };

            // 1) Vehículo existe
            vehicles.Setup(v => v.GetById(dto.VehicleId))
                    .ReturnsAsync(new VehicleDto { Id = 100, Plate = "ABC123" });

            // 2) No existe previamente en blacklist
            data.Setup(d => d.ExistsAsync(It.IsAny<Expression<Func<BlackList, bool>>>()))
                .ReturnsAsync(false);

            // 3) Guardado: devolver entidad con Id asignado
            data.Setup(d => d.Save(It.IsAny<BlackList>()))
                .ReturnsAsync((BlackList b) =>
                {
                    b.Id = 55;
                    return b;
                });

            // Act
            var result = await sut.Save(dto);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(55);
            result.VehicleId.Should().Be(100);
            result.Reason.Should().Be("Intento de fraude en acceso");
            result.RestrictionDate.Should().NotBe(default); // el negocio lo setea a UtcNow

            vehicles.Verify(v => v.GetById(100), Times.Once);
            data.Verify(d => d.ExistsAsync(It.IsAny<Expression<Func<BlackList, bool>>>()), Times.Once);
            data.Verify(d => d.Save(It.IsAny<BlackList>()), Times.Once);
            data.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Save_CasoInvalido_SinRazon_LanzaArgumentException()
        {
            // ⚠️ Con TU código actual esta prueba FALLA (y debe fallar),
            // porque hoy no exiges que Reason sea obligatorio.
            // Aplica el fix que te dejo abajo para que se ponga en verde.

            // Arrange
            var sut = BuildSut(out var data, out var vehicles);
            var dto = new BlackListDto
            {
                VehicleId = 101,
                Reason = "" // ← sin razón
            };

            vehicles.Setup(v => v.GetById(dto.VehicleId))
                    .ReturnsAsync(new VehicleDto { Id = 101, Plate = "XYZ987" });

            data.Setup(d => d.ExistsAsync(It.IsAny<Expression<Func<BlackList, bool>>>()))
                .ReturnsAsync(false);

            // Act
            var act = async () => await sut.Save(dto);

            // Assert (esperamos validación)
            await act.Should().ThrowAsync<ArgumentException>()
                     .WithMessage("*razón*");
            // Sin guardado
            data.Verify(d => d.Save(It.IsAny<BlackList>()), Times.Never);
        }

        [Fact]
        public async Task Save_ReglaNegocio_NoPermitirDuplicado_MismoVehiculo()
        {
            // Arrange
            var sut = BuildSut(out var data, out var vehicles);
            var dto = new BlackListDto
            {
                VehicleId = 200,
                Reason = "Reportes previos de incidente"
            };

            vehicles.Setup(v => v.GetById(dto.VehicleId))
                    .ReturnsAsync(new VehicleDto { Id = 200, Plate = "JKL456" });

            // Ya existe en blacklist -> debe fallar
            data.Setup(d => d.ExistsAsync(It.IsAny<Expression<Func<BlackList, bool>>>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await sut.Save(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("*ya está en la lista negra*");

            data.Verify(d => d.Save(It.IsAny<BlackList>()), Times.Never);
        }
    }
}
