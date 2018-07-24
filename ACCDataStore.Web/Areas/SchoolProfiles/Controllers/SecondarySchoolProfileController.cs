using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile.Entities;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using Common.Logging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class SecondarySchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public SecondarySchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        [SchoolAuthentication]
        [Transactional]
        // GET: SchoolProfiles/SecondarySchoolProfile
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/SecondarySchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = GetListSchool(rpGeneric2nd, "3");
                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x => x.year.Equals("2017")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x => x.seedcode.Equals("5244439")).ToList() : null;
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
        [Route("SchoolProfiles/SecondarySchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                string sSchoolType = "3";
                var listSchool = GetListSchool(rpGeneric2nd, sSchoolType);
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
                   // ListPIPSTransform = TempPIPSTransform
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private List<BaseSPDataModel> GetSchoolData(List<School> tListSchoolSelected, string sYear,string sSchoolType)
        {
            var listYear = GetListYear();
            var listSchoolData = new List<BaseSPDataModel>();
            SecondarySPDataModel tempSchool = new SecondarySPDataModel();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new School("1002", "Aberdeen Secondary Schools"));
            
            Year selectedyear = new Year(sYear);

            foreach (School school in tListSchoolSelected)
            {
                tempSchool = new SecondarySPDataModel();
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
                
                //tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                //tempSchool.SIMD = (selectedyear.year.Equals("2016") || selectedyear.year.Equals("2017")) ? tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault() : tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals("2017")).FirstOrDefault();
                tempSchool.listFSM = GetHistoricalFSMData(rpGeneric2nd, school.seedcode, listYear, sSchoolType);
                tempSchool.FSM = tempSchool.listFSM.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listStudentNeed = GetHistoricalStudentNeed(rpGeneric2nd, sSchoolType, school.seedcode,listYear);
                tempSchool.StudentNeed = tempSchool.listStudentNeed.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listAttendance = GetHistoricalAttendanceData(rpGeneric2nd, sSchoolType,school, listYear);
                tempSchool.SPAttendance = tempSchool.listAttendance.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listExclusion = GetHistoricalExclusionData(rpGeneric2nd, sSchoolType, school, listYear);
                tempSchool.SPExclusion = tempSchool.listExclusion.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listMiDYIS = GetHistoricalMiDYISData(school.seedcode);
                tempSchool.SPMiDYIS = tempSchool.listMiDYIS.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listSPCfElevel = GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, school.seedcode, sSchoolType);
                tempSchool.SPCfElevel = tempSchool.listSPCfElevel.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listSOSCA = GetHistoricalSOSCAData(school.seedcode);
                tempSchool.SOSCA = tempSchool.listSOSCA.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listdestinations = GetSummaryData(rpGeneric2nd, school.seedcode);
                listSchoolData.Add(tempSchool);
            }
            //save in session for using in doGetReport
            Session["listSchoolData"] = listSchoolData;
            return listSchoolData;
        }

        private ACCDataStore.Entity.SchoolProfiles.Census.Entity.ChartData GetChartData(List<BaseSPDataModel> listSchool, Year eYearSelected)
        {
            List<BaseSPDataModel> temp = new List<BaseSPDataModel>();
            SecondaryChartData ChartData = new SecondaryChartData();
 

  
                ChartData.ChartNationalityIdentity = GetChartNationalityIdentity(listSchool, eYearSelected);
                ChartData.ChartLevelOfEnglish = GetChartLevelofEnglish(listSchool, eYearSelected);
                ChartData.ChartLevelOfEnglishByCatagories = GetChartLevelofEnglishbyCatagories(listSchool, eYearSelected);
                //ChartData.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected);
                if (Convert.ToInt16(eYearSelected.year) < 2016)
                {
                    ChartData.ChartSIMD = null;
                }
                else
                {
                    ChartData.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected);
                }
                ChartData.CartSchoolRollForecast = GetChartSchoolRollForecast(listSchool);
                ChartData.ChartIEP = GetChartStudentNeedIEP(listSchool);
                ChartData.ChartCSP = GetChartStudentNeedCSP(listSchool);
                ChartData.ChartLookedAfter = GetChartLookedAfter(listSchool);
                ChartData.ChartAttendance = GetChartAttendance(listSchool, "Attendance");
                ChartData.ChartAuthorisedAbsence = GetChartAttendance(listSchool, "Authorised Absence");
                ChartData.ChartUnauthorisedAbsence = GetChartAttendance(listSchool, "Unauthorised Absence");
                ChartData.ChartTotalAbsence = GetChartAttendance(listSchool, "Total Absence");
                ChartData.ChartNumberofDaysLostExclusion = GetChartExclusion(listSchool, "Number of Days Lost Per 1000 Pupils Through Exclusions");
                ChartData.ChartNumberofExclusionRFR = GetChartExclusion(listSchool, "Number of Removals from the Register");
                ChartData.ChartNumberofExclusionTemporary = GetChartExclusion(listSchool, "Number of Temporary Exclusions");
                
                ChartData.ChartMiDYIS = GetChartMiDYIS(listSchool);
                ChartData.ChartCfe3Level = listSchool[0].SPCfElevel == null? null:GetChartCfe3LevelData(listSchool, eYearSelected);
                ChartData.ChartCfe4Level = listSchool[0].SPCfElevel ==null? null:GetChartCfe4LevelData(listSchool, eYearSelected);
                ChartData.ChartFSM = GetChartFSM(listSchool);
                ChartData.ChartCfe3LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfe3LevelbyQuaintileData(listSchool, eYearSelected);
                ChartData.ChartCfe4LevelbyQuintile = listSchool[0].SPCfElevel == null ? null : GetChartCfe4LevelbyQuaintileData(listSchool, eYearSelected);
                ChartData.ChartSOSCAMaths = GetChartSOSCA(listSchool, "Mathematics");
                ChartData.ChartSOSCAReading = GetChartSOSCA(listSchool, "Reading");
                ChartData.ChartSOSCAScience = GetChartSOSCA(listSchool, "Sciences");
                ChartData.ChartTimelineDestinations = GetChartTimelieDestinations(listSchool);
 
               return ChartData;
        }

        //Get SchoolRoll data
        private SchoolRoll GetSchoolRollData(School school, Year year)
        {

            SchoolRoll SchoolRoll = new SchoolRoll();

            if (school.seedcode.Equals("1002"))
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where schooltype=3 and year = " + year.year);

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
                        }
                    }
                }

            }
            else
            {

                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where schooltype=3 and year = " + year.year + " and seedcode =" + school.seedcode);
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

        private string GetSchoolCostperPupil(School school)
        {
            string costperpupil = "";

            if (school.seedcode.Equals("1002"))
            {
                costperpupil = NumberFormatHelper.FormatNumber(6195.6, 1).ToString();
            }
            else
            {

                costperpupil = NumberFormatHelper.FormatNumber(school.costperpupil, 1).ToString();

            }

            return costperpupil;
        }

        // FSM Chart
        private SplineCharts GetChartFSM (List<BaseSPDataModel> listSchool) // query from database and return charts object
        {

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "Free School Meal";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            string[] colors = { "#ED561B", "#DDDF00", "#24CBE5", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexcolour = 0;

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].listFSM.Select(x => x.year.academicyear).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of S1-S6 Roll Registered for FSM" };

                foreach (var eSchool in listSchool)
                {
                    var listSeriesStart = eSchool.listFSM.Select(x => x.GenericSchoolData.sPercent.Equals("n/a")? null: (float?)float.Parse(x.GenericSchoolData.sPercent)).ToList();

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

            //eSplineCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        //Historical MiDYIS data
        private List<SPMiDYIS> GetHistoricalMiDYISData(string seedcode)
        {
            List<SPMiDYIS> listMiDYIS = new List<SPMiDYIS>();
            SPMiDYIS tMiDYIS = new SPMiDYIS();

            if (seedcode.Equals("1002"))
            {

                //get actual number 
                string query = "Select * from view_midyis where seedcode = 1002";
                var listResult = rpGeneric2nd.FindByNativeSQL(query);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tMiDYIS = new SPMiDYIS();
                            tMiDYIS.year = new Year(itemRow[0].ToString());
                            tMiDYIS.GenericSchoolData = new GenericSchoolData("Average Score at the Start of S1", NumberFormatHelper.ConvertObjectToFloat(itemRow[2]));
                            listMiDYIS.Add(tMiDYIS);
                        }
                    }
                }

            }
            else
            {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select * from view_midyis where seedcode = " + seedcode);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tMiDYIS = new SPMiDYIS();
                            tMiDYIS.year = new Year(itemRow[0].ToString());
                            tMiDYIS = new SPMiDYIS();
                            tMiDYIS.year = new Year(itemRow[0].ToString());
                            tMiDYIS.GenericSchoolData = new GenericSchoolData("Average Score at the Start of S1", NumberFormatHelper.ConvertObjectToFloat(itemRow[2]));
                            listMiDYIS.Add(tMiDYIS);
                        }
                    }
                }

            }

            return listMiDYIS.OrderBy(x => x.year.year).ToList(); ;
        }

        //Historical MiDYIS data
        private List<SPPIPS> GetHistoricalSOSCAData(string seedcode)
        {
            List<SPPIPS> listSOSCA = new List<SPPIPS>();
            List<GenericSchoolData> tempListSchoolData = new List<GenericSchoolData>();
            SPPIPS temp = new SPPIPS();

            if (seedcode.Equals("1002"))
            {

                //get actual number 
                string query = "Select * from summary_sosca where seedcode = 1002";
                var listResult = rpGeneric2nd.FindByNativeSQL(query);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            temp = new SPPIPS();
                            temp.year = new Year(itemRow[0].ToString());
                            tempListSchoolData = new List<GenericSchoolData>();
                            tempListSchoolData.Add(new GenericSchoolData("Mathematics", NumberFormatHelper.ConvertObjectToFloat(itemRow[2])));
                            tempListSchoolData.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                            tempListSchoolData.Add(new GenericSchoolData("Sciences", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                            temp.ListGenericSchoolData = tempListSchoolData;
                            listSOSCA.Add(temp);
                        }
                    }
                }

            }
            else
            {
                //get actual number 
                var listResult = rpGeneric2nd.FindByNativeSQL("Select * from summary_sosca where seedcode = " + seedcode);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            temp = new SPPIPS();
                            temp.year = new Year(itemRow[0].ToString());
                            tempListSchoolData = new List<GenericSchoolData>();
                            tempListSchoolData.Add(new GenericSchoolData("Mathematics", NumberFormatHelper.ConvertObjectToFloat(itemRow[2])));
                            tempListSchoolData.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                            tempListSchoolData.Add(new GenericSchoolData("Sciences", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                            temp.ListGenericSchoolData = tempListSchoolData;
                            listSOSCA.Add(temp);
                        }
                    }
                }

            }

            return listSOSCA.OrderBy(x => x.year.year).ToList(); ;
        }

        // MiDYIS Chart
        private SplineCharts GetChartMiDYIS(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "MidYIS";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            string[] colors = { "#ED561B", "#DDDF00", "#24CBE5", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexcolour = 0;

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].listMiDYIS.Select(x => x.year.academicyear).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Average Score at the Start of S1" };
 
                foreach (var eSchool in listSchool)
                {
                    var listSeriesStart = eSchool.listMiDYIS.Select(x => (float?)x.GenericSchoolData.Percent).ToList();

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

        // SOSCA Chart
        private SplineCharts GetChartSOSCA(List<BaseSPDataModel> listSchool, string ssubject) // query from database and return charts object
        {

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "AfE (SOSCA) - " + ssubject;
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            string[] arraySubject = { "Mathematics", "Reading", "Sciences" };
            int indexsubject = Array.FindIndex(arraySubject, item => item.Equals(ssubject));
            string[] colors = { "#ED561B", "#DDDF00", "#24CBE5", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexcolour = 0;

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].listSOSCA.Select(x => x.year.academicyear).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Overall " + ssubject + " Value added"};

                foreach (var eSchool in listSchool)
                {
                    var listSeriesStart = eSchool.listSOSCA.Select(x => (float?)x.ListGenericSchoolData[indexsubject].Percent).ToList();

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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.ListThirdlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel.ListThirdlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.ListForthlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.SPCfElevel.ListForthlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
        protected ColumnCharts GetChartCfe3LevelbyQuaintileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.ListThirdlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    if (!eSchool.SeedCode.Equals("1002"))
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel.SIMDQ1_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.SIMDQ2_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.SIMDQ3_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.SIMDQ4_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.SIMDQ5_3Dlevel.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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
        protected ColumnCharts GetChartCfe4LevelbyQuaintileData(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
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
                eColumnCharts.xAxis.categories = listSchool[0].SPCfElevel.ListForthlevel.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchool)
                {
                    indexColor = 0;

                    if (!eSchool.SeedCode.Equals("1002"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 1 - Most Deprived",
                            data = eSchool.SPCfElevel.SIMDQ1_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });
                        indexColor++;

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 2",
                            data = eSchool.SPCfElevel.SIMDQ2_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 3",
                            data = eSchool.SPCfElevel.SIMDQ3_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 4",
                            data = eSchool.SPCfElevel.SIMDQ4_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                            color = colors[indexColor]
                        });

                        indexColor++;
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            type = gtype,
                            name = "SIMD Quintile 5 - Least Deprived",
                            data = eSchool.SPCfElevel.SIMDQ5_4level.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
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


        //GetPrimaryCfeLevelDataforReport
        protected List<SPCfEReport> GetSecondaryCfeLevelDataforReport(IGenericRepository2nd rpGeneric2nd, string seedcode, string schooltype)
        {
            List<SPCfEReport> listSPCfelevel = new List<SPCfEReport>();
            SPCfEReport tSPCfelevel = new SPCfEReport();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            SPReport tempSPreport = new SPReport();

            //get actual number 
            string query = "Select * from summary_secondary_cfe_report where seedcode =" + seedcode;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tSPCfelevel = new SPCfEReport();
                        tSPCfelevel.S34 = new List<SPReport>();
                        tSPCfelevel.S4 = new List<SPReport>();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ER";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S34.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "EW";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S34.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ELT";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S34.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "N";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S34.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ER";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "EW";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[33])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[34])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[35])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[36])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[37])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[38])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ELT";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[39])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[40])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[41])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[42])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[43])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[44])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "N";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[45])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[46])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[47])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[48])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[49])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[50])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.S4.Add(tempSPreport);

                        listSPCfelevel.Add(tSPCfelevel);
       
                    }
                }
            }

            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
        }

        //GetPrimaryCfeLevelDataforReport
        protected List<SPMiDYISReport> GetSecondarySOSCADataforReport(IGenericRepository2nd rpGeneric2nd, string seedcode)
        {
            List<SPMiDYISReport> listSPReport = new List<SPMiDYISReport>();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            SPMiDYISReport tempSPMiDYISReport = new SPMiDYISReport();
            SPReport tempSReport = new SPReport();
            //get actual number 
            string query = "Select * from summary_sosca_report where seedcode in (1002," + seedcode+")";
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tempSPMiDYISReport = new SPMiDYISReport();
                        tempSPMiDYISReport.listdata = new List<SPReport>();
                        tempSPMiDYISReport.year = new Year(itemRow[0].ToString());
                        tempSPMiDYISReport.school = itemRow[1].ToString();

                        tempSReport = new SPReport();
                        tempSReport.code = "Maths";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[2])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        tempSReport.listdata = temp;
                        tempSPMiDYISReport.listdata.Add(tempSReport);

                        tempSReport = new SPReport();
                        tempSReport.code = "Reading";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        tempSReport.listdata = temp;
                        tempSPMiDYISReport.listdata.Add(tempSReport);

                        tempSReport = new SPReport();
                        tempSReport.code = "Sciences";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        tempSReport.listdata = temp;
                        tempSPMiDYISReport.listdata.Add(tempSReport);
                        listSPReport.Add(tempSPMiDYISReport);

                    }
                }
            }

            //comparing data between school and city 
            List<string> years = listSPReport.Select(i => i.year.year).Distinct().ToList();
            SPMiDYISReport tempCitySPMiDYISReport = new SPMiDYISReport();
            GenericSchoolData tempCityData = new GenericSchoolData();
            List<SPReport> templistSPReport = new List<SPReport>() ;

            for (int i = 0; i < years.Count; i++) {
                //get school data
                tempSPMiDYISReport = listSPReport.Where(x => x.year.year.Equals(years[i]) && !x.school.Equals("1002")).FirstOrDefault();
                //get City data
                tempCitySPMiDYISReport = listSPReport.Where(x => x.year.year.Equals(years[i]) && x.school.Equals("1002")).FirstOrDefault();

                foreach (SPReport temp1 in tempSPMiDYISReport.listdata)
                {

                    foreach (GenericSchoolData temp2 in temp1.listdata)
                    {
                        tempCityData = tempCitySPMiDYISReport.listdata.Where(x => x.code.Equals(temp1.code)).FirstOrDefault().listdata.Where(y => y.Code.Equals(temp2.Code)).FirstOrDefault();

                        if (temp2.Percent < tempCityData.Percent)
                        {
                            temp2.color = "ff0000"; //red
                        }
                    }
                }
            }

           //return only school data not city
           return listSPReport.Where(x => !x.school.Equals("1002")).OrderBy(x => x.year.year).ToList(); 
        }

        [HttpGet]
        [Route("SchoolProfiles/SecondarySchoolProfile/GetReport")]
        public JsonResult GetReport(List<string> listSeedCode, string sYear)
        {
            List<BaseSPDataModel> listSchoolData = Session["listSchoolData"] as List<BaseSPDataModel>;
            List<SPCfEReport> CFElevle = GetSecondaryCfeLevelDataforReport(rpGeneric2nd, listSchoolData[0].SeedCode, "3");
            List<SPMiDYISReport> SOSCA = GetSecondarySOSCADataforReport(rpGeneric2nd, listSchoolData[0].SeedCode);
            List<SPCfElevel> listSPCfElevel = new List<SPCfElevel>();
            listSPCfElevel = GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, "1002", "3");
            listSPCfElevel.AddRange(GetHistoricalSecondaryCfeLevelData(rpGeneric2nd, "9999", "3"));
            try
            {

                //get path to template and instance output
                //string docTemplatePath = @"C:\Users\HSaevanee\Source\Repos\accdatastore\ACCDataStore.Web\download\templateSD.docx";
                string docTemplatePath = System.Web.HttpContext.Current.Server.MapPath(@"~\download\templateSD.docx"); 
                //string docOutputPath = @"C:\Users\HSaevanee\Source\Repos\accdatastore\ACCDataStore.Web\download\SDReport.docx";
                string docOutputPath = System.Web.HttpContext.Current.Server.MapPath(@"~\download\SDReport.docx"); 

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
                    ChangeTextInTBSchoolRoll(doc, listSchoolData[0].listStudentStage,3,0);
                    //AcceptRevisions(tempfilename, "Hataichanok Saevanee");
                    ChangeTextInTBSIMD(doc, listSchoolData[0].SIMD,1);
                    //add listlevelofEnglish
                    ChangeTextInTBEAL(doc, listSchoolData[0].listLevelOfEnglish,2);
                    //add additional support needs
                    ChangeTextInTBASN(doc, listSchoolData[0].listStudentNeed,3);
                    //Freeschoolmeal
                    ChangeTextInTBFSM(doc, listSchoolData[0].listFSM,4);
                    //Attendance
                    ChangeTextInTBAAETrend(doc, listSchoolData[0].listAttendance,5);
                    //Exclusions
                    ChangeTextInTBExclusionTrend(doc, listSchoolData[0].listExclusion,6);   
                    //get CFE data for 2015
                    SPCfEReport tempCFElevle = CFElevle.Where(x => x.year.year.Equals("2015")).FirstOrDefault();
                    //S34 CfE Level 2015/16
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.S34, 7);
                    //S34 CfE Level National 2015
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 8, "2015", "S34");
                    //S4 CfE Level 2015/16
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.S4, 11);
                    //S34 CfE Level National 2015
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 12, "2015", "S4");

                    tempCFElevle = CFElevle.Where(x => x.year.year.Equals("2016")).FirstOrDefault();
                    //S34 CfE Level 2016/17
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.S34, 9);
                    //S34 CfE Level National 2016
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 10, "2016", "S34");
                    //S4 CfE Level 2016/17
                    ChangeTextInTBCfeLevel(doc, tempCFElevle.S4, 13);
                    //S34 CfE Level National 2016
                    ChangeTextInTBCfENational(doc, listSPCfElevel, 14, "2016", "S4");

                    //add MiDYIS
                    ChangeTextInTBMiDYIS(doc, listSchoolData[0].listMiDYIS, listSchoolData[1].listMiDYIS, 15);
                    //SOSCA 5 year trends  
                    ChangeTextInTBSOSCATrend(doc, listSchoolData[0].listSOSCA,listSchoolData[1].listSOSCA, 16);
                    //SOSCA by FSM LAC SIMD 
                    SPMiDYISReport tempSOSCA = SOSCA.Where(x => x.year.year.Equals("2016")).FirstOrDefault();
                    ChangeTextInTBSOSCA(doc, tempSOSCA.listdata, 17);
                    doc.MainDocumentPart.Document.Save();
                }

                return Json(new
                {
                    DownloadUrl = "download/" + "SDReport.docx",
                    FileName = "SDReport.docx"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        // Change text in SOSCA 5 year trend table   
        protected void ChangeTextInTBSOSCATrend(WordprocessingDocument doc, List<SPPIPS> schooldata, List<SPPIPS> citydata, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                schooldata.RemoveAll(s => s.year.academicyear.Equals("2011/12"));
                string[] codes = new string[] { "Mathematics", "Reading", "Sciences"}; // using i to reference
                string[] years = new string[] { "2012/13", "2013/14", "2014/15", "2015/16", "2016/17" }; // using j to reference
                List<GenericSchoolData> tempdata,tempcitydata;
                string sdata, color = "";

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
                        tempdata = schooldata.Where(x => x.year.academicyear.Equals(years[j - 1])).Select(x => x.ListGenericSchoolData).First();
                        tempcitydata = citydata.Where(x => x.year.academicyear.Equals(years[j - 1])).Select(x => x.ListGenericSchoolData).First();

                        //the get data by code
                        sdata = tempdata.Where(x => x.Code.Equals(codes[i - 2])).Select(x => x.sPercent).First();

                        color = tempdata.Where(x => x.Code.Equals(codes[i - 2])).Select(x => x.Percent).First() < tempcitydata.Where(x => x.Code.Equals(codes[i - 2])).Select(x => x.Percent).First() ? "ff0000" : "#000000";

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();
                        r.RunProperties.Color = new Color() { Val = color }; 

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;

                    }
                }
                //}

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBSOSCATrend: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }
        }

        // Change text in SOSCA by LAC FSM and SIMD table   
        protected void ChangeTextInTBSOSCA(WordprocessingDocument doc, List<SPReport> data, int tableid)
        {
            try
            {
                string[] datasets = new string[] { "Maths", "Reading", "Sciences"}; // using i to reference
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
                        string color = tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.color).First();
                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        r.RunProperties.Color = new Color() { Val = color }; //red
                        //r.RunProperties.Bold = new Bold();
                        //r.RunProperties.Italic = new Italic();
                        //r.RunProperties.Underline = new Underline() { Val = UnderlineValues.Single };

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBSOSCA : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
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
                    else
                    {
                        tempSPCfElevel = data.Where(x => x.seedcode.Equals("9999") && x.year.year.Equals(selectedyear)).FirstOrDefault();
                    }

                    if (state.Equals("S34"))
                    {

                        tempdata = tempSPCfElevel == null ? null : tempSPCfElevel.ListThirdlevel;

                    }
                    else
                    {

                        tempdata = tempSPCfElevel == null ? null : tempSPCfElevel.ListForthlevel;

                    }
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        string sdata = tempdata == null ? "n/a" : tempdata.Where(x => x.Code.Equals(datasets[j - 1])).Select(x => x.sPercent).First();

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
        
        // Change text MIDYIS  
        protected void ChangeTextInTBMiDYIS(WordprocessingDocument doc, List<SPMiDYIS> schooldata, List<SPMiDYIS> citydata, int tableid)
        {
            try
            {
                string[] years = new string[] { "2012/13", "2013/14", "2014/15", "2015/16", "2016/17" }; // using j to reference
                List<SPMiDYIS> tempSPMiDYIS;

                //using (WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true))
                //{
                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                for (int i = 2; i < table.Elements<TableRow>().Count(); i++)
                {
                    if (i == 2)
                    {
                        tempSPMiDYIS = schooldata;

                    }
                    else {

                        tempSPMiDYIS = citydata;
                    }

                    // travel through each row from row 1.
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                       //the get data by code
                        string sdata = tempSPMiDYIS == null ? "n/a" : tempSPMiDYIS.Where(x => x.year.academicyear.Equals(years[j - 1])).Select(x => x.GenericSchoolData.sPercent).First();

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
                //}

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBMiDYIS: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }
        }

        protected IList<DataSetDate> GetListDataSetDate()
        {
            List<DataSetDate> temp = new List<DataSetDate>();
            temp.Add(new DataSetDate("October 2016", "102016", "10", "2016"));
            temp.Add(new DataSetDate("November 2016", "112016", "11", "2016"));
            temp.Add(new DataSetDate("December 2016", "122016", "12", "2016"));
            temp.Add(new DataSetDate("January 2017", "12017", "1", "2017"));
            temp.Add(new DataSetDate("February 2017", "22017", "2", "2017"));
            temp.Add(new DataSetDate("November 2017", "112017", "11", "2017"));
            temp.Add(new DataSetDate("December 2017", "122017", "12", "2017"));
            temp.Add(new DataSetDate("January 2018", "12018", "1", "2018"));
            temp.Add(new DataSetDate("February 2018", "22018", "2", "2018"));
            temp.Add(new DataSetDate("March 2018", "32018", "3", "2018"));
            temp.Add(new DataSetDate("April 2018", "42018", "4", "2018"));
            return temp;

        }

        protected List<SummaryDHdata> GetSummaryData(IGenericRepository2nd rpGeneric2nd, string seedcode)
        {

            List<SummaryDHdata> listResult = new List<SummaryDHdata>();
            List<DataSetDate> dataset = GetListDataSetDate().ToList();
            var listtemp = rpGeneric2nd.FindByNativeSQL("select * from datahub_summary where Year in (2017,2018) and TRIM(SeedCode) like '" + seedcode + "'");

            foreach (var itemrow in listtemp)
            {
                SummaryDHdata temp = new SummaryDHdata();
                if (itemrow != null)
                {
                    temp.year = Convert.ToInt16(itemrow[0].ToString());
                    temp.month = Convert.ToInt16(itemrow[1].ToString());
                    temp.seedcode = itemrow[2].ToString();
                    temp.listdata = new List<GenericData>();
                    temp.listdata.Add(new GenericData("Participating", NumberFormatHelper.ConvertObjectToFloat(itemrow[4])));
                    temp.listdata.Add(new GenericData("Not Participating", NumberFormatHelper.ConvertObjectToFloat(itemrow[5])));
                    temp.listdata.Add(new GenericData("Unknown", NumberFormatHelper.ConvertObjectToFloat(itemrow[6])));
                    temp.sdataset = dataset.Where(x => x.month.Equals(temp.month.ToString()) && x.year.Equals(temp.year.ToString())).FirstOrDefault();
                    listResult.Add(temp);
                }
            }

            return listResult.OrderBy(x => x.year).ThenBy(x => x.month).ToList();
        }

        // Overall Destinations Chart
        private SplineCharts GetChartTimelieDestinations(List<BaseSPDataModel> listSchoolData) // query from database and return charts object
        {

            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "Participation Timeline";
            eSplineCharts.yAxis.title.text = "% of pupils";

            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();


            if (listSchoolData != null && listSchoolData.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchoolData[0].listdestinations.Select(x => x.sdataset.name).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of pupils" };

                foreach (var eSchool in listSchoolData)
                {
                    var listSeriesStart = eSchool.listdestinations.Select(x => (float?)x.listdata[0].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Participating",
                        color = colors[indexColor],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexColor++;

                    listSeriesStart = eSchool.listdestinations.Select(x => (float?)x.listdata[1].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Not Participating",
                        color = colors[indexColor],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexColor++;

                    listSeriesStart = eSchool.listdestinations.Select(x => (float?)x.listdata[2].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Unknown",
                        color = colors[indexColor],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexColor++;

                }
            }

            eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eSplineCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

    }
}