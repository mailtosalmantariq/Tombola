using Microsoft.EntityFrameworkCore;
using MT.Tombola.Api.Data.Data;
using MT.Tombola.Api.Data.Models;

namespace MT.Tombola.Api.Data.Repos.BeanDay
{
    public class BeanOfTheDayRepository : IBeanOfTheDayRepository
    {
        private readonly BeansDbContext _context;
        private readonly Random _random = new();

        public BeanOfTheDayRepository(BeansDbContext context)
        {
            _context = context;
        }

        public async Task<Bean> GetTodayAsync()
        {
            try
            {
                var today = GetToday();

                var existing = await GetExistingBeanOfTheDayAsync(today);
                if (existing is not null)
                    return existing;

                var yesterdayBeanId = await GetYesterdayBeanIdAsync(today);

                var availableBeans = await GetAvailableBeansAsync(yesterdayBeanId);
                var selected = SelectRandomBean(availableBeans);

                await SaveBeanOfTheDayAsync(today, selected.Id);

                return selected;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve Bean of the Day.", ex);
            }
        }

        private static DateOnly GetToday() => DateOnly.FromDateTime(DateTime.UtcNow);

        private async Task<Bean?> GetExistingBeanOfTheDayAsync(DateOnly today)
        {
            try
            {
                return await _context.BeanOfTheDays
                    .Include(b => b.Bean)
                    .Where(b => b.Date == today)
                    .Select(b => b.Bean)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load existing Bean of the Day.", ex);
            }
        }

        private async Task<int?> GetYesterdayBeanIdAsync(DateOnly today)
        {
            try
            {
                var yesterday = today.AddDays(-1);

                return await _context.BeanOfTheDays
                    .Where(b => b.Date == yesterday)
                    .Select(b => (int?)b.BeanId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load yesterday's Bean of the Day.", ex);
            }
        }

        private async Task<List<Bean>> GetAvailableBeansAsync(int? excludeBeanId)
        {
            try
            {
                var query = _context.Beans.AsQueryable();

                if (excludeBeanId.HasValue)
                    query = query.Where(b => b.Id != excludeBeanId.Value);

                var beans = await query.ToListAsync();

                if (!beans.Any())
                    throw new InvalidOperationException("No beans available.");

                return beans;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load available beans.", ex);
            }
        }

        private Bean SelectRandomBean(List<Bean> beans)
        {
            return beans[_random.Next(beans.Count)];
        }

        private async Task SaveBeanOfTheDayAsync(DateOnly date, int beanId)
        {
            try
            {
                _context.BeanOfTheDays.Add(new BeanOfTheDay
                {
                    Date = date,
                    BeanId = beanId
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while saving Bean of the Day.", ex);
            }
        }
    }
}
