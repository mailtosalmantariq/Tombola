using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MT.Beans.API.Controllers;
using MT.Beans.API.Service.BeansServices;
using MT.Beans.API.Service.BeanDayService;
using MT.Tombola.Api.Data.Models;

namespace MT.Beans.Tests
{
    public class BeansControllerTests
    {
        private Mock<IBeanService> _beanService = default!;
        private Mock<IBeanOfTheDayService> _beanOfTheDayService = default!;
        private BeansController _controller = default!;

        [SetUp]
        public void Setup()
        {
            _beanService = new Mock<IBeanService>();
            _beanOfTheDayService = new Mock<IBeanOfTheDayService>();
            _controller = new BeansController(_beanService.Object, _beanOfTheDayService.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk_WithBeans()
        {
            var beans = new List<Bean> { new Bean { Name = "TestBean" } };

            _beanService.Setup(s => s.GetAllAsync(null))
                        .ReturnsAsync(beans);

            var result = await _controller.GetAll(null);

            var ok = result.Result as OkObjectResult;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ok, Is.Not.Null);
                Assert.That(((List<Bean>)ok!.Value!).Count, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task GetAll_Returns500_OnException()
        {
            _beanService.Setup(s => s.GetAllAsync(null))
                        .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.GetAll(null);

            var status = result.Result as ObjectResult;
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task Get_ReturnsOk_WhenBeanFound()
        {
            var bean = new Bean { ExternalId = "id1", Name = "ZILLAN" };

            _beanService.Setup(s => s.GetByExternalIdAsync("id1"))
                        .ReturnsAsync(bean);

            var result = await _controller.Get("id1");

            var ok = result.Result as OkObjectResult;
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ok, Is.Not.Null);
                Assert.That(((Bean)ok!.Value!).Name, Is.EqualTo("ZILLAN"));
            }
        }

        [Test]
        public async Task Get_ReturnsNotFound_WhenNull()
        {
            _beanService.Setup(s => s.GetByExternalIdAsync("id1"))
                        .ReturnsAsync((Bean?)null);

            var result = await _controller.Get("id1");

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Get_Returns500_OnException()
        {
            _beanService.Setup(s => s.GetByExternalIdAsync("id1"))
                        .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.Get("id1");

            var status = result.Result as ObjectResult;
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }
        
        [Test]
        public async Task Create_ReturnsCreated_WhenSuccessful()
        {
            var bean = new Bean { ExternalId = "id1", Name = "NewBean" };

            _beanService.Setup(s => s.CreateAsync(bean))
                        .ReturnsAsync(bean);

            var result = await _controller.Create(bean);

            var created = result.Result as CreatedAtActionResult;
            Assert.Multiple(() =>
            {
                Assert.That(created, Is.Not.Null);
                Assert.That(((Bean)created!.Value!).Name, Is.EqualTo("NewBean"));
            });
        }

        [Test]
        public async Task Create_Returns500_OnException()
        {
            var bean = new Bean();

            _beanService.Setup(s => s.CreateAsync(bean))
                        .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.Create(bean);

            var status = result.Result as ObjectResult;
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }

        
        [Test]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            var bean = new Bean { ExternalId = "id1" };

            var result = await _controller.Update("id1", bean);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Update_ReturnsBadRequest_OnArgumentException()
        {
            _beanService.Setup(s => s.UpdateAsync("id1", It.IsAny<Bean>()))
                        .ThrowsAsync(new ArgumentException("Mismatch"));

            var result = await _controller.Update("id1", new Bean());

            var bad = result as BadRequestObjectResult;
            Assert.That(bad, Is.Not.Null);
        }

        [Test]
        public async Task Update_ReturnsNotFound_OnKeyNotFound()
        {
            _beanService.Setup(s => s.UpdateAsync("id1", It.IsAny<Bean>()))
                        .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.Update("id1", new Bean());

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task Update_Returns500_OnException()
        {
            _beanService.Setup(s => s.UpdateAsync("id1", It.IsAny<Bean>()))
                        .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.Update("id1", new Bean());

            var status = result as ObjectResult;
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            var result = await _controller.Delete("id1");

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNotFound_OnKeyNotFound()
        {
            _beanService.Setup(s => s.DeleteAsync("id1"))
                        .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.Delete("id1");

            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task Delete_Returns500_OnException()
        {
            _beanService.Setup(s => s.DeleteAsync("id1"))
                        .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.Delete("id1");

            var status = result as ObjectResult;
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task GetBeanOfTheDay_ReturnsOk_WhenSuccessful()
        {
            var bean = new Bean { Name = "ISONUS" };

            _beanOfTheDayService.Setup(s => s.GetTodayAsync())
                                .ReturnsAsync(bean);

            var result = await _controller.GetBeanOfTheDay();

            var ok = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(ok, Is.Not.Null);
                Assert.That(((Bean)ok!.Value!).Name, Is.EqualTo("ISONUS"));
            });
        }

        [Test]
        public async Task GetBeanOfTheDay_Returns500_OnException()
        {
            _beanOfTheDayService.Setup(s => s.GetTodayAsync())
                                .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.GetBeanOfTheDay();

            var status = result.Result as ObjectResult;
            Assert.That(status!.StatusCode, Is.EqualTo(500));
        }
    }
}
