using Monibyte.Arquitectura.Comun.Nucleo;
using Monibyte.Arquitectura.Comun.Nucleo.Sesion;
using Monibyte.Arquitectura.Presentacion.Delegate;
using Monibyte.Arquitectura.Presentacion.Integracion;
using Monibyte.Arquitectura.Presentacion.Integracion.Dto;
using Monibyte.Arquitectura.Presentacion.Models;
using Monibyte.Arquitectura.Web.Nucleo.Controlador;
using Monibyte.Arquitectura.Web.Nucleo.Seguridad;
using Resources;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Monibyte.Arquitectura.Presentacion.Controllers
{
    [AllowAnonymous]
    public class AuthController : ControladorBase
    {
        public ActionResult Index()
        {
            return View("LoginEmbed");
        }

        [HttpPost, ExportModelStateToTempData]
        public ActionResult ValidarUsuario(ModLogin login)
        {
            Session.RemoveAll();
            login.CodSesion = SecurityProps.SessionCode;
            login.DireccionIpv4 = SecurityProps.Ipv4Address;
            PocInfoSesion user = null;
            try
            {
                user = SeguridadDelegate.ValidarUsuario(login);
            }
            catch (CoreException e)
            {
                if (login.IsEmbedded)
                {
                    e.Validation.Errors.ToList().ForEach(err =>
                    {
                        ModelState.AddModelError("", err.Message);
                    });
                    return RedirectToAction("Index", "Home");
                }
                throw e;
            }
            if (user.NotificaVencimiento)
            {
                if (user.FecVencePass.HasValue)
                {
                    var fecVencimiento = user.FecVencePass.Value;
                    var vencimiento = (fecVencimiento - DateTime.Today).TotalDays;
                    if (vencimiento <= 0)
                    {
                        ViewBag.Vencimiento = "VENCIMIENTO";
                        ViewBag.IsEmbedded = login.IsEmbedded;
                        var model = new ModCambioContrasena
                        {
                            CodUsuario = user.CodUsuario
                        };
                        if (login.IsEmbedded)
                        {
                            return View("_LogInRenew", model);
                        }
                        return PartialView("_LogInRenew", model);
                    }
                    if (login.IsEmbedded)
                    {
                        ViewBag.Vencimiento = vencimiento;
                        return View("_LogInRenewPrompt");
                    }
                    return Json(new
                    {
                        vencimiento = vencimiento
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            
            if (AppProperties.SECURITY == (int)EnumIdEstado.Activo)
            {
                if (user.OtpActivo)
                {
                    if (login.IsEmbedded)
                    {
                        return View("_AutenticarOTP");
                    }
                    return PartialView("_AutenticarOTP");
                }
                else if (user.IdEstado == (int)EnumEstadoUsuario.Activo &&
                    user.RequiereOtp && !user.OtpActivo)
                {
                    ViewBag.Action = "validaOTP";
                    var model = SeguridadDelegate.GenerarCodigo(user.CodUsuario);
                    SeguridadDelegate.UsuarioTemporal = user;
                    if (login.IsEmbedded)
                    {
                        return View("_CrearClaveAut", model);
                    }
                    return PartialView("_CrearClaveAut", model);
                }
            }
            return ProcesaRespuestaAuth(false, login.IsEmbedded);
        }

        [HttpPost]
        public ActionResult validaOTP(string pin, bool? isEmbedded)
        {
            var user = SeguridadDelegate.UsuarioTemporal;
            bool isCorrectPIN = SeguridadDelegate.ValidarPinAutenticacion(user.CodUsuario, pin);
            if (isCorrectPIN)
            {
                return ProcesaRespuestaAuth(false, isEmbedded);
            }
            else
            {
                throw new ControllerException(RecMensajes.Msj_PinInvalido);
            }
        }

        [HttpPost]
        public ActionResult ProcesaRespuestaAuth(bool? changePassword, bool? isEmbedded)
        {
            var user = SeguridadDelegate.UsuarioTemporal;
            if (changePassword.HasValue && changePassword.Value)
            {
                return PartialView("_LogInRenew", new ModCambioContrasena
                {
                    CodUsuario = user.CodUsuario
                });
            }
            try
            {
                if (user.IdEstado == (int)EnumEstadoUsuario.Registrado)
                {
                    return RedirectToAction("Index", "Registro");
                }
                //El usuario es valido y puede ingresar
                user.Ticket = SeguridadDelegate.IniciarSesion(user.CodUsuario, user.Ticket);
                var cookie = FormsAuthentication.GetAuthCookie(user.Ticket, false);
                //Decrypt the cookie
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                //Create a new ticket using the details from 
                //the generated cookie, but store the username &
                //token passed in from the authentication method
                var newticket = new FormsAuthenticationTicket(ticket.Version,
                    ticket.Name, ticket.IssueDate, ticket.Expiration,
                    ticket.IsPersistent, user.CodUsuario);
                //Encrypt the ticket & store in the cookie
                cookie.Value = FormsAuthentication.Encrypt(newticket);
                //Update the outgoing cookies collection.
                Response.Cookies.Set(cookie);
                Session.RemoveAll();
                var url = string.Format(AppProperties.TRANSACURL, user.Locale, user.Ticket);

                string lang = string.Empty;
                var idiomas = GeneralesDelegate.ConsultarIdiomas();
                foreach (var i in idiomas)
                {
                    if (i.IdIdioma == user.IdIdioma)
                    {
                        Response.Cookies.Remove(Config.LANG_COOKIE_NAME);

                        lang = i.Abreviatura;
                        var langCookie = new HttpCookie(Config.LANG_COOKIE_NAME, lang) { HttpOnly = true };
                        Response.AppendCookie(langCookie);
                    }
                }

                if (isEmbedded == true)
                {
                    return Redirect(url);
                }

                return Json(new
                {
                    redireccionar = url
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Session.RemoveAll();
                throw new ControllerException(RecErrores.Err_LogFallido);
            }
        }

        [HttpPost]
        public ActionResult ActualizaSeguridad(ModCambioContrasena modelo)
        {
            MembresiaDelegate.ActualizarContrasena(modelo);
            return Json(new { esvalido = true });
        }
    }
}