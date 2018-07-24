using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity.SchoolProfiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles
{
    public class IndexAberdeenProfileViewModel : BaseSchoolProfilesViewModel
    {
        public DataTable dataTableStagePrimary { get; set; }
        public DataTable dataTableStageSecondary { get; set; }
        public DataTable dataTableStageSpecial { get; set; }


    }
}