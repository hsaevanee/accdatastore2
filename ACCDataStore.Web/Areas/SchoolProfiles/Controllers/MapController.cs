using ACCDataStore.Core.Helper;
using ACCDataStore.Entity.DatahubProfile.Entities;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class MapController : BaseSchoolProfilesController
    {

        private static ILog log = LogManager.GetLogger(typeof(MapController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public MapController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: SchoolProfiles/Map
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("SchoolProfiles/Map/GetCondition")]
        public JsonResult GetCondition()
        {
            try
            {


                var layers = new List<object>() { new { Code = "S01", Name = "Datazone Zones" } }.ToList();

                var datasets = new List<object>() { new { Code = "FSM", Name = "Free School Meal 2016/17" }}.ToList();
                var datacatagories = new[] { new { Code = "P4-P7", Name = "Primary P4-P7" }, new { Code = "S1-S6", Name = "Secondary S1-S6" }, new { Code = "SP", Name = "Special" } }.ToList();

                object oResult = null;

                List<SummaryDHdata> heatmapdata = GetdatafromDB("");

                oResult = new
                {
                    layers = layers,
                    selectedLayer = new { Code = "S01", Name = "Datazone Zones" },
                    datacatagories = datacatagories,
                    selecteddatacatagory = datacatagories.First(),
                    datasets = datasets,
                    heatmapdata = heatmapdata,
                    selectedDataset = datasets.First()
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }



        protected List<SummaryDHdata> GetdatafromDB(string datasetDate)
        {
            List<SummaryDHdata> listResult = new List<SummaryDHdata>();

            var listtemp = rpGeneric2nd.FindByNativeSQL("SELECT * FROM accdatastore.testdatadatazones");
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    SummaryDHdata temp = new SummaryDHdata();

                    temp.seedcode = itemrow[2] == null ? "" : itemrow[2].ToString();
                    temp.centrename = itemrow[1] == null ? "" : itemrow[1].ToString();
                    temp.listdata = new List<GenericData>();
                    temp.listdata.Add(new GenericData("P4-P7", Convert.ToInt16(itemrow[3].ToString())));
                    temp.listdata.Add(new GenericData("S1-S6", Convert.ToInt16(itemrow[4].ToString())));
                    temp.listdata.Add(new GenericData("SP", Convert.ToInt16(itemrow[5].ToString())));
                    listResult.Add(temp);
                }
            }


            return listResult;

        }

        [HttpGet]
        [Route("SchoolProfiles/Map/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] string layertype, string datacatagory, string seedcode, string dataset)
        {
            try
            {


                var layers = new List<object>() { new { Code = "S01", Name = "Datazone Zones" } }.ToList();

                var datasets = new List<object>() { new { Code = "FSM", Name = "Free School Meal 2016/17" } }.ToList();
                var datacatagories = new[] { new { Code = "P4-P7", Name = "Primary P4-P7" }, new { Code = "S1-S6", Name = "Secondary S1-S6" }, new { Code = "SP", Name = "Special" } }.ToList();

                object oResult = null;

                List<SummaryDHdata> heatmapdata = GetdatafromDB("");

                oResult = new
                {
                    layers = layers,
                    selectedLayer = new { Code = "S01", Name = "Datazone Zones" },
                    datacatagories = datacatagories,
                    selecteddatacatagory = datacatagories.First(),
                    datasets = datasets,
                    heatmapdata = heatmapdata,
                    selectedDataset = datasets.First(),
                    showeddata = heatmapdata.Where(x => x.seedcode.Equals(seedcode)).FirstOrDefault()
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

    }


}