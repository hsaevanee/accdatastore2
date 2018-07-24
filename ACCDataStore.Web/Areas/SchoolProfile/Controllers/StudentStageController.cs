using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.StudentStage;
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
    public class StudentStageController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(StudentStageController));

        private readonly IGenericRepository rpGeneric;

        public StudentStageController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }
        // GET: SchoolProfile/StudentStage
        public ActionResult Index(string sSchoolName)
        {

            //page counter
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.StdSatgepgCounter++;
            ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

            var vmStudentStage = new StudentStageViewModel();

            var schoolname = new List<string>();
            var sStageCriteria = new List<string>();
            var setGenderCriteria = new List<string>();

            List<StdStageObj> ListStdStageData = new List<StdStageObj>();
            List<StdStageObj> temp = new List<StdStageObj>();


            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t t2 on t1.SeedCode = t2.SeedCode ");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmStudentStage.ListSchoolNameData = fooList;

            vmStudentStage.Listcheckmodel = new List<CheckModel> { new CheckModel(1, "Female"), new CheckModel(2, "Male"), new CheckModel(3, "Total") };
            
            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM sch_Student_t group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmStudentStage.ListStageCode = fooList;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Gender FROM sch_Student_t group by Gender");

            fooList = listResult.OfType<string>().ToList();
            fooList.Add("T");
            vmStudentStage.ListGenderCode = fooList;
            vmStudentStage.DicGender = GetDicGender();


            if (Request.HttpMethod == "GET") // get method
            {
                if (sSchoolName == null) // case of index page, show criteria
                {
                    vmStudentStage.IsShowCriteria = true;
                }
                else // case of detail page, by pass criteria
                {
                    vmStudentStage.IsShowCriteria = false;
                    vmStudentStage.ListSelectedGender = vmStudentStage.ListGenderCode;
                    Session["ListSelectedGender"] = vmStudentStage.ListSelectedGender;
                    vmStudentStage.ListSelectedStdStage = vmStudentStage.ListStageCode;
                    vmStudentStage.selectedschoolname = sSchoolName;
                    Session["sSchoolName"] = sSchoolName;
                }

            }
            else // post method
            {
                vmStudentStage.IsShowCriteria = true;
                sSchoolName = Request["selectedschoolname"];

                var tmpaa = Request["Selectedcheckmodel"];
                vmStudentStage.selectedschoolname = sSchoolName;
                Session["sSchoolName"] = sSchoolName;

                if (Request["stages"] != null)
                {
                    sStageCriteria = Request["stages"].Split(',').ToList();
                    vmStudentStage.ListSelectedStdStage = sStageCriteria;
                }
                else
                {
                    sStageCriteria = null;
                }
                if (Request["gender"] != null)
                {
                    vmStudentStage.ListSelectedGender = Request["gender"].Split(',').ToList();
                }
                else
                {
                    vmStudentStage.ListSelectedGender = vmStudentStage.ListGenderCode;
                }

                Session["ListSelectedGender"] = vmStudentStage.ListSelectedGender;
                // get parameter from Request object
            }

            vmStudentStage.DicGenderWithSelected = GetDicGenderWithSelected(vmStudentStage.ListSelectedGender);

            // process data
            if (sSchoolName ==null || sSchoolName.Equals(""))
            {
                vmStudentStage.IsShowData = false;
            }
            else if (sSchoolName!= null)
            {
                vmStudentStage.selectedschoolname = sSchoolName;
                ListStdStageData = GetStudentStageDatabySchoolname(rpGeneric, sSchoolName);

                if (sStageCriteria == null)
                {
                    vmStudentStage.ListStdStageData = null;
                    vmStudentStage.IsShowData = false;
                }
                else if (sStageCriteria.Count != 0 && sStageCriteria != null)
                {
                    vmStudentStage.IsShowData = true;
                    vmStudentStage.ListStdStageData = ListStdStageData.Where(x => sStageCriteria.Contains(x.StageCode)).ToList();
                }
                else
                {
                    vmStudentStage.IsShowData = true;
                    vmStudentStage.ListStdStageData = ListStdStageData;
                }
                Session["SessionListStudentStageData"] = vmStudentStage.ListStdStageData;
            }
            return View("Index", vmStudentStage);
        }

        [HttpPost]
        public JsonResult GetChartDataStudentStage(string[] arrParameterFilter)
        {
            try
            {
                object oChartData = new object();
                string[] Categories = new string[arrParameterFilter.Length];

                var listStdStageData = Session["SessionListStudentStageData"] as List<StdStageObj>;
                if (listStdStageData != null)
                {
                    var listStdStageFilter = listStdStageData.Where(x => arrParameterFilter.Contains(x.StageCode)).ToList();


                    // process chart data
                    oChartData = new
                    {
                        ChartTitle = "Stage",
                        ChartCategories = listStdStageFilter.Select(x => x.StageCode).ToArray(),
                        ChartSeries = ProcessChartDataStdStage(listStdStageFilter)
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

        private List<object> ProcessChartDataStdStage(List<StdStageObj> listStdStageFilter)
        {
            var listChartData = new List<object>();
            var ListSelectedGender = Session["ListSelectedGender"] as List<string>;
            var schoolname = Session["sSchoolName"];

            foreach (var itemGender in ListSelectedGender)
            {
                if (itemGender.Equals("F"))
                {
                    listChartData.Add(new { name = schoolname + " Female", data = listStdStageFilter.Select(x => x.PercentageFemaleInSchool).ToArray() });
                    listChartData.Add(new { name = "Female All School", data = listStdStageFilter.Select(x => x.PercentageFemaleAllSchool).ToArray() });

                }

                if (itemGender.Equals("M"))
                {
                    listChartData.Add(new { name = schoolname + " Male", data = listStdStageFilter.Select(x => x.PercentageMaleInSchool).ToArray() });
                    listChartData.Add(new { name = "Male All School", data = listStdStageFilter.Select(x => x.PercentageMaleAllSchool).ToArray() });

                }
                if (itemGender.Equals("T"))
                {
                    listChartData.Add(new { name = schoolname + " Total", data = listStdStageFilter.Select(x => x.PercentageInSchool).ToArray() });
                    listChartData.Add(new { name = "Total All School", data = listStdStageFilter.Select(x => x.PercentageAllSchool).ToArray() });
                }

            }
            return listChartData;
        }

        public ActionResult ExportExcel()
        {
            var listStdStageData = Session["SessionListStudentStageData"] as List<StdStageObj>;
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "STDStageExport.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
            var listStdStageData = Session["SessionListStudentStageData"] as List<StdStageObj>;
            // var listEthnicData2 = Session["SessionListEthnicData2"] as List<EthnicObj>;
            string sSchoolName = Session["sSchoolName"] as string;
            //string sSchoolName2 = Session["sSchoolName2"] as string;

            //var transformObject = new Object();

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("Stage", typeof(string));
            //dtResult.Columns.Add("Nationality", typeof(string));
            dtResult.Columns.Add("Female in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Female in All Primary school", typeof(double));
            dtResult.Columns.Add("Male in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Male in All  Primary school ", typeof(double));
            dtResult.Columns.Add("Total in " + sSchoolName, typeof(double));
            dtResult.Columns.Add("Total in All Primary school", typeof(double));

            var transformObject = new
            {
                Col1 = listStdStageData.Select(x => x.StageCode).ToList(),
                Col2 = listStdStageData.Select(x => x.PercentageFemaleInSchool).ToList(),
                Col3 = listStdStageData.Select(x => x.PercentageFemaleAllSchool).ToList(),
                Col4 = listStdStageData.Select(x => x.PercentageMaleInSchool).ToList(),
                Col5 = listStdStageData.Select(x => x.PercentageMaleAllSchool).ToList(),
                Col6 = listStdStageData.Select(x => x.PercentageInSchool).ToList(),
                Col7 = listStdStageData.Select(x => x.PercentageAllSchool).ToList(),
            };

            for (var i = 0; i < listStdStageData.Count; i++)
            {
                dtResult.Rows.Add(
                    transformObject.Col1[i],
                    transformObject.Col2[i],
                    transformObject.Col3[i],
                    transformObject.Col4[i],
                    transformObject.Col5[i],
                    transformObject.Col6[i],
                    transformObject.Col7[i]                    
                    );
            }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            //worksheet.Cell("A1").Value = schoolname; // use cell address in range
            worksheet.Cell("A1").Value = "Student Stages"; // use cell address in range
            worksheet.Cell("A2").Value = "% of pupils in each stage"; 
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

        private List<StdStageObj> GetdatabySchCode(int pSchcode)
        {
            Console.Write("GetdatabyPostcode ==> ");

            List<StdStageObj> listtemp = new List<StdStageObj>();
            StdStageObj tempNationalObj = new StdStageObj();


            //% for Specific Area like AB21
            var listResult = rpGeneric.FindByNativeSQL("Select StudentStage,Gender, (Count(*)* 100 / (Select Count(*) From sch_Student_t_v2 where Seedcode =" + pSchcode + " )) From sch_Student_t_v2 where Seedcode =" + pSchcode + " Group By StudentStage, Gender ");

            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Nationalcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Nationalcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new StdStageObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.StageCode = Convert.ToString(itemRow[0]);
                            //tempNationalObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempNationalObj.EthinicCode) ? GetDicEhtnicBG()[tempNationalObj.EthinicCode] : "NO NAME";

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

        private List<StdStageObj> GetdatabyZonecode(string pZonecode)
        {
            Console.Write("GetdatabyZonecode ==> ");

            List<StdStageObj> listtemp = new List<StdStageObj>();
            StdStageObj tempNationalObj = new StdStageObj();

            string query = " Select t1.StudentStage, t1.Gender, (Count(*)* 100 /";
            //query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            //query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By NationalIdentity, Gender ";
            query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN CityShire  t2 on  t1.PostCode = t2.PostCode where DataZone in (\"" + pZonecode + "\") )) ";
            query += " From sch_Student_t t1 INNER JOIN CityShire  t2 on  t1.PostCode = t2.PostCode where DataZone in (\"" + pZonecode + "\") Group By StudentStage, Gender";

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
                        tempNationalObj = new StdStageObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.StageCode = Convert.ToString(itemRow[0]);
                            //tempNationalObj.EthinicName = GetDicNational().ContainsKey(tempNationalObj.EthinicCode) ? GetDicNational()[tempNationalObj.EthinicCode] : "NO NAME";

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
        public JsonResult GetChartDataStudentStageforMap(List<StdStageObj> data)
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
                        ChartTitle = "Student Stages - Primary Schools (%pupils)",
                        ChartCategories = data.Select(x => x.StageCode).ToArray(),
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