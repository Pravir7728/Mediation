using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Contracts
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<TF> GetAllOfType<TF>();
        IQueryable<T> GetAll();
        IEnumerable<T> GetAllEnu();
        IQueryable<T> GetAllFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters);
        IQueryable<T> GetAllFilteredNoTrack(List<string> includes, params Expression<Func<T, bool>>[] filters);
        T GetByFilteredNoTrack(List<string> includes, params Expression<Func<T, bool>>[] filters);
        T GetByFiltered(List<string> includes, params Expression<Func<T, bool>>[] filters);
        T GetById(Guid id);
        T GetById(int id);
        void SetProxy(bool value);
        void Add(T model);
        void Detach(T model);
        void Update(T model);
        void Update(T model, Guid id);
        void Delete(T model);
        void DeleteFiltered(Expression<Func<T, bool>> filter);
        void Delete(Guid id);
        void AddOrUpdate(T model, Guid? id);
    }
}