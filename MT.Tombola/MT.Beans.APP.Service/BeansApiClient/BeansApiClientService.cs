using System.Net.Http.Json;
using MT.Beans.App.Service.Models;

namespace MT.Beans.App.Service.BeansApiClient
{
    public class BeansApiClientService<T> : IBeansApiClientService<T> where T : class
    {
        private readonly HttpClient _http;

        public BeansApiClientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<T>> GetBeansAsync()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<T>>("api/beans");
                return result ?? new List<T>();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load beans from API.", ex);
            }
        }

        public async Task<T?> GetBeanAsync(string externalId)
        {
            try
            {
                return await _http.GetFromJsonAsync<T>($"api/beans/{externalId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load bean '{externalId}' from API.", ex);
            }
        }

        public async Task<T?> GetBeanOfTheDayAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<T>("api/beans/bean-of-the-day");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load Bean of the Day from API.", ex);
            }
        }

        public async Task<bool> SubmitOrderAsync(OrderRequest order)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/orders", order);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to submit order to API.", ex);
            }
        }
    }
}
