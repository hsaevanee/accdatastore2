using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile
{
    public class SchoolProfileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SchoolProfile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SchoolProfile_SchoolName",
                "SchoolProfile/{controller}/{action}/{sSchoolName}",
                new { action = "Index", sSchoolName = UrlParameter.Optional }
            );

            context.MapRoute(
                "SchoolProfile_default",
                "SchoolProfile/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}