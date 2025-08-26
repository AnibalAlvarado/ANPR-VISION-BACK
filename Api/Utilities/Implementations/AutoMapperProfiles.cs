﻿using AutoMapper;
using Entity.Dtos;
using Entity.Models;
using System;
using System.Security;
using Utilities.Interfaces;


namespace Utilities.Implementations
{
    public class AutoMapperProfiles : Profile
    {
        

        public AutoMapperProfiles() : base()
        {
         

            // Ejemplo de mapeo
            CreateMap<Form, FormDto>();
            CreateMap<FormDto, Form>();

            CreateMap<Module, ModuleDto>();
            CreateMap<ModuleDto, Module>();

            CreateMap<FormModule, FormModuleDto>();
            CreateMap<FormModuleDto, FormModule>();

            CreateMap<Module, ModuleDto>();
            CreateMap<ModuleDto, Module>();

            CreateMap<Permission, PermissionDto>();
            CreateMap<PermissionDto, Permission>();

            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, Person>();


            CreateMap<Rol, RolDto>();
            CreateMap<RolDto, Rol>();

            CreateMap<RolFormPermission, RolFormPermissionDto>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form.Name))
                .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name))
                .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name));
            CreateMap<RolFormPermissionDto, RolFormPermission>();

            //CreateMap<RolUser, RolUserDto>();

            CreateMap<RolUser, RolUserDto>()
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
                        .ForMember(dest => dest.RolName, opt => opt.MapFrom(src => src.Rol.Name));

            CreateMap<RolUserDto, RolUser>();

            CreateMap<User, UserDto>()
                 .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.Person.FirstName));
            //CreateMap<UserDto, User>()
            //.ForPath(dest => dest.Person.Firstname, opt => opt.MapFrom(src => src.PersonName));

            CreateMap<UserDto, User>();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<BlackList, BlackListDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Memberships, MembershipsDto>().ReverseMap();
            CreateMap<MemberShipType, MemberShipTypeDto>().ReverseMap();
            CreateMap<Parking, ParkingDto>().ReverseMap();
            CreateMap<ParkingCategory, ParkingCategoryDto>().ReverseMap();
            CreateMap<Rates, RatesDto>().ReverseMap();
            CreateMap<RatesType, RatesTypeDto>().ReverseMap();
            CreateMap<RegisteredVehicles, RegisteredVehiclesDto>().ReverseMap();
            CreateMap<Sectors, SectorsDto>().ReverseMap();
            CreateMap<Slots, SlotsDto>().ReverseMap();
            CreateMap<TypeVehicle, TypeVehicleDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<Zones, ZonesDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();

        }
    }
}
