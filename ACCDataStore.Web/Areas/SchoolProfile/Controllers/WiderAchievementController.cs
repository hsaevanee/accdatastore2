using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.WiderAchievement;
using Common.Logging;
using ACCDataStore.Repository;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Entity;
using System.IO;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class WiderAchievementController : BaseSchoolProfileController
    {
        private static ILog log = LogManager.GetLogger(typeof(WiderAchievementController));

        private readonly IGenericRepository rpGeneric;

        public WiderAchievementController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        // GET: SchoolProfile/WiderAchievement
        //public ActionResult Index()
        //{
        //    string filePath = "C:\\data\\WiderAchievementData.xlsx";
        //    DataTable dt1 = new DataTable();
        //    //using (XLWorkbook workBook = new XLWorkbook(filePath))
        //    //{
        //    //    //Read the first Sheet from Excel file.
        //    //    IXLWorksheet workSheet = workBook.Worksheet(1);

        //    //    //Loop through the Worksheet rows.
        //    //    bool firstRow = true;
        //    //    foreach (IXLRow row in workSheet.Rows())
        //    //    {
        //    //        //Use the first row to add columns to DataTable.
        //    //        if (firstRow)
        //    //        {
        //    //            foreach (IXLCell cell in row.Cells())
        //    //            {
        //    //                dt.Columns.Add(cell.Value.ToString());
        //    //            }
        //    //            firstRow = false;
        //    //        }
        //    //        else
        //    //        {
        //    //            //Add rows to DataTable.
        //    //            dt.Rows.Add();
        //    //            int i = 0;
        //    //            foreach (IXLCell cell in row.Cells())
        //    //            {
        //    //                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
        //    //                i++;
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    using (XLWorkbook workBook = new XLWorkbook(filePath)){
        //        IXLWorksheet workSheet = workBook.Worksheet(1);
        //        //adding columns
        //        dt1.Columns.Add(workSheet.Row(3).Cell(1).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(2).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(3).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(4).Value.ToString());
        //        dt1.Columns.Add(workSheet.Row(3).Cell(5).Value.ToString());

        //        //adding table1
                

        //        for (int i = 4; i <= 26; i++ )
        //        {
        //            DataRow dataRow = dt1.NewRow();
        //            IXLRow row = workSheet.Row(i);
        //            for (int j = 0; j <= 4; j++)
        //            {
        //                dataRow[j] = (workSheet.Row(i).Cell(j+1).Value.ToString());
        //            }
        //            dt1.Rows.Add(dataRow);
        //        }
        //        DataRow dataRow1 = dt1.NewRow();
        //        dataRow1[0] = ("");
        //        dataRow1[1] = (workSheet.Row(31).Cell(2).Value.ToString());
        //        dataRow1[2] = (workSheet.Row(31).Cell(3).Value.ToString());
        //        dataRow1[3] = (workSheet.Row(31).Cell(4).Value.ToString());
        //        dataRow1[4] = (workSheet.Row(31).Cell(5).Value.ToString());
        //        dt1.Rows.Add(dataRow1);
        //    }


        //    DataTable dt2 = new DataTable();
        //    using (XLWorkbook workBook = new XLWorkbook(filePath))
        //    {
        //        IXLWorksheet workSheet = workBook.Worksheet(1);
        //        //adding columns
        //        dt2.Columns.Add(workSheet.Row(3).Cell(7).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(8).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(9).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(10).Value.ToString());
        //        dt2.Columns.Add(workSheet.Row(3).Cell(11).Value.ToString());

        //        //adding table1


        //        for (int i = 4; i <= 29; i++)
        //        {
        //            DataRow dataRow = dt2.NewRow();
        //            IXLRow row = workSheet.Row(i);
        //            for (int j = 0; j <= 4; j++)
        //            {
        //                dataRow[j] = (workSheet.Row(i).Cell(j + 7).Value.ToString());
        //            }
        //            dt2.Rows.Add(dataRow);
        //        }
        //        DataRow dataRow1 = dt2.NewRow();
        //        dataRow1[0] = ("");
        //        dataRow1[1] = (workSheet.Row(31).Cell(8).Value.ToString());
        //        dataRow1[2] = (workSheet.Row(31).Cell(9).Value.ToString());
        //        dataRow1[3] = (workSheet.Row(31).Cell(10).Value.ToString());
        //        dataRow1[4] = (workSheet.Row(31).Cell(11).Value.ToString());
        //        dt2.Rows.Add(dataRow1);
        //    }

        //    var vmWiderAchievement = new WiderAchievementViewModel();
        //    vmWiderAchievement.dtTable1 = dt1;

        //    vmWiderAchievement.dtTable2 = dt2;
        //    return View(vmWiderAchievement);
        //}


        public ActionResult Index(string schoolsubmitButton, string awardsubmitButton, string scqfsubmitButton)
        {
            //var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmWiderAchievement = new WiderAchievementViewModel();
            vmWiderAchievement.Listschoolname = GetListschoolname(this.rpGeneric);
            vmWiderAchievement.Listawardname = GetListAwardname(this.rpGeneric);
            vmWiderAchievement.Listscqf_rating = GetListSCQFRating(this.rpGeneric);

            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();

            if (schoolsubmitButton != null)
            {
                var sSchoolname= Request["selectedschoolname"];
                vmWiderAchievement.selectedschoolname = sSchoolname;
                if (sSchoolname != null)
                {
                    List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                    if (sSchoolname.Equals("Aberdeencity"))
                    {
                        if (listdata != null)
                        {
                            //temp = (from a in listdata where a.schoolname.Equals(sSchoolname) select a).ToList();
                            temp = listdata.GroupBy(a => new { a.age_range, a.awardname }).Select(x => new WiderAchievementObj
                            {
                                age_range = x.Key.age_range,
                                awardname = x.Key.awardname,
                                award2013 = x.Sum(y => y.award2013),
                                award2014 = x.Sum(y => y.award2014),
                                award2015 = x.Sum(y => y.award2015),
                            }).ToList();

                        }

                        temp = temp.OrderByDescending(x => x.age_range).ThenBy(x => x.awardname).ToList();
                    }
                    else {                                      
                        if (listdata != null) {
                            temp = (from a in listdata where a.centre.Equals(sSchoolname) select a).ToList();
                      
                        }
                    }

                }
            }
            if (awardsubmitButton != null)
            {
                var sAwardname = Request["selectedawardname"];
                vmWiderAchievement.selectedawardname = sAwardname;
                if (sAwardname != null)
                {
                    List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                    if (listdata != null)
                    {
                        temp = (from a in listdata where a.awardname.Equals(sAwardname) select a).ToList();

                    }

                }
            }
            if (scqfsubmitButton != null)
            {
                var sScqFname = Request["selectescqf_rating"];
                vmWiderAchievement.selectescqf_rating = sScqFname;
                if (sScqFname != null)
                {
                    List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                    if (listdata != null)
                    {
                        temp = (from a in listdata where a.scqf_rating.Equals(sScqFname) select a).ToList();

                    }

                }
            }

            vmWiderAchievement.Listresults = temp;
            Session["SessionWiderAchievementData"] = temp;
            Session["sSchoolName"] = vmWiderAchievement.selectedschoolname;

            return View("index", vmWiderAchievement);
        }

        protected List<School> GetListschoolname(IGenericRepository rpGeneric)
        {
            List<School> temp = new List<School>();

            //temp.Add(new School("Aberdeencity", "Aberdeencity"));

            var listdata = rpGeneric.FindSingleColumnByNativeSQL("Select distinct centre from WiderAchievementdata");
            if (listdata != null)
            {
                foreach (var item in listdata)
                {
                    if (item != null)
                    {
                        temp.Add(new School(item.ToString(), item.ToString()));
                    }

                }
            }
            return temp;

        }

        protected List<string> GetListAwardname(IGenericRepository rpGeneric)
        {
            List<string> temp = new List<string>();
            var listdata = rpGeneric.FindSingleColumnByNativeSQL("Select distinct awardname from WiderAchievementdata");
            if (listdata != null)
            {
                foreach (var item in listdata)
                {
                    if (item != null)
                    {
                        temp.Add(item.ToString());
                    }

                }
            }
            return temp;

        }

        protected List<string> GetListSCQFRating(IGenericRepository rpGeneric)
        {
            List<string> temp = new List<string>();
            var listdata = rpGeneric.FindSingleColumnByNativeSQL("Select distinct scqf_rating from WiderAchievementdata");
            if (listdata != null)
            {
                foreach (var item in listdata)
                {
                    if (item != null)
                    {
                        temp.Add(item.ToString());
                    }

                }
            }
            return temp;

        }

        public ActionResult ExportExcel()
        {
            var dataStream = GetWorkbookDataStream(GetData());
            return File(dataStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WiderAchievement.xlsx");
        }

        private DataTable GetData()
        {
            // simulate datatable
            var listWiderAchievementData = Session["SessionWiderAchievementData"] as List<WiderAchievementObj>;

            string sSchoolName = Session["sSchoolName"] as string;

            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("School", typeof(string));
            dtResult.Columns.Add("Age Range", typeof(string));
            dtResult.Columns.Add("Award", typeof(string));
            dtResult.Columns.Add("SCQF Rating", typeof(string));
            dtResult.Columns.Add("Gender", typeof(string));
            dtResult.Columns.Add("2013-2014", typeof(double));
            dtResult.Columns.Add("2014-2015", typeof(double));
            dtResult.Columns.Add("2015-2016", typeof(double));

            var transformObject = new
            {
                Col1 = listWiderAchievementData.Select(x => x.centre).ToList(),
                Col2 = listWiderAchievementData.Select(x => x.age_range).ToList(),
                Col3 = listWiderAchievementData.Select(x => x.awardname).ToList(),
                Col4 = listWiderAchievementData.Select(x => x.scqf_rating).ToList(),
                Col5 = listWiderAchievementData.Select(x => x.gender).ToList(),
                Col6 = listWiderAchievementData.Select(x => x.award2013).ToList(),
                Col7 = listWiderAchievementData.Select(x => x.award2014).ToList(),
                Col8 = listWiderAchievementData.Select(x => x.award2015).ToList(),
            };

            for (var i = 0; i < listWiderAchievementData.Count; i++)
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
            worksheet.Cell("A1").Value = "Wider Achievement Framework Data 2013-2015"; // use cell address in range
            worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Range("A1:H1").Merge();
            worksheet.Cell("D2").Value = "Citywide Totals";
            worksheet.Cell("D2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Range("D2:H2").Merge();
            // use cell address in range
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
            List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();

            Session["SessionWiderAchievementData"] = listdata;
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
            var listWiderAchievementData = Session["SessionWiderAchievementData"] as List<WiderAchievementObj>;

            try
            {
                object oChartData = new object();

                if (keyname.Equals("SchCode"))
                {
                    oChartData = new
                    {
                        dataTitle = keyvalue,
                        dataSeries = ""
                    };

                }
                else if (keyname.Equals("ZoneCode"))
                {
                    oChartData = new
                    {
                        dataTitle = "",
                        dataSeries = ""
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
    }
}