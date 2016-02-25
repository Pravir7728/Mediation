using System.Web.Http;
using FluentValidation.WebApi;
using WebApi.Infrastructure;

namespace WebApi
{
    public class FluentValidationConfig
    {
        public static void RegisterValidation(HttpConfiguration config)
        {
            FluentValidationModelValidatorProvider.Configure(config,
                provider => provider.ValidatorFactory = new ModelValidatorFactory());
        }
    }
}