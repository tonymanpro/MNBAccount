using Monibyte.Arquitectura.Comun.Nucleo;
using Monibyte.Arquitectura.Presentacion.Delegate;
using Monibyte.Arquitectura.Presentacion.Integracion;
using Monibyte.Arquitectura.Presentacion.Integracion.Dto;
using Monibyte.Arquitectura.Web.Nucleo.Controlador;
using System.Web;
using System.Web.Mvc;

namespace Monibyte.Arquitectura.Presentacion.Controllers
{
    [AllowAnonymous]
    public class HomeController : ControladorBase
    {
        [ImportModelStateFromTempData]
        public ActionResult Index()
        {
            var user = SeguridadDelegate.UsuarioTemporal;
            if (user != null && user.IdEstado == (int)EnumEstadoUsuario.Registrado)
            {
                return RedirectToAction("Index", "Registro");
            }
            return View();
        }

        [Route("translate-{lang}")]
        public ActionResult Translate(string lang, string returnUrl)
        {
            var langCookie = new HttpCookie(Config.LANG_COOKIE_NAME, lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);
            return Redirect(returnUrl ?? "/");
        }
    }
}
