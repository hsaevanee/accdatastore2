using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.Authorisation.ViewModels
{
    public class IndexViewModel
    {
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string jobtitle { get; set; }
        public string telno { get; set; }
        public string email { get; set; }
        public string function { get; set; }
        public string cluster { get; set; }
        public string jobrole { get; set; }
        //public string confirmpassword { get; set; }
        public string managername { get; set; }
        public string manageremail { get; set; }
        public bool datasets1 { get; set; } // schoolprofile
        public bool datasets2 { get; set; } //datahub
        public bool datasets3 { get; set; } //CSSF

    }
}