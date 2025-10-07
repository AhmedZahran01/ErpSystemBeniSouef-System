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
        #region Constractor Region
        private protected readonly ApplicationDbContext _context;

        public GenaricRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Get All Region

        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(m => m.IsDeleted == false);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public async Task<List<T>> GetAllProductsAsync(int comanyNo, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(m => m.IsDeleted == false);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(m => m.IsDeleted == false);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        #endregion

        #region Get By Condion And Inclide Region

        public List<T> GetByCondionAndInclide(
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

        #endregion

        #region Get By Id Region

        public T? GetById(int id)
           => _context.Set<T>().Find(id);

        public async Task<T?> GetByIdAsync(int id)
         => await _context.Set<T>().FindAsync(id);

        #endregion

        #region Add , Update , Delete Region

        public void Add(T model)
            => _context.Set<T>().Add(model);

        public void Delete(T model)
            => _context.Set<T>().Remove(model);

        public void Update(T model)
            => _context.Set<T>().Update(model);

        #endregion

        #region Find In Specific Condition Region

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }
        #endregion

        #region FindWithIncludesAsync Region

        public async Task<T?> FindWithIncludesAsync
            (Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return query.FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
       
        #region GetByIdWithIncludesAsync Region

        public async Task<T?> GetByIdWithIncludesAsync(
          int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        #endregion

    }
}
