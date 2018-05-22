using Monibyte.Arquitectura.Comun.Nucleo.Encryption;
using Monibyte.Arquitectura.Comun.Nucleo.Rest;
using Monibyte.Arquitectura.Comun.Nucleo.Sesion;
using Monibyte.Arquitectura.Presentacion.Models;

namespace Monibyte.Arquitectura.Presentacion.Delegate
{
    public static class SeguridadDelegate
    {

        public static PocInfoSesion UsuarioTemporal
        {
            set { InfoSesion.IncluirSesion("USRTEMP", value); }
            get { return InfoSesion.ObtenerSesion<PocInfoSesion>("USRTEMP"); }
        }

        public static PocInfoSesion ValidarUsuario(ModLogin login)
        {
            login.Password = login.Password.Encrypt();
            var url = RestConfig.Get("ApiCore", "Auth", "ValidarUsuario");
            var user = RestClient.CoreRequest<PocInfoSesion>(url, login);
            SeguridadDelegate.UsuarioTemporal = user;
            return user;
        }

        public static string IniciarSesion(string user, string ticket)
        {
            var url = RestConfig.Get("ApiCore", "Auth", "IniciarSesion");
            return RestClient.CoreRequest<string>(url, new
            {
                user = user,
                ticket = ticket
            });
        }

        public static PocInfoSesion ObtenerUsuarioPorTicket(string ticket)
        {
            var url = RestConfig.Get("ApiCore", "Auth", "ObtenerUsuarioTicket");
            return RestClient.CoreRequest<PocInfoSesion>(url, ticket);
        }

        public static void FinalizarSesion(string user, string ticket)
        {
            var url = RestConfig.Get("ApiCore", "Auth", "FinalizarSesion");
            RestClient.CoreRequest(url, new { user = user, ticket = ticket });
        }

        public static ModDobleFactorAut GenerarCodigo(string codUsuario)
        {
            var url = RestConfig.Get("ApiCore", "Auth", "GenerarCodigo");
            return RestClient.CoreRequest<ModDobleFactorAut>(url, codUsuario);
        }

        public static bool ValidarPinAutenticacion(string codUsuario, string codigo2FA)
        {
            var url = RestConfig.Get("ApiCore", "Auth", "ValidarPinAutenticacion");
            return RestClient.CoreRequest<bool>(url,
                new
                {
                    codUsuario = codUsuario,
                    codigo2FA = codigo2FA
                });
        }
    }
}