using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Implementations;
using Data.Interfaces;
using Entity.Models;
using FluentAssertions;
using Moq;

namespace AnprTests
{
    public class VehicleBusiness_RegisterSlot_Tests
    {
        private static IMapper DummyMapper()
        {
            // RegisterVehicleWithSlotAsync no usa mapper, pero la base lo pide
            return new MapperConfiguration(cfg => { }).CreateMapper();
        }

        private static VehicleBusiness BuildSut(
            out Mock<IVehicleData> vehicleData,
            out Mock<IRegisteredVehiclesData> regData,
            out Mock<ISectorsData> sectorsData,
            out Mock<ISlotsData> slotsData,
            IMapper mapper = null)
        {
            vehicleData = new Mock<IVehicleData>(MockBehavior.Strict);
            regData = new Mock<IRegisteredVehiclesData>(MockBehavior.Strict);
            sectorsData = new Mock<ISectorsData>(MockBehavior.Strict);
            slotsData = new Mock<ISlotsData>(MockBehavior.Strict);
            mapper ??= DummyMapper();

            return new VehicleBusiness(
                vehicleData.Object,
                mapper,
                regData.Object,
                sectorsData.Object,
                slotsData.Object
            );
        }

        [Fact]
        public async Task RegisterVehicleWithSlot_CasoValido_AsignaSlotYMarcaNoDisponible()
        {
            var sut = BuildSut(out var vData, out var regData, out var secData, out var sData);

            var vehicle = new Vehicle { Id = 1, TypeVehicleId = 10, Plate = "AAA111" };
            vData.Setup(x => x.GetById(vehicle.Id)).ReturnsAsync(vehicle);

            // Sector compatible con un solo slot disponible (para evitar indeterminismo del Random)
            var slot = new Slots { Id = 7, IsAvailable = true };
            var sector = new Sectors { Id = 99, Slots = new List<Slots> { slot } };
            secData.Setup(x => x.GetSectorsByVehicleTypeAsync(vehicle.TypeVehicleId))
                   .ReturnsAsync(new List<Sectors> { sector });

            // No hay vehículo activo en ese slot
            regData.Setup(x => x.AnyActiveRegisteredVehicleInSlotAsync(slot.Id)).ReturnsAsync(false);

            // Debe marcar el slot como ocupado
            sData.Setup(x => x.Update(It.Is<Slots>(s => s.Id == slot.Id && s.IsAvailable == false)))
                 .Returns(Task.CompletedTask);

            // Guardado del RegisteredVehicles; devolvemos la misma entidad con Id asignado
            regData.Setup(x => x.Save(It.Is<RegisteredVehicles>(rv =>
                    rv.VehicleId == vehicle.Id &&
                    rv.SlotsId == slot.Id &&
                    rv.EntryDate != default)))
                  .ReturnsAsync((RegisteredVehicles rv) =>
                  {
                      rv.Id = 123;
                      return rv;
                  });

            var result = await sut.RegisterVehicleWithSlotAsync(vehicle.Id);

            result.Should().NotBeNull();
            result.Id.Should().Be(123);
            result.VehicleId.Should().Be(vehicle.Id);
            result.SlotsId.Should().Be(slot.Id);
            result.EntryDate.Should().NotBe(default);

            vData.Verify(x => x.GetById(vehicle.Id), Times.Once);
            secData.Verify(x => x.GetSectorsByVehicleTypeAsync(vehicle.TypeVehicleId), Times.Once);
            regData.Verify(x => x.AnyActiveRegisteredVehicleInSlotAsync(slot.Id), Times.Once);
            sData.Verify(x => x.Update(It.IsAny<Slots>()), Times.Once);
            regData.Verify(x => x.Save(It.IsAny<RegisteredVehicles>()), Times.Once);
            vData.VerifyNoOtherCalls();
            secData.VerifyNoOtherCalls();
            regData.VerifyNoOtherCalls();
            sData.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RegisterVehicleWithSlot_VehiculoNoExiste_LanzaExcepcion()
        {
            var sut = BuildSut(out var vData, out _, out _, out _);
            vData.Setup(x => x.GetById(999)).ReturnsAsync((Vehicle)null);

            var act = async () => await sut.RegisterVehicleWithSlotAsync(999);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("*Vehículo no encontrado*");

            vData.Verify(x => x.GetById(999), Times.Once);
        }

        [Theory]
        // Combinaciones que resultan en "no hay slots disponibles"
        // (isAvailable, ocupado)
        [InlineData(false, false)] // no disponible
        [InlineData(true, true)]  // disponible pero ocupado
        public async Task RegisterVehicleWithSlot_SinSlotsDisponibles_LanzaExcepcion(bool isAvailable, bool ocupado)
        {
            var sut = BuildSut(out var vData, out var regData, out var secData, out var sData);

            var vehicle = new Vehicle { Id = 2, TypeVehicleId = 20, Plate = "BBB222" };
            vData.Setup(x => x.GetById(vehicle.Id)).ReturnsAsync(vehicle);

            var slot = new Slots { Id = 8, IsAvailable = isAvailable };
            var sector = new Sectors { Id = 100, Slots = new List<Slots> { slot } };
            secData.Setup(x => x.GetSectorsByVehicleTypeAsync(vehicle.TypeVehicleId))
                   .ReturnsAsync(new List<Sectors> { sector });

            regData.Setup(x => x.AnyActiveRegisteredVehicleInSlotAsync(slot.Id))
                   .ReturnsAsync(ocupado);

            var act = async () => await sut.RegisterVehicleWithSlotAsync(vehicle.Id);

            await act.Should().ThrowAsync<Exception>()
                     .WithMessage("*No hay slots disponibles*");

            // No debe intentar actualizar ni guardar nada
            sData.Verify(x => x.Update(It.IsAny<Slots>()), Times.Never);
            regData.Verify(x => x.Save(It.IsAny<RegisteredVehicles>()), Times.Never);
        }
    }
}
