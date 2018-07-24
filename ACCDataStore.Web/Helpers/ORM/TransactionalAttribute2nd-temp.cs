using System.Web.Mvc;

namespace ACCDataStore.Helpers.ORM
{
    public class TransactionalAttribute2nd : ActionFilterAttribute
    {
        private IUnitOfWork2nd _unitOfWork2nd;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.HttpContext.Error == null)
            {
                _unitOfWork2nd = DependencyResolver.Current.GetService<IUnitOfWork2nd>();
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.Exception == null && filterContext.HttpContext.Error == null && _unitOfWork2nd != null)
            {
               // _unitOfWork2nd.SaveChanges();
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
