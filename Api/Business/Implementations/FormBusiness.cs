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
    public class FormBusiness : RepositoryBusiness<Form, FormDto>, IFormBusiness
    {
        private readonly IFormData _data;
        private  readonly IMapper _mapper;
        public FormBusiness(IFormData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public override async Task<FormDto> Save(FormDto dto)
        {
            try
            {
                // 🔹 Validar que no exista un formulario con el mismo nombre
                if (await _data.ExistsAsync(x => x.Name == dto.Name))
                {
                    throw new InvalidOperationException("El nombre del formulario ya se encuentra registrado.");
                }

                // 🔹 Validar campos obligatorios
                Validations.ValidateDto(dto, "Name");

                if (string.IsNullOrWhiteSpace(dto.Name))
                    throw new ArgumentException("El campo 'Nombre' es obligatorio.");
                if (dto.Name.Length < 3)
                    throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
                if (dto.Name.Length > 50)
                    throw new ArgumentException("El nombre no puede superar los 50 caracteres.");

                // 🔹 Guardar entidad
                var entity = _mapper.Map<Form>(dto);
                entity = await _data.Save(entity);

                return _mapper.Map<FormDto>(entity);
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
