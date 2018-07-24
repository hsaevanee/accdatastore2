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
    public class BaseSchoolProfilesViewModel  
    {
        public DataTable dataTableNationality { get; set; }
        public DataTable dataTableNationalityHistory { get; set; }
        public DataTable dataTableStage { get; set; }
        public DataTable dataTableStageHistory { get; set; }
        public DataTable dataTableEnglishLevel { get; set; }
        public DataTable dataTableEnglishLevelHistory { get; set; }
        public DataTable dataTableEthnicBackground { get; set; }
        public DataTable dataTableEthnicBackgroundHistory { get; set; }
        public DataTable dataTableFreeSchoolMeal { get; set; }
        public DataTable dataTableLookedAfter{ get; set; }
        public DataTable dataTableLookedAfterHistory { get; set; }
        public DataTable dataTableAttendance { get; set; }
        public DataTable dataTableExclusion { get; set; }
        public DataTable dataTableBudget { get; set; }
        public DataTable dataTableSchoolRoll { get; set; }
        public DataTable dataTableSIMDDecile { get; set; }
        public DataTable dataTableSchoolRollForecast { get; set; }
        public DataTable dataTableCSP { get; set; }
        public DataTable dataTableIEP { get; set; }

        public List<Year> listYears { get; set; }
        public Year selectedYear { get; set; }
        public string stringYear { get; set; }
        public List<School> listSchoolname { get; set; }
        public List<School> listSelectedSchoolname { get; set; }

        public Dictionary<string, string> DicEthnicBG { get; set; }
        public Dictionary<string, string> DicNationalIdentity { get; set; }
        public Dictionary<string, string> DicEnglishLevel { get; set; }
        public Dictionary<string, string> DicStage{ get; set; }
        public Dictionary<string, string> DicFreeMeal { get; set; }
        public Dictionary<string, string> DicLookedAfter{ get; set; }
        public Dictionary<string, string> DicAttendance{ get; set; }
        public Dictionary<string, string> DicSIMDDecile { get; set; }

        public List<StudentObj> listAllPupils { get; set; }
        public List<DataSeries> listDataSeriesEnglishLevel { get; set; }
        public List<DataSeries> listDataSeriesHistoryEnglishLevel { get; set; }
        public List<DataSeries> listDataSeriesEthnicBackground { get; set; }
        public List<DataSeries> listDataSeriesNationality { get; set; }
        public List<DataSeries> listDataSeriesStage { get; set; }
        public List<DataSeries> listDataSeriesFreeMeal{ get; set; }
        public List<DataSeries> listDataSeriesLookedAfter{ get; set; }
        public List<DataSeries> listDataSeriesAttendance { get; set; }
        public List<DataSeries> listDataSeriesExclusion { get; set; }
        public List<DataSeries> listDataSeriesBudget { get; set; }
        public List<DataSeries> listDataSeriesSchoolRoll { get; set; }
        public List<DataSeries> listDataSeriesSIMDDecile { get; set; }
        public List<DataSeries> listDataSeriesSchoolRollForecast { get; set; }
        public List<DataSeries> listDataSeriesCSP { get; set; }
        public List<DataSeries> listDataSeriesIEP { get; set; }

        public string profiletitle;
        public bool? showTableAttendance = null;
        public bool? showTableExclusion = null;
        //public List<StudentObj> listPupils { get; set; }

        

    }
}