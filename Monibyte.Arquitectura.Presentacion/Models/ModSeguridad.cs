using Monibyte.Arquitectura.Web.Nucleo.Recursos;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monibyte.Arquitectura.Presentacion.Models
{
    public class ModLogin
    {
        public bool IsEmbedded { get; set; }
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_AliasUsuarioRequerido")]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_AliasUsuario")]
        public string Username { get; set; }
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_ContrasenaRequerido")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8)]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_Contrasena")]
        public string Password { get; set; }
        public string CodSesion { get; set; }
        public string DireccionIpv4 { get; set; }
    }

    public class ModToken
    {
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_OtpRequerido")]
        [StringLength(6, MinimumLength = 6)]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_OTP")]
        public string Otp { get; set; }
    }

    public class ModCambioContrasena
    {
        public string CodUsuario { get; set; }

        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_ContrasenaRequerido")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8)]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_ContrasenaActual")]
        public string ContrasenaActual { get; set; }

        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_ContrasenaRequerido")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8)]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_NuevaContrasena")]
        [RegularExpression(@"^(?=(?:.*?[A-Za-z]){4})(?=(?:.*?[0-9]){2})([A-Za-z0-9~@#$^*()_+=[\]{}|\\',/¡!.?: -]){8,16}$")]
        public string Contrasena { get; set; }

        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_ConfirmarRequerido")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8)]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_Confirmar")]
        [RegularExpression(@"^(?=(?:.*?[A-Za-z]){4})(?=(?:.*?[0-9]){2})([A-Za-z0-9~@#$^*()_+=[\]{}|\\',/¡!.?: -]){8,16}$")]
        [Compare("Contrasena")]
        public string ConfirmarContrasena { get; set; }
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_Requerido")]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_VigenciaContrasena")]
        public int DiasVigencia { get; set; }
    }

    public class ModUsuarioRegistro : ModUsuario
    {
        /// Preguntas
        public List<ModPreguntaUsuario> ListaPreguntas { get; set; }
    }

    public class ModRenovarContrasenaPaso1
    {
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_AliasUsuarioRequerido")]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_AliasUsuario")]
        public string AliasUsuario { get; set; }
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_CorreoRequerido")]
        [Display(Name = "Mod_Correo")]
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.([a-zA-Z0-9-.]{2,4})+$",
            ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_FormatoCorreo")]
        public string Correo { get; set; }
    }

    public class ModRenovarContrasenaPaso2
    {
        public int IdUsuario { get; set; }
        public string CodUsuario { get; set; }
        public List<ModPreguntaUsuario> ListaPreguntas { get; set; }
    }

    public class ModPreguntaUsuario
    {
        [Required]
        public int IdUsuario { get; set; }
        public string CodUsuario { get; set; }
        [Required]
        public int IdPregunta { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessageResourceType = typeof(RecErrores), ErrorMessageResourceName = "Err_RespuestaRequerido")]
        [DisplayGlobalizado(typeof(RecEtiquetas), "Mod_Respuesta")]
        public string Respuesta { get; set; }
    }
}
