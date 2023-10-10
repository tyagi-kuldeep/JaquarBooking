using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Persistence
{
    internal class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Properties
        private readonly ApplicationDbContext _context;

        protected DbSet<TEntity> _entities;
        #endregion
        #region Ctor

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion
        #region Utility
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            try
            {
                _context.SaveChanges();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                //if after the rollback of changes the context is still not saving,
                //return the full text of the exception that occurred when saving
                return ex.ToString();
            }
        }
        #endregion

        #region Repository Methods
        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();

                return _entities;
            }
        }
        public virtual async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
        }
        public virtual void Update(TEntity entity)
        {
            Entities.Update(entity);
        }
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await Entities.FindAsync(id);
            if (entity == null)
                return false;

            Entities.Remove(entity);
            return true;
        }
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            Entities.Remove(entity);
            return true;
        }
        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            var entities = Entities.Where(where);
            Entities.RemoveRange(entities);
            return true;
        }
        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await Entities.FindAsync(id);
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> where)
        {
            return await Entities.FirstOrDefaultAsync(where);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return Entities;
        }
        public virtual IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> where)
        {
            return Entities.Where(where);
        }
        public virtual async Task<int> CountAsync()
        {
            return await Entities.CountAsync();
        }
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            return await Entities.CountAsync(where);
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();
        #endregion
    }
}
