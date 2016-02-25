using System.Web.Http;
using log4net.Config;
using Microsoft.Owin;
using Owin;
using WebApi.App_Start;

[assembly: OwinStartup(typeof (Startup))]
[assembly: XmlConfigurator(Watch = true)]

namespace WebApi.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            AutofacConfig.ConfigureDependencyInjection(app, config);
            AutoMapperConfig.RegisterMapping();
            WebApiConfig.Register(config);
            FluentValidationConfig.RegisterValidation(config);
            XmlConfigurator.Configure();

            app.UseWebApi(config);
        }
    }
}