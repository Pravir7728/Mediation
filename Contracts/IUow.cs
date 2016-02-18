using System;
using Domain;

namespace Contracts
{
    public interface IUow
    {
        IRepository<User> Users { get; }
        IRepository<Object> Generic { get; }
        void Commit();
        void DisableLazyLoadingandProxyCreation();
    }
}