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
            CreateMap<Users_Accounts, AccountDto>().ReverseMap();
            CreateMap<Roles, RoleDto>().ReverseMap();
            CreateMap<Users_Accounts_Roles, AccountRoleDto>().ReverseMap();
            CreateMap<Students, StudentDto>().ReverseMap();
            CreateMap<Students_Addresses, AddressDto>().ReverseMap();
        }
    }
}
