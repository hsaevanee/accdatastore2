using ACCDataStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.EthnicBackground
{
    public class EthnicBgViewModel2 : SchoolProfileViewModel
    {
        public List<EthnicObj> ListEthnicData2 { get; set; }
        public List<string> ListSchoolNameData2 { get; set; }
        public string selectedschoolname2 { get; set; }
        public bool IsShowData { get; set; }
        public List<string> ListSelectedEthnicBg { get; set; }
        public int NoSelectedSchool { get; set; }
        //public List<GenderObj> listSelectedGender;
        //public List<GenderObj> listGender; 

    }
}