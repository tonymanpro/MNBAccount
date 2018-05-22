using Monibyte.Arquitectura.Presentacion.Delegate;
using Monibyte.Arquitectura.Web.Nucleo.Seguridad;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Monibyte.Arquitectura.Presentacion
{
    public class AuthTicketFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (SecurityProps.IsAuthenticated)
            {
                try
                {
                    var ticket = SecurityProps.AuthTicket.Name;
                    var user = SeguridadDelegate.ObtenerUsuarioPorTicket(ticket);
                    var req = filterContext.HttpContext.Request;
                    var isEmbedded = Boolean.TrueString
                        .Equals(req.QueryString["embed"],
                        StringComparison.CurrentCultureIgnoreCase);
                    if (!isEmbedded)
                    {
                        var url = string.Format(AppProperties.TRANSACURL, user.Locale, user.Ticket);
                        filterContext.Result = new RedirectResult(url);
                    }
                }
                catch (Exception)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
    }
}