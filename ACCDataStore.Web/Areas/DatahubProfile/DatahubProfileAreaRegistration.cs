using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.DatahubProfile
{
    public class DatahubProfileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DatahubProfile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();

            context.MapRoute(
                "DatahubProfile_default",
                "DatahubProfile/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}