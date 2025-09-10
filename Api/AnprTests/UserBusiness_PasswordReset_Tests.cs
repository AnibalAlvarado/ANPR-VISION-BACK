using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Implementations;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Utilities.Exceptions;
using Utilities.Interfaces;

namespace AnprTests
{
    public class UserBusiness_PasswordReset_Tests
    {
        private static UserBusiness BuildSut(
            Mock<IUserData> data,
            Mock<IEmailService> email,
            Mock<IRolBusiness> rolBusiness = null,
            Mock<IRolUserBusiness> rolUserBusiness = null,
            Mock<IJwtAuthenticationService> jwt = null,
            Mock<IPasswordHasher> hasher = null,
            Mock<IPasswordReset> resetRepo = null,
            Mock<ILogger<UserBusiness>> logger = null,
            IMapper mapper = null
        )
        {
            rolBusiness ??= new Mock<IRolBusiness>();
            rolUserBusiness ??= new Mock<IRolUserBusiness>();
            jwt ??= new Mock<IJwtAuthenticationService>();
            hasher ??= new Mock<IPasswordHasher>();
            resetRepo ??= new Mock<IPasswordReset>();
            logger ??= new Mock<ILogger<UserBusiness>>();
            mapper ??= new MapperConfiguration(cfg => { }).CreateMapper();

            return new UserBusiness(
                data.Object,
                mapper,
                logger.Object,
                email.Object,
                rolBusiness.Object,
                rolUserBusiness.Object,
                jwt.Object,
                hasher.Object,
                resetRepo.Object
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async Task RequestPasswordReset_Permite_Hasta5Menos1EnLaUltimaHora(int existentes)
        {
            // Arrange
            var email = "user@test.com";
            var user = new User { Id = 10, Email = email, Username = "u" };

            var data = new Mock<IUserData>(MockBehavior.Strict);
            data.Setup(d => d.GetUserByEmailsync(email)).ReturnsAsync(user);

            var resetRepo = new Mock<IPasswordReset>(MockBehavior.Strict);
            resetRepo
                .Setup(r => r.CountRequestsSinceAsync(user.Id, It.IsAny<DateTime>()))
                .ReturnsAsync(existentes);

            // Se espera que, al permitir, agregue el registro
            resetRepo
                .Setup(r => r.Add(It.Is<PasswordReset>(pr =>
                    pr.UsuarioId == user.Id &&
                    !string.IsNullOrWhiteSpace(pr.Code) &&
                    pr.ExpiryDate > pr.CreatedAt)))
                .Returns(Task.CompletedTask);

            var emailSvc = new Mock<IEmailService>(MockBehavior.Strict);
            emailSvc
                .Setup(s => s.SendEmailAsync(email, It.Is<string>(msg => msg.Contains("Tu código de recuperación es"))))
                .Returns(Task.CompletedTask);

            var sut = BuildSut(data, emailSvc, resetRepo: resetRepo);

            // Act
            Func<Task> act = async () => await sut.RequestPasswordResetAsync(email);

            // Assert
            await act.Should().NotThrowAsync();

            data.Verify(d => d.GetUserByEmailsync(email), Times.Once);
            resetRepo.Verify(r => r.CountRequestsSinceAsync(user.Id, It.IsAny<DateTime>()), Times.Once);
            resetRepo.Verify(r => r.Add(It.IsAny<PasswordReset>()), Times.Once);
            emailSvc.Verify(s => s.SendEmailAsync(email, It.IsAny<string>()), Times.Once);
            resetRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RequestPasswordReset_Rechaza_AlSerElSextoEn60Min_NoAgregaNiEnvia()
        {
            // Arrange
            var email = "user@test.com";
            var user = new User { Id = 20, Email = email, Username = "u" };

            var data = new Mock<IUserData>(MockBehavior.Strict);
            data.Setup(d => d.GetUserByEmailsync(email)).ReturnsAsync(user);

            var resetRepo = new Mock<IPasswordReset>(MockBehavior.Strict);
            resetRepo
                .Setup(r => r.CountRequestsSinceAsync(user.Id, It.IsAny<DateTime>()))
                .ReturnsAsync(5); // ya hay 5 en la ventana

            // Tu código consulta también la más antigua dentro de la ventana
            resetRepo
                .Setup(r => r.OldestRequestSinceAsync(user.Id, It.IsAny<DateTime>()))
                .ReturnsAsync(DateTime.UtcNow.AddMinutes(-30));

            var emailSvc = new Mock<IEmailService>(MockBehavior.Strict);

            var sut = BuildSut(data, emailSvc, resetRepo: resetRepo);

            // Act
            Func<Task> act = async () => await sut.RequestPasswordResetAsync(email);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage("*límite de 5 códigos por hora*");

            data.Verify(d => d.GetUserByEmailsync(email), Times.Once);
            resetRepo.Verify(r => r.CountRequestsSinceAsync(user.Id, It.IsAny<DateTime>()), Times.Once);
            resetRepo.Verify(r => r.OldestRequestSinceAsync(user.Id, It.IsAny<DateTime>()), Times.Once);
            resetRepo.Verify(r => r.Add(It.IsAny<PasswordReset>()), Times.Never);
            emailSvc.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            resetRepo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RequestPasswordReset_Error_SiCorreoNoExiste()
        {
            // Arrange
            var email = "pov34dasara769@gmail.com";

            var data = new Mock<IUserData>(MockBehavior.Strict);
            data.Setup(d => d.GetUserByEmailsync(email)).ReturnsAsync((User)null);

            var resetRepo = new Mock<IPasswordReset>(MockBehavior.Strict);
            var emailSvc = new Mock<IEmailService>(MockBehavior.Strict);

            var sut = BuildSut(data, emailSvc, resetRepo: resetRepo);

            // Act
            Func<Task> act = async () => await sut.RequestPasswordResetAsync(email);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*no existe con ese correo*");

            data.Verify(d => d.GetUserByEmailsync(email), Times.Once);
            resetRepo.VerifyNoOtherCalls();
            emailSvc.VerifyNoOtherCalls();
        }
    }
}
