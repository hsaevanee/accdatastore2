using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class LocalitySchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public LocalitySchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: SchoolProfiles/Locality
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("SchoolProfiles/LocalitySchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = new List<School>() { new School("100201", "Locality 1 - Torry East/Torry West"), new School("100202", "Locality 2 - Northfield/Mastrick/Cummings Park/Heathryfold and Middlefield"), new School("100203", "Locality 3 - Seaton/Tillydrone/Woodside") };

                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x => x.year.Equals("2016")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x => x.seedcode.Equals("100201")).ToList() : null;
                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()),
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected != null ? eYearSelected.GetJson() : null,
                    ListSchoolSelected = ListSchoolSelected.Where(x => x.seedcode.Equals("100201")).Select(x => x.GetSchoolDetailJson())
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [Route("SchoolProfiles/LocalitySchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                string sSchoolType = "2";
                var listSchool = new List<School>() { new School("100201", "Locality 1 - Torry East/Torry West"), new School("100202", "Locality 2 - Northfield/Mastrick/Cummings Park/Heathryfold and Middlefield"), new School("100203", "Locality 3 - Seaton/Tillydrone/Woodside") };
                var listYear = GetListYear();
                var eYearSelected = new Year(sYear);
                List<School> ListSchoolSelected = listSeedCode != null && listSeedCode.Count > 0 ? listSchool.Where(x => listSeedCode.Contains(x.seedcode)).ToList() : null;

                var listSchoolData = GetSchoolData(ListSchoolSelected, sYear, sSchoolType);
                //SchoolPIPSTransform TempPIPSTransform = GetSchoolPIPSTransform(listSchoolData);

                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()), // all school
                    ListSchoolSelected = ListSchoolSelected.Where(x => !x.seedcode.Equals("1002")).Select(x => x.GetSchoolDetailJson()), // set selected list of school
                    ListYear = listYear.Select(x => x.GetJson()),
                    YearSelected = eYearSelected.GetJson(),
                    ListingData = listSchoolData, // table data
                    ChartData = GetChartData(listSchoolData, eYearSelected),
                    //ListPIPSTransform = TempPIPSTransform
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
            try
            {
                var listYear = GetListYear();
                var listSchoolData = new List<BaseSPDataModel>();
                BaseSPDataModel tempSchool = new BaseSPDataModel();

                //add Aberdeen Primary School data
                tListSchoolSelected.Add(new School("1002", "Aberdeen City"));

                Year selectedyear = new Year(sYear);

                foreach (School school in tListSchoolSelected)
                {
                    tempSchool = new BaseSPDataModel();
                    tempSchool.SeedCode = school.seedcode;
                    tempSchool.SchoolName = school.name;
                    tempSchool.listSPCfElevel = GetHistoricalPrimaryCfeLevelData(rpGeneric2nd, school.seedcode, sSchoolType);
                    tempSchool.SPCfElevel = tempSchool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                    tempSchool.listSPCfElevel2 = GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, school.seedcode, "3");
                    tempSchool.SPCfElevel2 = tempSchool.listSPCfElevel2.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();

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
                Entity.SchoolProfiles.Census.Entity.LocalityChartData chartdata = new Entity.SchoolProfiles.Census.Entity.LocalityChartData();
                //chartdata.ChartNationalityIdentity = GetChartNationalityIdentity(listSchool, eYearSelected);
                //chartdata.ChartLevelOfEnglish = GetChartLevelofEnglish(listSchool, eYearSelected);
                //chartdata.ChartLevelOfEnglishByCatagories = GetChartLevelofEnglishbyCatagories(listSchool, eYearSelected);
                //chartdata.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected);
                //chartdata.ChartPIPSReading = GetChartPIPS(listSchool, "Reading");
                //chartdata.ChartPIPSMath = GetChartPIPS(listSchool, "Maths");
                //chartdata.ChartPIPSPhonics = GetChartPIPS(listSchool, "Phonics");
                //chartdata.ChartPIPSTotal = GetChartPIPS(listSchool, "Total Scores");
                //chartdata.CartSchoolRollForecast = GetChartSchoolRollForecast(listSchool);
                //chartdata.ChartIEP = GetChartStudentNeedIEP(listSchool);
                //chartdata.ChartCSP = GetChartStudentNeedCSP(listSchool);
                //chartdata.ChartLookedAfter = GetChartLookedAfter(listSchool);
                //chartdata.ChartAttendance = GetChartAttendance(listSchool, "Attendance");
                //chartdata.ChartAuthorisedAbsence = GetChartAttendance(listSchool, "Authorised Absence");
                //chartdata.ChartUnauthorisedAbsence = GetChartAttendance(listSchool, "Unauthorised Absence");
                //chartdata.ChartTotalAbsence = GetChartAttendance(listSchool, "Total Absence");
                //chartdata.ChartNumberofDaysLostExclusion = GetChartExclusion(listSchool, "Number of Days Lost Per 1000 Pupils Through Exclusions");
                //chartdata.ChartNumberofExclusionRFR = GetChartExclusion(listSchool, "Number of Removals from the Register");
                //chartdata.ChartNumberofExclusionTemporary = GetChartExclusion(listSchool, "Number of Temporary Exclusions");
                //chartdata.ChartFSM = GetChartFSM(listSchool);
                chartdata.ChartCfeP1Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP1LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP4Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP4LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP7Level = listSchool[0].SPCfElevel == null ? null : GetChartCfeP7LevelData(listSchool, eYearSelected);
                chartdata.ChartCfeP1LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP1LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfeP4LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP4LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfeP7LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfeP7LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfe3Level = listSchool[0].SPCfElevel2 == null ? null : GetChartCfe3LevelData(listSchool, eYearSelected);
                chartdata.ChartCfe4Level = listSchool[0].SPCfElevel2 == null ? null : GetChartCfe4LevelData(listSchool, eYearSelected);
                chartdata.ChartCfe3LevelbyQuintile = listSchool[0].SPCfElevel2 == null ? null : GetChartCfe3LevelbyQuantileData(listSchool, eYearSelected);
                chartdata.ChartCfe4LevelbyQuintile = listSchool[0].SPCfElevel2 == null ? null : GetChartCfe4LevelbyQuantileData(listSchool, eYearSelected);

                return chartdata;
            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
                return null;
            }
        }

        //Historical InCAS data
        protected new List<SPCfElevel> GetHistoricalPrimaryCfeLevelData(IGenericRepository2nd rpGeneric2nd, string seedcode, string schooltype)
        {
            List<SPCfElevel> listSPCfelevel = new List<SPCfElevel>();
            SPCfElevel tSPCfelevel = new SPCfElevel();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();

            //get actual number 
            string query = "Select * from summary_primary_cfe_locality where Localitycode =" + seedcode + " and SchoolType = " + schooltype;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tSPCfelevel = new SPCfElevel();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());
                        tSPCfelevel.seedcode = itemRow[1].ToString();
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        tSPCfelevel.P1EarlyLevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        tSPCfelevel.P4FirstLevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        tSPCfelevel.P7SecondLevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        tSPCfelevel.P1EarlyLevelQ1 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        tSPCfelevel.P1EarlyLevelQ2 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tSPCfelevel.P1EarlyLevelQ3 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        tSPCfelevel.P1EarlyLevelQ4 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[33])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[34])));
                        tSPCfelevel.P1EarlyLevelQ5 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[35])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[36])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[37])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[38])));
                        tSPCfelevel.P4FirstLevelQ1 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[39])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[40])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[41])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[42])));
                        tSPCfelevel.P4FirstLevelQ2 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[43])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[44])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[45])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[46])));
                        tSPCfelevel.P4FirstLevelQ3 = temp;


                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[47])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[48])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[49])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[50])));
                        tSPCfelevel.P4FirstLevelQ4 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[51])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[52])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[53])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[54])));
                        tSPCfelevel.P4FirstLevelQ5 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[55])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[56])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[57])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[58])));
                        tSPCfelevel.P7SecondLevelQ1 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[59])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[60])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[61])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[62])));
                        tSPCfelevel.P7SecondLevelQ2 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[63])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[64])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[65])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[66])));
                        tSPCfelevel.P7SecondLevelQ3 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[67])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[68])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[69])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[70])));
                        tSPCfelevel.P7SecondLevelQ4 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[71])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[72])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[73])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[74])));
                        tSPCfelevel.P7SecondLevelQ5 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", Convert.ToInt16(itemRow[75])));
                        temp.Add(new GenericSchoolData("Writing", Convert.ToInt16(itemRow[76])));
                        temp.Add(new GenericSchoolData("Listening & Talking", Convert.ToInt16(itemRow[77])));
                        temp.Add(new GenericSchoolData("Numeracy", Convert.ToInt16(itemRow[78])));
                        tSPCfelevel.P1EarlyLevel_no = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", Convert.ToInt16(itemRow[79])));
                        temp.Add(new GenericSchoolData("Writing", Convert.ToInt16(itemRow[80])));
                        temp.Add(new GenericSchoolData("Listening & Talking", Convert.ToInt16(itemRow[81])));
                        temp.Add(new GenericSchoolData("Numeracy", Convert.ToInt16(itemRow[82])));
                        tSPCfelevel.P4FirstLevel_no = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", Convert.ToInt16(itemRow[83])));
                        temp.Add(new GenericSchoolData("Writing", Convert.ToInt16(itemRow[84])));
                        temp.Add(new GenericSchoolData("Listening & Talking", Convert.ToInt16(itemRow[85])));
                        temp.Add(new GenericSchoolData("Numeracy", Convert.ToInt16(itemRow[86])));
                        tSPCfelevel.P7SecondLevel_no = temp;
                        listSPCfelevel.Add(tSPCfelevel);
                    }
                }
            }

            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
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
                            data = eSchool.SPCfElevel.P1EarlyLevelQ1.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ2.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ3.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ4.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.P1EarlyLevelQ5.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
                            data = eSchool.SPCfElevel.P4FirstLevelQ1.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.P4FirstLevelQ2.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.P4FirstLevelQ3.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.P4FirstLevelQ4.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.P4FirstLevelQ5.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
                            data = eSchool.SPCfElevel.P7SecondLevelQ1.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.P7SecondLevelQ2.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.P7SecondLevelQ3.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.P7SecondLevelQ4.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.P7SecondLevelQ5.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel2.ListThirdlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel2.ListThirdlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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

        // Cfe3Level Chart
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel2.ListForthlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel2.ListForthlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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

        // Cfe3Level Chart
        protected ColumnCharts GetChartCfe3LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[0].SchoolName;
            eColumnCharts.subtitle.text = "Third Level or better by SIMD, 2015/16";

            eColumnCharts.yAxis.title.text = "% of S3 pupils";
            eColumnCharts.yAxis.max = 100;
            eColumnCharts.chart.type = "";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel2.ListThirdlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    if (!eSchool.SeedCode.Equals("1002"))
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel2.SIMDQ1_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel2.SIMDQ2_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel2.SIMDQ3_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel2.SIMDQ4_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel2.SIMDQ5_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
        protected ColumnCharts GetChartCfe4LevelbyQuantileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[0].SchoolName;
            eColumnCharts.subtitle.text = "Fourth Level by SIMD," + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of S3 pupils";
            eColumnCharts.yAxis.max = 100;
            eColumnCharts.chart.type = "";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel2.ListForthlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    indexColor = 0;

                    if (!eSchool.SeedCode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel2.SIMDQ1_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel2.SIMDQ2_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel2.SIMDQ3_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel2.SIMDQ4_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel2.SIMDQ5_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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

        //Historical InCAS data
        protected new List<SPCfElevel> GetHistoricalSecondaryCfeLevelData(IGenericRepository2nd rpGeneric2nd, string seedcode, string schooltype)
        {
            List<SPCfElevel> listSPCfelevel = new List<SPCfElevel>();
            SPCfElevel tSPCfelevel = new SPCfElevel();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();

            //get actual number 
            string query = "Select * from summary_secondary_cfe_locality where seedcode =" + seedcode + " and SchoolType = " + schooltype;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tSPCfelevel = new SPCfElevel();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());
                        tSPCfelevel.seedcode = itemRow[1].ToString();

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        tSPCfelevel.ListThirdlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        tSPCfelevel.ListForthlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        tSPCfelevel.SIMDQ1_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        tSPCfelevel.SIMDQ1_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        tSPCfelevel.SIMDQ2_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tSPCfelevel.SIMDQ2_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[33])));
                        tSPCfelevel.SIMDQ3_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[34])));
                        tSPCfelevel.SIMDQ3_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[35])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[37])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[39])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[41])));
                        tSPCfelevel.SIMDQ4_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[36])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[38])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[40])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[42])));
                        tSPCfelevel.SIMDQ4_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[43])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[45])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[47])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[49])));
                        tSPCfelevel.SIMDQ5_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[44])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[46])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[48])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[50])));
                        tSPCfelevel.SIMDQ5_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", Convert.ToInt16(itemRow[51])));
                        temp.Add(new GenericSchoolData("Writing", Convert.ToInt16(itemRow[53])));
                        temp.Add(new GenericSchoolData("Listening & Talking", Convert.ToInt16(itemRow[55])));
                        temp.Add(new GenericSchoolData("Numeracy", Convert.ToInt16(itemRow[57])));
                        tSPCfelevel.ListThirdlevel_no = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", Convert.ToInt16(itemRow[52])));
                        temp.Add(new GenericSchoolData("Writing", Convert.ToInt16(itemRow[54])));
                        temp.Add(new GenericSchoolData("Listening & Talking", Convert.ToInt16(itemRow[56])));
                        temp.Add(new GenericSchoolData("Numeracy", Convert.ToInt16(itemRow[58])));
                        tSPCfelevel.ListForthlevel_no = temp;

                        listSPCfelevel.Add(tSPCfelevel);
                    }
                }
            }



            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
        }


    }
}