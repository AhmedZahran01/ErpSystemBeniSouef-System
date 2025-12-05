using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Infrastructer
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories;
        private readonly ApplicationDbContext _context;



        public UnitOfwork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenaricRepositoy<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenaricRepository<T>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenaricRepositoy<T>)_repositories[type];
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public int Complete()
            => _context.SaveChanges();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

       async Task<IDbContextTransaction> IUnitOfWork.BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
