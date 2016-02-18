using System.Web.Mvc;

namespace WebApi_Autofac
{
    public class FiltersConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}