using Monibyte.Arquitectura.Web.Nucleo.Controlador;
using System.Web.Mvc;

namespace Monibyte.Arquitectura.Presentacion
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthTicketFilter());
            filters.Add(new JsonNetActionFilter());
            filters.Add(new ManejadorError());
        }
    }
}