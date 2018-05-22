using Monibyte.Arquitectura.Comun.Nucleo.Extensiones;
using Monibyte.Arquitectura.Comun.Nucleo.Rest;
using Monibyte.Arquitectura.Presentacion.Delegate;
using Monibyte.Arquitectura.Web.Nucleo.Controlador;

namespace Monibyte.Arquitectura.Presentacion
{
    public class AppSessionWriter : SessionWriter
    {
        public override void AddHeaders(System.Net.WebHeaderCollection headers)
        {
            headers.Add("LANG", LanguageFilter.Current.ToJson().CompressStr());
            headers.Add("IDSISTEMA", AppProperties.SYSID.ToJson().CompressStr());
            headers.Add("SEGURIDADACTIVA", AppProperties.SECURITY.ToJson().CompressStr());

            var tempUser = SeguridadDelegate.UsuarioTemporal;
            if (tempUser != null)
            {
                if (headers["INFO"] == null)
                {
                    headers.Add("INFO", tempUser
                        .ToJson().CompressStr());
                }
            }
        }
    }
}