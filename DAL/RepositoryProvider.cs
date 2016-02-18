using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace DAL
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly RepositoryFactories _repositoryFactories;

        public RepositoryProvider(RepositoryFactories repositoryFactories)
        {
            _repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }

        protected Dictionary<Type, object> Repositories { get; set; }
        public DbContext DbContext { get; set; }

        public IRepository<T> GetRepositoryForEntityType<T>() where T : class
        {
            return GetRepository<IRepository<T>>(
                _repositoryFactories.GetRepositoryFactoryForEntityType<T>());
        }

        public T GetRepository<T>(Func<DbContext, object> factory = null) where T : class
        {
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);
            if (repoObj != null)
            {
                return (T)repoObj;
            }

            return MakeRepository<T>(factory, DbContext);
        }

        public void SetRepository<T>(T repository)
        {
            throw new NotImplementedException();
        }

        protected virtual T MakeRepository<T>(Func<DbContext, object> factory, DbContext dbContext)
        {
            var f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();
            if (f == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }
            var repo = (T)f(dbContext);
            Repositories[typeof(T)] = repo;
            return repo;
        }
    }
}
