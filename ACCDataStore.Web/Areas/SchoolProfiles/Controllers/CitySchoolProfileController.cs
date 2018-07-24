using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using ACCDataStore.Web.Helpers.Security;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class CitySchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public CitySchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        [PublicAuthentication]
        [Transactional]
        // GET: SchoolProfiles/CitySchoolProfile
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/CitySchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = new List<School>() { new School("1002", "Aberdeen City") };
                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x => x.year.Equals("2017")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x => x.seedcode.Equals("1002")).ToList() : null;
                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()),
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected != null ? eYearSelected.GetJson() : null,
                    ListSchoolSelected = listSchool.Select(x => x.GetJson()),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [PublicAuthentication]
        [Transactional]
        [HttpGet]
        [Route("SchoolProfiles/CitySchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;

                var listSchool = new List<School>() { new School("1002", "Aberdeen City", "5") };
                var listYear = GetListYear();
                var eYearSelected = new Year(sYear);
                List<School> ListSchoolSelected = new List<School>() ;

                var listSchoolData = GetSchoolData(ListSchoolSelected, sYear);

                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()), // all school
                    ListSchoolSelected = listSchool.Select(x => x.GetJson()), // set selected list of school
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected.GetJson(),
                    ListingData = listSchoolData, // table data
                    ChartData = GetChartData(listSchoolData, eYearSelected),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private List<BaseSPDataModel> GetSchoolData(List<School> tListSchoolSelected, string sYear)
        {
            var listYear = GetListYear();
            var listSchoolData = new List<BaseSPDataModel>();
            CitySchool tempSchool = new CitySchool();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new School("2", "Aberdeen Primary Schools", "2"));
            //add Aberdeen Secondary School data
            tListSchoolSelected.Add(new School("3", "Aberdeen Secondary Schools", "3"));
            //add Aberdeen Special School data
            tListSchoolSelected.Add(new School("4", "Aberdeen Special Schools", "4"));
            //Aberdeen City
            tListSchoolSelected.Add(new School("1002", "Aberdeen City", "5"));

            Year selectedyear = new Year(sYear);

            foreach (School school in tListSchoolSelected)
            {
                tempSchool = new CitySchool();
                tempSchool.SeedCode = school.seedcode;
                tempSchool.SchoolName = school.name;
                tempSchool.SchoolCostperPupil = GetSchoolCostperPupil(school);
                tempSchool.listNationalityIdentity = GetHistoricalNationalityData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.NationalityIdentity = tempSchool.listNationalityIdentity.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listEthnicbackground = GetHistoricalEthnicData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.Ethnicbackground = tempSchool.listEthnicbackground.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listLevelOfEnglish = GetHistoricalEALData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.LevelOfEnglish = tempSchool.listLevelOfEnglish.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listLookedAfter = GetHistoricalLookedAfterData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.LookedAfter = tempSchool.listLookedAfter.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.SchoolRoll = GetSchoolRollData(school, selectedyear);
                tempSchool.SchoolRollForecast = GetSchoolRollForecastData(rpGeneric2nd, school);
                tempSchool.listStudentStage = GetHistoricalStudentStageData(rpGeneric2nd, school.seedcode, listYear);
                tempSchool.StudentStage = tempSchool.listStudentStage.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                if (Convert.ToInt16(selectedyear.year) < 2016)
                {
                    tempSchool.listSIMD = null;
                    tempSchool.SIMD = null;
                } else {
                    tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                    tempSchool.SIMD = tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();               
                }
                tempSchool.listFSM = GetHistoricalFSMData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.FSM = tempSchool.listFSM.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listFSM_National = GetHistoricalNationalFSMData(rpGeneric2nd, school.schooltype, "9999", listYear);
                tempSchool.FSM_National = tempSchool.listFSM_National.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listStudentNeed = GetHistoricalStudentNeed(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.StudentNeed = tempSchool.listStudentNeed.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listAttendance = GetHistoricalAttendanceData(rpGeneric2nd, school.schooltype, school, listYear);
                tempSchool.SPAttendance = tempSchool.listAttendance.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listExclusion = GetHistoricalExclusionData(rpGeneric2nd, school.schooltype, school, listYear);
                tempSchool.SPExclusion = tempSchool.listExclusion.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listSPCfElevel = GetHistoricalCfELevelData(rpGeneric2nd, school.schooltype, school.seedcode, listYear);
                tempSchool.SPCfElevel = tempSchool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year) && x.seedcode.Equals("1002")).FirstOrDefault();
                tempSchool.SPCfElevel_NCfElevel = tempSchool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year) && x.seedcode.Equals("9999")).FirstOrDefault();
                listSchoolData.Add(tempSchool);
                //List<List<GenericSchoolData>> aaa = tempSchool.listSPCfElevel.Select(x => x.getP1EarlybySubjectAndSIMD("")).ToList();

            }
            return listSchoolData;
        }

        private ACCDataStore.Entity.SchoolProfiles.Census.Entity.ChartData GetChartData(List<BaseSPDataModel> listSchool, Year eYearSelected)
        {
            try {
                Entity.SchoolProfiles.Census.Entity.CityChartData chartdata = new Entity.SchoolProfiles.Census.Entity.CityChartData();
                chartdata.ChartNationalityIdentity = GetChartNationalityIdentity(listSchool, eYearSelected);
                chartdata.ChartLevelOfEnglish = GetChartLevelofEnglish(listSchool, eYearSelected);
                chartdata.ChartLevelOfEnglishByCatagories = GetChartLevelofEnglishbyCatagories(listSchool, eYearSelected);
                if (Convert.ToInt16(eYearSelected.year) < 2016) {
                    chartdata.ChartSIMD = null;
                }
                else { 
                    chartdata.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected); 
                }
                
                chartdata.CartSchoolRollForecast = GetChartSchoolRollForecast(listSchool);
                chartdata.ChartIEP = GetChartStudentNeedIEP(listSchool);
                chartdata.ChartCSP = GetChartStudentNeedCSP(listSchool);
                chartdata.ChartLookedAfter = GetChartLookedAfter(listSchool);
                chartdata.ChartAttendance = GetChartAttendance(listSchool, "Attendance");
                chartdata.ChartAuthorisedAbsence = GetChartAttendance(listSchool, "Authorised Absence");
                chartdata.ChartUnauthorisedAbsence = GetChartAttendance(listSchool, "Unauthorised Absence");
                chartdata.ChartTotalAbsence = GetChartAttendance(listSchool, "Total Absence");
                chartdata.ChartNumberofDaysLostExclusion = GetChartExclusion(listSchool, "Number of Days Lost Per 1000 Pupils Through Exclusions");
                chartdata.ChartNumberofExclusionRFR = GetChartExclusion(listSchool, "Number of Removals from the Register");
                chartdata.ChartNumberofExclusionTemporary = GetChartExclusion(listSchool, "Number of Temporary Exclusions");
                chartdata.ChartCfeP1Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP1LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP4Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP4LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP7Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP7LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP1LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP1LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfeP4LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP4LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfeP7LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP7LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfe3Level = listSchool[0].SPCfElevel == null? null:GetChartCfe3LevelData(listSchool, eYearSelected);
                chartdata.ChartCfe4Level = listSchool[0].SPCfElevel == null ? null : GetChartCfe4LevelData(listSchool, eYearSelected);
                chartdata.ChartCfe3LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfe3LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfe4LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfe4LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartFSMPrimary = listSchool[0].listFSM == null ? null : GetChartFSM(listSchool, "2");
                chartdata.ChartFSMSecondary = listSchool[1].listFSM == null ? null : GetChartFSM(listSchool, "3");
                chartdata.ChartFSMSpecial = listSchool[2].listFSM == null ? null : GetChartFSM(listSchool, "4");
                chartdata.ChartFSMCity = listSchool[3].listFSM == null ? null : GetChartFSM(listSchool, "5");


                chartdata.ChartP1TimelineCfEReading = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P1", "Reading", eYearSelected);
                chartdata.ChartP1TimelineCfEWriting = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P1", "Writing", eYearSelected);
                chartdata.ChartP1TimelineCfEELT = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P1", "Listening & Talking", eYearSelected);
                chartdata.ChartP1TimelineCfENumeracy = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P1", "Numeracy", eYearSelected);

                chartdata.ChartP4TimelineCfEReading = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P4", "Reading", eYearSelected);
                chartdata.ChartP4TimelineCfEWriting = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P4", "Writing", eYearSelected);
                chartdata.ChartP4TimelineCfEELT = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P4", "Listening & Talking", eYearSelected);
                chartdata.ChartP4TimelineCfENumeracy = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P4", "Numeracy", eYearSelected);

                chartdata.ChartP7TimelineCfEReading = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P7", "Reading", eYearSelected);
                chartdata.ChartP7TimelineCfEWriting = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P7", "Writing", eYearSelected);
                chartdata.ChartP7TimelineCfEELT = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P7", "Listening & Talking", eYearSelected);
                chartdata.ChartP7TimelineCfENumeracy = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "P7", "Numeracy", eYearSelected);

                chartdata.ChartS3ThirdTimelineCfEReading = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Third", "Reading", eYearSelected);
                chartdata.ChartS3ThirdTimelineCfEWriting = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Third", "Writing", eYearSelected);
                chartdata.ChartS3ThirdTimelineCfEELT = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Third", "Listening & Talking", eYearSelected);
                chartdata.ChartS3ThirdTimelineCfENumeracy = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Third", "Numeracy", eYearSelected);

                chartdata.ChartS3FourthTimelineCfEReading = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Fourth", "Reading", eYearSelected);
                chartdata.ChartS3FourthTimelineCfEWriting = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Fourth", "Writing", eYearSelected);
                chartdata.ChartS3FourthTimelineCfEELT = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Fourth", "Listening & Talking", eYearSelected);
                chartdata.ChartS3FourthTimelineCfENumeracy = listSchool[0].SPCfElevel == null ? null : GetChartCfESIMDComparisonData(listSchool, "S3Fourth", "Numeracy", eYearSelected);

                
                return chartdata; 

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
                return null;
            }
        }

        private string GetSchoolCostperPupil(School school)
        {
            string costperpupil = "";

            if (school.seedcode.Equals("1002"))
            {
                costperpupil = NumberFormatHelper.FormatNumber(4101.2, 1).ToString();
            }
            else if (school.seedcode.Equals("2"))
            {
                //primary schools
                costperpupil = NumberFormatHelper.FormatNumber(6195.6, 1).ToString();

            }
            else if (school.seedcode.Equals("3"))
            {
                //secondary schools
                costperpupil = NumberFormatHelper.FormatNumber(4101.2, 1).ToString();

            }
            else if (school.seedcode.Equals("4"))
            {
                //Special schools
                costperpupil = NumberFormatHelper.FormatNumber(0.0, 1).ToString();

            }

            return costperpupil;
        }

        //Get SchoolRoll data
        private SchoolRoll GetSchoolRollData(School school, Year year)
        {

            SchoolRoll SchoolRoll = new SchoolRoll();

            if (school.seedcode.Equals("1002"))
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where year = " + year.year);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = school.schoolCapacity;
                            SchoolRoll.schoolroll = Convert.ToInt16(itemRow[1].ToString());//NumberFormatHelper.FormatNumber(school.costperpupil, 1).ToString();    
                            SchoolRoll.percent = 0.00F;
                            //SchoolRoll.spercent = NumberFormatHelper.FormatNumber(itemRow[1], 1).ToString();
                            SchoolRoll.sschoolroll = NumberFormatHelper.FormatNumber(itemRow[1], 0).ToString();  
                        }
                    }
                }

            }
            else
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where year = " + year.year + " and SchoolType =" + school.schooltype);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = school.schoolCapacity;
                            SchoolRoll.schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            SchoolRoll.percent = 0.00F;
                            //SchoolRoll.spercent = NumberFormatHelper.FormatNumber(itemRow[1], 1).ToString();
                            SchoolRoll.sschoolroll = NumberFormatHelper.FormatNumber(itemRow[1], 0).ToString();  
                        }
                    }
                }


            }

            return SchoolRoll;
        }

        //Get SchoolRoll data
        private new SPSchoolRollForecast GetSchoolRollForecastData(IGenericRepository2nd rpGeneric2nd, School school)
        {

            SPSchoolRollForecast SchoolRollForecast = new SPSchoolRollForecast();
            List<GenericSchoolData> tempdataActualnumber = new List<GenericSchoolData>();

            if (!school.schooltype.Equals("5"))
            {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, 'Seedcode', SchoolType, sum(Count) from summary_schoolroll where SchoolType = " + school.schooltype + " group by schooltype, year");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempdataActualnumber.Add(new GenericSchoolData(new Year(itemRow[0].ToString()).academicyear, itemRow[3]==null? 0: Convert.ToInt32(itemRow[3].ToString())));
                        }
                    }
                }

                SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;
            }
            else {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select Year, 'Seedcode', 'SchoolType', sum(Count) from summary_schoolroll group by year");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempdataActualnumber.Add(new GenericSchoolData(new Year(itemRow[0].ToString()).academicyear, itemRow[3] == null ? 0 : Convert.ToInt32(itemRow[3].ToString())));
                        }
                    }
                }

                SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;
            }

            return SchoolRollForecast;
        }

        // SchoolRoll Forecast Chart
        private new SplineCharts GetChartSchoolRollForecast(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = " School Roll ";
            eSplineCharts.yAxis.title.text = "Number of Pupils";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();
            //finding subject index to query data from list

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].SchoolRollForecast.ListActualSchoolRoll.Select(x => x.Code).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Number of Pupils" };

                foreach (var eSchool in listSchool)
                {
                    if (!eSchool.SeedCode.Equals("1002"))
                    {
                        var listSeriesActual = eSchool.SchoolRollForecast.ListActualSchoolRoll.Select(x => float.Parse(x.sCount) == 0 ? null : (float?)float.Parse(x.sCount)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = eSchool.SchoolName,
                            color = colors[indexColor],
                            lineWidth = 2,
                            data = listSeriesActual,
                            visible = true
                        });

                    }
                    indexColor++;
                }
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        protected override List<ViewObj> GetListViewObj(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string datatitle)
        {
            List<ViewObj> listResult = new List<ViewObj>();
            string query = "";
            switch (datatitle)
            {
                case "eal":
                    query = "Select * from summary_levelofenglish";
                    break;
                case "ethnicbackground":
                    query = "Select * from summary_ethnicbackground";
                    break;
                case "stage":
                    query = "Select * from summary_studentstage";
                    break;
                case "nationality":
                    query = "Select * from summary_nationality";
                    break;
                case "needtype":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentneed";
                    break;
                case "lookedafter":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentlookedafter";
                    break;
                case "simd":
                    //to calculate IEP CSP
                    query = "Select * from summary_simd";
                    break;
                case "attendance":
                    //to calculate IEP CSP
                    query = "Select * from summary_attendance ";
                    break;
                case "schoolroll":
                    //to calculate IEP CSP
                    query = "Select * from summary_schoolroll ";
                    break;
            }

            var listtemp = rpGeneric2nd.FindByNativeSQL(query);
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    ViewObj temp = new ViewObj();
                    temp.year = new Year(itemrow[0].ToString());
                    temp.seedcode = itemrow[1].ToString();
                    temp.schooltype = itemrow[2].ToString();
                    temp.code = itemrow[3].ToString();
                    temp.count = Convert.ToInt32(itemrow[4].ToString());
                    listResult.Add(temp);
                }
            }


            return listResult;

        }
        //Historical StudentStage data
        private List<StudentStage> GetHistoricalStudentStageData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, List<Year> listyear)
        {
            List<StudentStage> listStudentStage = new List<StudentStage>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            StudentStage StudentStage = new StudentStage();

            Dictionary<string, string> DictStage = GetDicStage(rpGeneric2nd, sSchoolType);

            foreach (var item in DictStage)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "stage");

                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictStage[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sCount = NumberFormatHelper.FormatNumber(y.Select(a => a.count).Sum(), 0).ToString(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    StudentStage = new StudentStage();
                    StudentStage.YearInfo = year;
                    StudentStage.ListGenericSchoolData = groupedList;
                    StudentStage.totalschoolroll = total;
                    StudentStage.stotalschoolroll = NumberFormatHelper.FormatNumber(total, 0).ToString();
                    listStudentStage.Add(StudentStage);
                }


            return listStudentStage.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical NationalityData
        private new List<NationalityIdentity> GetHistoricalNationalityData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<NationalityIdentity> listNationalityIdentity = new List<NationalityIdentity>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            NationalityIdentity NationalityIdentity = new NationalityIdentity();

            Dictionary<string, string> DictNationality = GetDicNationalIdenity(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "nationality");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && !x.code.Equals("08")).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Sum(x => x.count),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList.OrderBy(x=>x.Code).ToList();
                    listNationalityIdentity.Add(NationalityIdentity);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Sum(x => x.count),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList.OrderBy(x => x.Code).ToList(); ;
                    listNationalityIdentity.Add(NationalityIdentity);
                }

            }

            return listNationalityIdentity.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical EthnicBackground data
        private new List<Ethnicbackground> GetHistoricalEthnicData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<Ethnicbackground> listEthnicbackground = new List<Ethnicbackground>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            Ethnicbackground Ethnicbackground = new Ethnicbackground();

            Dictionary<string, string> DictNationality = GetDicEhtnicBG(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "ethnicbackground");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList.OrderBy(x => x.Code).ToList(); ;
                    listEthnicbackground.Add(Ethnicbackground);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList.OrderBy(x => x.Code).ToList(); ;
                    listEthnicbackground.Add(Ethnicbackground);
                }

            }

            return listEthnicbackground.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        //Historical Level of English data
        private new List<LevelOfEnglish> GetHistoricalEALData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LevelOfEnglish> listLevelOfEnglish = new List<LevelOfEnglish>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LevelOfEnglish LevelOfEnglish = new LevelOfEnglish();

            Dictionary<string, string> DictEnglisheLevel = GetDicEnglisheLevel(rpGeneric2nd);

            foreach (var item in DictEnglisheLevel)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "eal");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictEnglisheLevel[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictEnglisheLevel[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }

            }

            return listLevelOfEnglish.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        //Historical Attendance Data
        private new List<SPAttendance> GetHistoricalAttendanceData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPAttendance> listAttendance = new List<SPAttendance>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPAttendance SPAttendance = new SPAttendance();
            int possibledays = 0;
            Dictionary<string, string> DictAttendance = GetDicAttendance(rpGeneric2nd);

            foreach (var item in DictAttendance)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "attendance");

            if (school.seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));


                        if (Convert.ToInt16(year.year) < 2014)
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();

                        }
                        else
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum();


                        }


                        //int possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12") || x.Code.Equals("13")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12/13",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32/33",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2") || x.Code.Equals("13")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12/13",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32/33",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }

                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));

                        if (Convert.ToInt16(year.year) < 2014)
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();

                        }
                        else
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum();


                        }

                        //int possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2") || x.Code.Equals("13")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "13/20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }
                }

            }

            return listAttendance.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical Looked After data
        private new List<LookedAfter> GetHistoricalLookedAfterData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LookedAfter> listLookedAfter = new List<LookedAfter>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LookedAfter LookedAfter = new LookedAfter();

            Dictionary<string, string> DictLookedAfter = GetDicLookAfter();

            foreach (var item in DictLookedAfter)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "lookedafter");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictLookedAfter[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictLookedAfter[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }

            }

            return listLookedAfter.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        //Historical StudentNeed
        private List<StudentNeed> GetHistoricalStudentNeed(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            StudentNeed StudentNeed = new StudentNeed(); ;
            List<StudentNeed> listStudentNeed = new List<StudentNeed>();
            int yschoolroll = 0;

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "needtype");

            if (seedcode.Equals("1002"))
            {
                //citywide
                foreach (Year year in listyear)
                {
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL("Select Year, sum(Count) from summary_schoolroll where year = " + year.year);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                yschoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }
                    StudentNeed = new StudentNeed();
                    StudentNeed.year = year;
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {

                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL("Select Year, sum(Count) from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                yschoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }
                    StudentNeed = new StudentNeed();
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    StudentNeed.year = year;
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }

            }

            return listStudentNeed.OrderBy(x => x.year.year).ToList();
        }

        //Historical Exclusion Data
        private new List<SPExclusion> GetHistoricalExclusionData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPExclusion> listExclusion = new List<SPExclusion>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>() { new GenericSchoolData("0", "Temporary Exclusions"), new GenericSchoolData("1", "Removed From Register") };
            SPExclusion SPExclusion = new SPExclusion();
            GenericSchoolData tempobj = new GenericSchoolData();
            string queryExclusion, querySchoolRoll = "";
            int schoolroll = 0;
            string yearNodata = "2017";

            foreach (Year year in listyear)
            {
                if (school.seedcode.Equals("1002"))
                {
                    queryExclusion = "SELECT Year, 1002, Code, sum(Count), sum(Sum)  FROM summary_exclusion where year = " + year.year + " group by Year, Code";
                    querySchoolRoll = "Select Year, sum(Count) from summary_schoolroll where year = " + year.year;
                }
                else
                {
                    queryExclusion = "SELECT Year, Seedcode, Code, sum(Count), sum(Sum)  FROM summary_exclusion where  SchoolType= " + sSchoolType + "  and year = " + year.year + " group by Year, Code;";
                    querySchoolRoll = "Select Year, sum(Count) from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year ;
                }

                var listResult = rpGeneric2nd.FindByNativeSQL(queryExclusion);
                if (listResult.Count > 0)
                {
                    tempdata = new List<GenericSchoolData>();
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempobj = new GenericSchoolData(itemRow[2].ToString(), itemRow[2].ToString().Equals("0") ? "Temporary Exclusions" : "Removed From Register");
                            tempobj.count = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sCount = NumberFormatHelper.FormatNumber(tempobj.count, 0).ToString();
                            tempobj.sum = Convert.ToInt16(itemRow[4].ToString());
                            tempobj.Percent = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                            tempdata.Add(tempobj);
                        }
                    }
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL(querySchoolRoll);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }

                    tempdata.AddRange(foo.Where(x => tempdata.All(p1 => !p1.Code.Equals(x.Code))));
                    SPExclusion = new SPExclusion();
                    SPExclusion.YearInfo = new Year(year.year);
                    tempobj = new GenericSchoolData("2", "Number of days per 1000 pupils lost to exclusions");
                    tempobj.count = tempdata.Sum(x => x.sum);  //Sum length of exclusion
                    tempobj.sCount = NumberFormatHelper.FormatNumber(tempobj.count, 0).ToString();
                    tempobj.sum = schoolroll;   //school Roll
                    tempobj.Percent = tempobj.count / 2.0F / schoolroll * 1000.0F;
                    tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                    //tempdata.Add(tempobj);
                    SPExclusion.ListGenericSchoolData = new List<GenericSchoolData>() { tempdata.Where(x => x.Code.Equals("0")).First(), tempdata.Where(x => x.Code.Equals("1")).First(), tempobj };
                    listExclusion.Add(SPExclusion);
                }
                else
                {

                    tempdata = new List<GenericSchoolData>();
                    tempdata.Add(new GenericSchoolData()
                    {
                        Name = "Temporary Exclusions",
                        Code = "0",
                        count = 0,
                        sCount = year.year.Equals(yearNodata) ? "n/a" : "0",
                        sum = 0,
                        Percent = year.year.Equals(yearNodata) ? null : (float?)0.0,
                        sPercent = year.year.Equals(yearNodata) ? "n/a" : "0.0",
                    });
                    tempdata.Add(new GenericSchoolData()
                    {
                        Name = "Removed From Register",
                        Code = "1",
                        count = 0,
                        sCount = year.year.Equals(yearNodata) ? "n/a" : "0",
                        sum = 0,
                        Percent = year.year.Equals(yearNodata) ? null : (float?)0.0,
                        sPercent = year.year.Equals(yearNodata) ? "n/a" : "0.0",
                    });
                    tempdata.Add(new GenericSchoolData()
                    {
                        Name = "Number of days per 1000 pupils lost to exclusions",
                        Code = "2",
                        count = 0,
                        sCount = year.year.Equals(yearNodata) ? "n/a" : "0",
                        sum = 0,
                        Percent = year.year.Equals(yearNodata) ? null : (float?)0.0,
                        sPercent = year.year.Equals(yearNodata) ? "n/a" : "0.0",
                    });

                    SPExclusion = new SPExclusion();
                    SPExclusion.YearInfo = new Year(year.year);
                    SPExclusion.ListGenericSchoolData = tempdata;
                    listExclusion.Add(SPExclusion);
                }

            }

            return listExclusion.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical SIMD data
        private new List<SPSIMD> GetHistoricalSIMDData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<SPSIMD> listSIMD = new List<SPSIMD>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPSIMD SPSIMD = new SPSIMD();

            Dictionary<string, string> DictSIMD = GetDicSIMDDecile();

            foreach (var item in DictSIMD)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "simd");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictSIMD[y.Key.ToString()],
                            count = y.Select(a => a.count).Sum(),
                            sum = total,
                            Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                            sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.schooltype.Equals(sSchoolType)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictSIMD[y.Key.ToString()],
                            count = y.Select(a => a.count).Sum(),
                            sum = total,
                            Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                            sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }

                }

            }

            return listSIMD.OrderBy(x => x.YearInfo.year).ToList();
        }

        //Historical Free School Meal Registered data
        private List<FreeSchoolMeal> GetHistoricalNationalFSMData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();

            listFSM = GetHistoricalFSMData(rpGeneric2nd, "9999", listyear, sSchoolType);

            return listFSM.OrderBy(x => x.year.year).ToList();
        }

        //Historical Free School Meal Registered data
        private List<FreeSchoolMeal> GetHistoricalFSMData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();
 
            listFSM = GetHistoricalFSMData(rpGeneric2nd, "1002", listyear, sSchoolType);

            return listFSM.OrderBy(x => x.year.year).ToList();
        }


        //Historical CfE data
        private List<SPCfElevel> GetHistoricalCfELevelData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<SPCfElevel> listSPCfElevel = new List<SPCfElevel>();

            if (sSchoolType.Equals("2")){
 
                    listSPCfElevel = GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, "1002", sSchoolType);
 
                    listSPCfElevel.AddRange(GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, "9999", sSchoolType));
 
            }
            else if (sSchoolType.Equals("3"))
            {
                listSPCfElevel = GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, "1002", sSchoolType);
                listSPCfElevel.AddRange(GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, "9999", sSchoolType));
            }

 

            return listSPCfElevel.OrderBy(x => x.year.year).ToList();
        }

        // CfeP1Level Chart
        protected ColumnCharts GetChartCfeP1LevelData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "P1 - Early, " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P1EarlyLevel.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    if (eSPCfE.year.year.Equals(selectedyear.year)) {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSPCfE.seedcode.Equals("1002") ? "Aberdeen City" : "National",
                            data = eSPCfE.P1EarlyLevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSPCfE.seedcode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;                   
                    }

                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts
            return eColumnCharts;
        }

        // CfeP4Level Chart
        protected ColumnCharts GetChartCfeP4LevelData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "P4 - First Level, " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P1EarlyLevel.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    if (eSPCfE.year.year.Equals(selectedyear.year))
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSPCfE.seedcode.Equals("1002") ? "Aberdeen City" : "National",
                            data = eSPCfE.P4FirstLevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSPCfE.seedcode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;
                    }

                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts
            return eColumnCharts;
        }

        // CfeP7Level Chart
        protected ColumnCharts GetChartCfeP7LevelData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "P7 - Second Level, " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P1EarlyLevel.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    if (eSPCfE.year.year.Equals(selectedyear.year))
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSPCfE.seedcode.Equals("1002") ? "Aberdeen City" : "National",
                            data = eSPCfE.P7SecondLevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSPCfE.seedcode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;
                    }

                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts
            return eColumnCharts;
        }

        // CfeP1Level by Quantile Chart
        protected ColumnCharts GetChartCfeP1LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[0].SchoolName;
            eColumnCharts.subtitle.text = "P1 - Early Levels " + selectedyear.academicyear + ", by SIMD 2016 Quintiles";
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P1EarlyLevelQ1.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    indexColor = 0;

                    if (eSPCfE.year.year.Equals(selectedyear.year) && eSPCfE.seedcode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSPCfE.P1EarlyLevelQ1.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSPCfE.P1EarlyLevelQ2.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSPCfE.P1EarlyLevelQ3.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSPCfE.P1EarlyLevelQ4.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSPCfE.P1EarlyLevelQ5.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // CfeP4Level by Quantile Chart
        protected ColumnCharts GetChartCfeP4LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[0].SchoolName;
            eColumnCharts.subtitle.text = "P4 - First Levels " + selectedyear.academicyear + ", by SIMD 2016 Quintiles";
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P1EarlyLevelQ1.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    indexColor = 0;

                    if (eSPCfE.year.year.Equals(selectedyear.year) && eSPCfE.seedcode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSPCfE.P4FirstLevelQ1.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSPCfE.P4FirstLevelQ2.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSPCfE.P4FirstLevelQ3.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSPCfE.P4FirstLevelQ4.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSPCfE.P4FirstLevelQ5.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // CfeP4Level by Quantile Chart
        protected ColumnCharts GetChartCfeP7LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[0].SchoolName;
            eColumnCharts.subtitle.text = "P7 - Second Levels " + selectedyear.academicyear + ", by SIMD 2016 Quintiles";
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P1EarlyLevelQ1.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    indexColor = 0;

                    if (eSPCfE.year.year.Equals(selectedyear.year) && eSPCfE.seedcode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSPCfE.P7SecondLevelQ1.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSPCfE.P7SecondLevelQ2.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSPCfE.P7SecondLevelQ3.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSPCfE.P7SecondLevelQ4.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSPCfE.P7SecondLevelQ5.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Cfe3Level Chart
        protected ColumnCharts GetChartCfe3LevelData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Third Level or better " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of S3 pupils";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[1].SPCfElevel.ListThirdlevel.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("3")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    if (eSPCfE.year.year.Equals(selectedyear.year))
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSPCfE.seedcode.Equals("1002") ? "Aberdeen City" : "National",
                            data = eSPCfE.ListThirdlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSPCfE.seedcode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;
                    }                   
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Cfe4Level Chart
        protected ColumnCharts GetChartCfe4LevelData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Fourth Level " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of S3 pupils";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[1].SPCfElevel.ListForthlevel.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("3")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    if (eSPCfE.year.year.Equals(selectedyear.year))
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSPCfE.seedcode.Equals("1002") ? "Aberdeen City" : "National",
                            data = eSPCfE.ListForthlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSPCfE.seedcode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // CfeP4Level by Quantile Chart
        protected ColumnCharts GetChartCfe3LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[1].SchoolName;
            eColumnCharts.subtitle.text = "Third Level or better " + selectedyear.academicyear + ", by SIMD 2016 Quintiles";
            eColumnCharts.yAxis.title.text = "% of S3 pupils";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[1].SPCfElevel.ListThirdlevel.Select(x => x.Code).ToList();
                //select secondary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("3")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    indexColor = 0;

                    if (eSPCfE.year.year.Equals(selectedyear.year) && eSPCfE.seedcode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSPCfE.SIMDQ1_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSPCfE.SIMDQ2_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSPCfE.SIMDQ3_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSPCfE.SIMDQ4_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSPCfE.SIMDQ5_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // CfeP4Level by Quantile Chart
        protected ColumnCharts GetChartCfe4LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[1].SchoolName;
            eColumnCharts.subtitle.text = "Fourth Level " + selectedyear.academicyear + ", by SIMD 2016 Quintiles";
            eColumnCharts.yAxis.title.text = "% of S3 pupils";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[1].SPCfElevel.ListForthlevel.Select(x => x.Code).ToList();
                //select primary school
                BaseSPDataModel tempschool = listSchool.Where(x => x.SeedCode.Equals("3")).FirstOrDefault();

                foreach (var eSPCfE in tempschool.listSPCfElevel)
                {
                    indexColor = 0;

                    if (eSPCfE.year.year.Equals(selectedyear.year) && eSPCfE.seedcode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSPCfE.SIMDQ1_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSPCfE.SIMDQ2_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSPCfE.SIMDQ3_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSPCfE.SIMDQ4_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSPCfE.SIMDQ5_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // FSM Chart
        private SplineCharts GetChartFSM(List<BaseSPDataModel> templistSchool, string schooltype) // query from database and return charts object
        {

            List<CitySchool> listSchool = templistSchool.Cast<CitySchool>().ToList();
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "Free School Meal";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            string[] colors = { "#ED561B", "#DDDF00", "#24CBE5", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            List<FreeSchoolMeal> tempdata = null, tempNationaldata = null;
            string schoolname = "";

            if (listSchool != null && listSchool.Count > 0)
            {
                switch (schooltype)
                {
                    case "2":
                        //select priary data from 2014 (P4-P7) 
                        schoolname = listSchool[0].SchoolName;
                        tempdata = listSchool[0].listFSM.Where(x => Convert.ToInt32(x.year.year) >= 2014).ToList();
                        tempNationaldata = listSchool[0].listFSM_National.Where(x => Convert.ToInt32(x.year.year) >= 2014).ToList();
                        eSplineCharts.xAxis.categories = tempdata.Select(x => x.year.academicyear).ToList(); // year on xAxis
                        eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of P4-P7 Roll Registered for FSM" };
                        break;
                    case "3":
                        //select secondary data
                        schoolname = listSchool[1].SchoolName;
                        tempdata = listSchool[1].listFSM;
                        tempNationaldata = listSchool[1].listFSM_National;
                        eSplineCharts.xAxis.categories = tempdata.Select(x => x.year.academicyear).ToList(); // year on xAxis
                        eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of S1-S6 Roll Registered for FSM" };
                        break;
                    case "4":
                        //select special data
                        schoolname = listSchool[2].SchoolName;
                        tempdata = listSchool[2].listFSM;
                        tempNationaldata = listSchool[2].listFSM_National;
                        eSplineCharts.xAxis.categories = tempdata.Select(x => x.year.academicyear).ToList(); // year on xAxis
                        eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of SP Roll Registered for FSM" };
                        break;
                    case "5":
                        //select city data
                        schoolname = listSchool[3].SchoolName;
                        tempdata = listSchool[3].listFSM;
                        tempNationaldata = listSchool[3].listFSM_National;
                        eSplineCharts.xAxis.categories = tempdata.Select(x => x.year.academicyear).ToList(); // year on xAxis
                        eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of pupils Roll Registered for FSM" };
                        break;
                }

                var listSeries = tempdata.Select(x => x.GenericSchoolData.sPercent.Equals("n/a") ? null : (float?)float.Parse(x.GenericSchoolData.sPercent)).ToList();

                eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                {
                    name = schoolname,
                    color = colors[0],
                    lineWidth = 2,
                    data = listSeries,
                    visible = true
                });


                listSeries = tempNationaldata.Select(x => x.GenericSchoolData.sPercent.Equals("n/a") ? null : (float?)float.Parse(x.GenericSchoolData.sPercent)).ToList();

                eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                {
                    name = "National",
                    color = colors[1],
                    lineWidth = 2,
                    data = listSeries,
                    visible = true
                });

                eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
                {
                    enabled = true
                };

                eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
                {
                    enabled = true,
                    filename = "export"
                };
            }

            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        protected  ColumnCharts GetChartCfESIMDComparisonData(List<BaseSPDataModel> listSchool, string stage, string subject, Year selectedyear) // query from database and return charts object
        {
            List<SPCfElevel> temp = new List<SPCfElevel>();
            BaseSPDataModel tempschool = new BaseSPDataModel();
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column", schoolname = "";
            List<float?> data = new List<float?>() { };

            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            
            eColumnCharts.subtitle.text = subject + ": "+stage+" Levels by SIMD 2016 Quintiles ";
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "SIMD Q1", "SIMD Q2", "SIMD Q3", "SIMD Q4", "SIMD Q5" };

                if (stage.Equals("P1") || stage.Equals("P4") || stage.Equals("P7"))
                {
                    //select secondary school
                    tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();
                    temp = tempschool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).ToList();
                    schoolname = tempschool.SchoolName;
                }
                else if (stage.Equals("S3Third") || stage.Equals("S3Fourth"))
                {
                    //select secondary school
                    tempschool = listSchool.Where(x => x.SeedCode.Equals("3")).FirstOrDefault();
                    temp = tempschool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).ToList();
                    schoolname = tempschool.SchoolName;
                }
                //else if (stage.Equals("S3"))
                //{
                //    //select secondary school
                //    tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();
                //    temp = tempschool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).Select(x => x.getP4FirstLevelbySubjectAndSIMD(subject)).ToList();

                //}
                //else if (stage.Equals("P7"))
                //{
                //    //select secondary school
                //    tempschool = listSchool.Where(x => x.SeedCode.Equals("2")).FirstOrDefault();
                //    temp = tempschool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).Select(x => x.getP7SecondLevelbySubjectAndSIMD(subject)).ToList();

                //}

                foreach (SPCfElevel tempObj in temp)
                {
                  

                    if (stage.Equals("P1"))
                    {
                        data = tempObj.getP1EarlybySubjectAndSIMD(subject).ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList();
                    }
                    else if (stage.Equals("P4")) {
                        data = tempObj.getP4FirstLevelbySubjectAndSIMD(subject).ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList();
                    
                    }
                    else if (stage.Equals("P7"))
                    {
                        data = tempObj.getP7SecondLevelbySubjectAndSIMD(subject).ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList();

                    }
                    else if (stage.Equals("S3Third"))
                    {
                        data = tempObj.getS3ThirdLevelbySubjectAndSIMD(subject).ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList();

                    }
                    else if (stage.Equals("S3Fourth"))
                    {
                        data = tempObj.getS3ForthLevelbySubjectAndSIMD(subject).ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList();

                    }

                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        type = gtype,
                        name = tempObj.seedcode.Equals("1002") ? schoolname : "Scotland",
                        data = data,
                        //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        color = colors[indexColor]
                    });
                    indexColor++;

                }
            }
            eColumnCharts.title.text = schoolname;
            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }
    }
}