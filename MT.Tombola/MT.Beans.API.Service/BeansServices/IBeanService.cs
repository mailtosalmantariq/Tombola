using MT.Tombola.Api.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Beans.API.Service.BeansServices
{
    public interface IBeanService
    {
        Task<List<Bean>> GetAllAsync(string? search);
        Task<Bean?> GetByExternalIdAsync(string externalId);
        Task<Bean> CreateAsync(Bean bean);
        Task UpdateAsync(string externalId, Bean bean);
        Task DeleteAsync(string externalId);
    }

}
