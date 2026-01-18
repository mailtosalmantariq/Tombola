using MT.Beans.App.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Beans.App.Service.BeansApiClient
{
    public interface IBeansApiClientService<T> where T : class
    {
        Task<List<T>> GetBeansAsync();
        Task<T?> GetBeanAsync(string externalId);
        Task<T?> GetBeanOfTheDayAsync();
        Task<bool> SubmitOrderAsync(OrderRequest order);

    }
}
