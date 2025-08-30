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
  
    public class MembershipsBusiness : RepositoryBusiness<Memberships, MembershipsDto>, IMembershipsBusiness
    {
        private readonly IMembershipsData _data;
        private readonly IMapper _mapper;
        public MembershipsBusiness(IMembershipsData data, IMapper mapper)
            : base(data, mapper)
        {
            _data = data;
            _mapper = mapper;
        }
        public override async Task<MembershipsDto> Save(MembershipsDto dto)
        {
            try
            {
             
                Validations.ValidateDto(dto, "MembershipTypeId", "VehicleId", "StartDate", "EndDate", "PriceAtPurchase", "DurationDays");

                if (dto.MembershipTypeId <= 0)
                    throw new ArgumentException("El campo MembershipTypeId debe ser mayor a 0.");

                if (dto.VehicleId <= 0)
                    throw new ArgumentException("El campo VehicleId debe ser mayor a 0.");

                if (dto.StartDate == null)
                    throw new ArgumentException("El campo StartDate es obligatorio.");

                if (dto.EndDate == null)
                    throw new ArgumentException("El campo EndDate es obligatorio.");

                if (dto.StartDate.Value.Date < DateTime.UtcNow.Date)
                    throw new ArgumentException("La fecha de inicio no puede ser anterior a la fecha actual.");

                if (dto.EndDate.Value <= dto.StartDate.Value)
                    throw new ArgumentException("La fecha de fin debe ser mayor a la fecha de inicio.");

                if (dto.PriceAtPurchase <= 0)
                    throw new ArgumentException("El precio de la membresía debe ser mayor que 0.");

                if (dto.DurationDays <= 0)
                    throw new ArgumentException("El campo DurationDays debe ser mayor que 0.");

                var diferenciaDias = (dto.EndDate.Value - dto.StartDate.Value).Days;
                if (dto.DurationDays != diferenciaDias)
                    throw new ArgumentException($"El campo DurationDays ({dto.DurationDays}) no coincide con la diferencia real de días ({diferenciaDias}).");

                if (!string.IsNullOrWhiteSpace(dto.Currency))
                {
                    dto.Currency = dto.Currency.Trim().ToUpper();
                    if (dto.Currency.Length > 3)
                        throw new ArgumentException("El campo Currency no puede tener más de 3 caracteres.");

                    if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Currency, @"^[A-Z]{3}$"))
                        throw new ArgumentException("El campo Currency debe contener solo 3 letras mayúsculas (ISO 4217).");
                }



                dto.Asset = true;


                BaseModel entity = _mapper.Map<Memberships>(dto);
                entity = await _data.Save((Memberships)entity);

                return _mapper.Map<MembershipsDto>(entity);
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
                throw new BusinessException("Error al crear la membresía.", ex);
            }
        }

        public override async Task Update(MembershipsDto dto)
        {
            try
            {
                Validations.ValidateDto(dto, "Id", "MembershipTypeId", "VehicleId", "StartDate", "EndDate", "PriceAtPurchase", "DurationDays");

                if (dto.Id <= 0)
                    throw new ArgumentException("El campo Id debe ser mayor a 0.");

                
                var membershipExistente = await _data.GetById(dto.Id);
                if (membershipExistente == null)
                    throw new InvalidOperationException($"No existe una membresía con Id {dto.Id}.");

               
                if (!membershipExistente.Asset)
                    throw new InvalidOperationException("No se puede actualizar una membresía deshabilitada.");

          
                if (dto.MembershipTypeId <= 0)
                    throw new ArgumentException("El campo MembershipTypeId debe ser mayor a 0.");

              
                if (dto.VehicleId <= 0)
                    throw new ArgumentException("El campo VehicleId debe ser mayor a 0.");

                if (dto.StartDate == null)
                    throw new ArgumentException("El campo StartDate es obligatorio.");

                if (dto.EndDate == null)
                    throw new ArgumentException("El campo EndDate es obligatorio.");

                if (dto.EndDate.Value <= dto.StartDate.Value)
                    throw new ArgumentException("La fecha de fin debe ser mayor que la fecha de inicio.");


                if (dto.PriceAtPurchase <= 0)
                    throw new ArgumentException("El precio de la membresía debe ser mayor que 0.");


                if (dto.DurationDays <= 0)
                    throw new ArgumentException("El campo DurationDays debe ser mayor que 0.");

                var diferenciaDias = (dto.EndDate.Value - dto.StartDate.Value).Days;
                if (dto.DurationDays != diferenciaDias)
                    throw new ArgumentException($"El campo DurationDays ({dto.DurationDays}) no coincide con la diferencia real de días ({diferenciaDias}).");


                if (!string.IsNullOrWhiteSpace(dto.Currency))
                {
                    dto.Currency = dto.Currency.Trim().ToUpper();
                    if (dto.Currency.Length > 3)
                        throw new ArgumentException("El campo Currency no puede tener más de 3 caracteres.");

                    if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Currency, @"^[A-Z]{3}$"))
                        throw new ArgumentException("El campo Currency debe contener solo 3 letras mayúsculas (ISO 4217).");
                }


       
                BaseModel entity = _mapper.Map<Memberships>(dto);
                await _data.Update((Memberships)entity);
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
                throw new BusinessException("Error al actualizar la membresía.", ex);
            }
        }

    }
}
