using Monibyte.Arquitectura.Comun.Nucleo;

namespace Monibyte.Arquitectura.Presentacion
{
    public static class AppProperties
    {
        public static int SYSID { get; private set; }
        public static int SECURITY { get; private set; }
        public static string TRANSACURL { get; private set; }

        static AppProperties()
        {
            SYSID = Config.ReadProp<int>("SYSID");
            SECURITY = Config.ReadProp<int>("SECURITY");
            TRANSACURL = Config.ReadProp<string>("TRANSACURL");
        }
    }
}