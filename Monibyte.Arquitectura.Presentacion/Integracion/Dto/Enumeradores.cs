
namespace Monibyte.Arquitectura.Presentacion.Integracion.Dto
{
    public enum EnumEstadoUsuario
    {
        Activo = 94,
        Inactivo = 95,
        Registrado = 96
    }

    public enum EnumIdSiNo
    {
        Si = 5,
        No = 6,
    }

    public enum EnumIdEstado
    {
        Activo = 1,
        Inactivo = 0,
    }

    public enum EnumTipoUsuario
    {
        Adicional = 413,
        SubAdministrador = 414,
        Administrador = 415,
        //Internos
        SuperUsuario = 416,
        JefeDeOficina = 417,
        UsuarioAdministrativo = 418,
        SoloConsulta = 419
    }
}