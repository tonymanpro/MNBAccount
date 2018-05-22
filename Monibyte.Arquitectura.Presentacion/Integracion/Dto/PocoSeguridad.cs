
namespace Monibyte.Arquitectura.Presentacion.Integracion.Dto
{
    public class PocAccion
    {
        public int IdAccion { get; set; }
        public string Descripcion { get; set; }
        public string Controlador { get; set; }
        public string Area { get; set; }
        public int IdEstado { get; set; }
    }
    public class PocAccionMenu
    {
        public int? IdAccion { get; set; }
        public int? IdMenu { get; set; }
        public int? IdMenuPadre { get; set; }
        public string Area { get; set; }
        public string Controlador { get; set; }
        public string Descripcion { get; set; }
        public string DescMenu { get; set; }
        public int AyudaVigente { get; set; }
    }

    public class PocPregunta
    {
        public int IdPregunta { get; set; }
        public string Descripcion { get; set; }
    }

    public class PocInfoAutorizacion
    {
        public string DescripcionProducto { get; set; }
        public string DetalleAutorizacion { get; set; }
    }
}
