using System.Web;
using System.Web.Mvc;

namespace CONTROL_HERRAMIENTA_SUNCORP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
