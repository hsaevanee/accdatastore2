using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.SIMD;
using ClosedXML.Excel;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class SIMDController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(SIMDController));

        private readonly IGenericRepository rpGeneric;

        public SIMDController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
 
        // GET: SchoolProfile/SIMD
        public ActionResult Index(string sSchoolName)
        {
            //page counter
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.SIMDpgCounter++;
            ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            
            
            var vmSIMD = new SIMDViewModel();

            var schoolname = new List<string>();

            var setSIMDCriteria = new List<string>();
            var setYearCriteria = new List<string>();

            List<SIMDObj> ListSIMDData = new List<SIMDObj>();
            List<SIMDObj> temp = new List<SIMDObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t t2 on t1.SeedCode = t2.SeedCode ");

            List<string> fooList = listResult.OfType<string>().ToList();            

            vmSIMD.ListSchoolNameData = fooList;

            fooList = new List<string>();
                
            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");

            if (listResult != null)
            {   
                foreach (var itemRow in listResult)
                {
                    if (itemRow!=null)
                    fooList.Add(Convert.ToString(itemRow));
                }
            }

            vmSIMD.ListSIMDdefinition= fooList;
            vmSIMD.ListYear = new List<string>(new string[] { "2009", "2012"});

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmSIMD.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmSIMD.IsShowCriteria = false;
                    vmSIMD.ListSelectedYear = new List<string>(new string[] { "2009", "2012" });
                    Session["ListSelectedYears"] = vmSIMD.ListSelectedYear;
                    vmSIMD.ListSelectedDeciles = vmSIMD.ListSIMDdefinition;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                // get parameter from Request object
                vmSIMD.IsShowCriteria = true;
                sSchoolName = Request["selectedschoolname"];
                vmSIMD.selectedschoolname = sSchoolName;
                Session["sSchoolName"] = sSchoolName;

                if (Request["SIMD"] != null)
                {
                    setSIMDCriteria = Request["SIMD"].Split(',').ToList();
                    vmSIMD.ListSelectedDeciles = setSIMDCriteria;
                }
                else
                {
                    setSIMDCriteria = null;
                }

                if (Request["years"] != null)
                {
                    vmSIMD.ListSelectedYear = Request["years"].Split(',').ToList(); 
                }
                else
                {
                    vmSIMD.ListSelectedYear = new List<string>(new string[] { "2009", "2012" });
                }

               Session["ListSelectedYears"] = vmSIMD.ListSelectedYear;
                               
            }

            // process data
            if (sSchoolName == null || sSchoolName.Equals(""))
            {
                vmSIMD.IsShowData = false;
            }
            else if (sSchoolName != null)
            {
                vmSIMD.selectedschoolname = sSchoolName;
                ListSIMDData = GetSIMDDatabySchoolname(rpGeneric, sSchoolName, setYearCriteria);
                if (setSIMDCriteria == null) {
                    vmSIMD.ListSIMDData = null;
                    vmSIMD.IsShowData = false;
                }
                else if (setSIMDCriteria.Count != 0 && setSIMDCriteria != null)
                {
                    vmSIMD.ListSIMDData= ListSIMDData.Where(x => setSIMDCriteria.Contains(x.SIMDCode)).ToList();
                    vmSIMD.IsShowData = true;
                }
                else
                {
                    vmSIMD.ListSIMDData = ListSIMDData;
                    vmSIMD.IsShowData = true;
                }
                Session["SessionListSIMDData"] = vmSIMD.ListSIMDData;
            }

           // vmSIMD.ListSIMDdata = GetSIMDDatabySchoolname(sSchoolName);
            return View("Index", vmSIMD);
        }

        [HttpPost]
        public JsonResult GetChartDataSIMD(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listSIMDData = Session["SessionListSIMDData"] as List<SIMDObj>;
                if (listSIMDData != null)
                {
                    var listSIMDFilter = listSIMDData.Where(x => arrParameterFilter.Contains(x.SIMDCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Scottish Index of Multiple Deprivation",
                        ChartCategories = listSIMDFilter.Select(x => x.SIMDCode).ToArray(),
                        ChartSeries = ProcessChartDataSIMD(listSIMDFilter)
                    };
                }


                return Json(oChartData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }

        private List<object> ProcessChartDataSIMD(List<SIMDObj> listSIMDFilter)
        {
            var listChartData = new List<object>();
            var ListSelectedYears =  Session["ListSelectedYears"] as List<string>;
            var schoolname =  Session["sSchoolName"];

            foreach (var itemYear in ListSelectedYears)
            {
                if (itemYear.Equals("2009"))
                {
                    listChartData.Add(new { name = schoolname+"2009", data = listSIMDFilter.Select(x => x.PercentageInSchool2009).ToArray() });
                    listChartData.Add(new { name = "AllSchool2009", data = listSIMDFilter.Select(x => x.PercentageAllSchool2009).ToArray() });
                }

                if (itemYear.Equals("2012"))
                {
                    listChartData.Add(new { name = schoolname+"2012", data = listSIMDFilter.Select(x => x.PercentageInSchool2012).ToArray() });
                    listChartData.Add(new { name = "AllSchool2012", data = listSIMDFilter.Select(x => x.PercentageAllSchool2012).ToArray() });
                }
            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listSIMDData = Session["SessionListSIMDData"] as List<SIMDObj>;
            string schoolname = Session["sSchoolName"].ToString();

            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SIMDexport.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
            var listSIMDData = Session["SessionListSIMDData"] as List<SIMDObj>;
            // var listEthnicData2 = Session["SessionListEthnicData2"] as List<EthnicObj>;
            string sSchoolName = Session["sSchoolName"] as string;
            //string sSchoolName2 = Session["sSchoolName2"] as string;

            //var transformObject = new Object();

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("Deciles", typeof(string));
            dtResult.Columns.Add("(2009) % pupils in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("(2009) % pupils in All Primary school", typeof(double));
            dtResult.Columns.Add("(2012) % pupils in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("(2012) % pupils in All Primary school ", typeof(double));
            
            var transformObject = new
            {
                Col1 = listSIMDData.Select(x => x.SIMDCode).ToList(),                
                Col2 = listSIMDData.Select(x => x.PercentageInSchool2009).ToList(),
                Col3 = listSIMDData.Select(x => x.PercentageAllSchool2009).ToList(),
                Col4 = listSIMDData.Select(x => x.PercentageInSchool2012).ToList(),
                Col5 = listSIMDData.Select(x => x.PercentageAllSchool2012).ToList(),
            };

            for (var i = 0; i < listSIMDData.Count; i++)
            {
                dtResult.Rows.Add(
                    transformObject.Col1[i],
                    transformObject.Col2[i],
                    transformObject.Col3[i],
                    transformObject.Col4[i],
                    transformObject.Col5[i]
                    );
            }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Scottish Index of Multiple Deprivation "; // use cell address in range
            //worksheet.Cell("A2").Value = "Nationality"; // use cell address in range
            worksheet.Cell("A2").Value = "% of pupils in each Decile";
            worksheet.Cell(3, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

    }
}