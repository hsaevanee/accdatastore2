using ACCDataStore.Core.Helper;
using ACCDataStore.Entity.DatahubProfile.Entities;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.DatahubProfile.Controllers
{
    public class CityProfileController : DataHubController
    {
        private static ILog log = LogManager.GetLogger(typeof(CityProfileController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public CityProfileController(IGenericRepository2nd rpGeneric2nd)
            : base(rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: DatahubProfile/CityProfile
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("DatahubProfile/CityProfile/GetCondition")]
        public override JsonResult GetCondition()
        {
            try
            {
                //var listSchool = GetListSchoolname();
                //var listNeighbourhoodName = GetListNeighbourhoodsname(rpGeneric2nd);
                var listdatasetdate = GetListDataSetDate();
                var ListCentreTypes = new[] { new { Code = "3", Name = "Participation Cohort by ASG" }, new { Code = "32", Name = "Participation Cohort by Neighbourhood" } }.ToList();

                object oResult = null;

                oResult = new
                {
                    //ListSchool = listSchool.Select(x => x.GetJson()),
                    //ListNeighbourhood = listNeighbourhoodName.Select(x => x.GetJson()),
                    ListDataset = listdatasetdate.Select(x => x.GetJson()),
                    DatasetSelected = listdatasetdate.Where(x => x.code.Equals("42018")).Select(x => x.GetJson()).First(),
                    ListCentreSelected = ListCentreTypes.Where(x => x.Code.Equals("3")).First(),
                    ListCentreTypes = ListCentreTypes
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [HttpGet]
        [Route("DatahubProfile/CityProfile/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] string sYear, string centretype) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                List<SummaryDHdata> listSchoolData = new List<SummaryDHdata>();

                var listdatasetdate = GetListDataSetDate();
                var ListCentreTypes = new[] { new { Code = "3", Name = "Participation Cohort by ASG" }, new { Code = "32", Name = "Participation Cohort by Neighbourhood" } }.ToList();

                listSchoolData = GetSummaryData(sYear, centretype);


                //var listNeighData = GetDataHubData(ListNeighbourSelected);

                oResult = new
                {
                    ListDataset = listdatasetdate.Select(x => x.GetJson()),
                    DatasetSelected = listdatasetdate.Where(x => x.code.Equals(sYear)).Select(x => x.GetJson()).First(),
                    ListingData = listSchoolData,
                    ListCentreSelected = ListCentreTypes.Where(x => x.Code.Equals(centretype)).First(),
                    ListCentreTypes = ListCentreTypes,
                    ChartData = GetChartData(listSchoolData)
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }
        public List<SummaryDHdata> GetSummaryData(string dataset, string centretype)
        {
            IList<DatahubCentre> listCentres = null;
            
            List<SummaryDHdata> listResult = new List<SummaryDHdata>();
            List<DataSetDate> datasets = GetListDataSetDate().ToList();
            DataSetDate selecteddataset = datasets.Where(x => x.code.Equals(dataset)).FirstOrDefault();

            string query = "select * from datahub_summary where schooltype = " + centretype + " and Year = " + selecteddataset.year + " and month = " + selecteddataset.month;

            if (centretype.Equals("32"))
            {
                listCentres = GetListNeighbourhoodsname(rpGeneric2nd);
                listCentres.Add(new DatahubCentre("1002", "Aberdeen City", "1"));

                //add city data to neighbourhoods 
                query = query + " union select * from datahub_summary where seedcode = 1002 and schooltype = 3 and Year = " + selecteddataset.year + " and month = " + selecteddataset.month;
            }
            else {

                listCentres = GetListSchoolname();
                listCentres.Add(new DatahubCentre("1002", "Aberdeen City", "1"));             
            }
            var listtemp = rpGeneric2nd.FindByNativeSQL(query);

            foreach (var itemrow in listtemp)
            {
                SummaryDHdata temp = new SummaryDHdata();
                if (itemrow != null)
                {
                    temp.year = Convert.ToInt16(itemrow[0].ToString());
                    temp.month = Convert.ToInt16(itemrow[1].ToString());
                    temp.seedcode = itemrow[2].ToString();
                    temp.centrename = listCentres.Where(x => x.seedcode.Equals(temp.seedcode)).Select(x => x.name).First().ToString();
                    temp.listdata = new List<GenericData>();
                    temp.listdata.Add(new GenericData("Positive Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[4])));
                    temp.listdata.Add(new GenericData("Non-Positive Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[5])));
                    temp.listdata.Add(new GenericData("Unknown Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[6])));
                    temp.sdataset = datasets.Where(x => x.month.Equals(temp.month.ToString()) && x.year.Equals(temp.year.ToString())).FirstOrDefault();
                    listResult.Add(temp);
                }
            }

            return listResult.OrderBy(x => x.centrename).ToList();
        }

        [HttpGet]
        [Route("DatahubProfile/CityProfile/GetDataDetails")]
        public JsonResult GetDataDetails([System.Web.Http.FromUri] string seedcode, [System.Web.Http.FromUri] string centretype, [System.Web.Http.FromUri] string dataname, [System.Web.Http.FromUri] string sYear)
        {
            try
            {

                ACCDataStore.Entity.Users temp = Session["SessionUser"] as ACCDataStore.Entity.Users;
                object oResult = null;
                List<DataHubData> listSchoolData = new List<DataHubData>();

                var listSchool = GetListSchoolname();
                listSchool.Add(new DatahubCentre("1002", "Aberdeen City", "1"));
                var listdatasetdate = GetListDataSetDate();
                var listNeighbourhoodName = GetListNeighbourhoodsname(rpGeneric2nd);
                listNeighbourhoodName.Add(new DatahubCentre("1002", "Aberdeen City", "1"));
                DatahubCentre selectedcentre = null;

                var selecteddatacatagory = new { Code = "Positive Destination", Name = "Positive Destination" };

                var layers = new[] { new { Code = "S01", Name = "Neighbourhood Zones" }, new { Code = "S02", Name = "School Locations" } }.ToList();
                var datacatagories = new[] { new { Code = "Positive Destination", Name = "Positive Destination" }, new { Code = "Non-Positive Destination", Name = "Non-Positive Destination" }, new { Code = "Unknown Destination", Name = "Unknown Destination" } }.ToList();

                if (centretype.Equals("32"))
                {
                    List<DatahubCentre> ListNeighbourSelected = seedcode != null ? listNeighbourhoodName.Where(x => x.seedcode.Equals(seedcode)).ToList() : null;
                    selectedcentre = listNeighbourhoodName.Where(x => x.seedcode.Equals(seedcode)).FirstOrDefault();
                    selecteddatacatagory = datacatagories.Where(x => x.Code.Equals(dataname)).FirstOrDefault();

                    if (ListNeighbourSelected != null && ListNeighbourSelected.Count > 0)
                    {
                        listSchoolData = GetDataHubNeighbourhoodDatabyStatuses(ListNeighbourSelected, sYear);
                    }

                }
                else
                {

                    List<DatahubCentre> ListSchoolSelected = seedcode != null ? listSchool.Where(x => x.seedcode.Equals(seedcode)).ToList() : null;
                    selectedcentre = listSchool.Where(x => x.seedcode.Equals(seedcode)).FirstOrDefault();
                    selecteddatacatagory = datacatagories.Where(x => x.Code.Equals(dataname)).FirstOrDefault();

                    if (ListSchoolSelected != null && ListSchoolSelected.Count > 0)
                    {
                        listSchoolData = GetDataHubSchoolDatabyStatuses(ListSchoolSelected, sYear);
                    }
                }

                oResult = new
                {
                    selectedcentre = selectedcentre,
                    datasets = listdatasetdate.Select(x => x.GetJson()),
                    selectedDataset = listdatasetdate.Where(x => x.code.Equals(sYear)).Select(x => x.GetJson()).First(),
                    selecteddatacatagory = selecteddatacatagory,
                    listSchoolData = listSchoolData
                };



                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }

        }

        private ACCDataStore.Entity.DatahubProfile.Entities.ChartData GetChartData(List<SummaryDHdata> listSchoolData)
        {

            ChartData chartdata = new ChartData();

            chartdata.ChartNonPositiveCohortbyCentre = GetChartCohortbyCentre(listSchoolData, "Non-Positive Destination");
            chartdata.ChartPositiveCohortbyCentre = GetChartCohortbyCentre(listSchoolData, "Positive Destination");
            chartdata.ChartUnknownCohortbyCentre = GetChartCohortbyCentre(listSchoolData, "Unknown Destination");
            return chartdata;
        }

        // Pupils by Ages Chart
        protected ColumnCharts GetChartCohortbyCentre(List<SummaryDHdata> listSchoolData, string dataname) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            string[] datacatagories = { "Positive Destination", "Non-Positive Destination", "Unknown Destination" };

            int indexColor = 1;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Percentage " +dataname + " by Centres";
            eColumnCharts.yAxis.title.text = "Percentage";
            //eColumnCharts.yAxis.min = 0;
            //eColumnCharts.yAxis.max = 100;
            //eColumnCharts.yAxis.tickInterval = 20;
            int indexsubject = Array.FindIndex(datacatagories, item => item.Equals(dataname));

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchoolData != null && listSchoolData.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchoolData.Select(x => x.centrename).ToList();

 
                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    type = "column",
                    name = "Centres",
                    data = listSchoolData.Select(x => (float?)float.Parse(x.listdata[indexsubject].sPercent)).ToList(),
                    color = colors[indexColor]
                });

                indexColor++;
 
            }

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