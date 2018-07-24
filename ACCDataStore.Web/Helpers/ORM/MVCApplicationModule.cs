using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;
using System.Web.Mvc;

namespace ACCDataStore.Helpers.ORM
{
    public class MVCApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.BindFilter<TransactionFilter>(FilterScope.Action, 0)
                .WhenActionMethodHas<TransactionAttribute>();
        }
    }
}
