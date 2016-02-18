using Contracts;
using Domain;

namespace DAL
{
    public class Uow : IUow
    {
        public Uow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();
            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        private MediationEntities DbContext { get; set; }
        protected IRepositoryProvider RepositoryProvider { get; set; }

        public IRepository<object> Generic
        {
            get { return GetStandardRepo<object>(); }
        }

        public IRepository<User> Users
        {
            get { return GetStandardRepo<User>(); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public void DisableLazyLoadingandProxyCreation()
        {
            DbContext.Configuration.ProxyCreationEnabled = false;
            DbContext.Configuration.LazyLoadingEnabled = false;
        }

        protected void CreateDbContext()
        {
            DbContext = new MediationEntities();
        }

        public IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }
    }
}