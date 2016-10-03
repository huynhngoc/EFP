using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace DataService.Repository
{
    public interface IRepository
    {

    }

    public interface IRepository<TEntity> : IRepository
        where TEntity : class
    {
        IQueryable<TEntity> Get();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        TEntity Get(object key);

        Task<TEntity> GetAsync(object key);

        void Create(TEntity entity);

        void Update(TEntity entity);
    }

    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private EFPEntities entites { get; set; }
        private DbSet<TEntity> dbSet { get; set; }

        public BaseRepository(EFPEntities dbContext)
        {
            this.entites = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public IQueryable<TEntity> Get()
        {
            return this.dbSet;
        }

        public TEntity Get(object key)
        {
            return this.dbSet.Find(key);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate);
        }

        public void Update(TEntity entity)
        {
            this.entites.Entry(entity).State = EntityState.Modified;
        }

        public async Task<TEntity> GetAsync(object key)
        {
            return await this.dbSet.FindAsync(key);
        }
    }
}
