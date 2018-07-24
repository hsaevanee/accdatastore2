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
    public class IndexCSSFController : BaseCSSFController
    {

        private static ILog log = LogManager.GetLogger(typeof(IndexCSSFController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public IndexCSSFController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        [AdminAuthentication]
        [Transactional]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexHome()
        {

            return View("Home");
        }

        
        [HttpGet]
        [Route("CSSF/IndexCSSF/GetCondition")]
        public virtual JsonResult GetCondition()
        {
            try
            {             
                var ListCategory = new[] { new { Code = "OOA", Name = "Out of Authority" }, new { Code = "CO", Name = "Commissioned" }, new { Code = "CD", Name = "Disability" }, new { Code = "SP", Name = "Special Parenting" } }.ToList();

                var ListServiceType = new[] { new { Code = "SC", Name = "Secure Care" }, new { Code = "CH", Name = "Children's Home" }, new { Code = "RS", Name = "Residential School " }, new { Code = "CS", Name = "Crisis Support/Crisis Care" }, new { Code = "TBC", Name = "To Be Confirmed" } }.ToList();

                IList<ChildPlacements> listChild_id = GetListChildId(rpGeneric2nd);
 
                object oResult = null;

                oResult = new
                {
                    ListClientID = listChild_id.Select(x => new
                    {
                        Client_Id = x.client_id,
                        PlacementCategory = x.placement_category,
                        ServiceType = x.service_type
                    }).OrderBy(x => x.Client_Id),
                    ClientSelected = listChild_id.Select(x => new
                    {
                        Client_Id = x.client_id,
                        PlacementCategory = x.placement_category
                    }).First(),
                    ListCategory = ListCategory,
                    ListCategorySelected = ListCategory.First(),
                    ListServiceType = ListServiceType,
                    ListServiceTypeSelected = ListServiceType.First(),
                 };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }


        [HttpGet]
        [Route("CSSF/IndexCSSF/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] string sClientID, [System.Web.Http.FromUri] string sCategoryID, [System.Web.Http.FromUri] string sServiceType) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                var ListCategory = new[] { new { Code = "OOA", Name = "Out of Authority" }, new { Code = "CO", Name = "Commissioned" }, new { Code = "CD", Name = "Disability" }, new { Code = "SP", Name = "Special Parenting" } }.ToList();
                var ListServiceType = new[] { new { Code = "SC", Name = "Secure Care" }, new { Code = "CH", Name = "Children's Home" }, new { Code = "RS", Name = "Residential School" }, new { Code = "CS", Name = "Crisis Support/Crisis Care" }, new { Code = "TBC", Name = "To Be Confirmed" } }.ToList();
                Dictionary<string, string> DictSDataset = GetDatasets();

                IList<ChildPlacements> listChild_id = GetListChildId(rpGeneric2nd);
                var listChildAgreements = GetChildAgreementByClientID(rpGeneric2nd, sClientID);
                var listChildPlacements = GetChildPlacementByClientID(rpGeneric2nd, sClientID);
 
                 oResult = new
                {
                    ListClientID = listChild_id.Select(x => new
                    {
                        Client_Id = x.client_id,
                        PlacementCategory = x.placement_category,
                        ServiceType = x.service_type,
                    }).OrderBy(x => x.Client_Id),


                    ClientSelected = listChild_id.Where(x => x.client_id.Equals(sClientID)).Select(x => new
                    {
                        Client_Id = x.client_id,
                        PlacementCategory = x.placement_category,
                        ServiceType = x.service_type,
                    }).First(),

                    ListCategory = ListCategory,
                    ListCategorySelected = ListCategory.Where(x => x.Code.Equals(sCategoryID)).First(),
                    ListServiceType = ListServiceType,
                    ListServiceTypeSelected = ListServiceType.Where(x => x.Code.Equals(sServiceType)).First(),


                    listChildAgreements = listChildAgreements.Select(x => new
                    {
                        Client_Id = x.client_id,
                        Agreement_Id = x.agreement_id,
                        Agreement_Started = x.agreement_started.ToShortDateString(),
                        Agreement_Ended = x.agreement_ended.ToShortDateString(),
                        Active_Cost = NumberFormatHelper.FormatNumber(x.active_weeks_cost, 2).ToString(),                      
                        Actual_Cost = NumberFormatHelper.FormatNumber(x.actual_cost, 2).ToString() ,
                        NumberofDays = x.numberofdays,
                        Supplier_Name = x.supplier_name,
                        Payattention = x.payattension,
                        DatasetDate = DictSDataset[x.dataset]
                    }).OrderBy(x => x.Agreement_Id).ThenBy(x=>x.Agreement_Started),

                    listChildPlacements = listChildPlacements.Select(x => new
                    {
                        Client_Id = x.client_id,
                        DateOfBirth = x.dob.ToShortDateString(),
                        Gender = x.gender,
                        Age = GetAge(x.dob),
                        Placement_Id = x.placement_id,
                        Palcement_Started = x.placement_started.ToShortDateString(),
                        Palcement_Ended = x.placement_ended.ToShortDateString(),
                        Placement_Name = x.placement_name,
                        Payattention = x.payattension,
                        DatasetDate = DictSDataset[x.dataset]
                    }).OrderBy(x => x.Placement_Id).ThenBy(x=>x.Palcement_Started),
                    Cost_OOA = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("OOA")&& x.payattension.Equals("")).Sum(x => x.actual_cost), 2).ToString(),
                    Cost_D = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("CD") && x.payattension.Equals("")).Sum(x => x.actual_cost), 2).ToString(),
                    Cost_CO = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("CO") && x.payattension.Equals("")).Sum(x => x.actual_cost), 2).ToString(),
                    Cost_SP = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("SP") && x.payattension.Equals("")).Sum(x => x.actual_cost), 2).ToString(),
                    Cost_total = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.payattension.Equals("")).Sum(x => x.actual_cost), 2).ToString(),
                    Active_Cost_OOA = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("OOA") && x.payattension.Equals("**")).Sum(x => x.actual_cost), 2).ToString(),
                    Active_Cost_D = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("CD") && x.payattension.Equals("**")).Sum(x => x.actual_cost), 2).ToString(),
                    Active_Cost_CO = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("CO") && x.payattension.Equals("**")).Sum(x => x.actual_cost), 2).ToString(),
                    Active_Cost_SP = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.Placement_Category.Equals("SP") && x.payattension.Equals("**")).Sum(x => x.actual_cost), 2).ToString(),
                    Active_Cost_total = NumberFormatHelper.FormatNumber(listChildAgreements.Where(x => x.payattension.Equals("**")).Sum(x => x.actual_cost), 2).ToString(),
                    listCostbySupplier = listChildAgreements.GroupBy(x=>x.supplier_code).Select(r => new
                    {
                        Supplier_name = r.First().supplier_name ,
                        Count = r.Count().ToString(),
                        Cost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("")).Sum(xl=>xl.actual_cost), 2).ToString(),
                        OngoingCost = NumberFormatHelper.FormatNumber(r.Where(x => x.payattension.Equals("**")).Sum(xl => xl.actual_cost), 2).ToString(), 

                    }).ToList(),
                    ChartCostbyAuthority = GetChartCostByAuthorities(listChildAgreements),
                    ChartCostbySupplier = GetChartCostBySupplier(listChildAgreements),
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