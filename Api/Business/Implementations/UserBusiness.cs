using AutoMapper;
using Business.Interfaces;
using Data.Implementations;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Implementations;
using Utilities.Interfaces;

namespace Business.Implementations
{
    public class UserBusiness : RepositoryBusiness<User, UserDto>, IUserBusiness
    {
        private readonly IUserData _data;
        private readonly IMapper _mapper;
        private readonly ILogger<UserBusiness> _logger;
        private readonly IEmailService _emailService;
        private readonly IRolBusiness _rolBusiness;
        private readonly IRolParkingUserBusiness _rolUserBusiness;
        private readonly IJwtAuthenticationService _jwtAuthenticatonService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordReset _passwordReset;

        public UserBusiness(
        IUserData data, IMapper mapper, ILogger<UserBusiness> logger,IEmailService emailService,IRolBusiness rolBusiness, IRolParkingUserBusiness rolUserBusiness, IJwtAuthenticationService jwtAuthenticatonService, IPasswordHasher passwordHasher, IPasswordReset passwordReset) : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _rolBusiness = rolBusiness;
            _rolUserBusiness = rolUserBusiness;
            _jwtAuthenticatonService = jwtAuthenticatonService;
            _passwordHasher = passwordHasher;
            _passwordReset = passwordReset;
             _clientData = clientData;
        }

        // Obtener todos los usuarios con información de persona

        // Obtener usuario por ID con información de persona
        public override async Task<UserDto> GetById(int id)
        {
            try
            {
                var user = await _data.GetById(id);
                if (user == null)
                    return null;

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por ID con información de persona");
                throw;
            }
        }

        public override async Task<UserDto> Save(UserDto dto)
        {
            try
            {
                if (await _data.ExistsAsync(x => x.Username == dto.Username))
                {
                    throw new InvalidOperationException("El nombre de usuario ya se encuentra registrado.");
                }
                // Aplicar valores predeterminados

                if (await _data.ExistsAsync(x => x.Email == dto.Email && x.Id != dto.Id))
                {
                    throw new InvalidOperationException("El correo de usuario ya se encuentra registrado.");
                }

                // 🚨 Validar que la persona no esté asociada a otro usuario
                if (dto.PersonId <= 0)
                    throw new ArgumentException("El campo PersonaId es obligatorio.");

                bool existeUsuarioParaPersona = await _data.ExistsAsync(x => x.PersonId == dto.PersonId);
                if (existeUsuarioParaPersona)
                {
                    throw new InvalidOperationException("Ya existe un usuario asociado a esta persona.");
                }



                dto.Asset = true;

                // Hashear la contraseña antes de guardar
                if (!string.IsNullOrEmpty(dto.Password))
                {
                    dto.Password = _passwordHasher.HashPassword(dto.Password);
                }

                // Mapear DTO a entidad
                User entity = _mapper.Map<User>(dto);

                // Guardar entidad en la base de datos
                entity = await _data.Save(entity);

                // Mapear la entidad guardada de vuelta a DTO
                var savedDto = _mapper.Map<UserDto>(entity);

                // Mapear la entidad guardada de vuelta a DTO y devolverla
                return savedDto;
            }
            catch (InvalidOperationException invOe)
            {
                throw new InvalidOperationException($"Error: {invOe.Message}", invOe);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException($"Error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el usuario.");
                throw new BusinessException("Error al crear el usuario.", ex);
            }
        }

       

        public async Task<UserResponseDto?> ValidateUserAsync(string username, string password)
        {
            try
            {
                // Validaciones iniciales
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    _logger.LogWarning("Intento de validación con username o password vacíos");
                    return null;
                }

                // Obtener usuario por nombre de usuario
                var user = await _data.GetUserByUsernameAsync(username);

                // Si el usuario no existe
                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado durante validación: {Username}", username);
                    return null;
                }

                // Validar que el hash tenga el formato correcto antes de verificar
                if (string.IsNullOrWhiteSpace(user.Password) || !user.Password.StartsWith("$2"))
                {
                    _logger.LogWarning("Formato de hash inválido para el usuario: {Username}. Hash recibido: {Hash}", username, user.Password);
                    return null;
                }

                // Verificar la contraseña
                if (!VerifyPassword(password, user.Password))
                {
                    _logger.LogWarning("Contraseña incorrecta para el usuario: {Username}", username);
                    return null;
                }

                // Obtener el rol del usuario a través de la tabla pivote
                var roleNames = await _data.GetUserRoleAsync(user.Id);

                // Usar AutoMapper para crear el DTO de respuesta
                // Usar AutoMapper para crear el DTO de respuesta
                var userResponseDto = _mapper.Map<UserResponseDto>(user);
                userResponseDto.Roles = roleNames;
                userResponseDto.UserId = user.Id;

                // ✅ Generar el token aquí mismo
                userResponseDto.Token = _jwtAuthenticatonService.GenerarToken(user, roleNames);

                // ==============================
                // ➕ BLOQUE NUEVO: contexto app
                // ==============================
                // Person info (si incluiste Include en GetUserByUsernameAsync, tendrás nombres)
                userResponseDto.PersonId = user.PersonId;
                userResponseDto.FirstName = user.Person?.FirstName;
                userResponseDto.LastName = user.Person?.LastName;

                // Cargar Client + Vehicles por PersonId
                if (user.PersonId > 0)
                {
                    // usa IClientData (inyéctalo en el constructor del Business)
                    var client = await _clientData.GetClientWithVehiclesByPersonIdAsync(user.PersonId);
                    if (client != null)
                    {
                        // Defensa: coherencia de identidad
                        if (client.PersonId == user.PersonId)
                        {
                            userResponseDto.Client = new ClientLiteDto
                            {
                                Id = client.Id,
                                PersonId = client.PersonId
                            };

                            //userResponseDto.Vehicles = client.Vehicles
                            //    .Select(v => new VehicleLiteDto
                            //    {
                            //        Id = v.Id,
                            //        Plate = v.Plate,
                            //        Color = v.Color,
                            //        TypeVehicleId = v.TypeVehicleId
                            //    })
                            //    .ToList();
                        }
                        else
                        {
                            // Si hay mismatch, no detiene el login; sólo no adjunta client/vehicles.
                            _logger.LogWarning("Mismatch PersonId User({U}:{UP}) vs Client({C}:{CP})",
                                user.Id, user.PersonId, client.Id, client.PersonId);
                        }
                    }
                }

                return userResponseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la validación del usuario: {Username}", username);
                throw;
            }
        }

        public string HashPassword(string password)
        {
            try
            {
                // BCrypt automáticamente genera y maneja la sal
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar hash de contraseña");
                throw;
            }
        }

        public bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(storedPasswordHash) || !storedPasswordHash.StartsWith("$2"))
                {
                    _logger.LogError("Hash inválido o mal formado: {Hash}", storedPasswordHash);
                    return false;
                }

                return BCrypt.Net.BCrypt.Verify(inputPassword, storedPasswordHash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar contraseña");
                throw;
            }
        }



        public async Task AssignDefaultRoleAsync(int userId)
        {
            // 1. Verificar que el usuario exista
            var user = await _data.GetById(userId);
            if (user == null)
                throw new Exception("El usuario no existe.");

            // 2. Obtener el rol por defecto ya creado
            var defaultRole = await _rolBusiness.GetByNameAsync("Usuario"); // Cambia "Usuario" por el nombre exacto si es diferente
            if (defaultRole == null)
                throw new Exception("El rol por defecto no existe.");

            // 3. Verificar si ya tiene el rol asignado
            var alreadyAssigned = await _rolUserBusiness.ExistsAsync(userId, defaultRole.Id);
            if (alreadyAssigned)
                return;

            // 4. Asignar el rol al usuario
            var userRoleDto = new RolParkingUserDto
            {
                UserId = user.Id,
                RolId = defaultRole.Id
            };

            await _rolUserBusiness.Save(userRoleDto);
        }

        public async Task SendWelcomeEmailAsync(string to)
        {
            try
            {
                // Armas el mensaje de bienvenida (puedes personalizarlo)
                string message = "¡Bienvenido! Tu usuario ha sido creado exitosamente.";

                // Llamas al servicio de email para enviar
                await _emailService.SendEmailAsync(to, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo de bienvenida al usuario: {Email}", to);
                // Decide si quieres relanzar la excepción o solo loguear
                // throw;
            }
        }

        public async Task<List<UserRoleStatusDto>> GetUserRolesAsync(int userId)
        {
            try
            {
                return await _data.GetUserRolesAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles del usuario con ID: {UserId}", userId);
                throw;
            }
        }

        //metodos para el reset

        // === Paso 1: Solicitar recuperación ===
        //public async Task RequestPasswordResetAsync(string email)
        //{
        //    var user = await _data.GetUserByEmailsync(email);
        //    if (user == null)
        //        throw new Exception("El usuario no existe con ese correo.");

        //    string code = GenerateResetCode();

        //    var reset = new PasswordReset
        //    {
        //        UsuarioId = user.Id,
        //        Code = code,
        //        ExpiryDate = DateTime.UtcNow.AddMinutes(10),
        //        Used = false
        //    };

        //    await _passwordReset.Add(reset);

        //    // Enviar correo
        //    await _emailService.SendEmailAsync(user.Email, $"Tu código de recuperación es: {code}");
        //}

        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _data.GetUserByEmailsync(email);
            if (user == null)
                throw new Exception("El usuario no existe con ese correo.");

            var nowUtc = DateTime.UtcNow;
            var windowStart = nowUtc.AddHours(-1);

            // ✅ Límite: 5 por hora
            int count = await _passwordReset.CountRequestsSinceAsync(user.Id, windowStart);
            if (count >= 5)
            {
                var oldest = await _passwordReset.OldestRequestSinceAsync(user.Id, windowStart);
                var nextUtc = (oldest ?? nowUtc).AddHours(1);

                throw new BusinessException(
                    $"Has alcanzado el límite de 5 códigos por hora. " +
                    $"Podrás solicitar uno nuevo despues de una hora");
            }

            // Generar y registrar el código
            string code = GenerateResetCode();
            var reset = new PasswordReset
            {
                UsuarioId = user.Id,
                Code = code,
                ExpiryDate = nowUtc.AddMinutes(10),
                Used = false,
                CreatedAt = nowUtc
                // RequestIp = ip (si decides capturarla en el controller y pasarla)
            };

            await _passwordReset.Add(reset);

            await _emailService.SendEmailAsync(user.Email, $"Tu código de recuperación es: {code}");
        }



        // === Paso 2: Validar código y actualizar contraseña ===
        public async Task VerifyCodeAndResetPasswordAsync(string email, string code, string newPassword)
        {
            var user = await _data.GetUserByEmailsync(email);
            if (user == null)
                throw new Exception("El usuario no existe.");

            var reset = await _passwordReset.GetValidCode(user.Id, code);
            if (reset == null)
                throw new Exception("Código inválido o expirado.");

            // Actualizar contraseña
            user.Password = _passwordHasher.HashPassword(newPassword);
            await _data.Update(user);

            // Marcar el código como usado
            await _passwordReset.MarkAsUsed(reset);
        }

        public async Task<bool> VerifyResetCodeAsync(string email, string code)
        {
            var user = await _data.GetUserByEmailsync(email);
            if (user == null)
                throw new Exception("El usuario no existe.");

            var reset = await _passwordReset.GetValidCode(user.Id, code);
            return reset != null;
        }


        // === Helpers ===
        private string GenerateResetCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // 6 dígitos
        }

        public async Task<UserAccessDto> GetUserAccessAsync(int userId, bool includePermissions = true, bool includeForms = true)
        {
            try
            {
                var access = await _data.GetUserAccessAsync(userId);

                if (!includePermissions)
                {
                    foreach (var role in access.Roles)
                        foreach (var module in role.Modules)
                            foreach (var form in module.Forms)
                                form.Permissions.Clear();
                }

                if (!includeForms)
                {
                    foreach (var role in access.Roles)
                        role.Modules.Clear();
                }

                return access;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener accesos del usuario con ID: {UserId}", userId);
                throw;
            }
        }



    }
}
