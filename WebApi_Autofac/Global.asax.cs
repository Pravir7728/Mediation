using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using WebApi_Autofac.App_Start;

namespace WebApi_Autofac
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FluentValidationModelValidatorProvider.Configure();
            AutofacDependencyInjector.RegisterAutoFac();
            AutoMapperConfiguration.RegisterMapping();
        }
    }
}