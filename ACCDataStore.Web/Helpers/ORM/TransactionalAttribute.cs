using System.Web.Mvc;

namespace ACCDataStore.Helpers.ORM
{
    public class TransactionalAttribute : ActionFilterAttribute
    {
        private IUnitOfWork _unitOfWork;
        private IUnitOfWork2nd _unitOfWork2nd;
        private IUnitOfWork3nd _unitOfWork3nd;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.HttpContext.Error == null)
            {
                _unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
                _unitOfWork2nd = DependencyResolver.Current.GetService<IUnitOfWork2nd>();
                _unitOfWork3nd = DependencyResolver.Current.GetService<IUnitOfWork3nd>();
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid && filterContext.Exception == null && filterContext.HttpContext.Error == null && _unitOfWork != null)
            {
                _unitOfWork.Commit();
                _unitOfWork2nd.Commit();
                _unitOfWork3nd.Commit();
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
