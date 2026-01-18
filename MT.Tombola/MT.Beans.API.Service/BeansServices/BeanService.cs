using MT.Tombola.Api.Data.Models;
using MT.Tombola.Api.Data.Repos.Beans;

namespace MT.Beans.API.Service.BeansServices
{
    public class BeanService : IBeanService
    {
        private readonly IBeanRepository<Bean> _repository;

        public BeanService(IBeanRepository<Bean> repository)
        {
            _repository = repository;
        }

        public async Task<List<Bean>> GetAllAsync(string? search)
        {
            try
            {
                return await _repository.GetAllAsync(search);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve beans.", ex);
            }
        }

        public async Task<Bean?> GetByExternalIdAsync(string externalId)
        {
            try
            {
                return await _repository.GetByExternalIdAsync(externalId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve bean '{externalId}'.", ex);
            }
        }

        public async Task<Bean> CreateAsync(Bean bean)
        {
            try
            {
                return await _repository.CreateAsync(bean);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create bean.", ex);
            }
        }

        public async Task UpdateAsync(string externalId, Bean bean)
        {
            try
            {
                if (externalId != bean.ExternalId)
                    throw new ArgumentException("Bean ID mismatch");

                await _repository.UpdateAsync(bean);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update bean '{externalId}'.", ex);
            }
        }

        public async Task DeleteAsync(string externalId)
        {
            try
            {
                var bean = await _repository.GetByExternalIdAsync(externalId);
                if (bean is null)
                    throw new KeyNotFoundException($"Bean with ID '{externalId}' not found");

                await _repository.DeleteAsync(bean);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete bean '{externalId}'.", ex);
            }
        }
    }
}
