using Google.Authenticator;
using Monibyte.Arquitectura.Comun.Nucleo.Encryption;
using Monibyte.Arquitectura.Comun.Nucleo.Extensiones;
using Monibyte.Arquitectura.Presentacion.Delegate;
using Monibyte.Arquitectura.Presentacion.Integracion.Dto;
using Monibyte.Arquitectura.Presentacion.Models;
using Monibyte.Arquitectura.Web.Nucleo.Controlador;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Monibyte.Arquitectura.Presentacion.Controllers
{
    [AllowAnonymous]
    public class RegistroController : ControladorBase
    {
        //[AjaxOnly]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(string pin)
        {
            var user = SeguridadDelegate.UsuarioTemporal;
            if ((user.RequiereOtp && !user.OtpActivo) &&
                string.IsNullOrEmpty(pin))
            {
                var model = SeguridadDelegate.GenerarCodigo(user.CodUsuario);
                SeguridadDelegate.UsuarioTemporal = user;
                ViewBag.Action = "Index";
                return PartialView("_CrearClaveAut", model);
            }
            if (!string.IsNullOrEmpty(pin))
            {
                var isCorrectPIN = SeguridadDelegate.ValidarPinAutenticacion(user.CodUsuario, pin);
                if (!isCorrectPIN)
                {
                    throw new ControllerException(RecMensajes.Msj_PinInvalido);
                }
            }
            var paises = GeneralesDelegate.ConsultarPaises();
            ViewBag.ListaPaises = paises;
            var idiomas = GeneralesDelegate.ConsultarIdiomas();
            ViewBag.ListaIdiomas = idiomas;
            var preguntas = MembresiaDelegate.ObtenerPreguntas();
            ViewBag.CollectionPreguntas = preguntas;
            return PartialView("_Registro", new ModUsuarioRegistro
            {
                ListaPreguntas = new List<ModPreguntaUsuario> {
                    new ModPreguntaUsuario(), new ModPreguntaUsuario(),
                    new ModPreguntaUsuario(), new ModPreguntaUsuario(),
                    new ModPreguntaUsuario()
                }
            });
        }

        [HttpPost]
        public ActionResult Registrar(ModUsuarioRegistro modelo)
        {
            if (modelo.ListaPreguntas == null || modelo.ListaPreguntas.Any(x => x.IdPregunta == 0))
            {
                throw new ControllerException(RecErrores.Err_UnaMasPreguntasInvalidas);
            }
            var distinct = modelo.ListaPreguntas.DistinctBy(m => m.IdPregunta).Count();
            if (distinct != modelo.ListaPreguntas.Count)
            {
                throw new ControllerException(RecErrores.Err_RegistrarPreguntas);
            }
            var usuarioTmp = SeguridadDelegate.UsuarioTemporal;
            modelo.IdUsuario = usuarioTmp.IdUsuario;
            modelo.ListaPreguntas.Update(m =>
            {
                m.CodUsuario = usuarioTmp.CodUsuario;
                m.Respuesta = m.Respuesta.ToLower().EncryptPBKDF2();
            });
            MembresiaDelegate.RegistrarPerfil(modelo);
            usuarioTmp.IdEstado = (int)EnumEstadoUsuario.Activo;
            SeguridadDelegate.UsuarioTemporal = usuarioTmp;
            return Json(new { esvalido = true });
        }

        [HttpGet]
        public ActionResult Renovar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RenovarValidaPaso1(ModRenovarContrasenaPaso1 modelo)
        {
            try
            {
                var renovacion = MembresiaDelegate.ValidarRenovacion(
                    modelo.AliasUsuario, modelo.Correo);
                return PartialView("_RenovarPaso2", renovacion);
            }
            catch (Exception)
            {
                throw new ControllerException(RecErrores.Err_RenovarContrasenaPaso1);
            }
        }

        [HttpPost]
        public ActionResult RenovarValidaPaso2(ModRenovarContrasenaPaso2 modelo)
        {
            var preguntas = modelo.ListaPreguntas;
            if (MembresiaDelegate.ComprobarPreguntasUsuario(modelo.CodUsuario, preguntas))
            {
                return PartialView("_RenovarPaso3", new ModCambioContrasena
                {
                    CodUsuario = modelo.CodUsuario
                });
            }
            throw new ControllerException(RecErrores.Err_RenovarContrasenaPaso2);
        }

        [HttpPost]
        public ActionResult RenovarValidaPaso3(ModCambioContrasena modelo)
        {
            MembresiaDelegate.ActualizarContrasena(modelo);
            return Json(new { esvalido = true });
        }
    }
}
