using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        void Update(T entity);
        Task<T> FindByIdAsync(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> where);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll();
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);
        Task<int> CountAsync(Expression<Func<T, bool>> where);
        Task<int> CountAsync();

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        #endregion
    }
}
