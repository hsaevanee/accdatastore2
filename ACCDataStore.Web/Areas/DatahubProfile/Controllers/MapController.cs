using ACCDataStore.Core.Helper;
using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Entity.DatahubProfile.Entities;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.DatahubProfile.Controllers
{
    public class MapController : DataHubController
    {

        private static ILog log = LogManager.GetLogger(typeof(DataHubController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public MapController(IGenericRepository2nd rpGeneric2nd)
            : base(rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }


        // GET: DatahubProfile/Map
        public override ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("DatahubProfile/Map/GetCondition")]
        public override JsonResult GetCondition()
        {
            try
            {

                var listdatasetdate = GetListDataSetDate();

                var layers = new List<object>() { new { Code = "S01", Name = "Neighbourhood Zones" }, new { Code = "S02", Name = "School Locations" }}.ToList();
                var datacatagories = new[] { new { Code = "Participating Destination", Name = "Participating Destination" }, new { Code = "Non-Participating Destination", Name = "Non-Participating Destination" }, new { Code = "Unknown Destination", Name = "Unknown Destination" } }.ToList();

                object oResult = null;
                //Load datahub summary for creating heatmap  
                List<SummaryDHdata> heatmapdata = GetHeatMapdata("42018");

                oResult = new
                {
                    layers = layers,
                    selectedLayer =  new { Code = "S01", Name = "Neighbourhood Zones" },
                    datasets = listdatasetdate.Select(x => x.GetJson()),
                    selectedDataset = listdatasetdate.Where(x => x.code.Equals("42018")).Select(x => x.GetJson()).First(),
                    selecteddatacatagory = new { Code = "Participating Destination", Name = "Participating Destination" },
                    heatmapdata = heatmapdata,
                    datacatagories = datacatagories
                    //return data for hilight
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [HttpGet]
        [Route("DatahubProfile/Map/GetGeoJson")]
        public JsonResult GetGeoJson([System.Web.Http.FromUri] string layertype)
        {
            try
            {


                IList<string> oResult = GeoJson(layertype);

                

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [HttpGet]
        [Route("DatahubProfile/Map/LoadHeatMapdata")]
        public JsonResult LoadHeatMapdata([System.Web.Http.FromUri] string dataset)
        {
            try
            {
                object oResult = null;

                List<SummaryDHdata> heatmapdata = GetHeatMapdata(dataset);

                oResult = new
                {
                    heatmapdata = heatmapdata
                };



                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }


        [HttpGet]
        [Route("DatahubProfile/Map/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] string layertype, string datacatagory, string seedcode, string dataset)
        {
            try
            {
                object oResult = null;
                List<DataHubData> listSchoolData = new List<DataHubData>();

                var listSchool = GetListSchoolname();
                var listdatasetdate = GetListDataSetDate();
                var listNeighbourhoodName = GetListNeighbourhoodsname(rpGeneric2nd);
                DatahubCentre selectedcentre = null;
                var selecteddatacatagory = new { Code = "Participating Destination", Name = "Participating Destination" };

                var layers = new [] { new { Code = "S01", Name = "Neighbourhood Zones" }, new { Code = "S02", Name = "School Locations" } }.ToList();
                var datacatagories = new[] { new { Code = "Participating Destination", Name = "Participating Destination" }, new { Code = "Non-Participating Destination", Name = "Non-Participating Destination" }, new { Code = "Unknown Destination", Name = "Unknown Destination" } }.ToList();

                if (layertype.Equals("S01"))
                {
                    List<DatahubCentre> ListNeighbourSelected = seedcode != null ? listNeighbourhoodName.Where(x => x.seedcode.Equals(seedcode)).ToList() : null;
                    selectedcentre = listNeighbourhoodName.Where(x => x.seedcode.Equals(seedcode)).FirstOrDefault();
                    selecteddatacatagory = datacatagories.Where(x => x.Code.Equals(datacatagory)).FirstOrDefault();

                    if (ListNeighbourSelected != null && ListNeighbourSelected.Count > 0)
                    {
                        listSchoolData = GetDataHubNeighbourhoodDatabyStatuses(ListNeighbourSelected, dataset);
                    }

                }
                else
                {

                    List<DatahubCentre> ListSchoolSelected = seedcode != null ? listSchool.Where(x => x.seedcode.Equals(seedcode)).ToList() : null;
                    selectedcentre = listSchool.Where(x => x.seedcode.Equals(seedcode)).FirstOrDefault();
                    selecteddatacatagory = datacatagories.Where(x => x.Code.Equals(datacatagory)).FirstOrDefault();

                    if (ListSchoolSelected != null && ListSchoolSelected.Count > 0)
                    {
                        listSchoolData = GetDataHubSchoolDatabyStatuses(ListSchoolSelected, dataset);
                    }
                }

                //Load datahub summary for creating heatmap  
                List<SummaryDHdata> heatmapdata = GetHeatMapdata(dataset);

                oResult = new
                {
                    selectedcentre = selectedcentre,
                    layers = layers,
                    selectedLayer = layers.Where(x => x.Code.Contains(layertype)).FirstOrDefault(),
                    datasets = listdatasetdate.Select(x => x.GetJson()),
                    selectedDataset = listdatasetdate.Where(x => x.code.Equals(dataset)).Select(x => x.GetJson()).First(),
                    datacatagories = datacatagories,
                    selecteddatacatagory = selecteddatacatagory,
                    heatmapdata = heatmapdata,
                    ChartData = GetChartData(listSchoolData)
                };



                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        private ACCDataStore.Entity.DatahubProfile.Entities.ChartData GetChartData(List<DataHubData> listSchoolData)
        {

            ChartData chartdata = new ChartData();

            chartdata.ChartPupilsAges = GetChartPupilsAges(listSchoolData);
            chartdata.ChartPositiveDestinations = GetChartbyDestinations(listSchoolData, "Participating");
            chartdata.ChartNonPositiveDestinations = GetChartbyDestinations(listSchoolData, "Not Participating");
            chartdata.ChartOverallDestinations = GetChartOverallDestinations(listSchoolData);
            chartdata.ChartTimelineDestinations = GetChartTimelieDestinations(listSchoolData);


            return chartdata;
        }

        public List<SummaryDHdata> GetHeatMapdata(string dataset)
        {

            List<SummaryDHdata> listResult = new List<SummaryDHdata>();
            List<DataSetDate> datasets = GetListDataSetDate().ToList();
            DataSetDate selecteddataset = datasets.Where(x => x.code.Equals(dataset)).FirstOrDefault();

            var listtemp = rpGeneric2nd.FindByNativeSQL("select * from datahub_summary where schooltype = 32 and Year = " + selecteddataset.year + " and month = " + selecteddataset.month);

            foreach (var itemrow in listtemp)
            {
                SummaryDHdata temp = new SummaryDHdata();
                if (itemrow != null)
                {
                    temp.year = Convert.ToInt16(itemrow[0].ToString());
                    temp.month = Convert.ToInt16(itemrow[1].ToString());
                    temp.seedcode = itemrow[2].ToString();
                    temp.listdata = new List<GenericData>();
                    temp.listdata.Add(new GenericData("Participating Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[4])));
                    temp.listdata.Add(new GenericData("Non-Participating Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[5])));
                    temp.listdata.Add(new GenericData("Unknown Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[6])));
                    temp.sdataset = datasets.Where(x => x.month.Equals(temp.month.ToString()) && x.year.Equals(temp.year.ToString())).FirstOrDefault();
                    listResult.Add(temp);
                }
            }

            return listResult.OrderBy(x => x.year).ThenBy(x => x.month).ToList();
        }
        
        public IList<string> GeoJson(string layertype)
        {
            //var datazoneList = rpGeneric2nd.FindByNativeSQL("SELECT GeoJSON FROM accdatastore.map_data_data_zone where Reference_Council = S12000033");

            //IList<string> result = new Collection<string>();
            //foreach (var item in datazoneList)
            //{
            //    result.Add(item[0].ToString());
            //}

            //return result;
            var datazoneList = this.rpGeneric2nd.QueryOver<DataZoneObj>()
    .Where(r => r.Reference == "S01000074")
    .List<DataZoneObj>();

            IList<string> result = new Collection<string>();
            foreach (var item in datazoneList)
            {
                result.Add(item.GeoJSON);
            }

            return result;
        }
    }
}