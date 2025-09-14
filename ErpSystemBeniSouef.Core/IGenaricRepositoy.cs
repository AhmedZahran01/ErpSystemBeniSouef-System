using ErpSystemBeniSouef.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core
{
    public interface IGenaricRepositoy<T> where T : BaseEntity
    {
        void Add(T model);

        void Update(T model);
        void Delete(T model);

        List<T> GetBy(
        Expression<Func<T, bool>> query,
        params Expression<Func<T, object>>[] includes);
        List<T> GetAll(params Expression<Func<T, object>>[] includes);
        T? GetById(int id);
        Task<T?> Find(Expression<Func<T, bool>> predicate);
        Task<T?> GetDriverOrPassengerByIdAsync(string Id);
    }
}
