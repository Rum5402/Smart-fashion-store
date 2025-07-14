using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fashion.Core.Entities;

namespace Fashion.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task<bool> CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangeAsync();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

    }
}
