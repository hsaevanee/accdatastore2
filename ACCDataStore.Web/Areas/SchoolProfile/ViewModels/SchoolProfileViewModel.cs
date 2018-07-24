using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels
{
    public class SchoolProfileViewModel
    {
        public List<string> ListSchoolNameData { get; set; }
        public string selectedschoolname { get; set; }
        public List<EthnicObj> ListEthnicData { get; set; }
        public List<NationalityObj> ListNationalityData { get; set; }
        public List<SIMDObj> ListSIMDData { get; set; }
        public List<StdStageObj> ListStdStageData { get; set; }
        public Dictionary<string, string> DicGender { get; set; }
        public Dictionary<string, string[]> DicGenderWithSelected { get; set; }
        public Dictionary<string, string> DicEthnicBG { get; set; }
        public Dictionary<string, string> DicLevelEN { get; set; }
        public List<string> ListGenderCode { get; set; }
        public List<string> ListSelectedGender { get; set; }
        public List<string> ListSIMDCode { get; set; }
        public List<string> ListEthnicCode { get; set; }
        public List<string> ListNationalityCode { get; set; }
        public List<string> ListStageCode { get; set; }
        public Dictionary<string, string> DicNational { get; set; }
        public List<string> ListLevelENCode { get; set; }
        public List<NationalityObj> ListLevelENData { get; set; }
        public List<FSMObj> ListFSMData { get; set; }
        public List<WiderAchievementObj> ListWiderAchievementData { get; set; }
        //Test Boostrapmultiselect
        public List<CheckModel> Listcheckmodel{ get; set; }
        public int[] Selectedcheckmodel { get; set; }

    }
}