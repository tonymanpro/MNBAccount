using Monibyte.Arquitectura.Web.Nucleo.Controlador;
using Resources;
using System.Web.Mvc;

namespace Monibyte.Arquitectura.Presentacion.Controllers
{
    [AllowAnonymous]
    public class ErrorController : ControladorBase
    {
        // GET: /Error/
        public ActionResult Index()
        {
            ViewBag.Message = RecErrores.Err_0000;
            return GetView();
        }

        public ActionResult NoAccess()
        {
            ViewBag.Message = RecErrores.Err_401;
            return GetView();
        }

        public ActionResult NotFound()
        {
            ViewBag.Message = RecErrores.Err_404;
            return GetView();
        }

        private ActionResult GetView()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("Error");
            }
            return View("Error");
        }
    }
}