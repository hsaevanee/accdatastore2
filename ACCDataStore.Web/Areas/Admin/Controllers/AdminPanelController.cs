using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Repository;
using ACCDataStore.Entity;
using Newtonsoft.Json;
using ACCDataStore.Web.Areas.Authorisation.ViewModels;

namespace ACCDataStore.Web.Areas.Admin
{
    public class AdminPanelController : Controller
    {
        private IGenericRepository2nd rpGeneric2nd;

        public AdminPanelController(IGenericRepository2nd rpGeneric2nd) {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: Admin/AdminPanel
        public ActionResult Index()
        {
            
            List<Users> users = new List<Users>();
            foreach(var i in this.rpGeneric2nd.FindByNativeSQL("select * from users")) {
                users.Add(new Users());
                users[users.Count - 1].UserName = (string)i[1];
                users[users.Count - 1].Firstname = (string)i[3];
                users[users.Count - 1].Lastname = (string)i[4];
                users[users.Count - 1].email = (string)i[6];
                users[users.Count - 1].IsAdministrator = Convert.ToInt16(i[7]) == 1;
                users[users.Count - 1].IsDataHubAdministrator = Convert.ToInt16(i[9]) == 1;
                users[users.Count - 1].IsPublicUser = Convert.ToInt16(i[10]) == 1;
                users[users.Count - 1].IsSchoolAdministrator = Convert.ToInt16(i[8]) == 1;
            }
            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public void UpdateUser()
        {
            try
            {
                Users user = this.rpGeneric2nd.Find<Users>(" From Users where UserName = :userName ", new string[] { "userName" }, new object[] { Request.Form["userName"] }).FirstOrDefault();
                user.email = Request.Form["email"];
                user.IsDataHubAdministrator = Request.Form["IsDataHubAdministrator"].Equals("true");
                user.IsPublicUser = Request.Form["IsPublicUser"].Equals("true");
                user.IsSchoolAdministrator = Request.Form["IsSchoolAdministrator"].Equals("true");
                user.IsAdministrator = Request.Form["IsAdministrator"].Equals("true");
                this.rpGeneric2nd.SaveOrUpdate<Users>(user);
                // TODO: Add update logic using Request.Form

            }
            catch {
            }
        }

        [HttpPost]
        public void DeleteUser() {
            Users user = this.rpGeneric2nd.Find<Users>(" From Users where UserName = :userName ", new string[] { "userName" }, new object[] { Request.Form["userName"] }).FirstOrDefault();
            this.rpGeneric2nd.Delete<Users>(user);
        }

        public ActionResult AddingUser(IndexViewModel vmIndex)
        {
            return View();
            //try
            //{

            //    Users eUser = new Users();
            //    eUser.UserName = vmIndex.Username;
            //    eUser.Password = EncryptString(vmIndex.Password, sKey);
            //    eUser.Firstname = vmIndex.firstname;
            //    eUser.Lastname = vmIndex.lastname;
            //    eUser.email = vmIndex.email;
            //    eUser.job_title = vmIndex.jobtitle;
            //    eUser.IsPublicUser = true;
            //    this.rpGeneric2nd.SaveOrUpdate<Users>(eUser);
            //    return RedirectToAction("Index");
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message, ex);
            //    return RedirectToAction("Index");
            //}

        }
    }
}
