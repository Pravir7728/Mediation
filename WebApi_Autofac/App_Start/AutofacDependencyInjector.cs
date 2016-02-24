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
            RegisterMediatorAndDecorateHandlers(builder);
            AddRegisterations(builder);
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
        private static void RegisterMediatorAndDecorateHandlers(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof (IMediator).Assembly).AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>) c.Resolve(
                    typeof (IEnumerable<>).MakeGenericType(t));
            });

            const string handlerKey = "requestHandler";
            const string pipeLineKey = "pipeLineKey";


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof (IPreRequestHandler<>));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof (IPostRequestHandler<,>));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(t => t.IsClosedTypeOf(typeof (Infrastructure.Processes.IRequestHandler<,>)))
                    .Select(t => new KeyedService(handlerKey, t)));

            builder.RegisterGenericDecorator(typeof (MediatorPipeline<,>), typeof (Infrastructure.Processes.IRequestHandler<,>),
                handlerKey, pipeLineKey);

            //builder.RegisterGenericDecorator(typeof (ValidationHandler<,>), typeof (MediatR.IRequestHandler<,>),
            //    pipeLineKey);
        }
    }
}