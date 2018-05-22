using Monibyte.Arquitectura.Web.Nucleo.Recursos;
using Resources;
using System.ComponentModel.DataAnnotations;

namespace Monibyte.Arquitectura.Presentacion.Models
{
    public class ModCorreo
    {
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_CorreoRequerido")]
        [Display(Name = "Correo")]
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+((\\.([a-zA-Z0-9])+){1,4})$",
            ErrorMessageResourceType = typeof(RecErrores),
            ErrorMessageResourceName = "Err_FormatoCorreo")]
        public string Correo { get; set; }
    }

    public partial class ModUsuario : ModCorreo
    {
        public int IdUsuario { get; set; }
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_Pais")]
        public string Pais { get; set; }
        public string Idioma { get; set; }

        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_AliasUsuarioRequerido")]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_AliasUsuario")]
        [RegularExpression(@"^\S+(?:\s+\S+)*$")]
        public string AliasUsuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_PaisRequerido")]
        public int IdPaisPreferencia { get; set; }

        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_IdiomaRequerido")]
        public int IdIdiomaPreferencia { get; set; }
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_RequiereOTPRequerido")]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_RequieresOTP")]
        public int IdRequiereOTP { get; set; }
        public string DobleFactorAut { get; set; }
    }

    public class ModDobleFactorAut
    {
        public string DobleFactorAut { get; set; }
        public string ManualEntrySetupCode { get; set; }
        public string QRCodeImageUrl { get; set; }
    }
}