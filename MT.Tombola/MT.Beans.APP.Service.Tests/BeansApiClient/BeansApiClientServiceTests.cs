using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MT.Beans.App.Service.BeansApiClient;
using MT.Beans.App.Service.Models;

namespace MT.Beans.App.Service.Tests.BeansApiClient
{
    public class BeansApiClientServiceTests : IDisposable
    {
        private BeansApiClientService<BeanDto> _service = default!;
        private FakeHttpMessageHandler _handler = default!;
        private HttpClient _client = default!;

        [SetUp]
        public void Setup()
        {
            _handler = new FakeHttpMessageHandler();
            _client = new HttpClient(_handler)
            {
                BaseAddress = new Uri("https://localhost:7138/")
            };

            _service = new BeansApiClientService<BeanDto>(_client);
        }

        [Test]
        public async Task GetBeansAsync_ReturnsList_WhenApiReturnsData()
        {
            var beans = new List<BeanDto>
            {
                new BeanDto { Name = "TestBean", Country = "Peru", Colour = "Dark" }
            };

            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(beans))
            };

            var result = await _service.GetBeansAsync();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("TestBean"));
        }

        [Test]
        public void GetBeansAsync_ThrowsException_WhenApiFails()
        {
            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            Assert.ThrowsAsync<Exception>(() => _service.GetBeansAsync());
        }

        [Test]
        public async Task GetBeanAsync_ReturnsBean_WhenFound()
        {
            var bean = new BeanDto { Name = "ZILLAN", Country = "Colombia" };

            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(bean))
            };

            var result = await _service.GetBeanAsync("123");

            Assert.That(result!.Name, Is.EqualTo("ZILLAN"));
        }

        [Test]
        public void GetBeanAsync_ThrowsException_WhenApiFails()
        {
            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            Assert.ThrowsAsync<Exception>(() => _service.GetBeanAsync("123"));
        }

        [Test]
        public async Task GetBeanOfTheDayAsync_ReturnsBean()
        {
            var bean = new BeanDto { Name = "ISONUS", Country = "Vietnam" };

            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(bean))
            };

            var result = await _service.GetBeanOfTheDayAsync();

            Assert.That(result!.Name, Is.EqualTo("ISONUS"));
        }

        [Test]
        public void GetBeanOfTheDayAsync_ThrowsException_WhenApiFails()
        {
            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            Assert.ThrowsAsync<Exception>(() => _service.GetBeanOfTheDayAsync());
        }

        [Test]
        public async Task SubmitOrderAsync_ReturnsTrue_WhenSuccess()
        {
            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            var result = await _service.SubmitOrderAsync(new OrderRequest());

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task SubmitOrderAsync_ReturnsFalse_WhenNotSuccess()
        {
            _handler.ResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var result = await _service.SubmitOrderAsync(new OrderRequest());

            Assert.That(result, Is.False);
        }

        [Test]
        public void SubmitOrderAsync_ThrowsException_WhenHttpFails()
        {
            _handler.ThrowException = true;

            Assert.ThrowsAsync<Exception>(() => _service.SubmitOrderAsync(new OrderRequest()));
        }

        public void Dispose()
        {
            _client?.Dispose();
            _handler?.Dispose();
        }
    }

    // -------------------------------------------------------
    // FAKE HTTP HANDLER
    // -------------------------------------------------------
    public class FakeHttpMessageHandler : HttpMessageHandler, IDisposable
    {
        public HttpResponseMessage? ResponseMessage { get; set; }
        public bool ThrowException { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (ThrowException)
                throw new HttpRequestException("Simulated network failure");

            return Task.FromResult(ResponseMessage!);
        }

        public new void Dispose()
        {
            ResponseMessage?.Dispose();
        }
    }
}
