using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DebtsAPI.Models;
using DebtsAPI.Dtos;
using DebtsAPI.Dtos.Debts;

namespace DebtsAPI.Mappings
{
    public class DebtProfile : Profile
    {
        public DebtProfile()
        {
            CreateMap<DebtInboxDto, Debt>();
            CreateMap<DebtEditDto, Debt>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserAuthenticateResponseDto>();
            CreateMap<UserAuthenticateDto, User>();

            CreateMap<PaymentInboxDto, Payment>();
            CreateMap<Payment, PaymentResponseDto>();

        }
    }
}