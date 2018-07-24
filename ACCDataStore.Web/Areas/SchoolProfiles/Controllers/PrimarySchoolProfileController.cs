using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.Generic;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class PrimarySchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public PrimarySchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        [SchoolAuthentication]
        [Transactional]
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/PrimarySchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = GetListSchool(rpGeneric2nd, "2");
                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x=>x.year.Equals("2017")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x=>x.seedcode.Equals("5237521")).ToList() : null;
                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()),
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected != null ? eYearSelected.GetJson() : null,
                    ListSchoolSelected = ListSchoolSelected.Select(x => x.GetSchoolDetailJson()),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }


        [SchoolAuthentication]
        [Transactional]
        [HttpGet]
        [Route("SchoolProfiles/PrimarySchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                string sSchoolType = "2";
                var listSchool = GetListSchool(rpGeneric2nd, sSchoolType);
                var listYear = GetListYear();
                var eYearSelected = new Year(sYear);
                List<School> ListSchoolSelected = listSeedCode != null && listSeedCode.Count > 0 ? listSchool.Where(x => listSeedCode.Contains(x.seedcode)).ToList() : null;

                var listSchoolData = GetSchoolData(ListSchoolSelected, sYear, sSchoolType);
                SchoolPIPSTransform TempPIPSTransform = GetSchoolPIPSTransform(listSchoolData);

                Users tempUser = Session["SessionUser"] as Users;

                if (tempUser.IsPublicUser) { 
                    //format * 
                
                
                }


                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()), // all school
                    ListSchoolSelected = ListSchoolSelected.Where(x=>!x.seedcode.Equals("1002")).Select(x => x.GetSchoolDetailJson()), // set selected list of school
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected.GetJson(),
                    ListingData = listSchoolData, // table data
                    ChartData = GetChartData(listSchoolData, eYearSelected),
                    ListPIPSTransform = TempPIPSTransform
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private List<BaseSPDataModel> GetSchoolData(List<School> tListSchoolSelected, string sYear, string sSchoolType)
        {
            try {
                var listYear = GetListYear();
                var listSchoolData = new List<BaseSPDataModel>();
                PrimarySPDataModel tempSchool = new PrimarySPDataModel();

                //add Aberdeen Primary School data
                tListSchoolSelected.Add(new School("1002", "Aberdeen Primary Schools"));

                Year selectedyear = new Year(sYear);

                foreach (School school in tListSchoolSelected)
                {
                    tempSchool = new PrimarySPDataModel();
                    tempSchool.SeedCode = school.seedcode;
                    tempSchool.SchoolName = school.name;
                    tempSchool.SchoolCostperPupil = GetSchoolCostperPupil(school);
                    tempSchool.listNationalityIdentity = GetHistoricalNationalityData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                    tempSchool.NationalityIdentity = tempSchool.listNationalityIdentity.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listEthnicbackground = GetHistoricalEthnicData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                    tempSchool.Ethnicbackground = tempSchool.listEthnicbackground.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listLevelOfEnglish = GetHistoricalEALData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                    tempSchool.LevelOfEnglish = tempSchool.listLevelOfEnglish.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listLookedAfter = GetHistoricalLookedAfterData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                    tempSchool.LookedAfter = tempSchool.listLookedAfter.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.SchoolRoll = GetSchoolRollData(school, selectedyear);
                    tempSchool.SchoolRollForecast = GetSchoolRollForecastData(rpGeneric2nd, school);
                    tempSchool.listPIPS = GetHistoricalPIPSData(school.seedcode);
                    tempSchool.PIPS = tempSchool.listPIPS.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    List<SPInCAS> templist = GetHistoricalInCASData(school.seedcode);
                    tempSchool.listInCASP2 = templist.Where(x => x.yeargroup == 1).ToList();
                    tempSchool.listInCASP3 = templist.Where(x => x.yeargroup == 2).ToList();
                    tempSchool.listInCASP4 = templist.Where(x => x.yeargroup == 3).ToList();
                    tempSchool.listInCASP5 = templist.Where(x => x.yeargroup == 4).ToList();
                    tempSchool.listInCASP6 = templist.Where(x => x.yeargroup == 5).ToList();
                    tempSchool.InCASP2 = templist.Where(x => x.yeargroup == 1 && x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.InCASP3 = templist.Where(x => x.yeargroup == 2 && x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.InCASP4 = templist.Where(x => x.yeargroup == 3 && x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.InCASP5 = templist.Where(x => x.yeargroup == 4 && x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.InCASP6 = templist.Where(x => x.yeargroup == 5 && x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listStudentStage = GetHistoricalStudentStageData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                    tempSchool.StudentStage = tempSchool.listStudentStage.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();

                    if (Convert.ToInt16(selectedyear.year) < 2016)
                    {
                        tempSchool.listSIMD = null;
                        tempSchool.SIMD = null;
                    }
                    else
                    {
                        tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                        tempSchool.SIMD = tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    }
                    tempSchool.listFSM = GetHistoricalFSMData(rpGeneric2nd, school.seedcode, listYear, sSchoolType);
                    tempSchool.FSM = tempSchool.listFSM.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listStudentNeed = GetHistoricalStudentNeed(rpGeneric2nd, sSchoolType, school.seedcode,listYear);
                    tempSchool.StudentNeed = tempSchool.listStudentNeed.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listAttendance = GetHistoricalAttendanceData(rpGeneric2nd, sSchoolType, school, listYear);
                    tempSchool.SPAttendance = tempSchool.listAttendance.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listExclusion = GetHistoricalExclusionData(rpGeneric2nd, sSchoolType, school, listYear);
                    tempSchool.SPExclusion = tempSchool.listExclusion.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listSPCfElevel = GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, school.seedcode, sSchoolType);
                    tempSchool.SPCfElevel = tempSchool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();

                    listSchoolData.Add(tempSchool);
                }
                //save in session for using in doGetReport
                Session["listSchoolData"] = listSchoolData;

                return listSchoolData;
            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
                return null;
            }
            
        }

        private ACCDataStore.Entity.SchoolProfiles.Census.Entity.ChartData GetChartData(List<BaseSPDataModel> listSchool, Year eYearSelected)
        {
            try
            {
                Entity.SchoolProfiles.Census.Entity.PrimaryChartData chartdata = new Entity.SchoolProfiles.Census.Entity.PrimaryChartData();
                chartdata.ChartNationalityIdentity = GetChartNationalityIdentity(listSchool, eYearSelected);
                chartdata.ChartLevelOfEnglish = GetChartLevelofEnglish(listSchool, eYearSelected);
                chartdata.ChartLevelOfEnglishByCatagories = GetChartLevelofEnglishbyCatagories(listSchool, eYearSelected);

                if (Convert.ToInt16(eYearSelected.year) < 2016)
                {
                    chartdata.ChartSIMD = null;
                }
                else
                {
                    chartdata.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected);
                }

                chartdata.ChartPIPSReading = GetChartPIPS(listSchool, "Reading");
                chartdata.ChartPIPSMath = GetChartPIPS(listSchool, "Maths");
                chartdata.ChartPIPSPhonics = GetChartPIPS(listSchool, "Phonics");
                chartdata.ChartPIPSTotal = GetChartPIPS(listSchool, "Total Scores");
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
                chartdata.ChartFSM = GetChartFSM(listSchool);
                chartdata.ChartCfeP1Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP1LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP4Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP4LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP7Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP7LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP1LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP1LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfeP4LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP4LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfeP7LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP7LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartTimelineCfE = GetChartCfETimelinebySIMDData(listSchool, "","Reading");

                chartdata.ChartP1TimelineCfEReading = GetChartCfETimelinebySIMDData(listSchool, "P1", "Reading");
                chartdata.ChartP1TimelineCfEWriting = GetChartCfETimelinebySIMDData(listSchool, "P1", "Writing");
                chartdata.ChartP1TimelineCfEELT = GetChartCfETimelinebySIMDData(listSchool, "P1", "Listening & Talking");
                chartdata.ChartP1TimelineCfENumeracy = GetChartCfETimelinebySIMDData(listSchool, "P1", "Numeracy");

                chartdata.ChartP4TimelineCfEReading = GetChartCfETimelinebySIMDData(listSchool, "P4", "Reading");
                chartdata.ChartP4TimelineCfEWriting = GetChartCfETimelinebySIMDData(listSchool, "P4", "Writing");
                chartdata.ChartP4TimelineCfEELT = GetChartCfETimelinebySIMDData(listSchool, "P4", "Listening & Talking");
                chartdata.ChartP4TimelineCfENumeracy = GetChartCfETimelinebySIMDData(listSchool, "P4", "Numeracy");

                chartdata.ChartP7TimelineCfEReading = GetChartCfETimelinebySIMDData(listSchool, "P7", "Reading");
                chartdata.ChartP7TimelineCfEWriting = GetChartCfETimelinebySIMDData(listSchool, "P7", "Writing");
                chartdata.ChartP7TimelineCfEELT = GetChartCfETimelinebySIMDData(listSchool, "P7", "Listening & Talking");
                chartdata.ChartP7TimelineCfENumeracy = GetChartCfETimelinebySIMDData(listSchool, "P7", "Numeracy");

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
            else {

                costperpupil = NumberFormatHelper.FormatNumber(school.costperpupil, 1).ToString();     
            
            }

            return costperpupil;
        }
       
        // PIPS Chart
        private SplineCharts GetChartPIPS(List<BaseSPDataModel> listSchool, string ssubject) // query from database and return charts object
        {
            
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "PIPS P1 "+ssubject ;
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();
 
            //finding subject index to query data from list
            string[] arraySubject = { "Reading","", "Maths","", "Phonics","", "Total Scores" };
            int indexsubject = Array.FindIndex(arraySubject, item => item.Equals(ssubject));

            string[] colors = {"#ED561B", "#DDDF00", "#24CBE5", "#64E572", "#FF9655", "#FFF263", "#6AF9C4"};
            int indexcolour = 0;

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].listPIPS.Select(x => x.year.year).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Average PIPS Score" };
                //eSplineCharts.yAxis.min = 30;
                //eSplineCharts.yAxis.max = 70;
                //eSplineCharts.yAxis.tickInterval = 5;
                foreach (var eSchool in listSchool)
                {
                    var listSeriesStart = eSchool.listPIPS.Select(x => (float?)x.ListGenericSchoolData[indexsubject].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Start P1",
                        color = colors[indexcolour],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexcolour++;
                    var listSeriesEnd = eSchool.listPIPS.Select(x => (float?)x.ListGenericSchoolData[indexsubject+1].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " End P1",
                        color = colors[indexcolour],
                        lineWidth = 2,
                        data = listSeriesEnd,
                        visible = true
                    });
                    indexcolour++;
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

        //Get SchoolRoll data
        private SchoolRoll GetSchoolRollData(School school, Year year)
        {
             
            SchoolRoll SchoolRoll = new SchoolRoll();

            if (school.seedcode.Equals("1002"))
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where schooltype=2 and year = " + year.year);
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
                            SchoolRoll.percent =0.00F;
                        }
                    }
                }
 
            }
            else
            {             
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where schooltype=2 and year = " + year.year + " and seedcode =" + school.seedcode);
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
                            SchoolRoll.percent = (float)((SchoolRoll.schoolroll * 100.00F) / SchoolRoll.capacity);
                        }
                    }
                }


            }

            return SchoolRoll;
        }

        //Historical PIPS data
        private List<SPPIPS> GetHistoricalPIPSData(string seedcode)
        {
            List<SPPIPS> listPIPS = new List<SPPIPS>();
            List<GenericSchoolData> tempListSchoolData = new List<GenericSchoolData>();

            SPPIPS tPIPS = new SPPIPS();

            if (seedcode.Equals("1002"))
            {

                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select year, 0, avg(SREAding) as SReading, avg(smath) as SMath, avg(sphonics) as SPhonics, avg(stotal) as STotal,avg(EREAding) as EReading, avg(Emath) as EMath, avg(Ephonics) as EPhonics, avg(Etotal) as ETotal from view_pips group by year");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tPIPS = new SPPIPS();
                            tPIPS.year = new Year(itemRow[0].ToString());
                            //stuck at number
                            tempListSchoolData = new List<GenericSchoolData>();
                            tempListSchoolData.Add(new GenericSchoolData("Reading Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[2])));
                            tempListSchoolData.Add(new GenericSchoolData("Reading End", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                            tempListSchoolData.Add(new GenericSchoolData("Math Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                            tempListSchoolData.Add(new GenericSchoolData("Math End", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                            tempListSchoolData.Add(new GenericSchoolData("Phonics Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                            tempListSchoolData.Add(new GenericSchoolData("Phonics End", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                            tempListSchoolData.Add(new GenericSchoolData("Total Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                            tempListSchoolData.Add(new GenericSchoolData("Total End", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                            tPIPS.ListGenericSchoolData = tempListSchoolData;
                            listPIPS.Add(tPIPS);
                        }
                    }
                }

            }
            else
            {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select * from view_pips where seedcode = " + seedcode);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tPIPS = new SPPIPS();
                            tPIPS.year = new Year(itemRow[0].ToString());
                            tempListSchoolData = new List<GenericSchoolData>();
                            tempListSchoolData.Add(new GenericSchoolData("Reading Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[2])));
                            tempListSchoolData.Add(new GenericSchoolData("Reading End", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                            tempListSchoolData.Add(new GenericSchoolData("Maths Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                            tempListSchoolData.Add(new GenericSchoolData("Maths End", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                            tempListSchoolData.Add(new GenericSchoolData("Phonics Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                            tempListSchoolData.Add(new GenericSchoolData("Phonics End", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                            tempListSchoolData.Add(new GenericSchoolData("Total Scores Start", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                            tempListSchoolData.Add(new GenericSchoolData("Total Scores End", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                            tPIPS.ListGenericSchoolData = tempListSchoolData;
                            listPIPS.Add(tPIPS);
                        }
                    }
                }

            }

            return listPIPS.OrderBy(x=>x.year.year).ToList();
        }

        private SchoolPIPSTransform GetSchoolPIPSTransform(List<BaseSPDataModel> listSchoolData)
        {
            var eSchoolPIPSTransform = new SchoolPIPSTransform();

            // add columns info
            eSchoolPIPSTransform.ListColumnInfo = listSchoolData[0].listPIPS[0].ListGenericSchoolData; // column header

            // add rows
            eSchoolPIPSTransform.ListSchoolPIPSTransformRow = new List<SchoolPIPSTransformRow>();
            foreach (var ePIPS in listSchoolData[0].listPIPS) // year loop
            {
                foreach (var eSchoolInfo in listSchoolData) // school loop
                {
                    eSchoolPIPSTransform.ListSchoolPIPSTransformRow.Add(new SchoolPIPSTransformRow()
                    {
                        Year = ePIPS.year,
                        SPSchool = new School(eSchoolInfo.SeedCode,eSchoolInfo.SchoolName), // filter only required properties
                        ListGenericSchoolData = eSchoolInfo.listPIPS.First(x => x.year.year == ePIPS.year.year).ListGenericSchoolData.ToList()
                    });
                }
            }

            return eSchoolPIPSTransform;
        }

        //Historical InCAS data
        private List<SPInCAS> GetHistoricalInCASData(string seedcode)
        {
            List<SPInCAS> listInCAS = new List<SPInCAS>();
            List<GenericSchoolData> tempdataAgeDiffrence = new List<GenericSchoolData>();
            List<GenericSchoolData> tempdataStandardised = new List<GenericSchoolData>();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            SPInCAS tInCAS = new SPInCAS();
 
            if (seedcode.Equals("1002"))
            {

                var listResult = rpGeneric2nd.FindByNativeSQL("Select * from view_incas where YearGroup in(1,2,3,4,5) and seedcode = 1002");
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tInCAS = new SPInCAS();
                            tInCAS.year = new Year(itemRow[0].ToString());
                            tInCAS.yeargroup = Convert.ToInt16(itemRow[2].ToString());
                            tempdataAgeDiffrence = new List<GenericSchoolData>();
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Developed Ability", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Spelling", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("General Maths", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Mental Arithmetics", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
 
                            tempdataStandardised = new List<GenericSchoolData>();
                            tempdataStandardised.Add(new GenericSchoolData("Developed Ability", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                            tempdataStandardised.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                            tempdataStandardised.Add(new GenericSchoolData("Spelling", NumberFormatHelper.ConvertObjectToFloat(null)));
                            tempdataStandardised.Add(new GenericSchoolData("General Maths", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                            tempdataStandardised.Add(new GenericSchoolData("Mental Arithmetics", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                            
                            tInCAS.ListGenericAgeDiffrence = tempdataAgeDiffrence;
                            tInCAS.ListGenericStandardised = tempdataStandardised;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                            tInCAS.ListGenericDevAbil = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                            tInCAS.ListGenericGenMath = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                            tInCAS.ListGenericMentArith = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                            tInCAS.ListGenericReading = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(null)));
                            tInCAS.ListGenericSpelling = temp;

                            listInCAS.Add(tInCAS);
                        }
                    }
                }

            }
            else
            {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select * from view_incas where YearGroup in(1,2,3,4,5) and seedcode = " + seedcode);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tInCAS = new SPInCAS();
                            tInCAS.year = new Year(itemRow[0].ToString());
                            tInCAS.yeargroup = Convert.ToInt16(itemRow[2].ToString());
                            tempdataAgeDiffrence = new List<GenericSchoolData>();
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Developed Ability", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Spelling", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("General Maths", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                            tempdataAgeDiffrence.Add(new GenericSchoolData("Mental Arithmetics", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));

                            tempdataStandardised = new List<GenericSchoolData>();
                            tempdataStandardised.Add(new GenericSchoolData("Developed Ability", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                            tempdataStandardised.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                            tempdataStandardised.Add(new GenericSchoolData("Spelling", NumberFormatHelper.ConvertObjectToFloat(null)));
                            tempdataStandardised.Add(new GenericSchoolData("General Maths", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                            tempdataStandardised.Add(new GenericSchoolData("Mental Arithmetics", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));

                            tInCAS.ListGenericAgeDiffrence = tempdataAgeDiffrence;
                            tInCAS.ListGenericStandardised = tempdataStandardised;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                            tInCAS.ListGenericDevAbil = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                            tInCAS.ListGenericGenMath = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                            tInCAS.ListGenericMentArith = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                            tInCAS.ListGenericReading = temp;

                            temp = new List<GenericSchoolData>();
                            temp.Add(new GenericSchoolData("Av. Age at Test (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                            temp.Add(new GenericSchoolData("Av. Age Equivalent Score (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                            temp.Add(new GenericSchoolData("Av. Age Diffrence (yrs)", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                            temp.Add(new GenericSchoolData("Av. Standardised Scores", NumberFormatHelper.ConvertObjectToFloat(null)));
                            tInCAS.ListGenericSpelling = temp;

                            listInCAS.Add(tInCAS);
                        }
                    }
                }

            }

            return listInCAS.OrderBy(x => x.year.year).ToList(); ;
        }

        [HttpGet]
        [Route("SchoolProfiles/PrimarySchoolProfile/ExportData")]
        public JsonResult ExportData(List<string> listSeedCode,string sYear, string tablename)
        {
            try
            {
                var listSchool = GetListSchool(rpGeneric2nd, "2");
                List<School> ListSchoolSelected = listSeedCode != null && listSeedCode.Count > 0 ? listSchool.Where(x => listSeedCode.Contains(x.seedcode)).ToList() : null;
                //add Aberdeen Primary School data
                ListSchoolSelected.Add(new School("1002", "Aberdeen Primary Schools"));

                var arrExportInfo = ExportDataToXLSX(rpGeneric2nd, ListSchoolSelected, sYear, tablename);
                return Json(new
                {
                    DownloadUrl = arrExportInfo[0],
                    FileName = arrExportInfo[1]
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        // FSM Chart
        private SplineCharts GetChartFSM(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "Free School Meal";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            string[] colors = { "#ED561B", "#DDDF00", "#24CBE5", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexcolour = 0;

            if (listSchool != null && listSchool.Count > 0)
            {
                //select data from 2014 (P4-P7) 
                var temp = listSchool[0].listFSM.Where(x => Convert.ToInt32(x.year.year) >= 2014).ToList();
                eSplineCharts.xAxis.categories = temp.Select(x => x.year.academicyear).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of P4-P7 Roll Registered for FSM" };

                foreach (var eSchool in listSchool)
                {
                    //select data from 2014 (P4-P7) 

                    temp = eSchool.listFSM.Where(x => Convert.ToInt32(x.year.year) >= 2014).ToList();

                    var listSeriesStart = temp.Select(x => x.GenericSchoolData.sPercent.Equals("n/a") ? null : (float?)float.Parse(x.GenericSchoolData.sPercent)).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName,
                        color = colors[indexcolour],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexcolour++;
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
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel.P1EarlyLevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
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

        // CfeP1Level Chart
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P4FirstLevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel.P4FirstLevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
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

        // CfeP1Level Chart
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P7SecondLevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel.P7SecondLevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
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

        // CfeP1Level Chart
        //protected ColumnCharts GetChartCfeLevelData(List<SPSchool> listSchool, Year selectedyear) // query from database and return charts object
        //{
        //    string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
        //    int indexColor = 0;
        //    var eColumnCharts = new ColumnCharts();
        //    eColumnCharts.SetDefault(false);
        //    eColumnCharts.title.text = "P7 - Second Level, " + selectedyear.academicyear;
        //    eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
        //    eColumnCharts.yAxis.max = 100;

        //    eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();

        //    if (listSchool != null && listSchool.Count > 0)
        //    {
        //        eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.EReading.Select(x => x.Code).ToList();
        //        foreach (var eSchool in listSchool)
        //        {
        //            eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
        //            {
        //                type = "column",
        //                name = "ER" + eSchool.SchoolName,
        //                data = eSchool.SPCfElevel.EReading.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
        //                color = eSchool.SeedCode == "1002" ? "#357821" : colors[indexColor]
        //            });
        //            indexColor++;
        //            eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
        //            {
        //                type = "column",
        //                name = "EW" + eSchool.SchoolName,
        //                data = eSchool.SPCfElevel.EWriting.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
        //                color = eSchool.SeedCode == "1002" ? "#1494a8" : colors[indexColor]
        //            });
        //            indexColor++;
        //            eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
        //            {
        //                type = "column",
        //                name = "ELT" + eSchool.SchoolName,
        //                data = eSchool.SPCfElevel.EListeningTalking.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
        //                color = eSchool.SeedCode == "1002" ? "#f61fdd" : colors[indexColor]
        //            });
        //            indexColor++;
        //            eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
        //            {
        //                type = "column",
        //                name = "N" + eSchool.SchoolName,
        //                data = eSchool.SPCfElevel.Numeracy.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
        //                color = eSchool.SeedCode == "1002" ? "#919300" : colors[indexColor]
        //            });
        //            indexColor++;
        //        }
        //    }

        //    eColumnCharts.options.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
        //    {
        //        enabled = true,
        //        filename = "export"
        //    };

        //    eColumnCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

        //    return eColumnCharts;
        //}


        // CfeP1Level Chart
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
                foreach (var eSchool in listSchool)
                {
                    indexColor = 0;

                    if (!eSchool.SeedCode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ1.Select(x => x.Percent ==null? null: (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ2.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ3.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ4.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ5.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                    }
                }
            }

            //eColumnCharts.tooltip = new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.tooltip()
            //{
            //    headerFormat = @"<span style='font - size:10px'>{point.key}</span><table>",
            //    pointFormat = "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y:,.2f}</b></td><td style='padding:0'><b>{series.exInfo:,.2f}</b></td></tr>",
            //    footerFormat = "</table>",
            //    shared = true,
            //    useHTML = true
            //};

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // CfeP4Level Chart
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P4FirstLevelQ1.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    indexColor = 0;

                    if (!eSchool.SeedCode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel.P4FirstLevelQ1.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.P4FirstLevelQ2.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.P4FirstLevelQ3.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.P4FirstLevelQ4.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.P4FirstLevelQ5.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
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

        // CfeP7Level Chart
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.P7SecondLevelQ1.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    indexColor = 0;

                    if (!eSchool.SeedCode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel.P7SecondLevelQ1.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.P7SecondLevelQ2.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.P7SecondLevelQ3.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.P7SecondLevelQ4.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.P7SecondLevelQ5.Select(x => x.Percent == null ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
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

        [SchoolAuthentication]
        [Transactional]
        [HttpGet]
        [Route("SchoolProfiles/PrimarySchoolProfile/GetReport")]
        public JsonResult GetReport([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear)
        {
            List<BaseSPDataModel> listSchoolData = Session["listSchoolData"] as List<BaseSPDataModel>;
            List<SPCfEReport> CFElevle = GetPrimaryCfeLevelDataforReport(rpGeneric2nd, listSchoolData[0].SeedCode, "2");
            List<SPCfEReport> listINCAS = GetPrimaryINCASDataforReport(rpGeneric2nd, listSchoolData[0].SeedCode);
            //List<SPExclusion> listSPExclusion = GetExclusionDataforReport(rpGeneric2nd, listSchoolData[0].SeedCode);
            List<SPCfElevel> listSPCfElevel = new List<SPCfElevel>();
            listSPCfElevel = GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, "1002", "2");
            listSPCfElevel.AddRange(GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, "9999", "2"));

            try
            {

                //get path to template and instance output
                //get path to template and instance output
                //string docTemplatePath = @"C:\Users\HSaevanee\Source\Repos\accdatastore\ACCDataStore.Web\download\templateSD.docx";
                string docTemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"~\download\templatePM.docx");
                //string docOutputPath = @"C:\Users\HSaevanee\Source\Repos\accdatastore\ACCDataStore.Web\download\SDReport.docx";
                string docOutputPath = System.Web.HttpContext.Current.Server.MapPath(@"~\download\PMReport.docx");
                //create copy of template so that we don't overwrite it
                System.IO.File.Copy(docTemplatePath, docOutputPath, true);

                //stand up object that reads the Word doc package
                using (WordprocessingDocument doc = WordprocessingDocument.Open(docOutputPath, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;
                    var paras = body.Elements<Paragraph>();
                    var document = doc.MainDocumentPart.Document;
                    foreach (var text in document.Descendants<Text>()) // <<< Here
                    {
                        if (text.Text.Contains("schoolname"))
                        {
                            text.Text = text.Text.Replace("schoolname", listSchoolData[0].SchoolName);
                        }
                    }

                 
                    //AcceptRevisions(tempfilename, "Hataichanok Saevanee");
                    //add School Roll data
                    ChangeTextInTBSchoolRoll(doc, listSchoolData[0].listStudentStage, 2, 0);
                    //AcceptRevisions(tempfilename, "Hataichanok Saevanee");
                    ChangeTextInTBSIMD(doc, listSchoolData[0].SIMD, 1);
                    //add listlevelofEnglish
                    ChangeTextInTBEAL(doc, listSchoolData[0].listLevelOfEnglish,2);
                    //add additional support needs
                    ChangeTextInTBASN(doc, listSchoolData[0].listStudentNeed,3);
                    //Freeschoolmeal
                    ChangeTextInTBFSM(doc, listSchoolData[0].listFSM,4);
                    //Attendance
                    ChangeTextInTBAAETrend(doc, listSchoolData[0].listAttendance,5);
                    //Exclusions
                    ChangeTextInTBExclusionTrend(doc,  listSchoolData[0].listExclusion, 6);                  
                    //get CFE data for 2015
                    SPCfEReport tempCFElevle = CFElevle.Where(x => x.year.year.Equals("2015")).FirstOrDefault();
                    //P1 CfE Level 2015/16
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.P1, 7);
                    //P1 National 2015
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 8, "2015","P1");
                    //P4 CfE Level 2015/16
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.P4, 11);
                    //P4 National 2015
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 12, "2015", "P4");

                    //P7 CfE Level 2015/16
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.P7, 15);
                    //P7 National 2015
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 16, "2015", "P7");


                    //get CFE data for 2016
                    tempCFElevle = CFElevle.Where(x => x.year.year.Equals("2016")).FirstOrDefault();
                    //P1 CfE Level 2016/17
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.P1, 9);
                    //P1 National 2016
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 10, "2016","P1");
                    //P4 CfE Level 2016/17
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.P4, 13);
                    //P4 National 2016
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 14, "2016", "P4");
                    //P7 CfE Level 2016/17
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.P7, 17);
                    //P7 National 2016
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 18, "2016", "P7");

                    //PIPS Baseline assessment: 5 Year trend
                    ChangeTextInTBPIPSTrend(doc,listSchoolData[0].listPIPS,19);
                    //INCASP2:5Year Trend 
                    ChangeTextInTBINCASTrend(doc,listSchoolData[0].listInCASP2,20);
                    //INCASP2:INCAS by FSM and LAC 
                    ChangeTextInTBINCAS(doc, listINCAS[0].P2, 21);
                    //INCASP3:5Year Trend 
                    ChangeTextInTBINCASTrend(doc, listSchoolData[0].listInCASP3, 22);
                    //INCASP3:INCAS by FSM and LAC 
                    ChangeTextInTBINCAS(doc, listINCAS[0].P3, 23);
                    //INCASP4:5Year Trend 
                    ChangeTextInTBINCASTrend(doc,listSchoolData[0].listInCASP4, 24);
                    //INCASP5:5Year Trend 
                    ChangeTextInTBINCASTrend(doc,listSchoolData[0].listInCASP5, 25);
                    //INCASP5:INCAS by FSM and LAC
                    ChangeTextInTBINCAS(doc, listINCAS[0].P5, 26);
                    //INCASP6:5Year Trend 
                    ChangeTextInTBINCASTrend(doc,listSchoolData[0].listInCASP6, 27);
                    //INCASP6:INCAS by FSM and LAC 
                    ChangeTextInTBINCAS(doc, listINCAS[0].P6, 28);



                    doc.MainDocumentPart.Document.Save();
                }

                return Json(new
                {
                    DownloadUrl = "download/" + "PMReport.docx",
                    FileName = "PMReport.docx"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private void createword(string filepath)
        {

            // Create a document by supplying the filepath. 
            using (WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(filepath, WordprocessingDocumentType.Document))
            {
                // Add a main document part. 
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                // Create the document structure and add some text.
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text("Create text in body - schoolname CreateWordprocessingDocument"));
            }
        
        
        }

        private void AcceptRevisions(string fileName, string authorName)
        {

            // Given a document name and an author name, accept revisions. 
            using (WordprocessingDocument wdDoc = 
                WordprocessingDocument.Open(fileName, true))
            {
                Body body = wdDoc.MainDocumentPart.Document.Body;

                // Handle the formatting changes.
                List<OpenXmlElement> changes = 
                    body.Descendants<ParagraphPropertiesChange>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList();

                foreach (OpenXmlElement change in changes)
                {
                    change.Remove();
                }

                // Handle the deletions.
                List<OpenXmlElement> deletions = 
                    body.Descendants<Deleted>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList();
        
                deletions.AddRange(body.Descendants<DeletedRun>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList());
        
                deletions.AddRange(body.Descendants<DeletedMathControl>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList());
        
                foreach (OpenXmlElement deletion in deletions)
                {
                    deletion.Remove();
                }

                // Handle the insertions.
                List<OpenXmlElement> insertions = 
                    body.Descendants<Inserted>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList();

                insertions.AddRange(body.Descendants<InsertedRun>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList());

                insertions.AddRange(body.Descendants<InsertedMathControl>()
                    .Where(c => c.Author.Value == authorName).Cast<OpenXmlElement>().ToList());

                foreach (OpenXmlElement insertion in insertions)
                {
                    // Found new content.
                    // Promote them to the same level as node, and then delete the node.
                    foreach (var run in insertion.Elements<Run>())
                    {
                        if (run == insertion.FirstChild)
                        {
                            insertion.InsertAfterSelf(new Run(run.OuterXml));
                        }
                        else
                        {
                            insertion.NextSibling().InsertAfterSelf(new Run(run.OuterXml));
                        }
                    }
                    insertion.RemoveAttribute("rsidR", 
                        "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                    insertion.RemoveAttribute("rsidRPr", 
                        "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                    insertion.Remove();
                }
            }


        }

        // Change text in 1.7 Exclusion by FSM and LAC
        private void ChangeTextInTBExclusion(WordprocessingDocument doc, List<SPExclusion> data, int tableid)
        {
            try
            {
                // code 0 = "Temporary Exclusions", 1="Removed From Register", 2="Number of days per 1000 pupils lost to exclusions" 
                string[] years = new string[] { "2015", "2016"}; // using i to reference
                string[] codes = new string[] { "ALL", "FSM", "LAC"}; // using j to reference
                List<GenericSchoolData> tempdata;

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 2; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    //get list data by dataset names

                    tempdata = data.Where(x => x.YearInfo.year.Equals(years[i - 2])).Select(x => x.ListGenericSchoolData).First();
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        string sdata = tempdata == null? "n/a": tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.sCount).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBINCAS : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 2.2 PIPS	baseline assessment:5 Year trend
        private void ChangeTextInTBPIPSTrend(WordprocessingDocument doc, List<SPPIPS> data, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.year.academicyear.Equals("2008/09"));
                data.RemoveAll(s => s.year.academicyear.Equals("2009/10"));
                data.RemoveAll(s => s.year.academicyear.Equals("2010/11"));
                data.RemoveAll(s => s.year.academicyear.Equals("2011/12"));
                string[] years = new string[] { "2012/13", "2013/14", "2014/15", "2015/16", "2016/17" }; // using j to reference
                List<GenericSchoolData> tempdata;

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 2; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    //get list data by year
                    tempdata = data.Where(x => x.year.academicyear.Equals(years[i - 2])).Select(x => x.ListGenericSchoolData).First();
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = tempdata[j-1].sPercent;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBPIPSTrend : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 2.3 INCAS P2:5 Year trend
        private void ChangeTextInTBINCASTrend(WordprocessingDocument doc, List<SPInCAS> data, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                string[] codes = new string[] { "Developed Ability", "Reading", "Spelling", "General Maths", "Mental Arithmetics"}; // using i to reference
                string[] years = new string[] { "2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                List<GenericSchoolData> tempdata;


                //using (WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true))
                //{
                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                for (int i = 2; i < table.Elements<TableRow>().Count(); i++)
                {

                    // travel through each row from row 1.
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        //get list data by year
                        tempdata = data.Where(x => x.year.academicyear.Equals(years[j - 1])).Select(x => x.ListGenericAgeDiffrence).FirstOrDefault();
                        //the get data by code
                        string sdata = tempdata==null? "n/a" : tempdata.Where(x => x.Code.Equals(codes[i - 2])).Select(x => x.sPercent).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata.Equals("n/a") ? "#" : sdata.Equals("0.0") ? "-" : sdata;

                    }
                }
                //}

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBINCASTrend: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 1.8 INCAS by FSM and LAC
        private void ChangeTextInTBINCAS(WordprocessingDocument doc, List<SPReport> data, int tableid)
        {
            try
            {
                // code 0 = "Temporary Exclusions", 1="Removed From Register", 2="Number of days per 1000 pupils lost to exclusions" 
                string[] datasets = new string[] { "DevAbil", "Reading", "Spelling", "GenMaths", "MentArith" }; // using i to reference
                string[] codes = new string[] { "ALL", "FSM", "LAC", "30M", "40M", "30L" }; // using j to reference
                List<GenericSchoolData> tempdata;

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 3; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    //get list data by dataset names

                    tempdata = data.Where(x => x.code.Equals(datasets[i - 3])).Select(x => x.listdata).First();
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        string sdata = tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.sPercent).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata.Equals("n/a") ? "#" : sdata.Equals("0.0") ? "-" : sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBINCAS : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text CfE level compare with national data
        protected void ChangeTextInTBCfENational(WordprocessingDocument doc, List<SPCfElevel> data, int tableid, string selectedyear, string state)
        {
            try
            {
                // code 0 = "Temporary Exclusions", 1="Removed From Register", 2="Number of days per 1000 pupils lost to exclusions" 
                string[] datasets = new string[] { "Reading", "Writing", "Listening & Talking", "Numeracy" }; // using i to reference              
                SPCfElevel tempSPCfElevel;
                List<GenericSchoolData> tempdata;

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 2; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    if (i == 2)
                    {
                        tempSPCfElevel = data.Where(x => x.seedcode.Equals("1002") && x.year.year.Equals(selectedyear)).FirstOrDefault();
                    }
                    else {
                        tempSPCfElevel = data.Where(x => x.seedcode.Equals("9999") && x.year.year.Equals(selectedyear)).FirstOrDefault();        
                    }

                    if (state.Equals("P1")) {

                        tempdata = tempSPCfElevel == null ? null : tempSPCfElevel.P1EarlyLevel;

                    }
                    else if (state.Equals("P4"))
                    {
                        tempdata = tempSPCfElevel == null ? null : tempSPCfElevel.P4FirstLevel;

                    }
                    else {

                        tempdata = tempSPCfElevel == null ? null : tempSPCfElevel.P7SecondLevel;
                    
                    }
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        string sdata = tempdata == null? "n/a":tempdata.Where(x => x.Code.Equals(datasets[j - 1])).Select(x => x.sPercent).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata.Equals("n/a") ? "--" : sdata.Equals("0.0") ? "-" : sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBCfENational : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

    }
}