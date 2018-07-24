using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Web.Areas.DatahubProfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.DatahubProfile.ViewModels.Datahub
{
    public class DatahubViewModel
    {

        public IList<School> ListSchoolNameData { get; set; }
        public IList<School> ListNeighbourhoodsName { get; set; }
        public IList<School> ListCouncilName { get; set; }
        public School selectedschool { get; set; }
        public List<string> listOfSchools { get; set; }
        public List<string> listOfNeightbourhoods { get; set; }
        public string selectedcouncil { get; set; }
        public string selectedschoolcode { get; set; } // for school dropdown list
        public string selectedneighbourhoods { get; set; } // for neighbourhood dropdown list
        public DatahubData AberdeencityData { get; set; }
        public DatahubData SchoolData { get; set; }
        public IList<DatahubDataObj> Listpupils { get; set; }
        public string levercategory { get; set; }
        // dataview for Heatmap
        public string selecteddataset { get; set; }
        public IList<string> ListDatasets { get; set; }
        public string seachby { get; set; }
        public string searchcode { get; set; }
        public List<PosNegSchoolList> summaryTableData { get; set; }
        public List<PosNegSchoolList> summaryNeighboursTableData { get; set; }
        public long benchmarkResults { get; set; }
        public List<SummaryDataViewModel> allCouncilTable { get; set; }
        public SummaryDataViewModel CityData { get; set; }
        public SummaryDataViewModel SelectedData { get; set; }
        public IEnumerable<SelectListItem> AvailableCouncis { get; set; }
        public IList<SummaryDataViewModel> ListSelectionData { get; set; }
        public ViewModelParams selectionParams { get; set; }
        public ViewModelParams selectionParamsRaw { get; set; }
    }
}