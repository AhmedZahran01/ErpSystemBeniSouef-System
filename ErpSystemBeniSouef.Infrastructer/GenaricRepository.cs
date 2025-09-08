using ErpSystemBeniSouef.Core;
using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.Infrastructer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Infrastructer
{
    public class GenaricRepository<T> : IGenaricRepositoy<T> where T : BaseEntity
    {
        private protected readonly ApplicationDbContext _context;

        public GenaricRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(T model)
            => _context.Set<T>().Add(model);

        public void Delete(T model)
            => _context.Set<T>().Remove(model);


        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(m => m.IsDeleted == false);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return  query.ToList();
        }

        public List<T> GetBy(
    Expression<Func<T, bool>> query,
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            // Apply includes if provided
            foreach (var include in includes)
            {
                dbQuery = dbQuery.Include(include);
            }

            return dbQuery.Where(query).ToList();
        } 

        public async Task<T> GetByIdAsync(int id)
           => await _context.Set<T>().FindAsync(id);
           //=> await _context.Set<T>().Where(m => m.IsDeleted==false).FindAsync(id);


        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> GetDriverOrPassengerByIdAsync(string Id)
            => await _context.Set<T>().FindAsync(Id);


        public void Update(T model)
            => _context.Set<T>().Update(model);

         

    }
}
