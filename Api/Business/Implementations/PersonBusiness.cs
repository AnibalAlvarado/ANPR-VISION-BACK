using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos;
using Entity.Models;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;
using Utilities.Helpers.Validators;

namespace Business.Implementations
{
    public class PersonBusiness : RepositoryBusiness<Person, PersonDto>, IPersonBusiness
    {
        private readonly IPersonData _data;
        private readonly IMapper _mapper;
        public PersonBusiness(IPersonData data, IMapper mapper)
            : base(data, mapper)
        {
            _mapper = mapper;
            _data = data;
        }

        public override async Task<PersonDto> Save(PersonDto dto)
        {
            try
            {
                // 🔹 Validar que no exista un formulario con el mismo nombre
                if (await _data.ExistsAsync(x => x.Document == dto.Document))
                {
                    throw new InvalidOperationException("El documento ya se encuentra registrado.");
                }
                if (await _data.ExistsAsync(x => x.Phone == dto.Phone))
                {
                    throw new InvalidOperationException("El Telefono ya se encuentra registrado.");
                }
                if (await _data.ExistsAsync(x => x.Email == dto.Email))
                {
                    throw new InvalidOperationException("El Email ya se encuentra registrado.");
                }
         
                var entity = _mapper.Map<Person>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<PersonDto>(entity);
            }
            catch (InvalidOperationException invOp)
            {
                throw new InvalidOperationException($"Error: {invOp.Message}", invOp);
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException($"Error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al registrar el formulario.", ex);
            }
        }
    }
}
