using ACCDataStore.Entity.SchoolProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.SIMD
{
    public class SIMDViewModel:SchoolProfileViewModel
    {
       // public List<SIMDObj> ListSIMDdata { get; set; }
        public List<string> ListSIMDdefinition { get; set; }
        public bool IsShowCriteria { get; set; }
        public List<string> ListYear { get; set; }
        public List<string> ListSelectedYear { get; set; }
        public List<string> ListSelectedDeciles { get; set; }
        public bool IsShowData { get; set; }

    }
}