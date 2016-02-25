using Autofac;
using WebApi.Infrastructure.Modules;

namespace WebApi.App_Start
{
    public class Log4NetConfig
    {
        public static void RegisterLogger(ContainerBuilder builder)
        {
            builder.RegisterModule(new LoggingModule());
        }
    }
}