using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using Autofac.Integration.WebApi;
using MediatR;
using Owin;
using WebApi.Infrastructure.Handlers.Validation;
using WebApi.Infrastructure.Mediator;
using WebApi.Infrastructure.Processes;

namespace WebApi
{
    public class AutofacConfig
    {
        public static void RegisterAutofacIoc(IAppBuilder app, HttpConfiguration config, ContainerBuilder builder)
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

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("ValidationHandler"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                typeof (ValidatorHandler<,>),
                typeof (IAsyncRequestHandler<,>),
                "asyncMediatorPipeline")
                .InstancePerLifetimeScope();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
        }
    }
}