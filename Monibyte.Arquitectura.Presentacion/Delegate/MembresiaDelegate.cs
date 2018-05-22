using Monibyte.Arquitectura.Comun.Nucleo.Encryption;
using Monibyte.Arquitectura.Comun.Nucleo.Extensiones;
using Monibyte.Arquitectura.Comun.Nucleo.Rest;
using Monibyte.Arquitectura.Presentacion.Integracion.Dto;
using Monibyte.Arquitectura.Presentacion.Models;
using System.Collections.Generic;

namespace Monibyte.Arquitectura.Presentacion.Delegate
{
    public class MembresiaDelegate
    {
        public static List<PocPregunta> ObtenerPreguntas()
        {
            var url = RestConfig.Get("ApiCore", "Membresia", "ObtenerPreguntas");
            return RestClient.CoreRequest<List<PocPregunta>>
                (url, restMethod: RestMethod.Get);
        }

        public static void RegistrarPerfil(ModUsuarioRegistro registro)
        {
            var url = RestConfig.Get("ApiCore", "Membresia", "RegistrarPerfil");
            RestClient.CoreRequest(url, registro);
        }

        public static ModRenovarContrasenaPaso2 ValidarRenovacion(string username, string email)
        {
            var url = RestConfig.Get("ApiCore", "Membresia", "ValidarRenovacion");
            return RestClient.CoreRequest<ModRenovarContrasenaPaso2>(url, new
            {
                AliasUsuario = username,
                Correo = email
            });
        }

        public static bool ComprobarPreguntasUsuario(string codUsuario,
            List<ModPreguntaUsuario> preguntas)
        {
            preguntas.Update(m =>
            {
                m.CodUsuario = codUsuario;
                m.Respuesta = m.Respuesta.ToLower().EncryptPBKDF2();
            });
            var url = RestConfig.Get("ApiCore", "Membresia", "ComprobarPreguntasUsuario");
            return RestClient.CoreRequest<bool>(url, preguntas);
        }

        public static void ActualizarContrasena(ModCambioContrasena cambio)
        {
            cambio.Contrasena = cambio.Contrasena.Encrypt();
            cambio.ContrasenaActual = cambio.ContrasenaActual.Encrypt();
            cambio.ConfirmarContrasena = cambio.ConfirmarContrasena.Encrypt();
            var url = RestConfig.Get("ApiCore", "Membresia", "ActualizarContrasena");
            RestClient.CoreRequest(url, cambio);
        }
        public static string ObtenerClaveAut()
        {
            var url = RestConfig.Get("ApiCore", "Membresia", "ObtenerClaveAut");
            return RestClient.CoreRequest<string>(url);
        }
    }
}