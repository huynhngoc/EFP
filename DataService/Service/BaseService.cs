using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using DataService.Repository;

namespace DataService.Service
{
    public interface IService
    {

    }

    public interface IService<TEntity> : IService
        where TEntity : class
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(object key);
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Save();

        void DeActivate(TEntity entity);

        Task DeActivateAsync(TEntity entity);

        void OnCreate(TEntity entity);

        #region Async Function
        Task<TEntity> GetAsync(object key);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task SaveAsync();
        #endregion
    }

    public abstract class BaseService<TEntity> : IService<TEntity>
        where TEntity : class
    {
        public BaseService()
        {

        }
        public IRepository<TEntity> repository { get; set; }
        public IUnitOfWork unitOfWork { get; set; }

        public BaseService(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public virtual void Create(TEntity entity)
        {
            this.OnCreate(entity);

            this.repository.Create(entity);
            this.Save();
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            this.OnCreate(entity);

            this.repository.Create(entity);
            await this.SaveAsync();
        }

        public virtual IQueryable<TEntity> Get()
        {
            return this.repository.Get();
        }

        public virtual TEntity Get(object key)
        {
            return this.repository.Get(key);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.repository.Get(predicate);
        }

        public virtual async Task<TEntity> GetAsync(object key)
        {
            return await this.repository.GetAsync(key);
        }

        public virtual void Save()
        {
            this.unitOfWork.Save();
        }

        public virtual async Task SaveAsync()
        {
            await this.unitOfWork.SaveAsync();
        }

        public virtual void Update(TEntity entity)
        {
            this.repository.Update(entity);
            this.Save();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            this.repository.Update(entity);
            await this.SaveAsync();
        }

        public virtual void OnCreate(TEntity entity)
        {
            this.SetActive(entity, true);
        }

        public void DeActivate(TEntity entity)
        {
            this.SetActive(entity, false);
            this.Save();
        }

        public async Task DeActivateAsync(TEntity entity)
        {
            this.SetActive(entity, false);
            await this.SaveAsync();
        }

        private void SetActive(TEntity entity, bool value)
        {
            var typeInfo = typeof(TEntity);

            var activeProperty = typeInfo.GetProperty("Active",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (activeProperty != null)
            {
                activeProperty.SetValue(entity, value);
            }
        }
    }
}
