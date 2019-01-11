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
using DebtsAPI.Dtos.Debts;
using System.Security.Claims;
using DebtsAPI.Test.Fakes;
using Microsoft.AspNetCore.Http;

namespace DebtsAPI.Tests.Services
{
    public class DebtsServiceTests : IDisposable
    {
        IMapper iMapper;
        Mock<FakeHttpContextAccessor>  mockAccessor;
        User currentUser;

        public DebtsServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DebtProfile>();
                cfg.AddProfile(new DebtProfile());
            });
            iMapper = config.CreateMapper();

            currentUser = new User { Id = 1 };

            mockAccessor = new Mock<FakeHttpContextAccessor>();
            var claimCurrentUser = new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString());
            mockAccessor.Setup(c => c.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)).Returns(claimCurrentUser);
        }

        public void Dispose()
        {

        }

        [Fact]
        public void Should_Get_All()
        {           
            IQueryable<Debt> debts = new List<Debt>
            {
                new Debt { Id = 1, Sum = 15, TakerId = currentUser.Id},
                new Debt { Id = 2, Sum = 20, GiverId = currentUser.Id }
            }.AsQueryable();           

            var mockSet = new Mock<DbSet<Debt>>();
            mockSet.As<IQueryable<Debt>>().Setup(m => m.Provider).Returns(debts.Provider);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.Expression).Returns(debts.Expression);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.ElementType).Returns(debts.ElementType);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.GetEnumerator()).Returns(debts.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Debts).Returns(mockSet.Object);

            var servicePayment = new PaymentService(mockContext.Object, iMapper, mockAccessor.Object);
            var service = new DebtsService(mockContext.Object, iMapper, mockAccessor.Object, new DebtDtoMapper(iMapper), servicePayment);
            var noFilter = new DebtFilterDto { RoleInDebt = null };

            var actual = service.GetAll(noFilter);

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
                GiverId = currentUser.Id,
                TakerId = 2
            };

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Debts.Add(myDebt));

            var servicePayment = new PaymentService(mockContext.Object, iMapper, mockAccessor.Object);
            var service = new DebtsService(mockContext.Object, iMapper, mockAccessor.Object, new DebtDtoMapper(iMapper), servicePayment);

            service.CreateDebt(
                 new DebtInboxDto()
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
                && t.IsRepaid == false
                && t.Date.Date == DateTime.Now.Date
                && t.Deadline == t.Deadline
                && t.Description == t.Description
            )));
        }
    }
}
