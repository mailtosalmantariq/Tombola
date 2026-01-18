using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MT.Beans.API.Service.BeansServices;
using MT.Tombola.Api.Data.Models;
using MT.Tombola.Api.Data.Repos.Beans;

namespace MT.Beans.API.Service.Tests.BeansServices
{
    public class BeanServiceTests
    {
        private Mock<IBeanRepository<Bean>> _repoMock = default!;
        private BeanService _service = default!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IBeanRepository<Bean>>();
            _service = new BeanService(_repoMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsBeans_WhenRepositoryReturnsData()
        {
            var beans = new List<Bean> { new Bean { Name = "TestBean" } };

            _repoMock.Setup(r => r.GetAllAsync(null))
                     .ReturnsAsync(beans);

            var result = await _service.GetAllAsync(null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestBean"));
        }

        [Test]
        public void GetAllAsync_ThrowsWrappedException_WhenRepositoryFails()
        {
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<string>()))
                     .ThrowsAsync(new Exception("DB error"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetAllAsync(null));

            Assert.That(ex!.Message, Is.EqualTo("Failed to retrieve beans."));
        }

        [Test]
        public async Task GetByExternalIdAsync_ReturnsBean_WhenFound()
        {
            var bean = new Bean { ExternalId = "abc123", Name = "ZILLAN" };

            _repoMock.Setup(r => r.GetByExternalIdAsync("abc123"))
                     .ReturnsAsync(bean);

            var result = await _service.GetByExternalIdAsync("abc123");

            Assert.That(result!.Name, Is.EqualTo("ZILLAN"));
        }

        [Test]
        public void GetByExternalIdAsync_ThrowsWrappedException_WhenRepositoryFails()
        {
            _repoMock.Setup(r => r.GetByExternalIdAsync("abc123"))
                     .ThrowsAsync(new Exception("DB error"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.GetByExternalIdAsync("abc123"));

            Assert.That(ex!.Message, Is.EqualTo("Failed to retrieve bean 'abc123'."));
        }

        [Test]
        public async Task CreateAsync_ReturnsCreatedBean()
        {
            var bean = new Bean { Name = "NewBean" };

            _repoMock.Setup(r => r.CreateAsync(bean))
                     .ReturnsAsync(bean);

            var result = await _service.CreateAsync(bean);

            Assert.That(result.Name, Is.EqualTo("NewBean"));
        }

        [Test]
        public void CreateAsync_ThrowsWrappedException_WhenRepositoryFails()
        {
            var bean = new Bean { Name = "NewBean" };

            _repoMock.Setup(r => r.CreateAsync(bean))
                     .ThrowsAsync(new Exception("DB error"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(bean));

            Assert.That(ex!.Message, Is.EqualTo("Failed to create bean."));
        }

        [Test]
        public void UpdateAsync_ThrowsArgumentException_WhenIdMismatch()
        {
            var bean = new Bean { ExternalId = "id1" };

            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync("id2", bean));

            Assert.That(ex!.InnerException, Is.TypeOf<ArgumentException>());
        }

        [Test]
        public async Task UpdateAsync_CallsRepository_WhenValid()
        {
            var bean = new Bean { ExternalId = "id1" };

            _repoMock.Setup(r => r.UpdateAsync(bean))
                     .Returns(Task.CompletedTask);

            await _service.UpdateAsync("id1", bean);

            _repoMock.Verify(r => r.UpdateAsync(bean), Times.Once);
        }

        [Test]
        public void UpdateAsync_ThrowsWrappedException_WhenRepositoryFails()
        {
            var bean = new Bean { ExternalId = "id1" };

            _repoMock.Setup(r => r.UpdateAsync(bean))
                     .ThrowsAsync(new Exception("DB error"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync("id1", bean));

            Assert.That(ex!.Message, Is.EqualTo("Failed to update bean 'id1'."));
        }

        [Test]
        public async Task DeleteAsync_DeletesBean_WhenFound()
        {
            var bean = new Bean { ExternalId = "id1" };

            _repoMock.Setup(r => r.GetByExternalIdAsync("id1"))
                     .ReturnsAsync(bean);

            _repoMock.Setup(r => r.DeleteAsync(bean))
                     .Returns(Task.CompletedTask);

            await _service.DeleteAsync("id1");

            _repoMock.Verify(r => r.DeleteAsync(bean), Times.Once);
        }

        [Test]
        public void DeleteAsync_ThrowsWrappedException_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByExternalIdAsync("id1"))
                     .ReturnsAsync((Bean?)null);

            var ex = Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync("id1"));

            Assert.That(ex!.InnerException, Is.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void DeleteAsync_ThrowsWrappedException_WhenRepositoryFails()
        {
            var bean = new Bean { ExternalId = "id1" };

            _repoMock.Setup(r => r.GetByExternalIdAsync("id1"))
                     .ReturnsAsync(bean);

            _repoMock.Setup(r => r.DeleteAsync(bean))
                     .ThrowsAsync(new Exception("DB error"));

            var ex = Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync("id1"));

            Assert.That(ex!.Message, Is.EqualTo("Failed to delete bean 'id1'."));
        }
    }
}
