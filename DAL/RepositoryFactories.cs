using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DAL
{
    public class RepositoryFactories
    {
        public IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        public RepositoryFactories()
        {
            _repositoryFactories = new Dictionary<Type, Func<DbContext, object>>();
        }

        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }

        private IDictionary<Type, Func<DbContext, object>> GetEFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>();
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof (T), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        public Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new Repository<T>(dbContext);
        }
    }
}