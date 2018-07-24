using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class PupilObject
    {
        public string SDSRef { get; set; }
        public string name { get; set; }
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string School { get; set; }
        public string SCN { get; set; }
        public string Telnumber { get; set; }
    }
}