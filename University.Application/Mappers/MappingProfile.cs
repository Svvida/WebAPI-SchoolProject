using AutoMapper;
using University.Application.DTOs;
using University.Domain.Entities;
using System.Linq;

namespace University.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Users_Accounts to AccountDto and reverse
            CreateMap<Users_Accounts, AccountDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ReverseMap()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Users_Accounts to AccountDto with Roles
            CreateMap<Users_Accounts, AccountDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => new RoleDto { Id = r.Role.Id, Name = r.Role.Name })));

            // AccountDto to Users_Accounts with Roles
            CreateMap<AccountDto, Users_Accounts>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => new Users_Accounts_Roles { RoleId = r.Id, AccountId = src.Id })));

            // RoleDto to Users_Accounts_Roles and reverse
            CreateMap<RoleDto, Users_Accounts_Roles>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name));

            // Roles to RoleDto and reverse
            CreateMap<Roles, RoleDto>().ReverseMap();

            // Users_Accounts_Roles to AccountRoleDto and reverse
            CreateMap<Users_Accounts_Roles, AccountRoleDto>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ReverseMap();

            // Students to StudentDto and reverse
            CreateMap<Students, StudentDto>().ReverseMap();

            // Students_Addresses to AddressDto and reverse
            CreateMap<Students_Addresses, AddressDto>().ReverseMap();

            // CreateRoleDto to Roles and reverse
            CreateMap<CreateRoleDto, Roles>().ReverseMap();

            // RoleDto to Roles and reverse
            CreateMap<RoleDto, Roles>().ReverseMap();
        }
    }
}
