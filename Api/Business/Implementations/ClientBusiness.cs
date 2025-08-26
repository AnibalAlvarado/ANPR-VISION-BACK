using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

namespace Business.Implementations
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


        public override async Task<ClientDto> Save(ClientDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "PersonaId");

                if (dto.PersonaId <= 0)
                    throw new ArgumentException("El campo PersonaId debe ser mayor que 0.");

                var persona = await _personRepository.GetById(dto.PersonaId);
                if (persona == null)
                    throw new InvalidOperationException($"No existe una persona con Id {dto.PersonaId}.");

                // Aquí puedes agregar la lógica de guardado real usando _data y _mapper
                return await base.Save(dto);
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
                throw new BusinessException("Error al registrar el cliente.", ex);
            }
        }

        public override async Task Update(ClientDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "PersonaId");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor que 0.");

                Client clienteExistente = await _data.GetById(dto.Id);
                if (clienteExistente == null)
                    throw new InvalidOperationException($"El cliente no existe.");
                if (dto.PersonaId <= 0)
                    throw new ArgumentException("El atributo persona es obligatorio.");

                Person persona = await _personRepository.GetById(dto.PersonaId);
                if (persona == null)
                    throw new InvalidOperationException($"No existe la persona que se ha seleccionado.");

                if(dto.PersonaId != clienteExistente.PersonId)
                {
                    bool existclient = await _data.ExistsAsync(x => x.PersonId == dto.PersonaId);
                    if (existclient)
                        throw new InvalidOperationException("Ya existe otro cliente activo para esta persona.");
                }

                BaseModel entity = _mapper.Map<Client>(dto);
                await _data.Update((Client)entity);
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
                throw new BusinessException("Error al actualizar el cliente.", ex);
            }
        }
    }
}
