using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.ModelBinding;
using Autofac;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using Contracts;
using DAL;
using FluentValidation;
using FluentValidation.WebApi;
using MediatR;
using Microsoft.Owin;
using Owin;
using WebApi.App_Start;
using WebApi.Features;
using WebApi.Handlers;
using WebApi.Infrastructure;
using WebApi.Infrastructure.Mediator;
using WebApi.Infrastructure.Processes;

[assembly: OwinStartup(typeof (Startup))]

namespace WebApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            ConfigureDependencyInjection(app, config);
            AutoMapperConfig.RegisterMapping();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            FluentValidationModelValidatorProvider.Configure(config,provider => provider.ValidatorFactory = new ModelValidatorFactory());

        }

        private static void ConfigureDependencyInjection(IAppBuilder app, HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new ContravariantRegistrationSource());

            builder.RegisterAssemblyTypes(typeof (IMediator).Assembly)
                .AsImplementedInterfaces();


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

            //register all pre handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof (IAsyncPreRequestHandler<>))));

            //register all post handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfacetype => interfacetype.IsClosedTypeOf(typeof (IAsyncPostRequestHandler<,>))));


            //register all handlers
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .As(type => type.GetInterfaces()
                    .Where(interfaceType => interfaceType.IsClosedTypeOf(typeof (IAsyncRequestHandler<,>)))
                    .Select(interfaceType => new KeyedService("asyncRequestHandler", interfaceType)));


            //register pipeline decorator
            builder.RegisterGenericDecorator(typeof (AsyncMediatorPipeline<,>), typeof (IAsyncRequestHandler<,>),
                "asyncRequestHandler");

            //register validator decorator
            builder.RegisterGenericDecorator(
                typeof (ValidatorHandler<,>),
                typeof (IAsyncRequestHandler<,>),
                "asyncMediatorPipeline")
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ValidationHandler"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register Web API controller in executing assembly.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterType<Uow>().As<IUow>();
            builder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            builder.RegisterType<RepositoryFactories>().As<RepositoryFactories>().SingleInstance();
            builder.RegisterType<AutofacServiceLocator>().AsImplementedInterfaces();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // This should be the first middleware added to the IAppBuilder.
            app.UseAutofacMiddleware(container);

            // Make sure the Autofac lifetime scope is passed to Web API.
            app.UseAutofacWebApi(config);
        }
    }
}