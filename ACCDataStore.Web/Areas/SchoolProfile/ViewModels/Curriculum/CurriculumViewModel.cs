using ACCDataStore.Entity.SchoolProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Curriculum
{
    public class CurriculumViewModel : SchoolProfileViewModel
    {
        public bool IsShowCriteria { get; set; }
        public List<string> ListSelectedStdStage { get; set; }
        public List<string> ListSelectedSubject { get; set; }
        public List<string> ListSubjects { get; set; }
        public List<string> ListSkills { get; set; }
        public List<CurriculumObj> ListNMMdata { get; set; }
        public List<CurriculumObj> ListSPMdata { get; set; }
        public List<CurriculumObj> ListIHdata { get; set; }
        public List<CurriculumObj> ListLiteracydata { get; set; }
        public List<CurriculumObj> ListReadingdata { get; set; }
        public List<CurriculumObj> ListWritingdata { get; set; }
        public List<CurriculumObj> ListLandTdata { get; set; }
        public List<CurriculumObj> ListNumeracydata { get; set; }
        public Dictionary<string, string> DicSubject { get; set; }
        public bool IsShowData { get; set; }
    }
}