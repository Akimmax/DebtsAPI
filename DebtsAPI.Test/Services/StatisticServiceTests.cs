using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;
using DebtsAPI.Data;
using DebtsAPI.Services;
using DebtsAPI.Models;
using System.Security.Claims;
using DebtsAPI.Test.Fakes;

namespace DebtsAPI.Tests.Services
{
    public class StatisticServiceTests : IDisposable
    {
        Mock<FakeHttpContextAccessor> mockAccessor;
        User currentUser;

        public StatisticServiceTests()
        {
            currentUser = new User { Id = 1 };

            mockAccessor = new Mock<FakeHttpContextAccessor>();
            var claimCurrentUser = new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString());
            mockAccessor.Setup(c => c.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)).Returns(claimCurrentUser);
        }

        public void Dispose()
        {

        }

        [Fact]
        public void Should_Get_Statistic()
        {
            IQueryable<Debt> debts = new List<Debt>
            {
                new Debt { Id = 1, Sum = 10, TakerId = currentUser.Id},
                new Debt { Id = 2, Sum = 20, TakerId = currentUser.Id},
                new Debt { Id = 3, Sum = 30, GiverId = currentUser.Id},
                new Debt { Id = 4, Sum = 40, GiverId = currentUser.Id }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Debt>>();
            mockSet.As<IQueryable<Debt>>().Setup(m => m.Provider).Returns(debts.Provider);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.Expression).Returns(debts.Expression);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.ElementType).Returns(debts.ElementType);
            mockSet.As<IQueryable<Debt>>().Setup(m => m.GetEnumerator()).Returns(debts.GetEnumerator());

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(c => c.Debts).Returns(mockSet.Object);
            var service = new StatisticService(mockContext.Object, mockAccessor.Object);

            var actual = service.GetTotalStatistic();

            mockContext.Verify(m => m.Debts);
            Assert.Equal(30, debts.First(d => d.Id == 1).Sum + debts.First(d => d.Id == 2).Sum);
            Assert.Equal(70, debts.First(d => d.Id == 3).Sum + debts.First(d => d.Id == 4).Sum);
        }
    }
}
