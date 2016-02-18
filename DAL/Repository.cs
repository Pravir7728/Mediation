using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Contracts;

namespace DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(DbContext dbContext)
        {
            ctx = dbContext;
            dataSet = ctx.Set<T>();
        }

        protected DbContext ctx { get; set; }
        protected DbSet<T> dataSet { get; set; }

        public void SetProxy(bool value)
        {
            ctx.Configuration.ProxyCreationEnabled = value;
            ctx.Configuration.LazyLoadingEnabled = value;
        }

        public IQueryable<TF> GetAllOfType<TF>()
        {
            return dataSet.OfType<TF>();
        }

        public IQueryable<T> GetAll()
        {
            return dataSet;
        }

        public IEnumerable<T> GetAllEnu()
        {
            return dataSet;
        }

        public IQueryable<T> GetAllFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet;

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    result = result.Where(filter);
                }
            }
            if (includes == null) return result;
            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return result;
        }

        public IQueryable<T> GetAllFilteredNoTrack(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet.AsNoTracking();

            foreach (var filter in filters)
            {
                result = result.Where(filter);
            }
            if (includes == null) return result;
            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return result;
        }

        public T GetByFilteredNoTrack(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet.AsNoTracking();

            foreach (var filter in filters)
            {
                result = result.Where(filter);
            }
            if (includes == null) return result.FirstOrDefault();
            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return result.FirstOrDefault();
        }

        public T GetByFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> result = dataSet;

            foreach (var filter in filters)
            {
                result = result.Where(filter);
            }
            if (includes == null) return result.FirstOrDefault();
            foreach (var include in includes)
            {
                result = result.Include(include);
            }

            return result.FirstOrDefault();
        }

        public T GetById(Guid id)
        {
            return dataSet.Find(id);
        }

        public T GetById(int id)
        {
            return dataSet.Find(id);
        }

        public void Add(T model)
        {
            DbEntityEntry entryitem = ctx.Entry(model);
            if (entryitem.State == EntityState.Detached)
            {
                entryitem.State = EntityState.Added;
            }
            else
            {
                dataSet.Add(model);
            }
        }

        public void Detach(T model)
        {
            DbEntityEntry entryitem = ctx.Entry(model);
            entryitem.State = EntityState.Detached;
        }

        public virtual void Update(T model)
        {
            DbEntityEntry entryitem = ctx.Entry(model);
            if (entryitem.State == EntityState.Detached)
            {
                dataSet.Attach(model);
            }
            entryitem.State = EntityState.Modified;
            ctx.SaveChanges();
        }

        public virtual void Update(T model, Guid id)
        {
            DbEntityEntry entryitem = ctx.Entry(model);
            if (entryitem.State != EntityState.Detached) return;
            var original = dataSet.Find(id);
            if (original != null)
            {
                ctx.Entry(original).CurrentValues.SetValues(model);
            }
            else
            {
                dataSet.Attach(model);
                ctx.Entry(model).State = EntityState.Modified;
            }
        }

        public virtual void Delete(T model)
        {
            DbEntityEntry entryitem = ctx.Entry(model);
            if (entryitem.State != EntityState.Deleted)
            {
                entryitem.State = EntityState.Deleted;
            }
            else
            {
                dataSet.Attach(model);
                dataSet.Remove(model);
            }
        }

        public void DeleteFiltered(Expression<Func<T, bool>> filter)
        {
            var results = dataSet.Where(filter);
            foreach (var item in results)
            {
                DbEntityEntry entryitem = ctx.Entry(item);
                if (entryitem.State != EntityState.Deleted)
                {
                    entryitem.State = EntityState.Deleted;
                }
                else
                {
                    dataSet.Attach(item);
                    dataSet.Remove(item);
                }
            }
        }

        public virtual void Delete(Guid id)
        {
            var model = GetById(id);
            if (model != null)
            {
                Delete(model);
            }
        }

        public void Dispose()
        {
        }

        public virtual void AddOrUpdate(T model, Guid? id)
        {
            var existing = dataSet.Find(id);

            if (existing != null && id.HasValue)
            {
                Update(model, (Guid) id);
            }
            else
            {
                Add(model);
            }
        }

        public virtual void Delete(int id)
        {
            var model = GetById(id);
            if (model != null)
            {
                Delete(model);
            }
        }
    }
}