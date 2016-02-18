using System.Web.Mvc;

namespace WebApi_Autofac.App_Start
{
    public class FiltersConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}