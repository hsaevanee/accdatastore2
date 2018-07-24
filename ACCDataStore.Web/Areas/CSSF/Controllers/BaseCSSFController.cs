using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using ACCDataStore.Repository;
using ACCDataStore.Entity.CSSF;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.PieCharts;

namespace ACCDataStore.Web.Areas.CSSF.Controllers
{
    public class BaseCSSFController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseCSSFController));

        protected IList<ChildPlacements> GetListChildId(IGenericRepository2nd rpGeneric2nd)
        {
            List<ChildPlacements> listResult = new List<ChildPlacements>();
            List<ChildPlacements> listchildID = rpGeneric2nd.FindAll<ChildPlacements>().ToList();
            var TempList = listchildID.GroupBy(x => x.client_id).Select(y => y.First());
            foreach (var item in TempList)
            {
                listResult.Add(item);
            }
            return listResult;

        }

        protected Dictionary<string, string> GetDictPlacementCategory()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("CO", "Commissioned");
            dictionary.Add("CD", "Disability");
            dictionary.Add("SP", "Special Parenting");
            dictionary.Add("OOA", "Out of Authority");

            return dictionary;
        
        }

        protected Dictionary<string, string> GetDatasets()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("180212", "12/02/2018");
            dictionary.Add("180604", "04/06/2018");

            return dictionary;

        }

        protected Dictionary<string, string> GetDictServiceType()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("RS", "Residential School");
            dictionary.Add("SC", "Secure Care");
            dictionary.Add("CH", "Children’s Home");
            dictionary.Add("CS", "Crisis Support/Crisis Care");
            dictionary.Add("ES", "Extra Support");
            dictionary.Add("AC", "Additional Care");
            dictionary.Add("TR", "Transport");
            dictionary.Add("TBC", "To Be Confirmed");

            return dictionary;

        }

        protected Int32 GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }

        protected IList<ChildPlacements> GetChildPlacementByClientID(IGenericRepository2nd rpGeneric2nd, string ClientID)
        {
            List<ChildPlacements> listResult = new List<ChildPlacements>();
            List<ChildPlacements> listChildPlacement = rpGeneric2nd.FindAll<ChildPlacements>().ToList();
            //List<Placements> listPlacements = rpGeneric2nd.FindAll<Placements>().ToList();
            var TempList  = listChildPlacement.Where(x => x.client_id.Equals(ClientID)).ToList();

            //var TempList = TempListALL.GroupBy(i => i.placement_id).Select(group => group.First()).ToList(); //Get data y unique agreement_id

            foreach (var item in TempList)
            {
                if (item.placement_ended.Equals(DateTime.MinValue))
                {
                    item.placement_ended = DateTime.Today;
                    item.payattension = "**";
                }
                else
                {
                    item.payattension = "";
                }
                listResult.Add(item);

            }
            return listResult;

        }

        protected IList<ChildAgreements> GetChildAgreementByClientID(IGenericRepository2nd rpGeneric2nd, string ClientID)
        {
            List<ChildAgreements> listResult = new List<ChildAgreements>();
            List<ChildAgreements> listChildAgreements = rpGeneric2nd.Find<ChildAgreements>(" from ChildAgreements where authorisation_status = :author_status ", new string[] { "author_status" }, new object[] { 1 }).ToList();

            //.Find<Users>(" from Users where UserName = :UserName ", new string[] { "UserName" }, new object[] { sUserName });
            var TempList = listChildAgreements.Where(x => x.client_id.Equals(ClientID)).ToList();
            double numberofdays = 0.0;

            foreach (var item in TempList)
            {
                //calculate number of days - (EndDate - StartDate).TotalDays 
                if (item.agreement_ended.Equals(DateTime.MinValue))
                {
                    item.agreement_ended = DateTime.Today;
                    item.payattension = "**";
                }
                else {
                    item.payattension = "";
                } 

                numberofdays =  (item.agreement_ended - item.agreement_started).TotalDays;
                item.numberofdays = Convert.ToInt32(numberofdays);
                item.actual_cost = (item.active_weeks_cost / 7) * numberofdays;
                listResult.Add(item);
            }
            return listResult;

        }

        protected IList<ChildAgreements> GetChildAgreement(IGenericRepository2nd rpGeneric2nd)
        {
            List<ChildAgreements> listResult = new List<ChildAgreements>();
//            List<ChildAgreements> listChildAgreements = rpGeneric2nd.FindAll<ChildAgreements>().ToList();
            List<ChildAgreements> listChildAgreements = rpGeneric2nd.Find<ChildAgreements>(" from ChildAgreements where authorisation_status = :author_status ", new string[] { "author_status" }, new object[] { 1 }).ToList();

            double numberofdays = 0.0;

            foreach (var item in listChildAgreements)
            {
                //calculate number of days - (EndDate - StartDate).TotalDays 
                if (item.agreement_ended.Equals(DateTime.MinValue))
                {
                    item.agreement_ended = DateTime.Today;
                    item.payattension = "**";
                }
                else
                {
                    item.payattension = "";
                } 
                numberofdays = (item.agreement_ended - item.agreement_started).TotalDays;

                item.numberofdays = Convert.ToInt32(numberofdays);
                item.actual_cost = (item.active_weeks_cost / 7) * numberofdays;
                listResult.Add(item);
            }
            return listResult;

        }

        protected IList<ChildPlacements> GetChildPlacements(IGenericRepository2nd rpGeneric2nd)
        {
            List<ChildPlacements> listResult = new List<ChildPlacements>();
            List<ChildPlacements> listchildplacements = rpGeneric2nd.FindAll<ChildPlacements>().ToList();

            return listchildplacements;

        }
        // Cost Breakdown by Authority
        protected ColumnCharts GetChartCostByAuthorities(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Cost breakdown by Placement Category";
            eColumnCharts.yAxis.title.text = "Cost (£)";
            eColumnCharts.yAxis.min = 0;
 
            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "Out of Authority", "Disability", "Commissioned", "Special Parenting" }; ;

                List<float?> data = new List<float?>() { };
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("OOA")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("CD")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("CO")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("SP")).Select(x => x.actual_cost).Sum());

                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    name = listChildAgreements[0].client_id,
                    data = data,
                    color = "#058DC7"
                });
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }


        // Cost Breakdown by Supplier
        protected ColumnCharts GetChartCostBySupplier(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Cost breakdown by Supplier (Agreement)";
            eColumnCharts.yAxis.title.text = "Cost (£)";
            eColumnCharts.yAxis.min = 0;
 
            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                var listCostbySupplier = listChildAgreements.GroupBy(x => x.supplier_code).Select(r => new
                   {
                       Supplier_name = r.First().supplier_name,
                       Count = r.Count().ToString(),
                       Cost = r.Sum(xl => xl.actual_cost).ToString()

                   }).ToList();

                eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
                eColumnCharts.xAxis.categories = new List<string>();
                List<float?> data = new List<float?>() { };

                foreach (var temp in listCostbySupplier)
                {
                    eColumnCharts.xAxis.categories.Add(temp.Supplier_name);
                    data.Add((float?)Convert.ToDouble(temp.Cost));
                }

                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    name = listChildAgreements[0].client_id,
                    data = data,
                    color = "#058DC7"
                });

            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }


        // Cost Breakdown by Placement Category
        protected PieCharts GetPieChartCostByPlacementCategory(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            Dictionary<string, string> DictPlacementCategory = GetDictPlacementCategory();

            var ePieCharts = new PieCharts();
            ePieCharts.SetDefault(false);
            ePieCharts.title.text = "Cost breakdown by Placement Category";

            ePieCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.PieCharts.series>();
            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                var listResults = listChildAgreements.GroupBy(x => x.Placement_Category).Select(r => new
                        {
                            Client_id = r.First().Placement_Category,
                            Count = r.Count().ToString(),
                            Cost = r.Sum(xl => xl.actual_cost).ToString()

                        }).OrderByDescending(x => x.Cost).ToList();


                List<dataItem> listdata = new List<dataItem>() { };

                foreach (var temp in listResults)
                {
                    dataItem dataItem  = new dataItem();
                    dataItem.name = DictPlacementCategory[temp.Client_id];
                    dataItem.y = (float?)Convert.ToDouble(temp.Cost);
                    dataItem.sliced = true;
                    listdata.Add(dataItem);
                }

                ePieCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.PieCharts.series()
                {
                    name = listChildAgreements[0].client_id,
                    data = listdata,
                    colorByPoint = true
                });

            }

            ePieCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //ePieCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return ePieCharts;
        }

        // #Agreements Breakdown by Authority
        protected PieCharts GetPieChartNoAgreementByPlacementCategory(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            Dictionary<string, string> DictPlacementCategory = GetDictPlacementCategory();

            var ePieCharts = new PieCharts();
            ePieCharts.SetDefault(false);
            ePieCharts.title.text = "Number of Agreement breakdown by Placement Category";

            ePieCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.PieCharts.series>();
            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                var listResults = listChildAgreements.GroupBy(x => x.Placement_Category).Select(r => new
                {
                    Client_id = r.First().Placement_Category,
                    Count = r.Count().ToString(),
                    Cost = r.Sum(xl => xl.actual_cost).ToString()

                }).OrderByDescending(x => x.Cost).ToList();


                List<dataItem> listdata = new List<dataItem>() { };

                foreach (var temp in listResults)
                {
                    dataItem dataItem = new dataItem();
                    dataItem.name = DictPlacementCategory[temp.Client_id];
                    dataItem.y = (float?)Convert.ToDouble(temp.Count);
                    dataItem.sliced = true;
                    listdata.Add(dataItem);
                }

                ePieCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.PieCharts.series()
                {
                    name = listChildAgreements[0].client_id,
                    data = listdata,
                    colorByPoint = true
                });

            }

            ePieCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //ePieCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return ePieCharts;
        }

        // Cost Breakdown by Service Type
        protected PieCharts GetPieChartCostByServiceType(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            Dictionary<string, string> DictServiceType = GetDictServiceType();

            var ePieCharts = new PieCharts();
            ePieCharts.SetDefault(false);
            ePieCharts.title.text = "Cost breakdown by Service Type";

            ePieCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.PieCharts.series>();
            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                var listResults = listChildAgreements.GroupBy(x => x.Service_Type).Select(r => new
                {
                    Client_id = r.First().Service_Type,
                    Count = r.Count().ToString(),
                    Cost = r.Sum(xl => xl.actual_cost).ToString()

                }).OrderByDescending(x => x.Cost).ToList();


                List<dataItem> listdata = new List<dataItem>() { };

                foreach (var temp in listResults)
                {
                    dataItem dataItem = new dataItem();
                    dataItem.name = DictServiceType[temp.Client_id];
                    dataItem.y = (float?)Convert.ToDouble(temp.Cost);
                    dataItem.sliced = true;
                    listdata.Add(dataItem);
                }

                ePieCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.PieCharts.series()
                {
                    name = listChildAgreements[0].client_id,
                    data = listdata,
                    colorByPoint = true
                });

            }

            ePieCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //ePieCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return ePieCharts;
        }

        // #Agreements Breakdown by Service Type
        protected PieCharts GetPieChartNoAgreementByServiceType(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            Dictionary<string, string> DictPlacementCategory = GetDictServiceType();

            var ePieCharts = new PieCharts();
            ePieCharts.SetDefault(false);
            ePieCharts.title.text = "Number of Agreement breakdown by Service Type";

            ePieCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.PieCharts.series>();
            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                var listResults = listChildAgreements.GroupBy(x => x.Service_Type).Select(r => new
                {
                    Client_id = r.First().Service_Type,
                    Count = r.Count().ToString(),
                    Cost = r.Sum(xl => xl.actual_cost).ToString()

                }).OrderByDescending(x => x.Cost).ToList();


                List<dataItem> listdata = new List<dataItem>() { };

                foreach (var temp in listResults)
                {
                    dataItem dataItem = new dataItem();
                    dataItem.name = DictPlacementCategory[temp.Client_id];
                    dataItem.y = (float?)Convert.ToDouble(temp.Count);
                    dataItem.sliced = true;
                    listdata.Add(dataItem);
                }

                ePieCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.PieCharts.series()
                {
                    name = listChildAgreements[0].client_id,
                    data = listdata,
                    colorByPoint = true
                });

            }

            ePieCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //ePieCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return ePieCharts;
        }


        // Barchart Cost Breakdown by ServiceType
        protected ColumnCharts GetChartCostByServiceType(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Cost breakdown by Service Type";
            eColumnCharts.yAxis.title.text = "Cost (£)";
            eColumnCharts.yAxis.min = 0;

            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "Secure Care", "Children's Home", "Residential School", "Crisis Support/Crisis Care", "To Be Confirmed" }; ;

                List<float?> data = new List<float?>() { };
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("SC")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("CH")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("RS")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("CS")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("TBC")).Select(x => x.actual_cost).Sum());

                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    name = "Service Type",
                    data = data,
                    color = "#058DC7"
                });
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Barchart Cost Breakdown by ServiceType
        protected ColumnCharts GetChartNoAgreementsByServiceType(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Number of Agreement Breakdown by Service Type";
            eColumnCharts.yAxis.title.text = "Number of Agreements";
            eColumnCharts.yAxis.min = 0;

            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "Secure Care", "Children's Home", "Residential School", "Crisis Support/Crisis Care", "To Be Confirmed" }; ;

                List<float?> data = new List<float?>() { };
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("SC")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("CH")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("RS")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("CS")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("TBC")).Count());

                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    name = "Service Type",
                    data = data,
                    color = "#058DC7"
                });
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Barchart Cost Breakdown by Placement Category
        protected ColumnCharts GetChartCostByPlacementCategory(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Cost breakdown by Placement Category";
            eColumnCharts.yAxis.title.text = "Cost (£)";
            eColumnCharts.yAxis.min = 0;

            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "Out of Authority", "Disability", "Commissioned", "Special Parenting" }; ;

                List<float?> data = new List<float?>() { };
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("OOA")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("CD")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("CO")).Select(x => x.actual_cost).Sum());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("SP")).Select(x => x.actual_cost).Sum());
               //data.Add((float?)listChildAgreements.Where(x => x.Service_Type.Equals("TBC")).Select(x => x.actual_cost).Sum());

                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    name = "Placement Category",
                    data = data,
                    color = "#058DC7"
                });
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Barchart Cost Breakdown by Placement Category
        protected ColumnCharts GetChartNoAgreementsByPlacementCategory(IList<ChildAgreements> listChildAgreements) // query from database and return charts object
        {
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Number of Agreement Breakdown by Placement Category";
            eColumnCharts.yAxis.title.text = "Number of Agreements";
            eColumnCharts.yAxis.min = 0;

            if (listChildAgreements != null && listChildAgreements.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "Out of Authority", "Disability", "Commissioned", "Special Parenting" }; ;

                List<float?> data = new List<float?>() { };
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("OOA")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("CD")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("CO")).Count());
                data.Add((float?)listChildAgreements.Where(x => x.Placement_Category.Equals("SP")).Count());

                eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                {
                    name = "Placement Category",
                    data = data,
                    color = "#058DC7"
                });
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