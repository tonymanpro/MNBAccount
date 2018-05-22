namespace Monibyte.Arquitectura.Presentacion.Integracion
{
    using System.Web.Mvc;

    public abstract class ModelStateTemp : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTemp).FullName;
    }

    public class ExportModelStateToTempData : ModelStateTemp
    {
        #region Public Methods and Operators

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }

        #endregion
    }

    public class ImportModelStateFromTempData : ModelStateTemp
    {
        #region Public Methods and Operators

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }

        #endregion
    }
}