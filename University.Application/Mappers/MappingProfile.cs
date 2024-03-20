using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }
    }
}
