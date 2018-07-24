using System.Web.Mvc;

namespace ACCDataStore.Helpers.ORM
{
    public class TransactionalAttribute3nd : ActionFilterAttribute
    {
        private IUnitOfWork3nd _unitOfWork3nd;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.HttpContext.Error == null)
            {
                _unitOfWork3nd = DependencyResolver.Current.GetService<IUnitOfWork3nd>();
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.Exception == null && filterContext.HttpContext.Error == null && _unitOfWork3nd != null)
            {
               // _unitOfWork3nd.SaveChanges();
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
