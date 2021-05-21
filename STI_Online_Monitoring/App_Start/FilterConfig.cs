using System.Web;
using System.Web.Mvc;

namespace STI_Online_Monitoring
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
