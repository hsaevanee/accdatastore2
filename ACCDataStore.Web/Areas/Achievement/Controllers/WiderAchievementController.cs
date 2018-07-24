﻿using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Areas.Achievement.ViewModels.WiderAchievement;
using ClosedXML.Excel;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.Achievement.Controllers
{
    public class WiderAchievementController : Controller
    {
        private static ILog log = LogManager.GetLogger(typeof(WiderAchievementController));

        private readonly IGenericRepository rpGeneric;
        //private readonly IGenericRepository3nd rpGeneric3nd;

        public WiderAchievementController(IGenericRepository rpGeneric, IGenericRepository3nd rpGeneric3nd)
        {
            this.rpGeneric = rpGeneric;
            //this.rpGeneric3nd = rpGeneric3nd;
        }

        public ActionResult IndexHome(string schoolsubmitButton, string awardsubmitButton)
        {
            //var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmWiderAchievement = new WiderAchievementViewModel();

            return View("Home", vmWiderAchievement);
        }

        public ActionResult Index(string schoolsubmitButton, string awardsubmitButton, string scqfsubmitButton, string searchButton)
        {
            //var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            //eGeneralSettings.CurriculumpgCounter++;
            //TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
            var vmWiderAchievement = new WiderAchievementViewModel();
            vmWiderAchievement.Listschoolname = GetListschoolname(this.rpGeneric);
            vmWiderAchievement.Listawardname = GetListAwardname(this.rpGeneric);
            vmWiderAchievement.Listscqf_rating = GetListSCQFRating(this.rpGeneric);

            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();

            //if (schoolsubmitButton != null)
            //{
            //    var sSchoolname = Request["selectedschoolname"];
            //    vmWiderAchievement.selectedschoolname = sSchoolname;
            //    if (sSchoolname != null)
            //    {
            //        List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
            //        if (sSchoolname.Equals("Citywide"))
            //        {
            //            if (listdata != null)
            //            {
            //                //temp = (from a in listdata where a.schoolname.Equals(sSchoolname) select a).ToList();
            //                temp = listdata.GroupBy(a => new { a.age_range, a.awardname }).Select(x => new WiderAchievementObj
            //                {
            //                    age_range = x.Key.age_range,
            //                    awardname = x.Key.awardname,
            //                    award2013 = x.Sum(y => y.award2013),
            //                    award2014 = x.Sum(y => y.award2014),
            //                    award2015 = x.Sum(y => y.award2015),
            //                }).ToList();

            //            }

            //            temp = temp.OrderByDescending(x => x.age_range).ThenBy(x => x.awardname).ToList();
            //        }
            //        else
            //        {
            //            if (listdata != null)
            //            {
            //                temp = (from a in listdata where a.centre.Equals(sSchoolname) select a).ToList();

            //            }
            //        }

            //    }
            //}
            //if (awardsubmitButton != null)
            //{
            //    var sAwardname = Request["selectedawardname"];
            //    vmWiderAchievement.selectedawardname = sAwardname;
            //    if (sAwardname != null)
            //    {
            //        List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
            //        if (listdata != null)
            //        {
            //            temp = (from a in listdata where a.awardname.Equals(sAwardname) select a).ToList();

            //        }

            //    }
            //}
            //if (scqfsubmitButton != null)
            //{
            //    var sScqFname = Request["selectescqf_rating"];
            //    vmWiderAchievement.selectescqf_rating = sScqFname;
            //    if (sScqFname != null)
            //    {
            //        List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
            //        if (listdata != null)
            //        {
            //            temp = (from a in listdata where a.scqf_rating.Equals(sScqFname) select a).ToList();

            //        }

            //    }
            //}
            if (searchButton != null)
            {
                var sSchoolname = Request["selectedschoolname"];
                vmWiderAchievement.selectedschoolname = sSchoolname;

                var sAwardname = Request["selectedawardname"];
                vmWiderAchievement.selectedawardname = sAwardname;

                var sScqFname = Request["selectescqf_rating"];
                vmWiderAchievement.selectescqf_rating = sScqFname;

                List<WiderAchievementObj> listdata = this.rpGeneric.FindAll<WiderAchievementObj>().ToList();
                if (!sSchoolname.Equals(""))
                {
                    if (sSchoolname.Equals("Aberdeen City"))
                    {
                        if (listdata != null)
                        {
                            //temp = (from a in listdata where a.schoolname.Equals(sSchoolname) select a).ToList();
                            temp = listdata.GroupBy(a => new { a.age_range, a.awardname, a.scqf_rating}).Select(x => new WiderAchievementObj
                            {
                                age_range = x.Key.age_range,
                                awardname = x.Key.awardname,
                                scqf_rating = x.Key.scqf_rating,
                                award2013 = x.Sum(y => y.award2013),
                                award2014 = x.Sum(y => y.award2014),
                                award2015 = x.Sum(y => y.award2015),
                                award2016 = x.Sum(y => y.award2016),
                            }).ToList();

                        }

                        temp = temp.OrderByDescending(x => x.age_range).ThenBy(x => x.awardname).ToList();
                    }
                    else
                    {
                        if (listdata != null)
                        {
                            temp = (from a in listdata where a.centre.Equals(sSchoolname) select a).ToList();

                        }
                    }

                }
                if (!sAwardname.Equals(""))
                {
                    if (temp.Count !=0)
                    {
                        temp = (from a in temp where a.awardname.Equals(sAwardname) select a).ToList();

                    }
                    else {
                        temp = (from a in listdata where a.awardname.Equals(sAwardname) select a).ToList();
                    }

                }
                if (!sScqFname.Equals(""))
                {
                    if (temp.Count != 0)
                    {
                        temp = (from a in temp where a.scqf_rating.Equals(sScqFname) select a).ToList();

                    }
                    else {
                        temp = (from a in listdata where a.scqf_rating.Equals(sScqFname) select a).ToList();
                    
                    }

                }

            }

            

            vmWiderAchievement.Listresults = temp;
            Session["SessionWiderAchievementData"] = temp;
            Session["sSchoolName"] = vmWiderAchievement.selectedschoolname;

            return View("index", vmWiderAchievement);
        }


        protected List<WiderAchievementObj> GetWiderAchievementdata(IGenericRepository rpGeneric)
        {
            Console.Write("GetWiderAchievementdata in BaseSchoolProfileController==> ");

            List<WiderAchievementObj> listdata = rpGeneric.FindAll<WiderAchievementObj>().ToList();
            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();

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
                    award2016 = x.Sum(y => y.award2016),
                }).ToList();

            }

            temp = temp.OrderByDescending(x => x.age_range).ThenBy(x => x.awardname).ToList();

            return temp;
        }

        protected List<School> GetListschoolname(IGenericRepository rpGeneric)
        {
            List<School> temp = new List<School>();

            temp.Add(new School("Aberdeen City", "Aberdeen City"));

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
            dtResult.Columns.Add("2016-2017", typeof(double));

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
                Col9 = listWiderAchievementData.Select(x => x.award2016).ToList(),
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
                    transformObject.Col8[i],
                    transformObject.Col9[i]
                    );
            }
            return dtResult;
        }

        private MemoryStream GetWorkbookDataStream(DataTable dtResult)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            worksheet.Cell("A1").Value = "Wider Achievement Framework Data 2013-2016"; // use cell address in range
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

            List<WiderAchievementObj> temp = new List<WiderAchievementObj>();
            List<WiderAchievementObj> first = new List<WiderAchievementObj>();

            try
            {
                object oChartData = new object();

                if (keyname.Equals("postcode"))
                {
                    if (listWiderAchievementData != null)
                    {
                        temp = (from a in listWiderAchievementData let postcode = String.Concat(a.post_out, a.post_in) where postcode.Equals(keyvalue) select a).ToList();
                        //temp = listWiderAchievementData.Where(x => String.Concat(x.post_out, x.post_in).Equals("keyvalue")).ToList();
                        first = temp.GroupBy(x => x.centre).Select(b => b.First()).ToList();
                    }
                    oChartData = new
                    {
                        dataTitle = first.Count() == 0 ? keyvalue : first[0].centre.ToString(),
                        dataSeries = temp
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