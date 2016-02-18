using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Contracts;
using DAL;
using MediatR;
using WebApi_Autofac.Handlers.Validation;
using WebApi_Autofac.Infrastructure.Mediator;
using WebApi_Autofac.Infrastructure.Processes;

namespace WebApi_Autofac
{
    public class AutofacDependencyInjector
    {
        public static IContainer RegisterAutoFac()
        {
            var builder = new ContainerBuilder();
            //call to private methods
            AddWebApiBindings(builder);
            RegisterMediatR(builder);
            AddRegisterations(builder);
            DecorateHandlers(builder);
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

        /// <summary>
        ///     Register Web API Components
        /// </summary>
        /// <param name="builder"></param>
        private static void AddWebApiBindings(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule<AutofacWebTypesModule>();
        }

        /// <summary>
        ///     Register Generic Repository
        /// </summary>
        /// <param name="builder"></param>
        private static void AddRegisterations(ContainerBuilder builder)
        {
            builder.RegisterType<Uow>().As<IUow>();
            builder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            builder.RegisterType<RepositoryFactories>().As<RepositoryFactories>().SingleInstance();
        }

        /// <summary>
        ///     Mediator will builds out handler -->
        ///     push to container to do so -->
        ///     builds the stand request handler, of which is then surrounded with additional work
        /// </summary>
        /// <param name="builder"></param>
        private static void DecorateHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<Mediator>().AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            //register all pre handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof (IPreRequestHandler<>))))
                .InstancePerLifetimeScope();

            //register all post handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof (IPostRequestHandler<,>))))
                .InstancePerLifetimeScope();

            //register all handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof (MediatR.IRequestHandler<,>)))
                    .Select(interfaceType => new KeyedService("requestHandler", interfaceType)))
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(typeof (MediatorPipeline<,>), typeof (MediatR.IRequestHandler<,>),
                "requestHandler");
            builder.RegisterGenericDecorator(typeof (ValidationHandler<,>), typeof (MediatR.IRequestHandler<,>),
                "requestHandler");
        }

        /// <summary>
        ///     Register MediatR
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterMediatR(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof (IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>) c.Resolve(typeof (IEnumerable<>).MakeGenericType(t));
            });
        }
    }
}