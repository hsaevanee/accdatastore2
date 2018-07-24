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
    public class AdminAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        //public void OnAuthentication(AuthenticationContext filterContext)
        //{
        //    var sUserLogined = filterContext.HttpContext.Session["SessionUser"] as string;
        //    if (sUserLogined != null)
        //    {
        //        // do nothing
        //    }
        //    else
        //    {
        //        if (filterContext.HttpContext.Request.IsAjaxRequest())
        //        {
        //            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
        //            filterContext.HttpContext.Response.End();
        //        }
        //        else
        //        {
        //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "Index" }, { "area", "" } });
        //        }
        //    }
        //}

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var eUsers = filterContext.HttpContext.Session["SessionUser"] as Users;
            if (eUsers != null && eUsers.IsAdministrator)
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
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "action", "Index" }, { "controller", "IndexAuthorisation" }, { "area", "Authorisation" } });
                }
            }
        }


        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}