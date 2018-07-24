using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles
{
    public class TredningViewModel
    {
        public List<School> school { get; set; }
        public string datatitle { get; set; }
        public List<Year> listYear { get; set; }
        public DataTable dataTableSchool { get; set; }
        public DataTable dataTableCity { get; set; }
        public DataTable dataTable { get; set; }
    }
}