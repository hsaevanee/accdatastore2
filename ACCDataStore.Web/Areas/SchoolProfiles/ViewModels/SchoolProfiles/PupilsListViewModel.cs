using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles
{
    public class PupilsListViewModel
    {
        public List<StudentObj> listPupils { get; set; }
        public School school { get; set; }
        public string catagory { get; set; }
        public string datatile { get; set; }

    }
}