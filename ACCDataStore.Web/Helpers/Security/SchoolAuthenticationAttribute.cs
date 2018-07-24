using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using ACCDataStore.Entity;

namespace ACCDataStore.Helpers.ORM.Helpers.Security
{
    public class SchoolAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var eUsers = filterContext.HttpContext.Session["SessionUser"] as Users;
            if (eUsers != null && eUsers.IsSchoolAdministrator)
            {
                // only administrator can access this action
            }
            else
            {
                // no right access, return to login page
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    //login page
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "IndexAuthorisation" }, { "area", "Authorisation" }, { "from", filterContext.HttpContext.Request.CurrentExecutionFilePath } });
                }
            }
        }


        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}