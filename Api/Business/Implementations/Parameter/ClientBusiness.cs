using AutoMapper;
using Business.Interfaces.Parameter;
using Data.Interfaces;
using Data.Interfaces.Parameter;
using Entity.Dtos.Parameter;
using Entity.Models.Parameter;
using Entity.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

namespace Business.Implementations.Parameter
{
    public class ClientBusiness : RepositoryBusiness<Client, ClientDto>, IClientBusiness
    {
        private readonly IClientData _data;
        private readonly IMapper _mapper;
        private readonly IRepositoryData<Person> _personRepository;
        public ClientBusiness(IClientData data, IMapper mapper, IRepositoryData<Person> personRepository)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<ClientDto>> GetAllJoinAsync()
        {
            try
            {
                IEnumerable<ClientDto> entities = await _data.GetAllJoinAsync();
                if (!entities.Any()) throw new InvalidOperationException("No se encontraron clientes.");
                return entities;
            }
            catch (InvalidOperationException invEx)
            {
                throw new InvalidOperationException("error: ", invEx);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException("error: ", argEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las clientes .", ex);
            }
        }
        private static string GetPersonDisplayName(Person? person)
        {
            if (person == null) return string.Empty;

            var t = person.GetType();

            // Propiedades candidatas ordenadas (ajusta si tu modelo usa otras)
            string[] candidates = { "FullName", "Fullname", "Name", "NombreCompleto", "Nombre", "FirstName", "First_Name", "GivenName" };

            // 1) Si existe FullName/Name/Nombres compuestos
            foreach (var cand in candidates)
            {
                var p = t.GetProperty(cand, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                if (p != null && p.PropertyType == typeof(string))
                {
                    var val = p.GetValue(person) as string;
                    if (!string.IsNullOrWhiteSpace(val))
                        return val.Trim();
                }
            }

            // 2) Si existe FirstName + LastName, intenta combinarlos
            var first = t.GetProperty("FirstName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase)
                    ?? t.GetProperty("GivenName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
            var last = t.GetProperty("LastName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase)
                    ?? t.GetProperty("Surname", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);

            if (first != null && last != null && first.PropertyType == typeof(string) && last.PropertyType == typeof(string))
            {
                var f = first.GetValue(person) as string;
                var l = last.GetValue(person) as string;
                var comb = $"{(f ?? string.Empty).Trim()} {(l ?? string.Empty).Trim()}".Trim();
                if (!string.IsNullOrWhiteSpace(comb))
                    return comb;
            }

            // 3) Fallback: primer string público no nulo
            var strProp = t.GetProperties()
                           .FirstOrDefault(pi => pi.PropertyType == typeof(string));
            if (strProp != null)
            {
                var v = strProp.GetValue(person) as string;
                if (!string.IsNullOrWhiteSpace(v)) return v.Trim();
            }

            return string.Empty;
        }

        // Reemplaza el método Save por este:
        public override async Task<ClientDto> Save(ClientDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "PersonaId");

                if (dto.PersonId <= 0)
                    throw new ArgumentException("El campo PersonaId debe ser mayor que 0.");

                var persona = await _personRepository.GetById(dto.PersonId);
                if (persona == null)
                    throw new InvalidOperationException($"No existe una persona con Id {dto.PersonId}.");

                // 1) Validación por PersonId (rápida, SQL)
                bool existePorPersonId = await _data.ExistsAsync(x => x.PersonId == dto.PersonId);
                if (existePorPersonId)
                    throw new InvalidOperationException("Ya existe un cliente asociado a esta persona.");

                // 2) Validación por NAME (en memoria usando GetAllJoinAsync para que incluya Person)
                var personName = GetPersonDisplayName(persona);
                if (!string.IsNullOrWhiteSpace(personName))
                {
                    // allWithJoin es IEnumerable<ClientDto>
                    var allWithJoin = await _data.GetAllJoinAsync();

                    bool existePorNombre = allWithJoin.Any(c =>
                        !string.IsNullOrWhiteSpace(c.Person) &&
                        string.Equals(c.Person!.Trim(), personName, StringComparison.OrdinalIgnoreCase)
                    );

                    if (existePorNombre)
                        throw new InvalidOperationException("Ya existe un cliente asociado a una persona con el mismo nombre.");

                }

                // Mapear y guardar
                Client entity = _mapper.Map<Client>(dto);
                entity.Asset = true;

                entity = await _data.Save(entity);

                return _mapper.Map<ClientDto>(entity);
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar el cliente.", ex);
            }
        }

        // Reemplaza el método Update por este:
        public override async Task Update(ClientDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "PersonaId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor que 0.");

                var clienteExistente = await _data.GetById(dto.Id);
                if (clienteExistente == null)
                    throw new InvalidOperationException("El cliente no existe.");

                if (dto.PersonId <= 0)
                    throw new ArgumentException("El atributo PersonaId es obligatorio.");

                var persona = await _personRepository.GetById(dto.PersonId);
                if (persona == null)
                    throw new InvalidOperationException("No existe la persona que se ha seleccionado.");

                // Si cambió la PersonId, comprobar PersonId (excluyendo propio Id)
                if (dto.PersonId != clienteExistente.PersonId)
                {
                    bool existclient = await _data.ExistsAsync(x => x.PersonId == dto.PersonId);
                    if (existclient)
                        throw new InvalidOperationException("Ya existe otro cliente asociado a esta persona.");
                }

                // Comprobación por NOMBRE (excluyendo propio Id) - en memoria con GetAllJoinAsync
                var personName = GetPersonDisplayName(persona);
                if (!string.IsNullOrWhiteSpace(personName))
                {
                    var allWithJoin = await _data.GetAllJoinAsync();

                    bool existsOtherByName = allWithJoin.Any(c =>
                        c.Id != dto.Id &&
                        !string.IsNullOrWhiteSpace(c.Person) &&
                        string.Equals(c.Person!.Trim(), personName, StringComparison.OrdinalIgnoreCase)
                    );

                    if (existsOtherByName)
                        throw new InvalidOperationException("Ya existe otro cliente asociado a una persona con el mismo nombre.");

                }

                // Mapear sobre la entidad traqueada (evita doble tracking)
                _mapper.Map(dto, clienteExistente);

                await _data.Update(clienteExistente);
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                throw new BusinessException("Error al actualizar el cliente.", ex);
            }
        }
    }
}
