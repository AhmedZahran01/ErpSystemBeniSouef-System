using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core
{
    public interface IGenaricRepositoy<T> where T : class
    {
        #region Get All Region 

        List<T> GetAll(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllProductsAsync(int comanyNo, params Expression<Func<T, object>>[] includes);

        #endregion

        #region Get By Condition And Include Region

        List<T> GetByCondionAndInclide(       Expression<Func<T, bool>> query,
                params Expression<Func<T, object>>[] includes);

      Task<T?> FindWithIncludesAsync(
   Expression<Func<T, bool>> predicate,
   params Expression<Func<T, object>>[] includes);
        #endregion

        #region Get By Id Region
        IQueryable<T> GetAllQueryable(params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(int id);
        T? GetById(int id);

        #endregion

        #region Add , Update , Delete Region

        void Add(T model);

        void Update(T model);

        void Delete(T model);

        #endregion

        #region Find In Specific Condition Region
        
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        #endregion
    }
}
