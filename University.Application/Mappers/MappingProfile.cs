using AutoMapper;
using University.Application.DTOs;
using University.Domain.Entities;

namespace University.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Existing mappings
            CreateMap<Users_Accounts, AccountDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ReverseMap()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Users_Accounts, AccountDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => new RoleDto { Id = r.RoleId, Name = r.Role.Name })));

            CreateMap<AccountDto, Users_Accounts>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => new Users_Accounts_Roles { RoleId = r.Id, AccountId = src.Id })));

            CreateMap<RoleDto, Users_Accounts_Roles>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<Roles, RoleDto>().ReverseMap();
            CreateMap<Users_Accounts_Roles, AccountRoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ReverseMap();

            CreateMap<Students, StudentDto>().ReverseMap();
            CreateMap<Students_Addresses, AddressDto>().ReverseMap();

            CreateMap<AddressDto, Students_Addresses>()
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.ApartmentNumber));

            CreateMap<AccountDto, Users_Accounts>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Add these mappings
            CreateMap<CreateRoleDto, Roles>().ReverseMap();
            CreateMap<RoleDto, Roles>().ReverseMap();
        }
    }

}
