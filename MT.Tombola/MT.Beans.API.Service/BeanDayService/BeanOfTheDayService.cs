using MT.Beans.API.Service.BeanDayService;
using MT.Tombola.Api.Data.Models;
using MT.Tombola.Api.Data.Repos.BeanDay;

namespace MT.Tombola.Api.Services
{
    public class BeanOfTheDayService : IBeanOfTheDayService
    {
        private readonly IBeanOfTheDayRepository _repository;

        public BeanOfTheDayService(IBeanOfTheDayRepository repository)
        {
            _repository = repository;
        }

        public async Task<Bean> GetTodayAsync()
        {
            try
            {
                return await _repository.GetTodayAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve Bean of the Day from repository.", ex);
            }
        }

    }
}
