using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fashion.Core.Entities;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Fashion.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fashion.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly FashionDbContext _dbContext;
        private Dictionary<string, object> repositories;

        public UnitOfWork(FashionDbContext dbContext)
        {
            _dbContext = dbContext;
            repositories = new Dictionary<string, object>();
        }
        public async Task<bool> CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
                return true;
            }
            catch
            {
                await _transaction.RollbackAsync();
                return false;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null!;
            }

        }

        public void Dispose()
        {
             _dbContext .Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);
                repositories.Add(type, repositoryInstance!);
            }
            
            return (IGenericRepository<TEntity>)repositories[type];
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _dbContext.Database.RollbackTransactionAsync();
                await _transaction.DisposeAsync();
                _transaction = null!;
            }
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }
    }
}
