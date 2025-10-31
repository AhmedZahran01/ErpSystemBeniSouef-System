using ErpSystemBeniSouef.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenaricRepositoy<T> Repository<T>() where T : BaseEntity;

        Task<int> CompleteAsync();
        int Complete();
      Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
