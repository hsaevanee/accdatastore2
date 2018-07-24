using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.InCAS;
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
    public class IndexPrimaryProfileController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexPrimaryProfileController));

        private readonly IGenericRepository2nd rpGeneric2nd;
        
        private IndexPrimarySchoolProfilesViewModel vmIndexPrimarySchoolProfilesModel;

        public IndexPrimaryProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd; //connect to accdatastore database in MySQL
            this.vmIndexPrimarySchoolProfilesModel = new IndexPrimarySchoolProfilesViewModel();
        }
        /*
        private void initData() {
            if (listeal == null || listeal.Count == 0)
            {
                List<ViewObj> temp = new List<ViewObj>();
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, Seedcode, SchoolType, code, count from summary_levelofenglish");
                foreach (var itemrow in listResult)
                {
                    if (itemrow != null)
                        temp.Add(new ViewObj(itemrow[0].ToString(), itemrow[1].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), Int32.Parse(itemrow[4].ToString())));

                }
                listeal = (from tempa in temp where tempa.schooltype == "2" select tempa).ToList();

            }

            if (listethnic ==null || listethnic.Count == 0)
            {
                List<ViewObj> temp = new List<ViewObj>();
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, Seedcode, SchoolType,code, count from summary_ethnicbackground");
                foreach (var itemrow in listResult)
                {
                    if (itemrow != null)
                        temp.Add(new ViewObj(itemrow[0].ToString(), itemrow[1].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), Int32.Parse(itemrow[4].ToString())));

                }
                listethnic = (from tempa in temp where tempa.schooltype == "2" select tempa).ToList();
            }

            if (listnational == null || listnational.Count == 0)
            {
                listnational = new List<ViewObj>();
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, Seedcode, SchoolType,code, count from summary_nationality");
                foreach (var itemrow in listResult)
                {
                    if (itemrow != null)
                        listnational.Add(new ViewObj(itemrow[0].ToString(), itemrow[1].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), Int32.Parse(itemrow[4].ToString())));

                }

            }
            if (listneedtype == null || listneedtype.Count == 0)
            {
                listneedtype = new List<ViewObj>();
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, Seedcode, SchoolType,code, count from summary_needtype");
                foreach (var itemrow in listResult)
                {
                    if (itemrow != null)
                        listneedtype.Add(new ViewObj(itemrow[0].ToString(), itemrow[1].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), Int32.Parse(itemrow[4].ToString())));

                }

            }
            if (liststdlookedafter == null || liststdlookedafter.Count == 0)
            {
                liststdlookedafter = new List<ViewObj>();
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, Seedcode, SchoolType,code, count from summary_studentlookedafter");
                foreach (var itemrow in listResult)
                {
                    if (itemrow != null)
                        liststdlookedafter.Add(new ViewObj(itemrow[0].ToString(), itemrow[1].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), Int32.Parse(itemrow[4].ToString())));

                }

            }
            //listResult = rpGeneric2nd.FindByNativeSQL("Select * from view_incas");
            //listResult = rpGeneric2nd.FindByNativeSQL("Select * from view_pips");

            if (liststdstage == null || liststdstage.Count == 0)
            {
                liststdstage = new List<ViewObj>();
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, Seedcode, SchoolType, code, count from summary_studentstage");
                foreach (var itemrow in listResult)
                {
                    if (itemrow != null)
                        liststdstage.Add(new ViewObj(itemrow[0].ToString(), itemrow[1].ToString(), itemrow[2].ToString(), itemrow[3].ToString(), Int32.Parse(itemrow[4].ToString())));

                }

            }

        }
         */
        
        [AdminAuthentication]
        [Transactional]
        public ActionResult IndexPrimaryProfiles(string sSchoolType)
        {
            //get data ready for set up profiles
            vmIndexPrimarySchoolProfilesModel.listSchoolname = GetListSchool(rpGeneric2nd, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listSelectedSchoolname = vmIndexPrimarySchoolProfilesModel.listSchoolname.Where(x => x.seedcode.Equals("5237521")).ToList();
            vmIndexPrimarySchoolProfilesModel.listYears = GetListYear();
            vmIndexPrimarySchoolProfilesModel.DicEnglishLevel = GetDicEnglisheLevel(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicEthnicBG = GetDicEhtnicBG(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicNationalIdentity = GetDicNationalIdenity(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.DicStage = GetDicStage(rpGeneric2nd, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.DicFreeMeal = GetDicFreeSchoolMeal();
            vmIndexPrimarySchoolProfilesModel.DicLookedAfter = GetDicLookAfter();
            vmIndexPrimarySchoolProfilesModel.DicSIMDDecile = GetDicSIMDDecile();
            //vmIndexPrimarySchoolProfilesModel.DicAttendance = GetDicAttendance(rpGeneric2nd);
            vmIndexPrimarySchoolProfilesModel.selectedYear = new Year("2015");
            vmIndexPrimarySchoolProfilesModel.showTableInCAS = null;
            vmIndexPrimarySchoolProfilesModel.stringYear = "2016";
            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        // GET: SchoolProfiles/IndexPrimaryProfile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPrimaryProfileData(string sSchoolType)
        {
            //process data using Mysql view
            List<string> templistSelectedSchoolname = new List<string>();
            List<StudentObj> listAllPupils = new List<StudentObj>();
            //List<PIPSObj> listPIPSPupils = new List<PIPSObj>();
            List<ViewPIPSObj> listViewPIPSObj = new List<ViewPIPSObj>();
            List<InCASObj> listInCASPupils = new List<InCASObj>();
            List<ViewObj> listViewStdStage = new List<ViewObj>();
            List<ViewObj> listViewEthnicbackground = new List<ViewObj>();
            List<ViewObj> listViewEAL = new List<ViewObj>();
            List<ViewObj> listViewNationality = new List<ViewObj>();
            List<ViewObj> listViewLookedAfter = new List<ViewObj>(); 
            List<ViewInCASObj> listViewInCASObj = new List<ViewInCASObj>();
            List<AaeAttendanceObj> listAaeAttendancelists = new List<AaeAttendanceObj>();
            List<ExclusionStudentObj> listExclusionPupils = new List<ExclusionStudentObj>();
            List<StudentNeedObj> listStudentNeed = new List<StudentNeedObj>();
            List<List<StudentObj>> templistAllPupils = new List<List<StudentObj>>();
            List<School> listSelectedSchoolname = new List<School>();
            bool schoolIsSelected = false;
            bool yesrIsSelected = false;
            Year selectedYear = null;

            vmIndexPrimarySchoolProfilesModel = Session["vmIndexPrimarySchoolProfilesModel"] as IndexPrimarySchoolProfilesViewModel;
            List<School> templistSchoolname = vmIndexPrimarySchoolProfilesModel.listSchoolname;
            List<Year> templistYears = vmIndexPrimarySchoolProfilesModel.listYears;


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
                vmIndexPrimarySchoolProfilesModel.stringYear = year;
                selectedYear = templistYears.Where(x => x.year.Contains(year)).FirstOrDefault();
            }


            if (schoolIsSelected && yesrIsSelected)
            {
                listAllPupils = GetListAllPupils(rpGeneric2nd, selectedYear, sSchoolType);
                listViewPIPSObj = GetPIPSPupils(rpGeneric2nd);//rpGeneric2nd.FindAll<PIPS>().ToList<PIPSObj>(); 
                //listPIPSPupils = GetPIPSPupils(rpGeneric2nd, selectedYear, listSelectedSchoolname);
                listInCASPupils = GetInCASPupils(rpGeneric2nd, selectedYear, listSelectedSchoolname);
                listViewInCASObj = GetInCASViewdata(rpGeneric2nd);
                listViewStdStage = GetListViewObj(rpGeneric2nd, sSchoolType,"stage");
                listViewEthnicbackground = GetListViewObj(rpGeneric2nd, sSchoolType, "ethnicbackground");
                listViewNationality = GetListViewObj(rpGeneric2nd, sSchoolType, "nationality");
                listViewEAL = GetListViewObj(rpGeneric2nd, sSchoolType,"eal");
                //listViewStudentNeed= GetListViewObj(rpGeneric2nd, sSchoolType, "needtype");
                listViewLookedAfter = GetListViewObj(rpGeneric2nd, sSchoolType, "lookedafter");
                listAaeAttendancelists = GetAaeAttendanceLists(rpGeneric2nd, sSchoolType, selectedYear, listSelectedSchoolname, listAllPupils);
                listExclusionPupils = GetListExclusionPupils(rpGeneric2nd, selectedYear, sSchoolType);
                listStudentNeed = GetStudentNeedLists(rpGeneric2nd, sSchoolType, selectedYear);

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
            vmIndexPrimarySchoolProfilesModel.profiletitle = tempProfiletitle;

            //store selected school into view model
            vmIndexPrimarySchoolProfilesModel.listSelectedSchoolname = listSelectedSchoolname;
            vmIndexPrimarySchoolProfilesModel.selectedYear = selectedYear;
            //vmIndexPrimarySchoolProfilesModel.listAllPupils = listAllPupils;
            //vmIndexPrimarySchoolProfilesModel.listPIPSPupils = listPIPSPupils;
            //setting english data and table
            List<DataSeries> temp = GetEnglishLevelDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesEnglishLevel = temp;
            //vmIndexPrimarySchoolProfilesModel.englishLevelDataTable = GenerateTransposedTable(CreateDataTale(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English"));
            vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevel = GenerateTransposedTable(CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage"));
            temp = GetHistoricEALObjDataSeries(listViewEAL, listSelectedSchoolname, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, templistYears, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesHistoryEnglishLevel = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevelHistory = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicEnglishLevel, "Level of English", "percentage");
            
            //setting ethnic data and table
            temp = GetEthnicityDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesEthnicBackground = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackground = GenerateTransposedTable(CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage"));
            temp = GetHistoricViewObjDataSeries(listViewEthnicbackground, listSelectedSchoolname, templistYears, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackgroundHistory = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicEthnicBG, "Ethnicity", "percentage");

            //setting Nationality data and table
            temp = GetNationalityDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesNationality = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableNationality = GenerateTransposedTable(CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage"));
            temp = GetHistoricViewObjDataSeries(listViewNationality, listSelectedSchoolname, templistYears, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.dataTableNationalityHistory = CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicNationalIdentity, "Nationality", "percentage");

            //setting Stage data and table
            temp = GetStageDataSeries(listViewStdStage, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesStage = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableStage = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicStage, "Stage", "number");
            temp = GetHistoricViewObjDataSeries(listViewStdStage, listSelectedSchoolname, templistYears, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.dataTableStageHistory = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicStage, "Stage", "number");
            //setting FreeSchoolMeal data and table
            temp = GetFreeMealDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesFreeMeal = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableFreeSchoolMeal = GenerateTransposedTable(CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicFreeMeal, "Free School Meal Entitlement", "percentage"));
            //setting LookAfter data and table
            temp = GetLookAfterDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesLookedAfter = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableLookedAfter = GenerateTransposedTable(CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicLookedAfter, "Looked After Children", "no+%"));
            temp = GetHistoricViewObjDataSeries(listViewLookedAfter, listSelectedSchoolname, templistYears, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.dataTableLookedAfterHistory = CreateDataTaleWithTotal(temp, vmIndexPrimarySchoolProfilesModel.DicLookedAfter, "Looked After Children", "number");

            //setting SIMD data and table
            temp = GetSIMDDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesSIMDDecile = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableSIMDDecile = temp == null ? null : CreateDataTable(temp, vmIndexPrimarySchoolProfilesModel.DicSIMDDecile, "SIMD_Decile", "percentage");

            //setting PIPS dataseries and datatable         
            temp = GetPIPsDataSeries(listViewPIPSObj, listSelectedSchoolname, selectedYear);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesPIPS = temp;
            vmIndexPrimarySchoolProfilesModel.dataTablePIPS = GenerateTransposedTable(CreatePIPSDataTable(temp, "P1"));

            temp = GetPIPsHistoricDataSeries(listViewPIPSObj, listSelectedSchoolname, templistYears);
            vmIndexPrimarySchoolProfilesModel.dataTablePIPShistoric = CreatePIPSDataTable(temp, "P1");

            //setting InCAS dataseries and datatable     

            vmIndexPrimarySchoolProfilesModel.showTableInCAS = listInCASPupils.Count == 0 ? false : true;
            temp = listInCASPupils.Count == 0 ? new List<DataSeries>() : GetInCASDataSeries(listViewInCASObj, listSelectedSchoolname, selectedYear, "1");
            vmIndexPrimarySchoolProfilesModel.listDataSeriesInCASP2 = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableInCASP2 = temp.Count == 0 ? null : GenerateTransposedTable(CreateInCASDataTable(temp, "P2"));

            temp = listInCASPupils.Count == 0 ? new List<DataSeries>() : GetInCASDataSeries(listViewInCASObj, listSelectedSchoolname, selectedYear, "3");
            vmIndexPrimarySchoolProfilesModel.listDataSeriesInCASP4 = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableInCASP4 = temp.Count == 0 ? null : GenerateTransposedTable(CreateInCASDataTable(temp, "P4"));

            temp = listInCASPupils.Count == 0 ? new List<DataSeries>() : GetInCASDataSeries(listViewInCASObj, listSelectedSchoolname, selectedYear, "5");
            vmIndexPrimarySchoolProfilesModel.listDataSeriesInCASP6 = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableInCASP6 = temp.Count == 0 ? null : GenerateTransposedTable(CreateInCASDataTable(temp, "P6"));

            //Attendance
            vmIndexPrimarySchoolProfilesModel.showTableAttendance = listAaeAttendancelists.Count == 0 ? false : true;
            temp = listAaeAttendancelists.Count == 0 ? new List<DataSeries>() : GetAaeAttendanceDataSeries("attendance", listAaeAttendancelists, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesAttendance = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableAttendance = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "School Attendance", "percentage"));

            //Exclusion
            vmIndexPrimarySchoolProfilesModel.showTableExclusion = listExclusionPupils.Count == 0 ? false : true;
            temp = listExclusionPupils.Count == 0 ? new List<DataSeries>() : GetExclusionDataSeries("exclusion", listExclusionPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesExclusion = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableExclusion = temp.Count == 0 ? null : GenerateTransposedTable(CreateDataTable(temp, "Exclusions-Annual", "number"));

            //Budget
            temp = GetBudgetDataSeries(templistSchoolname, listSelectedSchoolname);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesBudget = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableBudget = GenerateTransposedTable(CreatePIPSDataTable(temp, "School Cost"));

            //schoolRoll
            temp = GetSchoolRollDataSeries(listAllPupils, listSelectedSchoolname, selectedYear, sSchoolType);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesSchoolRoll = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableSchoolRoll = GenerateTransposedTable(CreatePIPSDataTable(temp, "School Roll"));

            temp = GetSRForecastDataSeries(rpGeneric2nd, listSelectedSchoolname);
            vmIndexPrimarySchoolProfilesModel.listDataSeriesSchoolRollForecast = temp;
            vmIndexPrimarySchoolProfilesModel.dataTableSchoolRollForecast = temp == null ? null : CreatePIPSDataTable(temp, "School Roll Forecast");



            Session["vmIndexPrimarySchoolProfilesModel"] = vmIndexPrimarySchoolProfilesModel;
            return View("IndexPrimarySchool", vmIndexPrimarySchoolProfilesModel);
        }

        public ActionResult GetSessionPrimaryProfileData(string datatitle)
        {
            IndexPrimarySchoolProfilesViewModel vmIndexPrimarySchoolProfilesModel = Session["vmIndexPrimarySchoolProfilesModel"] as IndexPrimarySchoolProfilesViewModel;
            TredningViewModel vmTrendingModel = new TredningViewModel();
            switch (datatitle)
            {
                case "englishlevel":
                    vmTrendingModel.dataTable = vmIndexPrimarySchoolProfilesModel.dataTableEnglishLevel;
                    vmTrendingModel.datatitle = "Level of English";
                    break;
                case "ethnicity":
                    vmTrendingModel.dataTable = vmIndexPrimarySchoolProfilesModel.dataTableEthnicBackground;
                    vmTrendingModel.datatitle = "Ethnicity";
                    break;
                case "nationality":
                    vmTrendingModel.dataTable = vmIndexPrimarySchoolProfilesModel.dataTableNationality;
                    vmTrendingModel.datatitle = "Nationality";
                    break;
                case "stage":
                    vmTrendingModel.dataTable = vmIndexPrimarySchoolProfilesModel.dataTableStage;
                    vmTrendingModel.datatitle = "Stage";

                    break;
            }

            return View("data", vmTrendingModel);
        }

        protected List<DataSeries> GetPIPsHistoricDataSeries(List<ViewPIPSObj> listData, List<School> listSelectedSchool, List<Year> listyear)
        {
            List<ViewPIPSObj> listtempData = new List<ViewPIPSObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();
            //calculate individual school
            foreach (Year iyear in listyear) 
            {
                foreach (School item in listSelectedSchool)
                {
                   // listtempData = listData.Where(x => x.seedcode.Equals(item.seedcode) && x.year.year.Equals(iyear.year)).ToList();

                    listtempData = listData.Where(x => x.seedcode.Equals(item.seedcode) && x.year.year.Equals(iyear.year)).ToList();

                    if (listtempData.Count > 0)
                    {
                        listResult = new List<PIPSObjDetail>();
                        listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(x => x.sreading).Average() });
                        listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(x => x.smath).Average() });
                        listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(x => x.sphonics).Average() });
                        listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(x => x.stotal).Average() });
                        listobject.Add(new DataSeries { dataSeriesNames = "Start P1 " +iyear.year, school = item, year = iyear, listPIPSdataitems = listResult });
                        listResult = new List<PIPSObjDetail>();
                        //calculate School End P1
                        listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(x => x.ereading).Average() });
                        listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(x => x.emath).Average() });
                        listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(x => x.ephonics).Average() });
                        listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(x => x.etotal).Average() });
                        listobject.Add(new DataSeries { dataSeriesNames = "End P1 " +iyear.year, school = item, year = iyear, listPIPSdataitems = listResult });
                    }
                    //else
                    //{
                    //    listobject.Add(new DataSeries { dataSeriesNames = "Start P1", school = item, year = iyear, listPIPSdataitems = new List<PIPSObjDetail>() });
                    //    listobject.Add(new DataSeries { dataSeriesNames = "End P1", school = item, year = iyear, listPIPSdataitems = new List<PIPSObjDetail>() });
                    //}
                
                }

                //aberdeen city
                listtempData = listData.Where(x => x.year.year.Equals(iyear.year)).ToList();
                if (listtempData.Count > 0)
                {
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(r => r.sreading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(r => r.smath).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(r => r.sphonics).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(r => r.stotal).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "Start P1 " + iyear.year, school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(r => r.ereading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(r => r.emath).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(r => r.ephonics).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(r => r.etotal).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "End P1 " + iyear.year, school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                }
                //else
                //{
                //    listobject.Add(new DataSeries { dataSeriesNames = "Start P1", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = new List<PIPSObjDetail>() });
                //    listobject.Add(new DataSeries { dataSeriesNames = "End P1", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = new List<PIPSObjDetail>() });

                //}
            
            }

            return listobject;
        }

        protected List<DataSeries> GetPIPsDataSeries(List<ViewPIPSObj> listData, List<School> listSelectedSchool, Year iyear)
        {
            List<ViewPIPSObj> listtempData = new List<ViewPIPSObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();
            //calculate individual school

            foreach (School item in listSelectedSchool)
            {
                //select data for each school
                listtempData = listData.Where(x => x.seedcode.Equals(item.seedcode) && x.year.year.Equals(iyear.year)).ToList();
                //calculate School Start P1
                if (listtempData.Count > 0)
                {
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(x => x.sreading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(x => x.smath).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(x => x.sphonics).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(x => x.stotal).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "Start P1 " + iyear.year, school = item, year = iyear, listPIPSdataitems = listResult });
                    listResult = new List<PIPSObjDetail>();
                    //calculate School End P1
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(x => x.ereading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(x => x.emath).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(x => x.ephonics).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(x => x.etotal).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "End P1 " + iyear.year, school = item, year = iyear, listPIPSdataitems = listResult });
                }
                else
                {
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = null });
                    listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = null });
                    listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = null });
                    listResult.Add(new PIPSObjDetail { dataName = "Total", average = null });
                    listobject.Add(new DataSeries { dataSeriesNames = "Start P1 " + iyear.year, school = item, year = iyear, listPIPSdataitems = listResult });
                    listResult = new List<PIPSObjDetail>();
                    //calculate School End P1
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = null });
                    listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = null });
                    listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = null });
                    listResult.Add(new PIPSObjDetail { dataName = "Total", average = null });
                    listobject.Add(new DataSeries { dataSeriesNames = "End P1 " + iyear.year, school = item, year = iyear, listPIPSdataitems = listResult });


                }
            }

            listtempData = listData.Where(x => x.year.year.Equals(iyear.year)).ToList();
            if (listtempData.Count > 0)
            {
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(r => r.sreading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(r => r.smath).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(r => r.sphonics).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(r => r.stotal).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Start P1 " + iyear.year, school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempData.Select(r => r.ereading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = listtempData.Select(r => r.emath).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = listtempData.Select(r => r.ephonics).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Total", average = listtempData.Select(r => r.etotal).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "End P1 " + iyear.year, school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            }
            else
            {
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = null });
                listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = null });
                listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = null });
                listResult.Add(new PIPSObjDetail { dataName = "Total", average = null });
                listobject.Add(new DataSeries { dataSeriesNames = "Start P1 " + iyear.year, school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                listResult = new List<PIPSObjDetail>();
                //calculate School End P1
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = null });
                listResult.Add(new PIPSObjDetail { dataName = "Mathematics", average = null });
                listResult.Add(new PIPSObjDetail { dataName = "Phonics", average = null });
                listResult.Add(new PIPSObjDetail { dataName = "Total", average = null });
                listobject.Add(new DataSeries { dataSeriesNames = "End P1 " + iyear.year, school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });


            }

            return listobject;
        }

        protected List<ViewPIPSObj> GetPIPSPupils(IGenericRepository2nd rpGeneric2nd)
        {
            List<ViewPIPSObj> listResult = new List<ViewPIPSObj>();
            ViewPIPSObj tempobj = new ViewPIPSObj();
            var listtemp = rpGeneric2nd.FindByNativeSQL("Select * from view_pipsall");
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    tempobj = new ViewPIPSObj();
                    tempobj.year = new Year(itemrow[0].ToString());
                    tempobj.seedcode = itemrow[1].ToString();
                    tempobj.sreading = Convert.ToDouble(itemrow[2].ToString());
                    tempobj.smath = Convert.ToDouble(itemrow[3].ToString());
                    tempobj.sphonics = Convert.ToDouble(itemrow[4].ToString());
                    tempobj.stotal = Convert.ToDouble(itemrow[5].ToString());
                    tempobj.ereading = Convert.ToDouble(itemrow[6].ToString());
                    tempobj.emath = Convert.ToDouble(itemrow[7].ToString());
                    tempobj.ephonics = Convert.ToDouble(itemrow[8].ToString());
                    tempobj.etotal = Convert.ToDouble(itemrow[9].ToString());

                    listResult.Add(tempobj);
                }
            }

            return listResult;

        }
       
        protected List<PIPSObj> GetPIPSPupils(IGenericRepository2nd rpGeneric2nd, Year year, List<School> schools)
        {
            List<PIPSObj> listResult = new List<PIPSObj>();
            switch (year.year)
            {
                case "2008":
                    listResult = rpGeneric2nd.FindAll<PIPS2008>().ToList<PIPSObj>();
                    break;
                case "2009":
                    listResult = rpGeneric2nd.FindAll<PIPS2009>().ToList<PIPSObj>();
                    break;
                case "2010":
                    listResult = rpGeneric2nd.FindAll<PIPS2010>().ToList<PIPSObj>();
                    break;
                case "2011":
                    listResult = rpGeneric2nd.FindAll<PIPS2011>().ToList<PIPSObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<PIPS2012>().ToList<PIPSObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<PIPS2013>().ToList<PIPSObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<PIPS2014>().ToList<PIPSObj>();
                    break;
                case "2015":
                    listResult = rpGeneric2nd.FindAll<PIPS2015>().ToList<PIPSObj>();
                    break;
            }

            return listResult;

        }

        protected List<ViewInCASObj> GetInCASViewdata(IGenericRepository2nd rpGeneric2nd)
        {
            List<ViewInCASObj> listResult = new List<ViewInCASObj>();
            ViewInCASObj tempobj = new ViewInCASObj();
            var listtemp = rpGeneric2nd.FindByNativeSQL("Select * from view_incas where YearGroup in (1,3,5)");
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    tempobj = new ViewInCASObj();
                    tempobj.year = new Year(itemrow[0].ToString());
                    tempobj.seedcode = itemrow[1].ToString();
                    tempobj.YearGroup = itemrow[2].ToString();
                    tempobj.AgeDiff_DevAbil = itemrow[3]== null? double.NaN:Convert.ToDouble(itemrow[3].ToString());
                    tempobj.AgeDiff_GenMaths = itemrow[4] == null ? double.NaN : Convert.ToDouble(itemrow[4].ToString());
                    tempobj.AgeDiff_MentArith = itemrow[5] == null ? double.NaN : Convert.ToDouble(itemrow[5].ToString());
                    tempobj.AgeDiff_Reading = itemrow[6] == null ? double.NaN : Convert.ToDouble(itemrow[6].ToString());
                    tempobj.AgeDiff_Spelling = itemrow[7] == null ? double.NaN : Convert.ToDouble(itemrow[7].ToString());
                    tempobj.Standardised_DevAbil = itemrow[8] == null ? double.NaN : Convert.ToDouble(itemrow[8].ToString());
                    tempobj.Standardised_GenMaths = itemrow[9] == null ? double.NaN : Convert.ToDouble(itemrow[9].ToString());
                    tempobj.Standardised_MentArith = itemrow[10] == null ? double.NaN : Convert.ToDouble(itemrow[10].ToString());
                    tempobj.Standardised_Reading = itemrow[11] == null ? double.NaN : Convert.ToDouble(itemrow[11].ToString());

                    listResult.Add(tempobj);
                }
            }

            return listResult;

        }

        protected List<DataSeries> GetInCASDataSeries(List<ViewInCASObj> listPupilData, List<School> listSelectedSchool, Year iyear, string YearGroup)
        {
            List<ViewInCASObj> listtempPupilData = new List<ViewInCASObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();

            //select specific YearGroup
            listPupilData = listPupilData.Where(x => x.YearGroup.Equals(YearGroup)).ToList();

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select data for each school
                listtempPupilData = listPupilData.Where(x => x.seedcode.ToString().Equals(item.seedcode) && x.year.year.Equals(iyear.year)).ToList();
                if (listtempPupilData.Count > 0)
                {
                    //calculate AgeDiff_DevAbil
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Select(x => x.AgeDiff_DevAbil).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Select(x => x.AgeDiff_Reading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = listtempPupilData.Select(x => x.AgeDiff_Spelling).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Select(x => x.AgeDiff_GenMaths).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Select(x => x.AgeDiff_MentArith).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = item, year = iyear, listPIPSdataitems = listResult });
                    //calculate Standardised_DevAbil
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Select(x => x.Standardised_DevAbil).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Select(x => x.Standardised_Reading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Select(x => x.Standardised_GenMaths).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Select(x => x.Standardised_MentArith).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = item, year = iyear, listPIPSdataitems = listResult });
                }
                else {

                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = double.NaN });
                    listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = item, year = iyear, listPIPSdataitems = listResult });
                    //calculate Standardised_DevAbil
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = double.NaN });
                    listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = item, year = iyear, listPIPSdataitems = listResult });              
                }

            }

            //calculate for aberdeen city
            //calculate AgeDiff_DevAbil
            //select list of pupils by yeargroup
            listPupilData = listPupilData.Where(x => x.year.year.Equals(iyear.year)).ToList();

                if (listtempPupilData.Count > 0)
                {
                    //calculate AgeDiff_DevAbil
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Select(x => x.AgeDiff_DevAbil).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Select(x => x.AgeDiff_Reading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = listtempPupilData.Select(x => x.AgeDiff_Spelling).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Select(x => x.AgeDiff_GenMaths).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Select(x => x.AgeDiff_MentArith).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                    //calculate Standardised_DevAbil
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Select(x => x.Standardised_DevAbil).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Select(x => x.Standardised_Reading).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Select(x => x.Standardised_GenMaths).Average() });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Select(x => x.Standardised_MentArith).Average() });
                    listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                }
                else {

                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = double.NaN });
                    listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
                    //calculate Standardised_DevAbil
                    listResult = new List<PIPSObjDetail>();
                    listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Reading", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = double.NaN });
                    listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = double.NaN });
                    listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });              
                }

            return listobject;
        }

        protected List<DataSeries> GetInCAShistoricalDataSeries(List<ViewInCASObj> listPupilData, List<School> listSelectedSchool, Year iyear, string YearGroup)
        {
            List<ViewInCASObj> listtempPupilData = new List<ViewInCASObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();

            //select list of pupils by yeargroup
            listPupilData = listPupilData.Where(x => x.YearGroup.Equals(YearGroup)).ToList();

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select data for each school
                listtempPupilData = listPupilData.Where(x => x.seedcode.ToString().Equals(item.seedcode)).ToList();
                //calculate AgeDiff_DevAbil
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = listtempPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Where(x => x.AgeDiff_GenMaths != 0).Select(r => r.AgeDiff_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = item, year = iyear, listPIPSdataitems = listResult });
                //calculate Standardised_DevAbil
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Where(x => x.Standardised_GenMaths != 0).Select(r => r.Standardised_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = item, year = iyear, listPIPSdataitems = listResult });

            }

            //calculate for aberdeen city
            //calculate AgeDiff_DevAbil
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = listPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listPupilData.Where(x => x.AgeDiff_GenMaths != 0).Select(r => r.AgeDiff_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            //calculate Standardised_DevAbil
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
            listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listPupilData.Where(x => x.Standardised_GenMaths != 0).Select(r => r.Standardised_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });


            return listobject;
        }

        protected List<InCASObj> GetInCASPupils(IGenericRepository2nd rpGeneric2nd, Year year, List<School> schools)
        {
            List<InCASObj> listResult = new List<InCASObj>();
            switch (year.year)
            {
                case "2008":
                    listResult = new List<InCASObj>();
                    break;
                case "2009":
                    listResult = new List<InCASObj>();
                    break;
                case "2010":
                    listResult = new List<InCASObj>();
                    break;
                case "2011":
                    listResult = new List<InCASObj>();
                    break;
                case "2012":
                    listResult = rpGeneric2nd.FindAll<InCAS2012>().ToList<InCASObj>();
                    break;
                case "2013":
                    listResult = rpGeneric2nd.FindAll<InCAS2013>().ToList<InCASObj>();
                    break;
                case "2014":
                    listResult = rpGeneric2nd.FindAll<InCAS2014>().ToList<InCASObj>();
                    break;
                case "2015":
                    listResult = rpGeneric2nd.FindAll<InCAS2015>().ToList<InCASObj>();
                    break;
            }

            return listResult;

        }

        protected List<DataSeries> GetInCASDataSeries(List<InCASObj> listPupilData, List<School> listSelectedSchool, Year iyear, string YearGroup)
        {
            List<InCASObj> listtempPupilData = new List<InCASObj>();
            List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
            List<DataSeries> listobject = new List<DataSeries>();
            
            //select list of pupils by yeargroup
            listPupilData = listPupilData.Where(x => x.YearGroup.Equals(YearGroup)).ToList();

            //calculate individual school
            foreach (School item in listSelectedSchool)
            {
                //select data for each school
                listtempPupilData = listPupilData.Where(x => x.SchoolId.ToString().Equals(item.seedcode)).ToList();
                //calculate AgeDiff_DevAbil
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = listtempPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Where(x => x.AgeDiff_GenMaths != 0).Select(r => r.AgeDiff_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = item, year = iyear, listPIPSdataitems = listResult });
                //calculate Standardised_DevAbil
                listResult = new List<PIPSObjDetail>();
                listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listtempPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listtempPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
                listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listtempPupilData.Where(x => x.Standardised_GenMaths != 0).Select(r => r.Standardised_GenMaths).Average() });
                listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listtempPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
                listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = item, year = iyear, listPIPSdataitems = listResult });               

            }

            //calculate for aberdeen city
            //calculate AgeDiff_DevAbil
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = listPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listPupilData.Where(x => x.AgeDiff_GenMaths != 0).Select(r => r.AgeDiff_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Age Diffrence (Yrs)", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
            //calculate Standardised_DevAbil
            listResult = new List<PIPSObjDetail>();
            listResult.Add(new PIPSObjDetail { dataName = "Developed Ability", average = listPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Reading", average = listPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Spelling", average = double.NaN });
            listResult.Add(new PIPSObjDetail { dataName = "General Maths", average = listPupilData.Where(x => x.Standardised_GenMaths != 0).Select(r => r.Standardised_GenMaths).Average() });
            listResult.Add(new PIPSObjDetail { dataName = "Mental Arithmetics", average = listPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
            listobject.Add(new DataSeries { dataSeriesNames = "Standardised Scores", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });               


            return listobject;
        }

        //protected List<DataSeries> GetInCASDataSeries(List<InCASObj> listPupilData, List<School> listSelectedSchool, Year iyear, string YearGroup)
        //{
        //    List<InCASObj> listtempPupilData = new List<InCASObj>();
        //    List<PIPSObjDetail> listResult = new List<PIPSObjDetail>();
        //    List<DataSeries> listobject = new List<DataSeries>();

        //    //select list of pupils by yeargroup
        //    listPupilData = listPupilData.Where(x => x.YearGroup.Equals(YearGroup)).ToList();

        //    //calculate individual school
        //    foreach (School item in listSelectedSchool)
        //    {
        //        //select data for each school
        //        listtempPupilData = listPupilData.Where(x => x.SchoolId.ToString().Equals(item.seedcode)).ToList();
        //        //calculate Develop Ability
        //        listResult = new List<PIPSObjDetail>();
        //        //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listtempPupilData.Where(x => x.AgeAtTest_DevAbil.HasValue == true).Select(r => r.AgeAtTest_DevAbil).Average() });
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listtempPupilData.Where(x => x.AgeAtTest_DevAbil != 0).Select(r => r.AgeAtTest_DevAbil).Average() });
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_DevAbil", average = listtempPupilData.Where(x => x.AgeEquiv_DevAbil.HasValue == true).Select(r => r.AgeEquiv_DevAbil).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_DevAbil", average = listtempPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "Standardised_DevAbil", average = listtempPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });
        //        listobject.Add(new DataSeries { dataSeriesNames = "Develop Ability", school = item, year = iyear, listPIPSdataitems = listResult });
        //        //calculate reading
        //        listResult = new List<PIPSObjDetail>();
        //        //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Reading", average = listtempPupilData.Where(x => x.AgeAtTest_Reading.HasValue == true).Select(r => r.AgeAtTest_Reading).Average() });
        //        //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Reading", average = listtempPupilData.Where(x => x.AgeEquiv_Reading.HasValue == true).Select(r => r.AgeEquiv_Reading).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Reading", average = listtempPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = listtempPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
        //        listobject.Add(new DataSeries { dataSeriesNames = "Reading", school = item, year = iyear, listPIPSdataitems = listResult });
        //        //calculate Spelling
        //        listResult = new List<PIPSObjDetail>();
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Spelling", average = listtempPupilData.Where(x => x.AgeAtTest_Spelling.HasValue == true).Select(r => r.AgeAtTest_Spelling).Average() });
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Spelling", average = listtempPupilData.Where(x => x.AgeEquiv_Spelling.HasValue == true).Select(r => r.AgeEquiv_Spelling).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Spelling", average = listtempPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = double.NaN });
        //        listobject.Add(new DataSeries { dataSeriesNames = "Spelling", school = item, year = iyear, listPIPSdataitems = listResult });
        //        //calculate General Math
        //        listResult = new List<PIPSObjDetail>();
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_GenMaths", average = listtempPupilData.Where(x => x.AgeAtTest_GenMaths.HasValue == true).Select(r => r.AgeAtTest_GenMaths).Average() });
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_GenMaths", average = listtempPupilData.Where(x => x.AgeEquiv_GenMaths.HasValue == true).Select(r => r.AgeEquiv_GenMaths).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_GenMaths", average = listtempPupilData.Where(x => x.AgeDiff_GenMaths.HasValue == true).Select(r => r.AgeDiff_GenMaths).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "Standardised_GenMaths", average = listtempPupilData.Where(x => x.Standardised_GenMaths.HasValue == true).Select(r => r.Standardised_GenMaths).Average() });
        //        listobject.Add(new DataSeries { dataSeriesNames = "General Maths", school = item, year = iyear, listPIPSdataitems = listResult });
        //        //calculate Mental Arithmetics
        //        listResult = new List<PIPSObjDetail>();
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_MentArith", average = listtempPupilData.Where(x => x.AgeAtTest_MentArith.HasValue == true).Select(r => r.AgeAtTest_MentArith).Average() });
        //        // listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_MentArith", average = listtempPupilData.Where(x => x.AgeEquiv_MentArith.HasValue == true).Select(r => r.AgeEquiv_MentArith).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_MentArith", average = listtempPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
        //        listResult.Add(new PIPSObjDetail { dataName = "Standardised_MentArith", average = listtempPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
        //        listobject.Add(new DataSeries { dataSeriesNames = "Mental Arithmetics", school = item, year = iyear, listPIPSdataitems = listResult });

        //    }

        //    //calculate for aberdeen city
        //    listResult = new List<PIPSObjDetail>();
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listPupilData.Where(x => x.AgeAtTest_DevAbil.HasValue == true).Select(r => r.AgeAtTest_DevAbil).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_DevAbil", average = listPupilData.Where(x => x.AgeEquiv_DevAbil.HasValue == true).Select(r => r.AgeEquiv_DevAbil).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_DevAbil", average = listPupilData.Where(x => x.AgeDiff_DevAbil.HasValue == true).Select(r => r.AgeDiff_DevAbil).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "Standardised_DevAbil", average = listPupilData.Where(x => x.Standardised_DevAbil.HasValue == true).Select(r => r.Standardised_DevAbil).Average() });

        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_DevAbil", average = listPupilData.Where(x => x.AgeAtTest_DevAbil != 0).Select(r => r.AgeAtTest_DevAbil).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_DevAbil", average = listPupilData.Where(x => x.AgeEquiv_DevAbil != 0).Select(r => r.AgeEquiv_DevAbil).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_DevAbil", average = listPupilData.Where(x => x.AgeDiff_DevAbil != 0).Select(r => r.AgeDiff_DevAbil).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "Standardised_DevAbil", average = listPupilData.Where(x => x.Standardised_DevAbil != 0).Select(r => r.Standardised_DevAbil).Average() });
        //    listobject.Add(new DataSeries { dataSeriesNames = "Develop Ability", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
        //    //calculate reading
        //    listResult = new List<PIPSObjDetail>();
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Reading", average = listPupilData.Where(x => x.AgeAtTest_Reading.HasValue == true).Select(r => r.AgeAtTest_Reading).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Reading", average = listPupilData.Where(x => x.AgeEquiv_Reading.HasValue == true).Select(r => r.AgeEquiv_Reading).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Reading", average = listPupilData.Where(x => x.AgeDiff_Reading.HasValue == true).Select(r => r.AgeDiff_Reading).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = listPupilData.Where(x => x.Standardised_Reading.HasValue == true).Select(r => r.Standardised_Reading).Average() });
        //    listobject.Add(new DataSeries { dataSeriesNames = "Reading", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
        //    //calculate Spelling
        //    listResult = new List<PIPSObjDetail>();
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_Spelling", average = listPupilData.Where(x => x.AgeAtTest_Spelling.HasValue == true).Select(r => r.AgeAtTest_Spelling).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_Spelling", average = listPupilData.Where(x => x.AgeEquiv_Spelling.HasValue == true).Select(r => r.AgeEquiv_Spelling).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_Spelling", average = listPupilData.Where(x => x.AgeDiff_Spelling.HasValue == true).Select(r => r.AgeDiff_Spelling).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "Standardised_Reading", average = double.NaN });
        //    listobject.Add(new DataSeries { dataSeriesNames = "Spelling", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
        //    //calculate General Math
        //    listResult = new List<PIPSObjDetail>();
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_GenMaths", average = listPupilData.Where(x => x.AgeAtTest_GenMaths.HasValue == true).Select(r => r.AgeAtTest_GenMaths).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_GenMaths", average = listPupilData.Where(x => x.AgeEquiv_GenMaths.HasValue == true).Select(r => r.AgeEquiv_GenMaths).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_GenMaths", average = listPupilData.Where(x => x.AgeDiff_GenMaths.HasValue == true).Select(r => r.AgeDiff_GenMaths).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "Standardised_GenMaths", average = listPupilData.Where(x => x.Standardised_GenMaths.HasValue == true).Select(r => r.Standardised_GenMaths).Average() });
        //    listobject.Add(new DataSeries { dataSeriesNames = "General Maths", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });
        //    //calculate Mental Arithmetics
        //    listResult = new List<PIPSObjDetail>();
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeAtTest_MentArith", average = listPupilData.Where(x => x.AgeAtTest_MentArith.HasValue == true).Select(r => r.AgeAtTest_MentArith).Average() });
        //    //listResult.Add(new PIPSObjDetail { dataName = "AgeEquiv_MentArith", average = listPupilData.Where(x => x.AgeEquiv_MentArith.HasValue == true).Select(r => r.AgeEquiv_MentArith).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "AgeDiff_MentArith", average = listPupilData.Where(x => x.AgeDiff_MentArith.HasValue == true).Select(r => r.AgeDiff_MentArith).Average() });
        //    listResult.Add(new PIPSObjDetail { dataName = "Standardised_MentArith", average = listPupilData.Where(x => x.Standardised_MentArith.HasValue == true).Select(r => r.Standardised_MentArith).Average() });
        //    listobject.Add(new DataSeries { dataSeriesNames = "Mental Arithmetics", school = new School("Aberdeen City", "Aberdeen City"), year = iyear, listPIPSdataitems = listResult });

        //    return listobject;
        //}

        protected DataTable CreateInCASDataTable(List<DataSeries> listobject, string firstColName)
        {
            DataTable dataTable = new DataTable();
            List<string> temprowdata = new List<string>();

            //create column names
            dataTable.Columns.Add(firstColName, typeof(string));

            if (listobject.Count == 0)
            {
                dataTable.Rows.Add("Data is not available");
            }
            else
            {

                //if (listobject != null && listobject[0].listPIPSdataitems.Count() > 0)
                //{
                foreach (var item in listobject[0].listPIPSdataitems)
                {
                    dataTable.Columns.Add(item.dataName, typeof(string));
                }

                //}


                //adding row data
                foreach (var item in listobject)
                {
                    temprowdata = new List<string>();
                    temprowdata.Add(item.school.name + " " + item.dataSeriesNames);
                    foreach (var temp in item.listPIPSdataitems)
                    {
                        //temprowdata.Add("na" );
                        temprowdata.Add((temp.average.HasValue && !Double.IsNaN(temp.average.Value)) == false ? "na" : temp.average.Value.ToString("0.00"));
                    }
                    dataTable.Rows.Add(temprowdata.ToArray());
                }

            }



            return dataTable;
        }
        
    }
}