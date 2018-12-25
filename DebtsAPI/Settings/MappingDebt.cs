using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DebtsAPI.Models;
using DebtsAPI.Dtos;

namespace DebtsAPI.Mappings
{
    public class DebtProfile : Profile
    {
        public DebtProfile()
        {
            CreateMap<DebtDto, Debt>();
            
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            
            CreateMap<User, UserAuthenticateResponseDto>();

            CreateMap<UserAuthenticateDto, User>();

        }
    }
}