using System.Web.Optimization;

namespace Monibyte.Arquitectura.Presentacion
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/scripts/jquery-{version}.js",
                        "~/scripts/jquery.unobtrusive*",
                        "~/scripts/kendo.custom.min.js",
                        "~/scripts/jquery.blockui.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/scripts/bootstrap.js",
                      "~/scripts/respond.js"));

            // bundles comunes
            var jqueryBundle = new ScriptBundle("~/bundles/monibyte").Include(
                "~/scripts/monibyte.utiles.comun.js",
                "~/scripts/monibyte.utiles.jquery.ui.js");
            jqueryBundle.Transforms.Add(new JsMinify());
            bundles.Add(jqueryBundle);

            //Estilos monibyte
            var monibyteCssBundle = new StyleBundle("~/content/css").Include(
                "~/content/kendoui/kendo.common.min.css",
                "~/content/kendoui/kendo.default.min.css",
                "~/content/bootstrap.css",
                "~/content/monibyte.css",
                "~/content/monibyte.formularios.css",
                "~/content/monibyte.externo.css",
                "~/content/monibyte.responsive.css");
            monibyteCssBundle.Transforms.Add(new CssMinify());
            bundles.Add(monibyteCssBundle);
        }
    }
}