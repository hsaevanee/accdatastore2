using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles
{
    public class SchoolProfilesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SchoolProfiles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapMvcAttributeRoutes();

            context.MapRoute(
                "SchoolProfiles_default",
                "SchoolProfiles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}