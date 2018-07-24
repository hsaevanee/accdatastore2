using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ACCDataStore.Entity;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Nationality
{
    public class NationalityViewModel: SchoolProfileViewModel
    {
        //public List<NationalityObj> ListNationalityData { get; set; }
        //public string selectedschoolname { get; set; }
        //public List<string> ListNationalCode { get; set; }
        public List<string> ListNational { get; set; }
        public List<string> ListSelectedNationality { get; set; }
        public bool IsShowCriteria { get; set; }
        public bool IsShowData { get; set; }
    }
}