using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class IndexAberdeenProfileController : BaseSchoolProfileController
    {

        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private IndexAberdeenProfileViewModel vmIndexAberdeenCityProfilesModel;

        public IndexAberdeenProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexAberdeenCityProfilesModel = new IndexAberdeenProfileViewModel();

        }

        [AdminAuthentication]
        [Transactional]
        public ActionResult IndexAberdeenProfiles(string sSchoolType)
        {
            //get data ready for set up profiles
            vmIndexAberdeenCityProfilesModel.listSchoolname = new List<School>() { new School("Aberdeen City", "Aberdeen City") };
            vmIndexAberdeenCityProfilesModel.listSelectedSchoolname = new List<School>() { new School("Aberdeen City", "Aberdeen City") };
            vmIndexAberdeenCityProfilesModel.listYears = GetListYear();
            vmIndexAberdeenCityProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexAberdeenCityProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexAberdeenCityProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            //vmIndexAberdeenCityProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexAberdeenCityProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexAberdeenCityProfilesModel.DicLookedAfter = GetDicLookAfter();
            vmIndexAberdeenCityProfilesModel.selectedYear = new Year("2015");
            vmIndexAberdeenCityProfilesModel.showTableAttendance = null;
            vmIndexAberdeenCityProfilesModel.showTableExclusion = null;
            Session["vmIndexAberdeenCityProfilesModel"] = vmIndexAberdeenCityProfilesModel;
            return View("IndexAberdeenCity", vmIndexAberdeenCityProfilesModel);
        }


        public ActionResult GetAberdeenProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<AaeAttendanceObj> listAaeAttendancelists = new List<AaeAttendanceObj>();
            List<ExclusionStudentObj> listExclusionPupils = new List<ExclusionStudentObj>();

            List<School> listSchoolType = GetSchoolType();
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexAberdeenCityProfilesModel = Session["vmIndexAberdeenCityProfilesModel"] as IndexAberdeenProfileViewModel;
             
            List<Year> templistYears = vmIndexAberdeenCityProfilesModel.listYears;

            if (Request["stringYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["stringYear"].ToString();
                vmIndexAberdeenCityProfilesModel.stringYear = year;
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
                listAaeAttendancelists = GetAaeAttendanceLists(rpGeneric2nd, sSchoolType, selectedYear, new List<School>(), listAllPupils);
                listExclusionPupils = GetListExclusionPupils(rpGeneric2nd, selectedYear, sSchoolType);

            }

            vmIndexAberdeenCityProfilesModel.profiletitle = "Aberdeen City";

            //store selected school into view model
            //vmIndexAberdeenCityProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexAberdeenCityProfilesModel.selectedYear = selectedYear;
            vmIndexAberdeenCityProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeriesForAberdeenCity("englishlevel", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesEnglishLevel = temp;
            vmIndexAberdeenCityProfilesModel.dataTableEnglishLevel = GenerateTransposedTable(CreateDataTable(temp, vmIndexAberdeenCityProfilesModel.DicEnglishLevel, "Level of English", "percentage"));
            //setting ethnic data and table
            temp = GetDataSeriesForAberdeenCity("ethnicity", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexAberdeenCityProfilesModel.dataTableEthnicBackground = GenerateTransposedTable(CreateDataTable(temp, vmIndexAberdeenCityProfilesModel.DicEthnicBG, "Ethnicity", "percentage"));
            //setting Nationality data and table
            temp = GetDataSeriesForAberdeenCity("nationality", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesNationality = temp;
            vmIndexAberdeenCityProfilesModel.dataTableNationality = GenerateTransposedTable(CreateDataTable(temp, vmIndexAberdeenCityProfilesModel.DicNationalIdentity, "Nationality", "percentage"));
            //setting Stage data and table
            temp = GetDataSeriesForAberdeenCity("stage", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesStage = temp;
            vmIndexAberdeenCityProfilesModel.dataTableStage = CreateDataTaleWithCheckSumTotal(temp, "School Roll", "number");

            vmIndexAberdeenCityProfilesModel.dataTableStagePrimary = CreateDataTaleWithTotal(temp.Where(x => x.school.schooltype.Equals("2")).ToList(), GetDicStage(rpGeneric2nd, "2"), "Stage", "number");
            vmIndexAberdeenCityProfilesModel.dataTableStageSecondary = CreateDataTaleWithTotal(temp.Where(x => x.school.schooltype.Equals("3")).ToList(), GetDicStage(rpGeneric2nd, "3"), "Stage", "number");
            vmIndexAberdeenCityProfilesModel.dataTableStageSpecial = CreateDataTaleWithTotal(temp.Where(x => x.school.schooltype.Equals("4")).ToList(), GetDicStage(rpGeneric2nd, "4"), "Stage", "number");

            //vmIndexAberdeenCityProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexAberdeenCityProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeriesForAberdeenCity("freemeal", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexAberdeenCityProfilesModel.dataTableFreeSchoolMeal = GenerateTransposedTable(CreateDataTable(temp, vmIndexAberdeenCityProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage"));

            //setting LookAfter data and table
            temp = GetDataSeriesForAberdeenCity("lookafter", listAllPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexAberdeenCityProfilesModel.dataTableLookedAfter = GenerateTransposedTable(CreateDataTaleWithTotal(temp, vmIndexAberdeenCityProfilesModel.DicLookedAfter, "Looked After Children", "no+%"));
            //Attendance
            vmIndexAberdeenCityProfilesModel.showTableAttendance = listAaeAttendancelists.Count == 0 ? false : true;
            temp = listAaeAttendancelists.Count == 0 ? new List<DataSeries>() : GetAttendanceDataSeriesForAberdeenCity("attendance", listAaeAttendancelists, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesAttendance = temp;
            vmIndexAberdeenCityProfilesModel.dataTableAttendance = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "School Attendance", "percentage"));

            //Exclusion
            vmIndexAberdeenCityProfilesModel.showTableExclusion = listExclusionPupils.Count == 0 ? false : true;
            temp = listExclusionPupils.Count == 0 ? new List<DataSeries>() : GetExclusionDataSeriesForAberdeenCity("exclusion", listExclusionPupils, listSchoolType, selectedYear);
            vmIndexAberdeenCityProfilesModel.listDataSeriesExclusion = temp;
            vmIndexAberdeenCityProfilesModel.dataTableExclusion = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "Exclusions-Annual", "number"));

            Session["vmIndexAberdeenCityProfilesModel"] = vmIndexAberdeenCityProfilesModel;
            return View("IndexAberdeenCity", vmIndexAberdeenCityProfilesModel);
        }

        protected List<School> GetSchoolType() {
            List<School> schooltype = new List<School>();

            schooltype.Add(new School("2", "Primary School", "2"));
            schooltype.Add(new School("3", "Secondary School", "3"));
            schooltype.Add(new School("4", "Special School", "4"));
            schooltype.Add(new School("5", "Aberdeen City", "5"));
            return schooltype;
        
        }

        protected List<DataSeries> GetDataSeriesForAberdeenCity(string datatitle, List<StudentObj> listPupilData, List<School> schooltype, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            //var listResultwithPercentage = null;
            double sum = 0.0;
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual schooltype
            foreach (School item in schooltype)
            {
                switch (item.schooltype)
                {
                    case "2": //select only primary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("P")) && (x.StudentStatus.Equals("01"))).ToList();
                        break;
                    case "3": 
                        //select only secondary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("S")) && (x.StudentStatus.Equals("01"))).ToList();
                        listtempPupilData = listtempPupilData.Where(x => !x.StudentStage.Equals("SP")).ToList();
                        break;
                    case "4": 
                        //select only special pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.Equals("SP")) && (x.StudentStatus.Equals("01"))).ToList();
                        break;
                    case "5":
                        //select only special pupils
                        listtempPupilData = listPupilData.ToList();
                        break;
                }

                switch (datatitle)
                {

                    case "nationality":
                        var listResultforAll = listtempPupilData.GroupBy(x => x.NationalIdentity).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "ethnicity":
                        listResultforAll = listtempPupilData.GroupBy(x => x.EthnicBackground).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "englishlevel":
                        listResultforAll = listtempPupilData.GroupBy(x => x.LevelOfEnglish).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "stage":
                        listResultforAll = listtempPupilData.GroupBy(x => x.StudentStage).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;
                    case "freemeal":
                        if (schooltype.Equals("2"))
                        {
                            //select only pupils between stage 4 and 7
                            var temp = (from a in listtempPupilData where a.StudentStage.Equals("P4") || a.StudentStage.Equals("P5") || a.StudentStage.Equals("P6") || a.StudentStage.Equals("P7") select a).ToList();
                            var listResultforP4P7 = temp.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            listResultforAll = listtempPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();

                            //calculate the total number of pupils in Aberdeen
                            sum = (double)listResultforAll.Select(r => r.count).Sum();
                            listResultwithPercentage = listResultforP4P7.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        }
                        else
                        {
                            listResultforAll = listtempPupilData.GroupBy(x => x.FreeSchoolMealRegistered).Select(y => new { Code = y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                            //calculate the total number of pupils in Aberdeen
                            sum = (double)listResultforAll.Select(r => r.count).Sum();
                            listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        }
                        break;
                    case "lookafter":
                        listResultforAll = listtempPupilData.GroupBy(x => x.StudentLookedAfter).Select(y => new { Code = y.Key == null ? "" : y.Key, list = y.ToList(), count = y.ToList().Count() }).ToList();
                        //calculate the total number of pupils in Aberdeen
                        sum = (double)listResultforAll.Select(r => r.count).Sum();
                        listResultwithPercentage = listResultforAll.Select(y => new ObjectDetail { itemcode = y.Code, liststudents = y.list, count = y.count, percentage = sum != 0 ? (y.count / sum) * 100 : 0 }).ToList();
                        break;

                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }
            
            return listobject;
        }

        protected List<DataSeries> GetAttendanceDataSeriesForAberdeenCity(string datatitle, List<AaeAttendanceObj> listPupilData, List<School> schooltype, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<AaeAttendanceObj> listtempPupilData = new List<AaeAttendanceObj>();
            //var listResultwithPercentage = null;
            double sum = 0, sumUnauthorisedAb = 0, sumAuthorised = 0, sumAttendance = 0, sumAbExclusion = 0;
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual schooltype
            foreach (School item in schooltype)
            {

                switch (item.schooltype)
                {
                    case "2": //select only primary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("P"))).ToList();
                        break;
                    case "3":
                        //select only secondary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("S"))).ToList();
                        listtempPupilData = listtempPupilData.Where(x => !x.StudentStage.Equals("SP")).ToList();
                        break;
                    case "4":
                        //select only special pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.Equals("SP"))).ToList();
                        break;
                    case "5":
                        //select only special pupils
                        listtempPupilData = listPupilData.ToList();
                        break;
                }

                listResultwithPercentage = new List<ObjectDetail>();
                sum = 0;
                sumUnauthorisedAb = 0;
                sumAuthorised = 0;
                sumAttendance = 0;
                sumAbExclusion = 0;

                switch (datatitle)
                {
                    case "attendance":
                        sumAttendance = (double)listtempPupilData.Where(x => x.AttendanceCode.Equals("11")).Select(r => r.Total).Sum() + (double)listtempPupilData.Where(x => x.AttendanceCode.StartsWith("10")).Select(r => r.Total).Sum()+listtempPupilData.Where(x => x.AttendanceCode.Equals("12")).Select(r => r.Total).Sum() ;

                        sum = (double)listtempPupilData.Where(x => x.AttendanceCode.StartsWith("01")).Select(r => r.Total).Sum() - listtempPupilData.Where(x => x.AttendanceCode.Equals("02")).Select(r => r.Total).Sum();
                        // including code 30/31/32/33
                        sumUnauthorisedAb = (double)listtempPupilData.Where(x => x.AttendanceCode.StartsWith("3")).Select(r => r.Total).Sum();
                        // including code 11-13/20-24
                        sumAuthorised = listtempPupilData.Where(x => x.AttendanceCode.Equals("13")).Select(r => r.Total).Sum() + listtempPupilData.Where(x => x.AttendanceCode.StartsWith("2")).Select(r => r.Total).Sum();
                        sumAbExclusion = (double)listtempPupilData.Where(x => x.AttendanceCode.Equals("40")).Select(r => r.Total).Sum();

                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Attendance", count = (int)sumAttendance, percentage = sum != 0 ? (sumAttendance / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Authorised Absence", count = (int)sumAuthorised, percentage = sum != 0 ? (sumAuthorised / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Unauthorised Absence", count = (int)sumUnauthorisedAb, percentage = sum != 0 ? (sumUnauthorisedAb / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Absense due to exclusion", count = (int)sumAbExclusion, percentage = sum != 0 ? (sumAbExclusion / sum) * 100 : 0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Total Absence", count = (int)(sumAuthorised + sumUnauthorisedAb), percentage = sum != 0 ? (sumAuthorised + sumUnauthorisedAb) / sum * 100 : 0 });

                        break;
                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }

            return listobject;
        }

        protected List<DataSeries> GetExclusionDataSeriesForAberdeenCity(string datatitle, List<ExclusionStudentObj> listPupilData, List<School> schooltype, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            List<ExclusionStudentObj> listtempPupilData = new List<ExclusionStudentObj>();
            //var listResultwithPercentage = null;
            double sum0 = 0, sum1 = 0, sumLength = 0;
            List<ObjectDetail> listResultwithPercentage = new List<ObjectDetail>();
            //calculate individual schooltype
            foreach (School item in schooltype)
            {

                switch (item.schooltype)
                {
                    case "2": //select only primary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("P")) && (x.StudentStatus.Equals("01"))).ToList();
                        break;
                    case "3":
                        //select only secondary pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.StartsWith("S")) && (x.StudentStatus.Equals("01"))).ToList();
                        listtempPupilData = listtempPupilData.Where(x => !x.StudentStage.Equals("SP")).ToList();
                        break;
                    case "4":
                        //select only special pupils
                        listtempPupilData = listPupilData.Where(x => (x.StudentStage.Equals("SP")) && (x.StudentStatus.Equals("01"))).ToList();
                        break;
                    case "5":
                        //select only special pupils
                        listtempPupilData = listPupilData.ToList();
                        break;
                }

                listResultwithPercentage = new List<ObjectDetail>();
                sum0 = 0;
                sum1 = 0;
                sumLength = 0;

                switch (datatitle)
                {
                    case "exclusion":
                        sum0 = (double)listtempPupilData.Count(x => x.RemovedFromRegister.Equals("0"));
                        sum1 = (double)listtempPupilData.Count(x => x.RemovedFromRegister.Equals("1"));
                        // including code 30/31/32/33
                        sumLength = (double)listtempPupilData.Where(x => x.LengthOfExclusion != 0).Select(r => r.LengthOfExclusion).Sum();
                        //adding to list
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Temporary Exclusions", count = (int)sum0 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Removals from the Register", count = (int)sum1 });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "#Days lost per 1000 pupils", count = (int)(sumLength != 0 ? (sumLength / 2) / 1000 : 0) });
                        listResultwithPercentage.Add(new ObjectDetail { itemcode = "Total Exclusions", count = (int)(sum0 + sum1) });


                        break;
                }

                listobject.Add(new DataSeries { dataSeriesNames = datatitle, school = item, year = iyear, listdataitems = listResultwithPercentage, checkSumPercentage = (double)listResultwithPercentage.Select(r => r.percentage).Sum(), checkSumCount = (int)listResultwithPercentage.Select(r => r.count).Sum() });
            }

            return listobject;
        }

        protected DataTable CreateDataTaleWithCheckSumTotal(List<DataSeries> listobject, string tabletitle, string showtype)
        {
            // create data table with count total data show in each row
            DataTable dataTable = new DataTable();
            List<string> temprowdata;
 
            if (showtype.Equals("number"))
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                dataTable.Columns.Add("Total", typeof(string));

                //display number
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    temprowdata.Add(temp.checkSumCount.ToString(""));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            else
            {
                dataTable.Columns.Add(tabletitle, typeof(string));
                dataTable.Columns.Add("Total", typeof(string));

                //display number
                foreach (var temp in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(temp.school.name);
                    temprowdata.Add(temp.checkSumPercentage.ToString(""));
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }
            return dataTable;
        }


    }
}