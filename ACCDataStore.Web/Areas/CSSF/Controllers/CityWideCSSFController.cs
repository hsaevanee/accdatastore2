using ACCDataStore.Core.Helper;
using ACCDataStore.Entity.CSSF;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.CSSF.Controllers
{
    public class CityWideCSSFController : BaseCSSFController
    {
        
        private static ILog log = LogManager.GetLogger(typeof(CityWideCSSFController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public CityWideCSSFController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        [AdminAuthentication]
        [Transactional]
        [HttpGet]
        // GET: CSSF/CityWide
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("CSSF/CityWideCSSF/GetCondition")]
        public virtual JsonResult GetCondition()
        {
            try
            {
                var ListReports = new[] { new { Code = "1", Name = "Cost Breakdown By Child" }, new { Code = "2", Name = "Cost Breakdown By Supplier" }, new { Code = "3", Name = "Cost Breakdown By Placement Category" }, new { Code = "4", Name = "Cost Breakdown By Service Type" } }.ToList();
                //, new { Code = "4", Name = "Cost Breakdown By Service Type" }
                object oResult = null;

                oResult = new
                {
                    ListReports = ListReports,
                    ListReportSelected = ListReports.First()
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }


        [HttpGet]
        [Route("CSSF/CityWideCSSF/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] string sReportID) // get selected list of school's id
        {
            try
            {
                var ListReports = new[] { new { Code = "1", Name = "Cost Breakdown By Child" }, new { Code = "2", Name = "Cost Breakdown By Supplier" }, new { Code = "3", Name = "Cost Breakdown By Placement Category" }, new { Code = "4", Name = "Cost Breakdown By Service Type" } }.ToList();
                
                Dictionary<string, string> DictPlacementCategory = GetDictPlacementCategory();
                Dictionary<string, string> DictServiceType = GetDictServiceType();

                var listChildAgreements = GetChildAgreement(rpGeneric2nd);

                var listResults = new { };
 
                object oResult = null;

                // Cost Breakdown By Child
                if (sReportID.Equals("1")) {

                    oResult = new
                    {
                        ListReports = ListReports,
                        ListReportSelected = ListReports.Where(x => x.Code.Equals(sReportID)).First(),
                        listResults = listChildAgreements.GroupBy(x => x.client_id).Select(r => new
                        {
                            Client_id = r.First().client_id,
                            Count = r.Count().ToString(),
                            CountClosed = r.Where(x => x.payattension.Equals("")).Count().ToString(),
                            CountOngoing = r.Where(x => x.payattension.Equals("**")).Count().ToString(),
                            CountChild = r.Count().ToString(),
                            TotalCost = NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 2).ToString(),
                            ClosedCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("")).Sum(xl => xl.actual_cost), 2).ToString(),
                            OngoingCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("**")).Sum(xl => xl.actual_cost), 2).ToString()

                        }).OrderByDescending(x => x.TotalCost).ToList(),
                        ShowCountChild = false

                    };

                }
                else if (sReportID.Equals("2"))
                {

                    oResult = new
                    {
                        ListReports = ListReports,
                        ListReportSelected = ListReports.Where(x => x.Code.Equals(sReportID)).First(),
                        listResults = listChildAgreements.GroupBy(x => x.supplier_code).Select(r => new
                        {
                            Client_id = r.First().supplier_name ,
                            Count = r.Count().ToString(),
                            CountClosed = r.Where(x => x.payattension.Equals("")).Count().ToString(),
                            CountOngoing = r.Where(x=>x.payattension.Equals("**")).Count().ToString(),
                            CountChild = r.Select(x => x.client_id).Distinct().Count().ToString(), //grp.Select(x => x.SomeField).Distinct().Count()
                            TotalCost = NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 2).ToString(),
                            ClosedCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("")).Sum(xl => xl.actual_cost), 2).ToString(),
                            OngoingCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("**")).Sum(xl => xl.actual_cost), 2).ToString()

                        }).OrderByDescending(x => x.TotalCost).ToList(),
                        ShowCountChild = true

                    };



                }
                else if (sReportID.Equals("3"))
                {
                                    oResult = new
                    {
                        ListReports = ListReports,
                        ListReportSelected = ListReports.Where(x => x.Code.Equals(sReportID)).First(),
                        listResults = listChildAgreements.GroupBy(x => x.Placement_Category).Select(r => new
                        {
                            Client_id = DictPlacementCategory[r.First().Placement_Category],                           
                            Count = r.Count().ToString(),
                            CountChild =r.Select(x => x.client_id).Distinct().Count().ToString(),
                            CountClosed = r.Where(x => x.payattension.Equals("")).Count().ToString(),
                            CountOngoing = r.Where(x => x.payattension.Equals("**")).Count().ToString(),
                            TotalCost = NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 2).ToString(),
                            ClosedCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("")).Sum(xl => xl.actual_cost), 2).ToString(),
                            OngoingCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("**")).Sum(xl => xl.actual_cost), 2).ToString()

                            // NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 1).ToString()
                        }).OrderByDescending(x => x.TotalCost).ToList(),
                        ShowCountChild = true,
                        PieChartCost = GetPieChartCostByPlacementCategory(listChildAgreements),
                        PieChartAgreements = GetPieChartNoAgreementByPlacementCategory(listChildAgreements),
                        BarChartCost = GetChartCostByPlacementCategory(listChildAgreements),
                        BarChartNoAgreement = GetChartNoAgreementsByPlacementCategory(listChildAgreements)

                    };
                }
                else
                {

                    oResult = new
                    {
                        ListReports = ListReports,
                        ListReportSelected = ListReports.Where(x => x.Code.Equals(sReportID)).First(),
                        listResults = listChildAgreements.GroupBy(x => x.Service_Type).Select(r => new
                        {
                            Client_id = DictServiceType[r.First().Service_Type],                           
                            Count = r.Count().ToString(),
                            CountChild =r.Select(x => x.client_id).Distinct().Count().ToString(),
                            CountClosed = r.Where(x => x.payattension.Equals("")).Count().ToString(),
                            CountOngoing = r.Where(x => x.payattension.Equals("**")).Count().ToString(),
                            TotalCost = NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 2).ToString(),
                            ClosedCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("")).Sum(xl => xl.actual_cost), 2).ToString(),
                            OngoingCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("**")).Sum(xl => xl.actual_cost), 2).ToString()

                           // Cost = NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 1).ToString()
                            // NumberFormatHelper.FormatNumber(r.Sum(xl => xl.actual_cost), 1).ToString()
                        }).OrderByDescending(x => x.TotalCost).ToList(),
                        ShowCountChild = true,
                        PieChartCost = GetPieChartCostByServiceType(listChildAgreements),
                        PieChartAgreements = GetPieChartNoAgreementByServiceType(listChildAgreements),
                        BarChartCost = GetChartCostByServiceType(listChildAgreements),
                        BarChartNoAgreement = GetChartNoAgreementsByServiceType(listChildAgreements)
                    };

                }

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }


    }
}