using System.Reflection;
using System.Web.Http;
using System.Web.Http.Validation;
using Autofac;
using FluentValidation;
using FluentValidation.WebApi;
using WebApi.Infrastructure.Factories;

namespace WebApi
{
    public class FluentValidationConfig
    {
        public static void RegisterValidation(ContainerBuilder builder, HttpConfiguration config)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();

            builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }
    }
}