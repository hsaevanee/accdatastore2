using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.InCAS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles
{
    public class IndexPrimarySchoolProfilesViewModel : BaseSchoolProfilesViewModel
    {
        // for PIPS data
        public DataTable dataTablePIPS { get; set; }
        public DataTable dataTablePIPShistoric { get; set; }
        public List<DataSeries> listDataSeriesPIPS { get; set; }
        public List<PIPSObj> listPIPSPupils { get; set; }
        // for InCAS data
        public DataTable dataTableInCASP2 { get; set; }
        public List<DataSeries> listDataSeriesInCASP2{ get; set; }
        public DataTable dataTableInCASP4 { get; set; }
        public List<DataSeries> listDataSeriesInCASP4 { get; set; }
        public DataTable dataTableInCASP6 { get; set; }
        public List<DataSeries> listDataSeriesInCASP6 { get; set; }

        public List<InCASObj> listInCASPupils { get; set; }

        public bool? showTableInCAS = null;
    }
}