﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace DataService.Repository
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> List { get; }
        bool Create(TEntity entity);
        bool Delete(Object key);
        bool Update(TEntity entity);
        TEntity FindByKey(Object Key);
    }
    public abstract class BaseRepository<TEntity>: IRepository<TEntity>
        where TEntity:class
    {
        protected EFPEntities entites { get; set; }
        protected DbSet<TEntity> dbSet { get; set; }

        public IEnumerable<TEntity> List
        {
            get
            {
                try
                {
                    return this.dbSet;
                }
                catch
                {
                    return null;
                }
            }
        }

        public BaseRepository()
        {
            EFPEntities dbContext = new EFPEntities();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = true;
            this.entites = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public BaseRepository(EFPEntities dbContext)
        {
            this.entites = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public bool Create(TEntity entity)
        {
            try
            {
                dbSet.Add(entity);
                entites.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return false;
            }
        }

        public TEntity CreateNew(TEntity entity)
        {
            try
            {
                TEntity rs = dbSet.Add(entity);
                entites.SaveChanges();
                return rs;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(object key)
        {
            try
            {
                dbSet.Remove(FindByKey(key));
                entites.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(TEntity entity)
        {
            try
            {
                this.entites.Entry(entity).State = EntityState.Modified;
                this.entites.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TEntity FindByKey(object Key)
        {
            try
            {
                return dbSet.Find(Key);
            }
            catch
            {
                return null;
            }
        }
    }
}
