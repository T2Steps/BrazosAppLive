using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BrazosApp.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = null);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null,
        string includeProperties = null, bool isTracking = true);

        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
        string includeProperties = null, bool isTracking = true);

        Task<bool> AddAsync(T entity);

        Task<bool> RemoveAsync(T entity);

        Task <bool> UpdateAsync(T entity);

         Task <bool> Update(T entity);
        Task<bool> SaveChangesAsync();
        
    }
}
