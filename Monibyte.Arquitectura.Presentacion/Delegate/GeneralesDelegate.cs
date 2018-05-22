using Monibyte.Arquitectura.Comun.Nucleo.Rest;
using Monibyte.Arquitectura.Presentacion.Integracion.Dto;
using System.Collections.Generic;

namespace Monibyte.Arquitectura.Presentacion.Delegate
{
    public class GeneralesDelegate
    {
        public static IEnumerable<PocIdioma> ConsultarIdiomas()
        {
            var url = RestConfig.Get("ApiCore", "General", "ConsultarIdiomas");
            return RestClient.CoreRequest<IEnumerable<PocIdioma>>
                (url, restMethod: RestMethod.Get);
        }

        public static IEnumerable<PocPais> ConsultarPaises()
        {
            var url = RestConfig.Get("ApiCore", "General", "ConsultarPaises");
            return RestClient.CoreRequest<IEnumerable<PocPais>>
                (url, restMethod: RestMethod.Get);
        }
    }
}