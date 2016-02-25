using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using Contracts;
using DAL;
using MediatR;
using Owin;
using WebApi.Handlers.Validation;
using WebApi.Infrastructure.Mediator;
using WebApi.Infrastructure.Modules;
using WebApi.Infrastructure.Processes;

namespace WebApi
{
    public class AutofacConfig
    {
        public static void ConfigureDependencyInjection(IAppBuilder app, HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

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
                return t => (IEnumerable<object>) c.Resolve(typeof (IEnumerable<>).MakeGenericType(t));
            });

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof (IAsyncPreRequestHandler<>))));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof (IAsyncPostRequestHandler<,>))));

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof (IAsyncRequestHandler<,>)))
                    .Select(interfaceType => new KeyedService("asyncRequestHandler", interfaceType)));

            builder.RegisterGenericDecorator(typeof (AsyncMediatorPipeline<,>), typeof (IAsyncRequestHandler<,>),
                "asyncRequestHandler");

            builder.RegisterGenericDecorator(
                typeof (ValidatorHandler<,>),
                typeof (IAsyncRequestHandler<,>),
                "asyncMediatorPipeline")
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ValidationHandler"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterType<Uow>().As<IUow>();
            builder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            builder.RegisterType<RepositoryFactories>().As<RepositoryFactories>().SingleInstance();
            builder.RegisterType<AutofacServiceLocator>().AsImplementedInterfaces();

            builder.RegisterModule(new LoggingConfig());

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
        }
    }
}