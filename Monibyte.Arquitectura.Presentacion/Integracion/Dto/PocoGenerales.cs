namespace Monibyte.Arquitectura.Presentacion.Integracion.Dto
{
    public class PocPais
    {
        public int IdPais { get; set; }
        public int IdRegion { get; set; }
        public string Descripcion { get; set; }
    }

    public class PocIdioma
    {
        public int IdIdioma { get; set; }
        public string Idioma { get; set; }
        public int IdEstado { get; set; }
        public string Abreviatura { get; set; }
        public string CodCultura { get; set; }
    }
}