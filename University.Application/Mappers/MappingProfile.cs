using AutoMapper;
using University.Application.DTOs;
using University.Domain.Entities;

namespace University.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<TSource, TDestination>();
            CreateMap<Users_Accounts, AccountDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.is_active))
                .ReverseMap()
                .ForMember(dest=> dest.is_active, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Users_Accounts, AccountDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.roles.Select(r => new RoleDto { Id = r.role_id, Name = r.role.name })));

            CreateMap<AccountDto, Users_Accounts>()
                .ForMember(dest => dest.roles, opt => opt.MapFrom(src => src.Roles.Select(r => new Users_Accounts_Roles { role_id = r.Id, account_id =src.Id })));

            CreateMap<RoleDto, Users_Accounts_Roles>()
                .ForMember(dest => dest.role_id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.role_id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.role.name));

            CreateMap<Roles, RoleDto>().ReverseMap();
            CreateMap<Users_Accounts_Roles, AccountRoleDto>()
                .ForMember(dest=>dest.RoleId, opt => opt.MapFrom(src => src.role_id))
                .ForMember(dest=>dest.AccountId, opt => opt.MapFrom(src => src.account_id))
                .ReverseMap();

            CreateMap<Students, StudentDto>().ReverseMap();
            CreateMap<Students_Addresses, AddressDto>().ReverseMap();

            CreateMap<AddressDto, Students_Addresses>()
                .ForMember(dest => dest.postal_code, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.building_number, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.apartment_number, opt => opt.MapFrom(src => src.ApartmentNumber));

            CreateMap<AccountDto, Users_Accounts>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.is_active, opt => opt.MapFrom(src => src.IsActive));

        }
    }
}
