using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtsAPI.Models;
using AutoMapper;

namespace DebtsAPI.Dtos.Debts
{
    public class DebtDtoMapper
    {
        private readonly IMapper _mapper;
        public DebtDtoMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public DebtDetailResponseDto MapToDebtDetailResponseDto(Debt debt)
        {
            return new DebtDetailResponseDto {
                Id = debt.Id,
                Giver = _mapper.Map<UserDto>(debt.Giver),
                Taker = _mapper.Map<UserDto>(debt.Taker),
                Sum = debt.Sum,
                Date = debt.Date,
            };
        }
    }
}
