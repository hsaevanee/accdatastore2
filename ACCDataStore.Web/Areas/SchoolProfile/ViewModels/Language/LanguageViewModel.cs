using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Language
{
    public class LanguageViewModel : SchoolProfileViewModel
    {
        public bool IsShowCriteria { get; set; }
        public bool IsShowData { get; set; }
        public List<string> ListSelectedLevelENCode { get; set; }
    }
}