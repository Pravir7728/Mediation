using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Contracts;
using DAL;

namespace WebApi.App_Start
{
    public class ContextConfig
    {
        public static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<Uow>().As<IUow>();
            builder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            builder.RegisterType<RepositoryFactories>().As<RepositoryFactories>().SingleInstance();
            builder.RegisterType<AutofacServiceLocator>().AsImplementedInterfaces();
        }
    }
}