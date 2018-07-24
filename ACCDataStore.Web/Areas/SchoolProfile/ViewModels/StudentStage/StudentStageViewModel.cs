using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.StudentStage
{
    public class StudentStageViewModel : SchoolProfileViewModel
    {
        // public List<SIMDObj> ListSIMDdata { get; set; }
        //public List<string> ListStage { get; set; }
        public bool IsShowCriteria { get; set; }
        public List<string> ListSelectedStdStage { get; set; }
        public bool IsShowData { get; set; }
    }
}