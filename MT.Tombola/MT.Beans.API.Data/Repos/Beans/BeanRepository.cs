using Microsoft.EntityFrameworkCore;
using MT.Tombola.Api.Data.Data;
using MT.Tombola.Api.Data.Models;

namespace MT.Tombola.Api.Data.Repos.Beans
{
    public class BeanRepository<T> : IBeanRepository<T> where T : Bean
    {
        private readonly BeansDbContext _context;

        public BeanRepository(BeansDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(string? search)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();

                    query = query.Where(b =>
                        b.Name.ToLower().Contains(search) ||
                        b.Country.ToLower().Contains(search) ||
                        b.Colour.ToLower().Contains(search));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve beans.", ex);
            }
        }

        public async Task<T?> GetByExternalIdAsync(string externalId)
        {
            try
            {
                return await _context.Set<T>()
                    .FirstOrDefaultAsync(b => b.ExternalId == externalId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve bean with ExternalId '{externalId}'.", ex);
            }
        }

        public async Task<T> CreateAsync(T bean)
        {
            try
            {
                _context.Set<T>().Add(bean);
                await _context.SaveChangesAsync();
                return bean;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while creating a bean.", ex);
            }
        }

        public async Task UpdateAsync(T bean)
        {
            try
            {
                _context.Entry(bean).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while updating bean.", ex);
            }
        }

        public async Task DeleteAsync(T bean)
        {
            try
            {
                _context.Set<T>().Remove(bean);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while deleting bean.", ex);
            }
        }
    }
}
