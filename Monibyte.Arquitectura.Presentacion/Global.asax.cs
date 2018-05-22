using Monibyte.Arquitectura.Comun.Nucleo.Rest;
using Monibyte.Arquitectura.Web.Nucleo.Controlador;
using Monibyte.Arquitectura.Web.Nucleo.Controlador.Binder;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Monibyte.Arquitectura.Presentacion
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ViewEngines.Engines.Add(new CustomRazorViewEngine());

            ModelBinders.Binders.Add(typeof(int), new IntModelBinder());
            ModelBinders.Binders.Add(typeof(int?), new IntModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());

            RestClient.SetSessionWriter(new AppSessionWriter());
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                LanguageFilter.Filter(HttpContext.Current);
            }
        }
    }
}