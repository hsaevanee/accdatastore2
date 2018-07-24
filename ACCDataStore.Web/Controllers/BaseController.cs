using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Controllers
{
    public class BaseController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseController));

        protected JsonResult ThrowJsonError(Exception ex)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
            log.Error(ex.Message, ex);
            return Json(new { Message = sErrorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}