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

                var clienteExistente = await _data.GetById(dto.Id);
                if (clienteExistente == null)
                    throw new InvalidOperationException($"No existe un cliente con Id {dto.Id}.");

                if (!clienteExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar un cliente deshabilitado.");

                if (dto.PersonaId <= 0)
                    throw new ArgumentException("El campo PersonaId debe ser mayor que 0.");

                var persona = await _personRepository.GetById(dto.PersonaId);
                if (persona == null)
                    throw new InvalidOperationException($"No existe una persona con Id {dto.PersonaId}.");

                var clientes = await _data.GetAll(null);
                bool existeCliente = clientes.Any(x =>
                    x.PersonId == dto.PersonaId &&
                    x.Id != dto.Id &&
                    x.Asset == true
                );
                if (existeCliente)
                    throw new InvalidOperationException("Ya existe otro cliente activo para esta persona.");

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
