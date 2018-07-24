using ACCDataStore.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.Language;
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
    public class LanguageController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(LanguageController));

        private readonly IGenericRepository rpGeneric;

        public LanguageController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: SchoolProfile/Language
        public ActionResult Index(string sSchoolName)
        {
            //level of english page counter
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.EALpgCounter++;
            ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));


            var vmLanguage = new LanguageViewModel();            
            var sNationalCriteria = new List<string>();
            var setGenderCriteria = new List<string>();
            List<NationalityObj> ListLevelENData = new List<NationalityObj>();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t t2 on t1.SeedCode = t2.SeedCode ");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmLanguage.ListSchoolNameData = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW LevelOfEnglish FROM sch_Student_t group by LevelOfEnglish");

            fooList = listResult.OfType<string>().ToList();
            vmLanguage.ListLevelENCode = fooList;
            vmLanguage.DicLevelEN = GetDicLevelEnglish();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM sch_Student_t group by Gender");

            fooList = listResult.OfType<string>().ToList();
            fooList.Add("T");
            vmLanguage.ListGenderCode = fooList;
            vmLanguage.DicGender = GetDicGender();

            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmLanguage.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmLanguage.IsShowCriteria = false;
                    vmLanguage.ListSelectedGender = vmLanguage.ListGenderCode;
                    vmLanguage.ListLevelENCode = vmLanguage.ListLevelENCode;
                    Session["ListSelectedGender"] = vmLanguage.ListSelectedGender;
                    //Session["ListSelectedNationality"] = vmNationality.ListSelectedNationality;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                vmLanguage.IsShowCriteria = true;
                sSchoolName = Request["selectedschoolname"];
                vmLanguage.selectedschoolname = sSchoolName;
                Session["sSchoolName"] = sSchoolName;

                if (Request["levelofen"] != null)
                {
                    sNationalCriteria = Request["levelofen"].Split(',').ToList();
                    vmLanguage.ListSelectedLevelENCode = sNationalCriteria;
                }
                else
                {
                    sNationalCriteria = null;
                }
                if (Request["gender"] != null)
                {
                    vmLanguage.ListSelectedGender = Request["gender"].Split(',').ToList();
                }
                else
                {
                    vmLanguage.ListSelectedGender = vmLanguage.ListGenderCode;
                }

                Session["ListSelectedGender"] = vmLanguage.ListSelectedGender;
                //Session["ListSelectedNationality"] = vmNationality.ListSelectedNationality;
                // get parameter from Request object
            }

            vmLanguage.DicGenderWithSelected = GetDicGenderWithSelected(vmLanguage.ListSelectedGender);

            // process data
            if (sSchoolName == null || sSchoolName.Equals(""))
            {
                vmLanguage.IsShowData = false;
            }
            else if (sSchoolName != null)
            {
                vmLanguage.selectedschoolname = sSchoolName;
                ListLevelENData = GetLevelENDatabySchoolname(rpGeneric, sSchoolName);

                if (sNationalCriteria == null)
                {
                    vmLanguage.IsShowData = false;
                    vmLanguage.ListLevelENData = null;
                }
                else if (sNationalCriteria.Count != 0 && sNationalCriteria != null)
                {
                    vmLanguage.IsShowData = true;
                    vmLanguage.ListLevelENData = ListLevelENData.Where(x => sNationalCriteria.Contains(x.IdentityCode)).ToList();
                }
                else
                {
                    vmLanguage.IsShowData = true;
                    vmLanguage.ListLevelENData = ListLevelENData;
                }
                Session["SessionListLevelENData"] = vmLanguage.ListLevelENData;
            }

            return View("index", vmLanguage);
        }

        [HttpPost]
        public JsonResult GetChartDataLevelEnglish(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var ListLevelENData = Session["SessionListLevelENData"] as List<NationalityObj>;
                if (ListLevelENData != null)
                {
                    var listLevelENFilter = ListLevelENData.Where(x => arrParameterFilter.Contains(x.IdentityCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "test",
                        ChartCategories = listLevelENFilter.Select(x => x.IdentityName).ToArray(),
                        ChartSeries = ProcessChartDataEthnic(listLevelENFilter)
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

        private List<object> ProcessChartDataEthnic(List<NationalityObj> listNationalFilter)
        {
            var listChartData = new List<object>();
            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            var schoolname = Session["sSchoolName"];

            foreach (var itemGender in ListSelectedGender)
            {
                if (itemGender.Equals("F"))
                {
                    listChartData.Add(new { name = schoolname + " Female", data = listNationalFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                    listChartData.Add(new { name = "Female All School", data = listNationalFilter.Select(x => x.PercentageFemaleAllSchool).ToArray() });                    
                }

                if (itemGender.Equals("M"))
                {
                    listChartData.Add(new { name = schoolname + " Male", data = listNationalFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                    listChartData.Add(new { name = "Male All School", data = listNationalFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });
                }
                if (itemGender.Equals("T"))
                {                    
                    listChartData.Add(new { name = schoolname + " Total", data = listNationalFilter.Select(x => x.PercentageInSchool).ToArray() });
                    listChartData.Add(new { name = "Total All School", data = listNationalFilter.Select(x => x.PercentageAllSchool).ToArray() });
                }

            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            //var listNationalityData = Session["SessionListLevelENData"] as List<NationalityObj>;
            //string schoolname = Session["sSchoolName"].ToString();
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EALExport.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
            var listNationalityData = Session["SessionListLevelENData"] as List<NationalityObj>;
            // var listEthnicData2 = Session["SessionListEthnicData2"] as List<EthnicObj>;
            string sSchoolName = Session["sSchoolName"] as string;
            //string sSchoolName2 = Session["sSchoolName2"] as string;

            //var transformObject = new Object();

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("Level Of English Code", typeof(string));
            dtResult.Columns.Add("Level Of English", typeof(string));
            dtResult.Columns.Add("Female in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Female in All Primary school", typeof(double));
            dtResult.Columns.Add("Male in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Male in All  Primary school ", typeof(double));
            dtResult.Columns.Add("Total in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Total in All Primary school", typeof(double));

            var transformObject = new
            {
                Col1 = listNationalityData.Select(x => x.IdentityCode).ToList(),
                Col2 = listNationalityData.Select(x => x.IdentityName).ToList(),
                Col3 = listNationalityData.Select(x => x.PercentageFemaleInSchool).ToList(),
                Col4 = listNationalityData.Select(x => x.PercentageFemaleAllSchool).ToList(),
                Col5 = listNationalityData.Select(x => x.PercentageMaleInSchool).ToList(),
                Col6 = listNationalityData.Select(x => x.PercentageMaleAllSchool).ToList(),
                Col7 = listNationalityData.Select(x => x.PercentageInSchool).ToList(),
                Col8 = listNationalityData.Select(x => x.PercentageAllSchool).ToList(),
            };

            for (var i = 0; i < listNationalityData.Count; i++)
            {
                dtResult.Rows.Add(
                    transformObject.Col1[i],
                    transformObject.Col2[i],
                    transformObject.Col3[i],
                    transformObject.Col4[i],
                    transformObject.Col5[i],
                    transformObject.Col6[i],
                    transformObject.Col7[i],
                    transformObject.Col8[i]
                    );
            }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Level of English"; // use cell address in range
            //worksheet.Cell("A2").Value = "Nationality"; // use cell address in range
            worksheet.Cell("A2").Value = "% of pupils in each level of english";
            worksheet.Cell(3, 1).InsertTable(dtResult); // use row & column index
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public ActionResult MapData()
        {
            //var listNationalityData = Session["SessionListNationalityData"] as List<NationalityObj>;
            return View("MapIndex");
        }

        protected JsonResult ThrowJSONError(Exception ex)
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
            var sErrorMessage = "Error : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
            return Json(new { Message = sErrorMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SearchByName(string keyvalue, string keyname)
        {
            try
            {
                object oChartData = new object();

                if (keyname.Equals("SchCode"))
                {
                    oChartData = new
                    {
                        dataTitle = GetSchNamebySchCode(int.Parse(keyvalue)),
                        dataSeries = GetdatabySchCode(int.Parse(keyvalue))
                    };

                }
                else if (keyname.Equals("ZoneCode"))
                {
                    oChartData = new
                    {
                        dataTitle = keyvalue,
                        dataSeries = GetdatabyZonecode(keyvalue)
                    };
                }

                // use sName (AB24) to query data from database
                return Json(oChartData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return ThrowJSONError(ex);
            }
        }

        private string GetSchNamebySchCode(int pSchcode)
        {
            Console.Write("GetSchNamebySchCode ==> ");

            string SchoolName = "";
            //% for Specific Area like AB21
            //var listResult = rpGeneric.FindByNativeSQL("Select NationalIdentity,Gender, (Count(NationalIdentity)* 100 / (Select Count(*) From sch_Student_t_v2 where PostOut in (\"" + pPostcode + "\") ))  From sch_Student_t_v2 where PostOut in (\"" + pPostcode + "\")  Group By NationalIdentity, Gender ");
            var listResult = rpGeneric.FindSingleColumnByNativeSQL("Select Name From sch_PrimarySchool_t  where Seedcode =" + pSchcode);

            if (listResult.Any())
            {
                SchoolName = Convert.ToString(listResult[0]);

            }
            else
            {

                SchoolName = "No School data";

            }


            return SchoolName;

        }

        private List<NationalityObj> GetdatabySchCode(int pSchcode)
        {
            Console.Write("GetdatabyPostcode ==> ");

            List<NationalityObj> listtemp = new List<NationalityObj>();
            NationalityObj tempNationalObj = new NationalityObj();


            //% for Specific Area like AB21
            var listResult = rpGeneric.FindByNativeSQL("Select LevelOfEnglish,Gender, (Count(*)* 100 / (Select Count(*) From sch_Student_t_v2 where Seedcode =" + pSchcode + " ))  From sch_Student_t_v2 where Seedcode =" + pSchcode + " Group By LevelOfEnglish, Gender ");

            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Nationalcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Nationalcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new NationalityObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.IdentityName = GetDicLevelEnglish().ContainsKey(tempNationalObj.IdentityCode) ? GetDicLevelEnglish()[tempNationalObj.IdentityCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempNationalObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempNationalObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempNationalObj);
                    }
                }
            }

            foreach (var itemRow in listtemp)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listtemp;


        }

        private List<NationalityObj> GetdatabyZonecode(string pZonecode)
        {
            Console.Write("GetdatabyZonecode ==> ");

            List<NationalityObj> listtemp = new List<NationalityObj>();
            NationalityObj tempNationalObj = new NationalityObj();

            string query = " Select t1.LevelOfEnglish, t1.Gender, (Count(*)* 100 /";
            //query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            //query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By NationalIdentity, Gender ";
            query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN CityShire  t2 on  t1.PostCode = t2.PostCode where DataZone in (\"" + pZonecode + "\") )) ";
            query += " From sch_Student_t t1 INNER JOIN CityShire  t2 on  t1.PostCode = t2.PostCode where DataZone in (\"" + pZonecode + "\") Group By LevelOfEnglish, Gender";

            //Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in ('Brimmond Primary School')
            var listResult = rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Nationalcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Nationalcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new NationalityObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.IdentityName = GetDicLevelEnglish().ContainsKey(tempNationalObj.IdentityCode) ? GetDicLevelEnglish()[tempNationalObj.IdentityCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempNationalObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempNationalObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempNationalObj);
                    }
                }
            }


            foreach (var itemRow in listtemp)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listtemp;


        }


        [HttpPost]
        public JsonResult GetChartDataLanguageforMap(List<NationalityObj> data)
        {
            try
            {
                object oChartData = new object();
                if (data != null)
                {
                    var listChartData = new List<object>();
                    listChartData.Add(new { name = "Female", data = data.Select(x => x.PercentageFemaleAllSchool).ToArray() });
                    listChartData.Add(new { name = "Male", data = data.Select(x => x.PercentageMaleAllSchool).ToArray() });
                    listChartData.Add(new { name = "Total", data = data.Select(x => x.PercentageAllSchool).ToArray() });
                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Level of English - Primary Schools (%pupils)",
                        ChartCategories = data.Select(x => x.IdentityName).ToArray(),
                        ChartSeries = listChartData
                    };
                }
                else
                {

                    oChartData = new
                    {
                        ChartTitle = "No data available",
                        ChartCategories = new List<string>(),
                        ChartSeries = new List<double>()
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

    }
}