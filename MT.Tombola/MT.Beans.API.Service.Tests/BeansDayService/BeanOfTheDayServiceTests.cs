using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;
using MT.Tombola.Api.Services;
using MT.Tombola.Api.Data.Models;
using MT.Tombola.Api.Data.Repos.BeanDay;

namespace MT.Beans.API.Service.Tests.BeanDayService
{
    public class BeanOfTheDayServiceTests
    {
        private Mock<IBeanOfTheDayRepository> _repoMock = default!;
        private BeanOfTheDayService _service = default!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IBeanOfTheDayRepository>();
            _service = new BeanOfTheDayService(_repoMock.Object);
        }

       [Test]
        public async Task GetTodayAsync_ReturnsBean_WhenRepositoryReturnsBean()
        {
            var bean = new Bean
            {
                ExternalId = "abc123",
                Name = "ZILLAN",
                Country = "Colombia"
            };

            _repoMock.Setup(r => r.GetTodayAsync())
                     .ReturnsAsync(bean);

            var result = await _service.GetTodayAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("ZILLAN"));
        }

        [Test]
        public void GetTodayAsync_ThrowsWrappedException_WhenRepositoryThrows()
        {
            _repoMock.Setup(r => r.GetTodayAsync())
                     .ThrowsAsync(new Exception("DB failure"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetTodayAsync());

            Assert.That(ex!.Message, Is.EqualTo("Failed to retrieve Bean of the Day from repository."));
            Assert.That(ex.InnerException!.Message, Is.EqualTo("DB failure"));
        }
    }
}
