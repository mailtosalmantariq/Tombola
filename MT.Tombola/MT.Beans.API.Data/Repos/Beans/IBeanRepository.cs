using MT.Tombola.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tombola.Api.Data.Repos.Beans
{
    public interface IBeanRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(string? search); 
        Task<T?> GetByExternalIdAsync(string externalId); 
        Task<T> CreateAsync(T bean); 
        Task UpdateAsync(T bean); 
        Task DeleteAsync(T bean);
    }
}
