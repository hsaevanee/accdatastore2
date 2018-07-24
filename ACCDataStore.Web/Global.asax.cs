using ACCDataStore.Web.App_Start;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace ACCDataStore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ProcessApplicationStart();
        }

        private void ProcessApplicationStart()
        {
            Application["APP_NAME"] = System.Configuration.ConfigurationManager.AppSettings["applicationName"];
            Application["APP_VERSION"] = GetApplicationVersion();
        }

        private string GetApplicationVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
