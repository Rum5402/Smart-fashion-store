using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly FashionDbContext _dbContext;

        public GenericRepository(FashionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }


        public Task UpdateAsync(T entity)
        {
            var trackedEntity = _dbContext.Set<T>().Local.FirstOrDefault(e => e.Equals(entity));
            if (trackedEntity == null)
            {
                _dbContext.Attach(entity);
            }

            _dbContext.Entry(entity).State = EntityState.Modified;

            foreach (var navigation in _dbContext.Entry(entity).Navigations)
            {
                if (navigation.IsModified)
                {
                    _dbContext.Entry(navigation.CurrentValue!).State = EntityState.Modified;
                }
            }

            return Task.CompletedTask;
        }
    }
}
