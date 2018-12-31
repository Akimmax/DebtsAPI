using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;
using DebtsAPI.Data;
using DebtsAPI.Services;
using DebtsAPI.Models;
using AutoMapper;
using DebtsAPI.Mappings;
using DebtsAPI.Dtos;
using Microsoft.AspNetCore.Http;

namespace DebtsAPI.Tests.Services
{
    public class DebtsServiceTests : IDisposable
    {
        IMapper iMapper;

        public DebtsServiceTests()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<DebtProfile>();
                cfg.AddProfile(new DebtProfile());
            });
            iMapper = config.CreateMapper();
        }

        public void Dispose()
        {

        }

        [Fact]
        public void Should_Get_All()
        {
            IQueryable<Debt> debts = new List<Debt>
            {
                new Debt { Id = 1, Sum = 15},
                new Debt { Id = 2, Sum = 20 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Debt>>();
            mockSet.As<IQueryable<Debt>>().Setup(m => m.Provider).Returns(debts.Provider);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.Expression).Returns(debts.Expression);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.ElementType).Returns(debts.ElementType);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.GetEnumerator()).Returns(debts.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Debts).Returns(mockSet.Object);
            var service = new DebtsService(mockContext.Object, iMapper, new HttpContextAccessor());

            var actual = service.GetAll();

            Assert.Equal(2, actual.Count());
            mockContext.Verify(m => m.Debts);
            Assert.Equal(debts.First(d => d.Id == 1).Sum, actual.First(d => d.Id == 1).Sum);
            Assert.Equal(debts.First(d => d.Id == 2).Sum, actual.First(d => d.Id == 2).Sum);

        }

        [Fact]
        public void Should_Create()
        {
            Debt myDebt = new Debt
            {
                Sum = 15,
                GiverId = 1,
                TakerId = 2
            };

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Debts.Add(myDebt));
            var service = new DebtsService(mockContext.Object, iMapper, new HttpContextAccessor());

            var actual = service.CreateDebt(
                new DebtDto()
                {
                    GiverId = (int)myDebt.GiverId,
                    TakerId = (int)myDebt.TakerId,
                    Sum = myDebt.Sum
                }
            );

            mockContext.Verify(m => m.SaveChanges());
            mockContext.Verify(m => m.Debts.Add(It.Is<Debt>(t =>
                t.Sum == myDebt.Sum
                && t.TakerId == myDebt.TakerId
                && t.GiverId == myDebt.GiverId
                && t.IsActive == true
                && t.Date.Date == DateTime.Now.Date
            )));

        }
    }
}
