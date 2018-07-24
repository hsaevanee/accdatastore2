using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class SpecialSchoolProfileController : BaseSchoolProfilesController
    {
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfilesController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public SpecialSchoolProfileController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }


        [SchoolAuthentication]
        [Transactional]
        // GET: SchoolProfiles/SpecialSchool
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/SpecialSchoolProfile/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {
                object oResult = null;

                var listSchool = GetListSchool(rpGeneric2nd, "4");
                var listYear = GetListYear();
                var eYearSelected = listYear != null ? listYear.Where(x => x.year.Equals("2017")).First() : null;
                List<School> ListSchoolSelected = listSchool != null ? listSchool.Where(x => x.seedcode.Equals("5245044")).ToList() : null;
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
        [Route("SchoolProfiles/SpecialSchoolProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] string sYear) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                string sSchoolType = "4";
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

        private List<BaseSPDataModel> GetSchoolData(List<School> tListSchoolSelected, string sYear, string sSchoolType)
        {
            var listYear = GetListYear();
            var listSchoolData = new List<BaseSPDataModel>();
            BaseSPDataModel tempSchool = new BaseSPDataModel();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new School("1002", "Aberdeen Special Schools"));

            Year selectedyear = new Year(sYear);

            foreach (School school in tListSchoolSelected)
            {
                tempSchool = new BaseSPDataModel();
                tempSchool.SeedCode = school.seedcode;
                tempSchool.SchoolName = school.name;
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
                //tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);

                if (Convert.ToInt16(selectedyear.year) < 2016)
                {
                    tempSchool.listSIMD = null;
                    tempSchool.SIMD = null;
                }
                else
                {
                    if (school.seedcode.Equals("1004140"))
                    {
                        if (Convert.ToInt16(selectedyear.year) == 2016)
                        {
                            tempSchool.listSIMD = null;
                            tempSchool.SIMD = null;
                        }
                        else {
                            tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                            tempSchool.SIMD = tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();

                        }

                    }
                    else
                    {

                        tempSchool.listSIMD = GetHistoricalSIMDData(rpGeneric2nd, sSchoolType, school.seedcode, listYear);
                        tempSchool.SIMD = tempSchool.listSIMD.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                    }
                }

                tempSchool.listFSM = GetHistoricalFSMData(rpGeneric2nd, school.seedcode, listYear, sSchoolType);
                tempSchool.FSM = tempSchool.listFSM.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listStudentNeed = GetHistoricalStudentNeed(rpGeneric2nd, sSchoolType, school.seedcode,listYear);
                tempSchool.StudentNeed = tempSchool.listStudentNeed.Where(x => x.year.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listAttendance = GetHistoricalAttendanceData(rpGeneric2nd, sSchoolType, school, listYear);
                tempSchool.SPAttendance = tempSchool.listAttendance.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();
                tempSchool.listExclusion = GetHistoricalExclusionData(rpGeneric2nd, sSchoolType, school, listYear);
                tempSchool.SPExclusion = tempSchool.listExclusion.Where(x => x.YearInfo.year.Equals(selectedyear.year)).FirstOrDefault();

                listSchoolData.Add(tempSchool);
            }
            return listSchoolData;
        }

        private ACCDataStore.Entity.SchoolProfiles.Census.Entity.ChartData GetChartData(List<BaseSPDataModel> listSchool, Year eYearSelected)
        {

            try
            {
                Entity.SchoolProfiles.Census.Entity.ChartData chartdata = new Entity.SchoolProfiles.Census.Entity.ChartData();
                chartdata.ChartNationalityIdentity = GetChartNationalityIdentity(listSchool, eYearSelected);
                chartdata.ChartLevelOfEnglish = GetChartLevelofEnglish(listSchool, eYearSelected);
                chartdata.ChartLevelOfEnglishByCatagories = GetChartLevelofEnglishbyCatagories(listSchool, eYearSelected);

                if (Convert.ToInt16(eYearSelected.year) < 2016)
                {
                    chartdata.ChartSIMD = null;
                }
                else
                {
                    if (listSchool.FirstOrDefault(i => i.SeedCode.Equals("1004140")) !=null) //school.seedcode.Equals("1004140")
                    {
                        if (Convert.ToInt16(eYearSelected.year) == 2016)
                        {
                            chartdata.ChartSIMD = null;
                        }
                        else
                        {
                            chartdata.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected);
                        }

                    }
                    else
                    {

                        chartdata.ChartSIMD = GetChartSIMDDecile(listSchool, eYearSelected);
                    }
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
                chartdata.ChartFSM = GetChartFSM(listSchool);
                return chartdata;

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
                return null;
            }

        }

        //Get SchoolRoll data
        private SchoolRoll GetSchoolRollData(School school, Year year)
        {

            SchoolRoll SchoolRoll = new SchoolRoll();

            if (school.seedcode.Equals("1002"))
            {

                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where schooltype=4 and year = " + year.year);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = school.schoolCapacity;
                            SchoolRoll.schoolroll = itemRow[1] == null ? 0 : Convert.ToInt16(itemRow[1].ToString());
                            SchoolRoll.percent = 0.00F;
                        }
                    }
                }

            }
            else
            {
                var listResult = rpGeneric2nd.FindByNativeSQL("Select 000 as total, sum(Count) from summary_schoolroll where schooltype=4 and year = " + year.year + " and seedcode =" + school.seedcode);
                if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SchoolRoll = new SchoolRoll();
                            SchoolRoll.year = year;
                            SchoolRoll.capacity = 0;
                            SchoolRoll.schoolroll = itemRow[1]==null ? 0 : Convert.ToInt16(itemRow[1].ToString());
                            SchoolRoll.percent = 0.00F;
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

            //get actual number 
            var listResult = rpGeneric2nd.FindByNativeSQL("Select * from summary_schoolroll where seedcode = " + school.seedcode);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tempdataActualnumber.Add(new GenericSchoolData(new Year(itemRow[0].ToString()).academicyear, NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                    }
                }
            }

            SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;

            return SchoolRollForecast;
        }

        // SchoolRoll Forecast Chart
        private new SplineCharts GetChartSchoolRollForecast(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {

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
                        var listSeriesActual = eSchool.SchoolRollForecast.ListActualSchoolRoll.Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = eSchool.SchoolName,
                            color = "#24CBE5",
                            lineWidth = 2,
                            data = listSeriesActual,
                            visible = true
                        });

                    }


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
                eSplineCharts.xAxis.categories = listSchool[0].listFSM.Select(x => x.year.academicyear).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of SP Roll Registered for FSM" };

                foreach (var eSchool in listSchool)
                {
                    var listSeriesStart = eSchool.listFSM.Select(x => x.GenericSchoolData.sPercent.Equals("n/a") ? null : (float?)float.Parse(x.GenericSchoolData.sPercent)).ToList();

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

    }
}