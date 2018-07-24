using Common.Logging;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using ACCDataStore.Repository;
using ACCDataStore.Entity;
using ACCDataStore.Web.ViewModels;


namespace ACCDataStore.Web.Controllers
{
    public class IndexController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public IndexController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        public ActionResult Index(string id)
        {
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;

            CreateUserProfile();

            // just git test
            if (id == null)
            {
                eGeneralSettings.HomepgCounter++;
                ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

                var vmIndex = new IndexViewModel();
                vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
                vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
                return View("index", vmIndex);

            }
            else
            {
                if (id.ToLower().Equals("theteam"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("theTeam");
                }
                else if (id.ToLower().Equals("about"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("About");
                }
                else if (id.ToLower().Equals("contact"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Contact");
                }
                else if (id.ToLower().Equals("datacentre"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("DataCentre");
                }
                else if (id.ToLower().Equals("finance"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Finance");
                }
                else if (id.ToLower().Equals("management"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Management");
                }
                else if (id.ToLower().Equals("education"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Education");
                }
                else if (id.ToLower().Equals("pandp"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("PandP");
                }
                else if (id.ToLower().Equals("test"))
                {
                    eGeneralSettings.TeampgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
                    return View("Test");
                }
                else {
                    eGeneralSettings.HomepgCounter++;
                    ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

                    var vmIndex = new IndexViewModel();
                    vmIndex.ApplicationName = HttpContext.Application["APP_NAME"] as string;
                    vmIndex.ApplicationVersion = HttpContext.Application["APP_VERSION"] as string;
                    return View("index", vmIndex);
                }
            }

        }

        public void CreateUserProfile() {

            Users eUser = this.rpGeneric2nd.Find<Users>(" From Users where UserName = :userName ", new string[] { "userName" }, new object[] { User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1) }).FirstOrDefault();

            if (eUser == null)
            {
                eUser = new Users();
                eUser.UserName = User.Identity.Name.Substring(User.Identity.Name.IndexOf(@"\") + 1); 
                //eUser.Password = EncryptString(vmIndex.Password, sKey);
                //eUser.Firstname = vmIndex.firstname;
                //eUser.Lastname = vmIndex.lastname;
                //eUser.email = vmIndex.email;
                //eUser.job_title = vmIndex.jobtitle;
                eUser.IsPublicUser = true;
                eUser.enable = 0;
            }

            Session["SessionUser"] = eUser;
        
        }
       
    }
}