using ACCDataStore.Entity;
using ACCDataStore.Web.Helpers;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfiles.ViewModels.SchoolProfiles;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Helpers.ORM;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class IndexSchoolProfilesController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        private IndexPrimarySchoolProfilesViewModel vmIndexPrimarySchoolProfilesModel;
        private IndexSecondarySchoolProfilesViewModel vmIndexSecondarySchoolProfilesModel;
        private IndexSecondarySchoolProfilesViewModel vmIndexSpecialSchoolProfilesModel;

        public IndexSchoolProfilesController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexPrimarySchoolProfilesModel = new IndexPrimarySchoolProfilesViewModel();
            this.vmIndexSecondarySchoolProfilesModel = new IndexSecondarySchoolProfilesViewModel();
            this.vmIndexSpecialSchoolProfilesModel = new IndexSecondarySchoolProfilesViewModel();
            
        }

        // GET: SchoolProfiles/IndexSchoolProfiles
        public ActionResult Index()
        {
             //this.vmIndexPrimarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd);
            return View("Home");
        }

        // GET: SchoolProfiles/IndexSchoolProfiles
        //public void Index()
        //{
        //    // this.vmIndexPrimarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd);
        //    Response.Write(System.Web.HttpContext.Current.User.Identity.Name);
        //}

        public ActionResult SchoolWebsites()
        {
            // this.vmIndexPrimarySchoolProfilesModel.listAllPupils = GetListAllPupils(rpGeneric2nd);
            return View("SchoolWebsites");
        }

        
        [AdminAuthentication]
        [Transactional]
        public ActionResult IndexSecondaryProfiles(string sSchoolType)
        {

            vmIndexSecondarySchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listSelectedSchoolname = vmIndexSecondarySchoolProfilesModel.listSchoolname.Where(x => x.seedcode.Equals("5244439")).ToList();
            vmIndexSecondarySchoolProfilesModel.listYears = GetListYear();
            vmIndexSecondarySchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexSecondarySchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexSecondarySchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            vmIndexSecondarySchoolProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexSecondarySchoolProfilesModel.DicLookedAfter = GetDicLookAfter();
            vmIndexSecondarySchoolProfilesModel.selectedYear = new Year("2015");
            vmIndexSecondarySchoolProfilesModel.showTableAttendance = null;
            vmIndexSecondarySchoolProfilesModel.showTableExclusion = null;
            Session["vmIndexSecondarySchoolProfilesModel"] = vmIndexSecondarySchoolProfilesModel;
            return View("IndexSecondarySchool", vmIndexSecondarySchoolProfilesModel);
        }

        [AdminAuthentication]
        [Transactional]
        public ActionResult IndexSpecialProfiles(string sSchoolType)
        {

            vmIndexSpecialSchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listSelectedSchoolname = vmIndexSpecialSchoolProfilesModel.listSchoolname.Where(x => x.seedcode.Equals("5245044")).ToList();
            vmIndexSpecialSchoolProfilesModel.listYears = GetListYear();
            vmIndexSpecialSchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexSpecialSchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexSpecialSchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            vmIndexSpecialSchoolProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexSpecialSchoolProfilesModel.DicLookedAfter = GetDicLookAfter();
            vmIndexSpecialSchoolProfilesModel.selectedYear = new Year("2015");
            vmIndexSpecialSchoolProfilesModel.showTableAttendance = null;
            vmIndexSpecialSchoolProfilesModel.showTableExclusion = null;
            Session["vmIndexSpecialSchoolProfilesModel"] = vmIndexSpecialSchoolProfilesModel;
            return View("IndexSpecialSchool", vmIndexSpecialSchoolProfilesModel);
        }




        public ActionResult GetSecondaryProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<School> listSelectedSchoolname = new List<School>();
            List<AaeAttendanceObj> listAaeAttendancelists = new List<AaeAttendanceObj>();
            List<ExclusionStudentObj> listExclusionPupils = new List<ExclusionStudentObj>();

            bool schoolIsSelected = false;
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexSecondarySchoolProfilesModel = Session["vmIndexSecondarySchoolProfilesModel"] as IndexSecondarySchoolProfilesViewModel;
            List<School> templistSchoolname = vmIndexSecondarySchoolProfilesModel.listSchoolname;
            List<Year> templistYears = vmIndexSecondarySchoolProfilesModel.listYears;

            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelected = true;
                //get CostCentreKey from dropdownlist in UI
                templistSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                //select selected CostCentre from dropdownlist in UI
                listSelectedSchoolname = templistSchoolname.Where(x => templistSelectedSchoolname.Any(y => y.Contains(x.seedcode))).ToList();
            }

            if (Request["stringYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["stringYear"].ToString();
                vmIndexSecondarySchoolProfilesModel.stringYear = year;
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (schoolIsSelected && yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
                listAaeAttendancelists = GetAaeAttendanceLists(rpGeneric2nd, sSchoolType, selectedYear, listSelectedSchoolname, listAllPupils);
                listExclusionPupils = GetListExclusionPupils(rpGeneric2nd, selectedYear, sSchoolType);

            }

            //create profiletitle
            string tempProfiletitle = null;
            foreach (var item in listSelectedSchoolname)
            {
                if (String.IsNullOrEmpty(tempProfiletitle))
                {
                    tempProfiletitle = item.name;
                }
                else
                {
                    tempProfiletitle = tempProfiletitle + " / " + item.name;

                }


            }



            vmIndexSecondarySchoolProfilesModel.profiletitle = tempProfiletitle;

            //store selected school into view model
            vmIndexSecondarySchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexSecondarySchoolProfilesModel.selectedYear = selectedYear;
            vmIndexSecondarySchoolProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeries("englishlevel", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexSecondarySchoolProfilesModel.dataTableEnglishLevel = GenerateTransposedTable(CreateDataTable(temp, vmIndexSecondarySchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage"));
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableEthnicBackground = GenerateTransposedTable(CreateDataTable(temp, vmIndexSecondarySchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage"));
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableNationality = GenerateTransposedTable(CreateDataTable(temp, vmIndexSecondarySchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage"));
            //setting Stage data and table
            temp = GetDataSeries("stage", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexSecondarySchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeries("freemeal", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableFreeSchoolMeal = GenerateTransposedTable(CreateDataTable(temp, vmIndexSecondarySchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage"));

            //setting LookAfter data and table
            temp = GetDataSeries("lookafter", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableLookedAfter = GenerateTransposedTable(CreateDataTaleWithTotal(temp, vmIndexSecondarySchoolProfilesModel.DicLookedAfter, "Looked After Children", "no+%"));
            //Attendance
            vmIndexSecondarySchoolProfilesModel.showTableAttendance = listAaeAttendancelists.Count == 0 ? false : true;
            temp = listAaeAttendancelists.Count == 0 ? new List<DataSeries>() : GetAaeAttendanceDataSeries("attendance", listAaeAttendancelists, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesAttendance = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableAttendance = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "School Attendance", "percentage"));

            //Exclusion
            vmIndexSecondarySchoolProfilesModel.showTableExclusion = listExclusionPupils.Count == 0 ? false : true;
            temp = listExclusionPupils.Count == 0 ? new List<DataSeries>() : GetExclusionDataSeries("exclusion", listExclusionPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesExclusion = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableExclusion = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "Exclusions-Annual", "number"));

            //Budget
            temp = GetBudgetDataSeries(templistSchoolname, listSelectedSchoolname);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesBudget = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableBudget = GenerateTransposedTable(CreatePIPSDataTable(temp, "School Cost"));
            //schoolRoll
            temp = GetSchoolRollDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSecondarySchoolProfilesModel.listDataSeriesSchoolRoll = temp;
            vmIndexSecondarySchoolProfilesModel.dataTableSchoolRoll = GenerateTransposedTable(CreatePIPSDataTable(temp, "School Roll"));


            Session["vmIndexSecondarySchoolProfilesModel"] = vmIndexSecondarySchoolProfilesModel;
            return View("IndexSecondarySchool", vmIndexSecondarySchoolProfilesModel);
        }

        public ActionResult GetSpecialProfileData(string sSchoolType)
        {
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<School> listSelectedSchoolname = new List<School>();
            List<AaeAttendanceObj> listAaeAttendancelists = new List<AaeAttendanceObj>();
            List<ExclusionStudentObj> listExclusionPupils = new List<ExclusionStudentObj>();

            bool schoolIsSelected = false;
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexSpecialSchoolProfilesModel = Session["vmIndexSpecialSchoolProfilesModel"] as IndexSecondarySchoolProfilesViewModel;
            List<School> templistSchoolname = vmIndexSpecialSchoolProfilesModel.listSchoolname;
            List<Year> templistYears = vmIndexSpecialSchoolProfilesModel.listYears;

            if (Request["listSelectedSchoolname"] != null)
            {
                schoolIsSelected = true;
                //get CostCentreKey from dropdownlist in UI
                templistSelectedSchoolname = Request["listSelectedSchoolname"].Split(',').ToList();
                //select selected CostCentre from dropdownlist in UI
                listSelectedSchoolname = templistSchoolname.Where(x => templistSelectedSchoolname.Any(y => y.Contains(x.seedcode))).ToList();
            }

            if (Request["stringYear"] != null)
            {
                yesrIsSelected = true;
                string year = Request["stringYear"].ToString();
                vmIndexSpecialSchoolProfilesModel.stringYear = year;
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }

            if (schoolIsSelected && yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
                listAaeAttendancelists = GetAaeAttendanceLists(rpGeneric2nd, sSchoolType, selectedYear, listSelectedSchoolname, listAllPupils);
                listExclusionPupils = GetListExclusionPupils(rpGeneric2nd, selectedYear, sSchoolType);

            }

            //create profiletitle
            string tempProfiletitle = null;
            foreach (var item in listSelectedSchoolname)
            {
                if (String.IsNullOrEmpty(tempProfiletitle))
                {
                    tempProfiletitle = item.name;
                }
                else
                {
                    tempProfiletitle = tempProfiletitle + " / " + item.name;

                }


            }



            vmIndexSpecialSchoolProfilesModel.profiletitle = tempProfiletitle;

            //store selected school into view model
            vmIndexSpecialSchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexSpecialSchoolProfilesModel.selectedYear = selectedYear;
            vmIndexSpecialSchoolProfilesModel.listAllPupils = listAllPupils;
            //setting english data and table
            List<DataSeries> temp = GetDataSeries("englishlevel", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexSpecialSchoolProfilesModel.dataTableEnglishLevel = GenerateTransposedTable(CreateDataTable(temp, vmIndexSpecialSchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage"));
            //setting ethnic data and table
            temp = GetDataSeries("ethnicity", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableEthnicBackground = GenerateTransposedTable(CreateDataTable(temp, vmIndexSpecialSchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage"));
            //setting Nationality data and table
            temp = GetDataSeries("nationality", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableNationality = GenerateTransposedTable(CreateDataTable(temp, vmIndexSpecialSchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage"));
            //setting Stage data and table
            temp = GetDataSeries("stage", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexSpecialSchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetDataSeries("freemeal", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableFreeSchoolMeal = GenerateTransposedTable(CreateDataTable(temp, vmIndexSpecialSchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage"));

            //setting LookAfter data and table
            temp = GetDataSeries("lookafter", listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableLookedAfter = GenerateTransposedTable(CreateDataTaleWithTotal(temp, vmIndexSpecialSchoolProfilesModel.DicLookedAfter, "Looked After Children", "no+%"));

            //Attendance
            vmIndexSpecialSchoolProfilesModel.showTableAttendance = listAaeAttendancelists.Count == 0 ? false : true;
            temp = listAaeAttendancelists.Count == 0 ? new List<DataSeries>() : GetAaeAttendanceDataSeries("attendance", listAaeAttendancelists, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesAttendance = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableAttendance = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "School Attendance", "percentage"));

            //Exclusion
            vmIndexSpecialSchoolProfilesModel.showTableExclusion = listExclusionPupils.Count == 0 ? false : true;
            temp = listExclusionPupils.Count == 0 ? new List<DataSeries>() : GetExclusionDataSeries("exclusion", listExclusionPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexSpecialSchoolProfilesModel.listDataSeriesExclusion = temp;
            vmIndexSpecialSchoolProfilesModel.dataTableExclusion = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "Exclusions-Annual", "number"));


            Session["vmIndexSpecialSchoolProfilesModel"] = vmIndexSpecialSchoolProfilesModel;
            return View("IndexSpecialSchool", vmIndexSpecialSchoolProfilesModel);
        }


        public ActionResult GetPrimaryListpupils(string datatitle, string Indexrow, string Indexcol)
        {
            PupilsListViewModel vmPupilsListViewModel = new PupilsListViewModel();
            //pull data from session that has been stored in GetPrimaryProfileData function
            vmIndexPrimarySchoolProfilesModel = Session["vmIndexPrimarySchoolProfilesModel"] as IndexPrimarySchoolProfilesViewModel;
            List<DataSeries> listAllPupils = new List<DataSeries>();
            List<StudentObj> listtempPupilData = new List<StudentObj>();
            DataTable dataTable = new DataTable();
            string colName = "";
            string catagory = "";
            string code = "";
            string title = "";

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            switch (datatitle)
            {
                case "englishlevel":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesEnglishLevel;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevel;
                    //colName = dataTable.Rows[Convert.ToInt16(Indexrow)][0].ToString();
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicEnglishLevel;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Level of English";
                    break;

                case "ethnicity":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesEthnicBackground;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackground;
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicEthnicBG;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Ethnicity";
                    break;

                case "nationality":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableNationality;
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicNationalIdentity;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Nationality";
                    break;

                case "stage":
                    listAllPupils = vmIndexPrimarySchoolProfilesModel.listDataSeriesStage;
                    dataTable = vmIndexPrimarySchoolProfilesModel.dataTableStage;
                    colName = dataTable.Columns[Convert.ToInt16(Indexcol)].ToString();
                    dictionary = vmIndexPrimarySchoolProfilesModel.DicStage;
                    code = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Key; //get englishlevelcode
                    catagory = dictionary.FirstOrDefault(x => x.Value.Contains(colName)).Value;
                    //query to get pupils list by code from DataSeries
                    listtempPupilData = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).listdataitems.Where(x => x.itemcode.Equals(code)).FirstOrDefault().liststudents;
                    title = "Satge";
                    break;


            }
            vmPupilsListViewModel.listPupils = listtempPupilData;
            vmPupilsListViewModel.school = listAllPupils.ElementAt(Convert.ToInt16(Indexrow)).school;
            vmPupilsListViewModel.catagory = catagory;
            vmPupilsListViewModel.datatile = title;
            return View("Pupilslist", vmPupilsListViewModel);
        }

        public ActionResult GetTrendData(string sSchoolType, string datatitle, string sSchoolName)
        {
            TredningViewModel vmTrendingModel = new TredningViewModel();
            vmTrendingModel.listYear = GetListYear();

            //declare variable
            List<StudentObj> listAllPupils = new List<StudentObj>();
            List<DataSeries> tempDataSeries = new List<DataSeries>();
            List<List<DataSeries>> listobject = new List<List<DataSeries>>();
            //get school from list of primaryschool based on schoolname
            List<School> listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            List<School> school = listSchoolname.Where(x => x.name.Equals(sSchoolName)).ToList();

            
            string tabletitle = "";
            string datashowtype = "percentage";
            
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            //get dictionary based on dataset
            switch (datatitle)
            {
                case "englishlevel":
                    dictionary = GetDicEnglisheLevel(rpGeneric2nd);
                    tabletitle = "Level of English";
                    break;
                case "ethnicity":
                    dictionary = GetDicEhtnicBG(rpGeneric2nd);
                    tabletitle = "Ethnicity";
                    break;
                case "nationality":
                    dictionary = GetDicNationalIdenity(rpGeneric2nd);
                    tabletitle = "Nationality";
                    break;
                case "stage":
                    dictionary = GetDicStage(rpGeneric2nd, sSchoolType);
                    tabletitle = "Stage";
                    datashowtype = "number";
                    break;   
            }

            foreach (var item in vmTrendingModel.listYear) {
                listAllPupils = GetListAllPupils(rpGeneric2nd, item, sSchoolType);
                tempDataSeries = GetDataSeries(datatitle, listAllPupils, school, item, sSchoolType);
                listobject.Add(tempDataSeries);
            
            }

            //create dataTable for school
            foreach (var item in listobject)
            {
                List<double> tdata = new List<double>();
                foreach (var obj in item)
                {
                    tdata.Add(obj.percentage);
                }
                //tempChartdata.Add(new ChartData(item.sc)


            }





            if (sSchoolName.Equals("Aberdeen City") && school.Count() == 0)
            {
                school = new List<School>() { new School("Aberdeen City", "Aberdeen City") };
            }
            vmTrendingModel.school = school;
            //vmTrendingModel.listDataSeries = listobject;
            vmTrendingModel.dataTableSchool = GenerateTransposedTable(CreateDataTable(listobject, dictionary, tabletitle, datashowtype));
            vmTrendingModel.datatitle = tabletitle;
            
            


            return View("Trending", vmTrendingModel);
        }

        private List<DataSeries> CreateDataSeries(string datatitle, List<StudentObj> listPupilData, List<School> listSelectedSchool, Year iyear)
        {
            List<DataSeries> listobject = new List<DataSeries>();
            //get dataSeries for each School
            List<DataSeries> temp = GetDataSeries(datatitle, listPupilData, listSelectedSchool, iyear, "2");
            //temp.Add(GetDataSeries(datatitle, listPupilData, listSelectedSchool, iyear);)

            return listobject;
        
        }

        
    }
}