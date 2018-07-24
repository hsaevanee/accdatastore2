using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.IndexSchoolProfile
{
    public class Index2SchoolProfileViewModel : SchoolProfileViewModel
    {
        public List<string> ListSchoolNameData2 { get; set; }
        public string selectedschoolname2 { get; set; }
        public List<EthnicObj> ListEthnicData2 { get; set; }
        public List<NationalityObj> ListNationalityData2 { get; set; }
        public List<SIMDObj> ListSIMDData2 { get; set; }
        public List<StdStageObj> ListStdStageData2 { get; set; }        
        public List<string> ListSelectedGender2 { get; set; }
        public List<NationalityObj> ListLevelENData2 { get; set; }
    }
}