using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Entity.SchoolProfiles.Census.Entity;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using ClosedXML.Excel;
using Common.Logging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolProfiles.Controllers
{
    public class BaseSchoolProfilesController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseSchoolProfilesController));

        protected Dictionary<string, string> GetDicEhtnicBG(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from ethnicbackground");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicNationalIdenity(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from nationality");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicFreeSchoolMeal()
        {

            var dictionary = new Dictionary<string, string>();


            dictionary.Add("1", "Pupils registered as entitled to free school meals");

            return dictionary;

        }

        protected Dictionary<string, string> GetDicLookAfter()
        {

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("01", "Looked After At Home");

            dictionary.Add("02", "Looked After Away From Home");

            dictionary.Add("99", "N/A");

            return dictionary;

        }

        protected Dictionary<string, string> GetDicEnglisheLevel(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from englishlevel");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicStage(IGenericRepository2nd rpGeneric2nd, string sSchoolType)
        {
            dynamic listResult = null;

            switch (sSchoolType)
            {
                case "2":
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code LIKe 'P%'");
                    break;
                case "3":
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code in ('S1','S2','S3','S4','S5','S6')");
                    break;
                case "4":
                    listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from stage where Code in ('SP')");
                    break;
            }

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicAttendance(IGenericRepository2nd rpGeneric2nd)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select Code, Description from attendancecodes");

            var dictionary = new Dictionary<string, string>();

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        dictionary.Add(itemRow[0].ToString(), itemRow[1].ToString());
                    }
                }
            }
            return dictionary;

        }

        protected Dictionary<string, string> GetDicSIMDDecile()
        {

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("1", "1");
            dictionary.Add("2", "2");
            dictionary.Add("3", "3");
            dictionary.Add("4", "4");
            dictionary.Add("5", "5");
            dictionary.Add("6", "6");
            dictionary.Add("7", "7");
            dictionary.Add("8", "8");
            dictionary.Add("9", "9");
            dictionary.Add("10", "10");
            dictionary.Add("99", "99");
            return dictionary;

        }

        protected List<School> GetListSchool(IGenericRepository2nd rpGeneric2nd, string sSchoolType)
        {
            var listResult = rpGeneric2nd.FindByNativeSQL("Select sed_no, name, hmie_report, school_website,revisec_capacity,hmie_date,budget from View_Costcentre_nine where ClickNGo !=0 AND schoolType_id = " + sSchoolType);

            //var listResult = this.rpGeneric3nd.FindAll<Costcentre>().ToList();

            List<School> listdata = new List<School>();
            School temp = null;

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        temp = new School(itemRow[0].ToString(), itemRow[1].ToString());
                        temp.hmie_report = itemRow[2] != null ? itemRow[2].ToString() : "";
                        temp.website_link = itemRow[3] != null ? itemRow[3].ToString() : "";
                        temp.schoolCapacity = Convert.ToInt32(itemRow[4]);
                        temp.hmieLastReport = itemRow[5] != null ? Convert.ToDateTime(itemRow[5].ToString()) : (DateTime?)null;
                        temp.costperpupil = Convert.ToDouble(itemRow[6]);
                        listdata.Add(temp);
                    }
                }
            }
            return listdata.OrderBy(x => x.name).ToList();

        }

        protected List<Year> GetListYear()
        {
            List<Year> temp = new List<Year>();
            temp.Add(new Year("2011"));
            temp.Add(new Year("2012"));
            temp.Add(new Year("2013"));
            temp.Add(new Year("2014"));
            temp.Add(new Year("2015"));
            temp.Add(new Year("2016"));
            temp.Add(new Year("2017"));
            return temp;

        }

        protected virtual List<ViewObj> GetListViewObj(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string datatitle)
        {
            List<ViewObj> listResult = new List<ViewObj>();
            string query = "";
            switch (datatitle)
            {
                case "eal":
                    query = "Select * from summary_levelofenglish where schooltype = " + sSchoolType;
                    break;
                case "ethnicbackground":
                    query = "Select * from summary_ethnicbackground where schooltype = " + sSchoolType;
                    break;
                case "stage":
                    query = "Select * from summary_studentstage where schooltype = " + sSchoolType;
                    break;
                case "nationality":
                    query = "Select * from summary_nationality where schooltype = " + sSchoolType;
                    break;
                case "needtype":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentneed where schooltype = " + sSchoolType;
                    break;
                case "lookedafter":
                    //to calculate IEP CSP
                    query = "Select * from summary_studentlookedafter where schooltype = " + sSchoolType;
                    break;
                case "simd":
                    //to calculate IEP CSP
                    query = "Select * from summary_simd where schooltype = " + sSchoolType;
                    break;
                case "attendance":
                    //to calculate IEP CSP
                    query = "Select * from summary_attendance where schooltype = " + sSchoolType;
                    break;
                case "schoolroll":
                    //to calculate IEP CSP
                    query = "Select * from summary_schoolroll where schooltype = " + sSchoolType;
                    break;
            }

            var listtemp = rpGeneric2nd.FindByNativeSQL(query);
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    ViewObj temp = new ViewObj();
                    temp.year = new Year(itemrow[0].ToString());
                    temp.seedcode = itemrow[1].ToString();
                    temp.schooltype = itemrow[2].ToString();
                    temp.code = itemrow[3].ToString();
                    temp.count = Convert.ToInt32(itemrow[4].ToString());
                    listResult.Add(temp);
                }
            }


            return listResult;

        }

        //Historical NationalityData
        protected List<NationalityIdentity> GetHistoricalNationalityData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<NationalityIdentity> listNationalityIdentity = new List<NationalityIdentity>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            NationalityIdentity NationalityIdentity = new NationalityIdentity();

            Dictionary<string, string> DictNationality = GetDicNationalIdenity(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "nationality");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && !x.code.Equals("08")).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Sum(x => x.count),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList.OrderBy(x => x.Code).ToList();
                    listNationalityIdentity.Add(NationalityIdentity);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictNationality[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    NationalityIdentity = new NationalityIdentity();
                    NationalityIdentity.YearInfo = year;
                    NationalityIdentity.ListGenericSchoolData = groupedList.OrderBy(x => x.Code).ToList();
                    listNationalityIdentity.Add(NationalityIdentity);
                }

            }

            return listNationalityIdentity.OrderBy(x => x.YearInfo.year).ToList();
        }

        // NationalityIdentity Chart
        protected ColumnCharts GetChartNationalityIdentity(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {

            try
            {
                string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
                int indexColor = 0;
                var eColumnCharts = new ColumnCharts();
                eColumnCharts.SetDefault(false);
                eColumnCharts.title.text = "Nationality - Census " + selectedyear.academicyear;
                eColumnCharts.yAxis.title.text = "% of pupils";

                eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
                if (listSchool != null && listSchool.Count > 0)
                {
                    eColumnCharts.xAxis.categories = listSchool[0].NationalityIdentity.ListGenericSchoolData.Select(x => x.Name).ToList();
                    foreach (var eSchool in listSchool)
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSchool.SchoolName,
                            data = eSchool.NationalityIdentity.ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;
                    }
                }

                eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
                {
                    enabled = true,
                    filename = "export"
                };

                eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

                return eColumnCharts;

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in GetChartNationalityIdentity: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
                return null;
            }
  
        }

        //Historical EthnicBackground data
        protected List<Ethnicbackground> GetHistoricalEthnicData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<Ethnicbackground> listEthnicbackground = new List<Ethnicbackground>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            Ethnicbackground Ethnicbackground = new Ethnicbackground();

            Dictionary<string, string> DictNationality = GetDicEhtnicBG(rpGeneric2nd);

            foreach (var item in DictNationality)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "ethnicbackground");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictNationality[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList.OrderBy(x=>x.Code).ToList();
                    listEthnicbackground.Add(Ethnicbackground);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictNationality[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    Ethnicbackground = new Ethnicbackground();
                    Ethnicbackground.YearInfo = year;
                    Ethnicbackground.ListGenericSchoolData = groupedList.OrderBy(x => x.Code).ToList(); ;
                    listEthnicbackground.Add(Ethnicbackground);
                }

            }

            return listEthnicbackground.OrderBy(x => x.YearInfo.year).ToList(); ;
        }
        
        //Historical Level of English data
        protected List<LevelOfEnglish> GetHistoricalEALData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LevelOfEnglish> listLevelOfEnglish = new List<LevelOfEnglish>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LevelOfEnglish LevelOfEnglish = new LevelOfEnglish();

            Dictionary<string, string> DictEnglisheLevel = GetDicEnglisheLevel(rpGeneric2nd);

            foreach (var item in DictEnglisheLevel)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "eal");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictEnglisheLevel[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictEnglisheLevel[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LevelOfEnglish = new LevelOfEnglish();
                    LevelOfEnglish.YearInfo = year;
                    LevelOfEnglish.ListGenericSchoolData = groupedList.OrderBy(x => x.Name).ToList();
                    listLevelOfEnglish.Add(LevelOfEnglish);
                }

            }

            return listLevelOfEnglish.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        // Level of English Chart
        protected ColumnCharts GetChartLevelofEnglish(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Level of English - Census " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of pupils";
            //eColumnCharts.yAxis.min = 0;
            //eColumnCharts.yAxis.max = 100;
            //eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].LevelOfEnglish.ListGenericSchoolData.Select(x => x.Name).ToList();

                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.LevelOfEnglish.ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }
            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // Level of English Chart by catagories
        protected ColumnCharts GetChartLevelofEnglishbyCatagories(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Level of English - Census " + selectedyear.academicyear;
            eColumnCharts.yAxis.title.text = "% of pupils";
            //eColumnCharts.yAxis.min = 0;
            //eColumnCharts.yAxis.max = 100;
            //eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool.Select(x => x.SchoolName).ToList();
                foreach (var edata in listSchool[0].LevelOfEnglish.ListGenericSchoolData)
                {
                    temp = new List<GenericSchoolData>();
                    foreach (var eSchool in listSchool)
                    {
                        temp.AddRange(eSchool.LevelOfEnglish.ListGenericSchoolData);
                    }
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = edata.Name,
                        data = temp.Where(x => x.Name.Equals(edata.Name)).Select(x => (float?)Convert.ToDouble(x.Percent)).ToList()

                    });
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Historical Looked After data
        protected List<LookedAfter> GetHistoricalLookedAfterData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<LookedAfter> listLookedAfter = new List<LookedAfter>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            LookedAfter LookedAfter = new LookedAfter();

            Dictionary<string, string> DictLookedAfter = GetDicLookAfter();

            foreach (var item in DictLookedAfter)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "lookedafter");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictLookedAfter[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 2).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictLookedAfter[y.code],
                        count = y.count,
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 2).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    LookedAfter = new LookedAfter();
                    LookedAfter.YearInfo = year;
                    LookedAfter.GenericSchoolData = new GenericSchoolData()
                    {
                        Code = "1&2",
                        Name = "LookedafterPupils",
                        Value = "",
                        count = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.count).Sum(),
                        sum = total,
                        Percent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => x.Percent).Sum(),
                        sPercent = groupedList.Where(x => x.Code.Equals("01") || x.Code.Equals("02")).Select(x => Convert.ToDouble(x.sPercent)).Sum().ToString()
                    };
                    LookedAfter.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => x.Code).ToList(); ;
                    listLookedAfter.Add(LookedAfter);
                }

            }

            return listLookedAfter.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        // Looked After Chart
        protected ColumnCharts GetChartLookedAfter(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Looked After Children ";
            eColumnCharts.yAxis.title.text = "% of pupils Looked After";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].listLookedAfter.Select(x => x.YearInfo.academicyear).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.listLookedAfter.Select(x => (float?)Convert.ToDouble(x.ListGenericSchoolData.Select(y => Convert.ToDouble(y.sPercent)).Sum())).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };


            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Historical SIMD data
        protected List<SPSIMD> GetHistoricalSIMDData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<SPSIMD> listSIMD = new List<SPSIMD>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPSIMD SPSIMD = new SPSIMD();

            Dictionary<string, string> DictSIMD = GetDicSIMDDecile();

            foreach (var item in DictSIMD)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "simd");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictSIMD[y.Key.ToString()],
                            count = y.Select(a => a.count).Sum(),
                            sum = total,
                            Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                            sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString(),
                            sCount = NumberFormatHelper.FormatNumber(y.Select(a => a.count).Sum(), 0).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    if (listresult != null && listresult.Count > 0)
                    {
                        int total = listresult.Select(s => s.count).Sum();
                        var groupedList = listresult.Select(y => new GenericSchoolData
                        {
                            Code = y.code,
                            Name = DictSIMD[y.code],
                            count = y.count,
                            sum = total,
                            Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                            sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString(),
                            sCount = NumberFormatHelper.FormatNumber(y.count, 0).ToString()
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                        SPSIMD = new SPSIMD();
                        SPSIMD.YearInfo = year;
                        SPSIMD.ListGenericSchoolData = groupedList.Where(x => !x.Code.Equals("99")).OrderBy(x => Convert.ToInt16(x.Code)).ToList();
                        listSIMD.Add(SPSIMD);
                    }

                }

            }

            return listSIMD.OrderBy(x => x.YearInfo.year).ToList();
        }

        // SIMD Chart
        protected ColumnCharts GetChartSIMDDecile(List<BaseSPDataModel> listSchool, Year selectedyear) // query from database and return charts object
        {

            try
            {
                //SIMD2016 matched with only pupil census 2016 and 2017 only 
                //Query for historical data will return the latest census (2017)
                int yeardata = Convert.ToInt32(selectedyear.year) < 2016 ? 2017 : Convert.ToInt32(selectedyear.year);
                string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
                int indexColor = 0;
                var eColumnCharts = new ColumnCharts();
                eColumnCharts.SetDefault(false);
                eColumnCharts.title.text = "Scottish Index of Multiple Deprivation";
                eColumnCharts.subtitle.text = "% of pupils in Each Decile (Census " + yeardata + ")";
                eColumnCharts.yAxis.title.text = "% of pupils";

                eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
                if (listSchool != null && listSchool.Count > 0)
                {
                    eColumnCharts.xAxis.categories = listSchool[0].SIMD.ListGenericSchoolData.Select(x => "Decile " + x.Name).ToList();
                    foreach (var eSchool in listSchool)
                    {
                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSchool.SchoolName,
                            data = eSchool.SIMD.ListGenericSchoolData.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;
                    }
                }

                eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
                {
                    enabled = true,
                    filename = "export"
                };

                eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

                return eColumnCharts;

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in GetChartSIMDDecile: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
                return null;
            }

        }

        //Historical Attendance Data
        protected List<SPAttendance> GetHistoricalAttendanceData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPAttendance> listAttendance = new List<SPAttendance>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            SPAttendance SPAttendance = new SPAttendance();
            int possibledays = 0;

            Dictionary<string, string> DictAttendance = GetDicAttendance(rpGeneric2nd);

            foreach (var item in DictAttendance)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "attendance");

            if (school.seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));

                        if (Convert.ToInt16(year.year) < 2014)
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();

                        }
                        else
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum();


                        }
                        
                        
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12") || x.Code.Equals("13")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12/13",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32/33",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12/13",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32/33",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(school.seedcode)).ToList();
                    if (listresult.Count > 0)
                    {
                        tempdata = new List<GenericSchoolData>();
                        var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                        {
                            Code = y.Key.ToString(),
                            Name = DictAttendance[y.Key.ToString()],
                            count = y.Sum(x => x.count)
                        }).ToList();
                        groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));

                        if (Convert.ToInt16(year.year) < 2014)
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum() - groupedList.Where(x => x.Code.Equals("02")).Select(x => x.count).Sum();

                        }
                        else
                        {
                            possibledays = groupedList.Where(x => x.Code.Equals("01")).Select(x => x.count).Sum();


                        }
                        int sum = groupedList.Where(x => x.Code.Equals("10") || x.Code.Equals("11") || x.Code.Equals("12") || x.Code.Equals("13")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12/13",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });
                        int sumUnauthorised = groupedList.Where(x => x.Code.StartsWith("3")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32/33",
                            count = sumUnauthorised,
                            sum = possibledays,
                            Percent = sumUnauthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumUnauthorised * 100.00F / possibledays), 1).ToString()
                        });

                        int sumAuthorised = groupedList.Where(x => x.Code.StartsWith("2")).Select(x => x.count).Sum();

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "20/21/22/23/24",
                            count = sumAuthorised,
                            sum = possibledays,
                            Percent = sumAuthorised * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sumAuthorised * 100.00F / possibledays), 1).ToString()
                        });

                        sum = groupedList.Where(x => x.Code.Equals("40")).Select(x => x.count).Sum();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = sum,
                            sum = possibledays,
                            Percent = sum * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber((sum * 100.00F / possibledays), 1).ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = sumAuthorised + sumUnauthorised,
                            sum = possibledays,
                            Percent = (sumAuthorised + sumUnauthorised) * 100.00F / possibledays,
                            sPercent = NumberFormatHelper.FormatNumber(((sumAuthorised + sumUnauthorised) * 100.00F / possibledays), 1).ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);

                    }
                    else
                    {
                        tempdata = new List<GenericSchoolData>();
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Attendance",
                            Code = "10/11/12/13",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Unauthorised Absence",
                            Code = "30/31/32/33",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Authorised Absence",
                            Code = "20/21/22/23/24",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Absense due to Exclusion",
                            Code = "40",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });

                        tempdata.Add(new GenericSchoolData()
                        {
                            Name = "Total Absence",
                            Code = "Authorised + Unauthorised",
                            count = 0,
                            sum = 0,
                            Percent = 0.0F,
                            sPercent = NumberFormatHelper.FormatNumber(null, 1, "n/a").ToString()
                        });
                        SPAttendance = new SPAttendance();
                        SPAttendance.YearInfo = year;
                        SPAttendance.ListGenericSchoolData = tempdata;
                        listAttendance.Add(SPAttendance);
                    }

                }

            }

            return listAttendance.OrderBy(x => x.YearInfo.year).ToList();
        }

        // Attendance Chart
        protected SplineCharts GetChartAttendance(List<BaseSPDataModel> listSchool, string ssubject) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = ssubject;
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            //finding subject index to query data from list
            string[] arraySubject = { "Attendance", "Unauthorised Absence", "Authorised Absence", "Absense due to Exclusion", "Total Absence" };
            int indexsubject = Array.FindIndex(arraySubject, item => item.Equals(ssubject));

            if (listSchool != null && listSchool.Count > 0)
            {

                foreach (var eSchool in listSchool)
                {
                    var listSeries = eSchool.listAttendance.Select(x => x.ListGenericSchoolData[indexsubject].sPercent.Equals("n/a")? null : (float?)float.Parse(x.ListGenericSchoolData[indexsubject].sPercent)).ToList();
                    //Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList()
                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName,
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor],
                        lineWidth = 2,
                        data = listSeries,
                        visible = true
                    });
                    indexColor++;
                }

                eSplineCharts.xAxis.categories = listSchool[0].listAttendance.Select(x => x.YearInfo.year).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = " % " + ssubject };
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        //Historical Exclusion Data
        protected List<SPExclusion> GetHistoricalExclusionData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, School school, List<Year> listyear)
        {
            List<SPExclusion> listExclusion = new List<SPExclusion>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>() { new GenericSchoolData("0", "Temporary Exclusions"), new GenericSchoolData("1", "Removed From Register") };
            SPExclusion SPExclusion = new SPExclusion();
            GenericSchoolData tempobj = new GenericSchoolData();
            string queryExclusion, querySchoolRoll = "";
            int schoolroll = 0;
            string yearNodata = "2017";

            foreach (Year year in listyear)
            {
                if (school.seedcode.Equals("1002"))
                {
                    queryExclusion = "SELECT Year, 1002, Code, sum(Count), sum(Sum)  FROM summary_exclusion where  SchoolType=" + sSchoolType + " and year = " + year.year + " group by Year, Code";
                    querySchoolRoll = "Select Year, sum(Count) from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year;
                }
                else
                {
                    queryExclusion = "SELECT Year, Seedcode, Code, sum(Count), sum(Sum)  FROM summary_exclusion where  SchoolType= " + sSchoolType + "  and year = " + year.year + " and seedcode = " + school.seedcode + " group by Year, Code";
                    querySchoolRoll = "Select Year, Count from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year + " and seedcode = " + school.seedcode;
                }

                var listResult = rpGeneric2nd.FindByNativeSQL(queryExclusion);
                if (listResult.Count > 0)
                {
                    tempdata = new List<GenericSchoolData>();
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            tempobj = new GenericSchoolData(itemRow[2].ToString(), itemRow[2].ToString().Equals("0") ? "Temporary Exclusions" : "Removed From Register");
                            tempobj.count = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sCount = NumberFormatHelper.FormatNumber(tempobj.count, 0).ToString();
                            tempobj.sum = Convert.ToInt16(itemRow[4].ToString());
                            tempobj.Percent = Convert.ToInt16(itemRow[3].ToString());
                            tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                            tempdata.Add(tempobj);
                        }
                    }
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL(querySchoolRoll);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                schoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }

                    tempdata.AddRange(foo.Where(x => tempdata.All(p1 => !p1.Code.Equals(x.Code))));
                    SPExclusion = new SPExclusion();
                    SPExclusion.YearInfo = new Year(year.year);
                    tempobj = new GenericSchoolData("2", "Number of days per 1000 pupils lost to exclusions");
                    tempobj.count = tempdata.Sum(x => x.sum);  //Sum length of exclusion
                    tempobj.sCount = NumberFormatHelper.FormatNumber(tempobj.count, 0).ToString();
                    tempobj.sum = schoolroll;   //school Roll
                    tempobj.Percent = schoolroll == 0 ? 0.00F : tempobj.count / 2.0F / schoolroll * 1000.0F;
                    tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                    //tempdata.Add(tempobj);
                    SPExclusion.ListGenericSchoolData = new List<GenericSchoolData>() { tempdata.Where(x => x.Code.Equals("0")).First(), tempdata.Where(x => x.Code.Equals("1")).First(), tempobj };
                    listExclusion.Add(SPExclusion);
                }
                else {

                    tempdata = new List<GenericSchoolData>();
                    tempdata.Add(new GenericSchoolData()
                    {
                        Name = "Temporary Exclusions",
                        Code = "0",
                        count = 0,
                        sCount = year.year.Equals(yearNodata)? "n/a":"0",
                        sum = 0,
                        Percent = year.year.Equals(yearNodata) ? null : (float?)0.0,
                        sPercent = year.year.Equals(yearNodata) ? "n/a" : "0.0",
                    });
                    tempdata.Add(new GenericSchoolData()
                    {
                        Name = "Removed From Register",
                        Code = "1",
                        count = 0,
                        sCount = year.year.Equals(yearNodata) ? "n/a" : "0",
                        sum = 0,
                        Percent = year.year.Equals(yearNodata) ? null : (float?)0.0,
                        sPercent = year.year.Equals(yearNodata) ? "n/a" : "0.0",
                    });
                    tempdata.Add(new GenericSchoolData()
                    {
                        Name = "Number of days per 1000 pupils lost to exclusions",
                        Code = "2",
                        count = 0,
                        sCount = year.year.Equals(yearNodata) ? "n/a" : "0",
                        sum = 0,
                        Percent = year.year.Equals(yearNodata) ? null : (float?)0.0,
                        sPercent = year.year.Equals(yearNodata) ? "n/a" : "0.0",
                    });

                    SPExclusion = new SPExclusion();
                    SPExclusion.YearInfo = new Year(year.year);
                    SPExclusion.ListGenericSchoolData = tempdata;
                    listExclusion.Add(SPExclusion);
                }

            }



            return listExclusion.OrderBy(x => x.YearInfo.year).ToList();
        }

        // Exclusion Chart
        protected SplineCharts GetChartExclusion(List<BaseSPDataModel> listSchool, string dataset) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = dataset;
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();

            //finding subject index to query data from list
            string[] arraySubject = { "Number of Temporary Exclusions", "Number of Removals from the Register", "Number of Days Lost Per 1000 Pupils Through Exclusions" };
            int indexsubject = Array.FindIndex(arraySubject, item => item.Equals(dataset));

            if (listSchool != null && listSchool.Count > 0)
            {

                foreach (var eSchool in listSchool)
                {
                    var listSeries = eSchool.listExclusion.Select(x => (float?)x.ListGenericSchoolData[indexsubject].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName,
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor],
                        lineWidth = 2,
                        data = listSeries,
                        visible = true
                    });
                    indexColor++;
                }

                eSplineCharts.xAxis.categories = listSchool[0].listExclusion.Select(x => x.YearInfo.year).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = dataset };
                //eSplineCharts.yAxis.min = 0;
                //eSplineCharts.yAxis.max = 10;
                //eSplineCharts.yAxis.tickInterval = 1;
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        //Historical StudentNeed
        protected List<StudentNeed> GetHistoricalStudentNeed(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            StudentNeed StudentNeed = new StudentNeed(); ;
            List<StudentNeed> listStudentNeed = new List<StudentNeed>();
            int yschoolroll = 0;

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "needtype");
            
            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL("Select Year, sum(Count) from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                yschoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }

                    StudentNeed = new StudentNeed();
                    StudentNeed.year = year;
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("06")).Select(x => x.count).Sum();
                    StudentNeed.ChildPlan = new GenericSchoolData()
                    {
                        Code = "06",
                        Name = "ChildPlan",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listResultSchoolRoll = rpGeneric2nd.FindByNativeSQL("Select Year, Count from summary_schoolroll where  SchoolType=" + sSchoolType + " and year = " + year.year + " and seedcode = " + seedcode);
                    if (listResultSchoolRoll != null)
                    {
                        foreach (var itemRow in listResultSchoolRoll)
                        {
                            if (itemRow != null)
                            {
                                yschoolroll = Convert.ToInt16(itemRow[1].ToString());
                            }
                        }
                    }
                    StudentNeed = new StudentNeed();
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    StudentNeed.year = year;
                    int totalcount = listresult.Where(x => x.code.Equals("02")).Select(x => x.count).Sum();
                    StudentNeed.IEP = new GenericSchoolData()
                    {
                        Code = "02",
                        Name = "IEP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("01")).Select(x => x.count).Sum();
                    StudentNeed.CSP = new GenericSchoolData()
                    {
                        Code = "01",
                        Name = "CSP",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    totalcount = listresult.Where(x => x.code.Equals("06")).Select(x => x.count).Sum();
                    StudentNeed.ChildPlan = new GenericSchoolData()
                    {
                        Code = "06",
                        Name = "ChildPlan",
                        count = totalcount,
                        sCount = NumberFormatHelper.FormatNumber(totalcount, 0).ToString(),
                        sum = yschoolroll,
                        Percent = yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.0F,
                        sPercent = NumberFormatHelper.FormatNumber((yschoolroll != 0 ? (totalcount * 100.00F / yschoolroll) : 0.00F), 1).ToString()
                    };
                    listStudentNeed.Add(StudentNeed);
                }

            }

            return listStudentNeed.OrderBy(x => x.year.year).ToList();
        }

        // StudentNeed Chart
        protected ColumnCharts GetChartStudentNeedIEP(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Pupils with an IEP ";
            eColumnCharts.yAxis.title.text = "% of pupils with an IEP";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].listStudentNeed.Select(x => x.year.academicyear).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.listStudentNeed.Select(x => (float?)Convert.ToDouble(x.IEP.sPercent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        // StudentNeed Chart
        protected ColumnCharts GetChartStudentNeedCSP(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Pupils with a CSP ";
            eColumnCharts.yAxis.title.text = "% of pupils with a CSP";

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchool[0].listStudentNeed.Select(x => x.year.academicyear).ToList();
                foreach (var eSchool in listSchool)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.listStudentNeed.Select(x => (float?)Convert.ToDouble(x.CSP.sPercent)).ToList(),
                        color = eSchool.SeedCode == "1002" ? "#058DC7" : colors[indexColor]
                    });
                    indexColor++;
                }
            }
            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }

        //Get SchoolRoll data
        protected SPSchoolRollForecast GetSchoolRollForecastData(IGenericRepository2nd rpGeneric2nd, School school)
        {

            SPSchoolRollForecast SchoolRollForecast = new SPSchoolRollForecast();
            List<GenericSchoolData> tempdataActualnumber = new List<GenericSchoolData>();
            List<GenericSchoolData> tempdataForecastnumber = new List<GenericSchoolData>();

            //get actual number 
            var listResult = rpGeneric2nd.FindByNativeSQL("Select * from schoolrollforecast where seedcode = " + school.seedcode);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        for (int i = 3; i <= 17; i++)
                        {
                            if (i <= 12)
                            {
                                tempdataActualnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear, NumberFormatHelper.ConvertObjectToFloat(itemRow[i])));
                                tempdataForecastnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear.ToString(), 0));

                            }
                            else
                            {
                                tempdataForecastnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear, NumberFormatHelper.ConvertObjectToFloat(itemRow[i])));
                                tempdataActualnumber.Add(new GenericSchoolData(new Year((i + 2005).ToString()).academicyear, 0));

                            }
                        }
                    }
                }
            }

            SchoolRollForecast.ListActualSchoolRoll = tempdataActualnumber;
            SchoolRollForecast.ListForecastSchoolRoll = tempdataForecastnumber;

            return SchoolRollForecast;
        }
       
        // SchoolRoll Forecast Chart
        protected SplineCharts GetChartSchoolRollForecast(List<BaseSPDataModel> listSchool) // query from database and return charts object
        {

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = " School Roll ";
            eSplineCharts.yAxis.title.text = "Number of Pupils";
            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();
            //finding subject index to query data from list

            if (listSchool != null && listSchool.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchool[0].SchoolRollForecast.ListActualSchoolRoll.Select(x => x.Code).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "Number of Pupils" };

                foreach (var eSchool in listSchool)
                {
                    if (!eSchool.SeedCode.Equals("1002"))
                    {
                        var listSeriesActual = eSchool.SchoolRollForecast.ListActualSchoolRoll.Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = eSchool.SchoolName,
                            color = "#24CBE5",
                            lineWidth = 2,
                            data = listSeriesActual,
                            visible = true
                        });


                        var listSeriesForeCast = eSchool.SchoolRollForecast.ListForecastSchoolRoll.Select(x => float.Parse(x.sPercent) == 0 ? null : (float?)float.Parse(x.sPercent)).ToList();

                        eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                        {
                            name = "Forecast (2015 based)",
                            color = "#f969e8",
                            lineWidth = 2,
                            data = listSeriesForeCast,
                            visible = true
                        });

                    }


                }
            }

            eSplineCharts.plotOptions.spline.marker = new ACCDataStore.Entity.RenderObject.Charts.Generic.marker()
            {
                enabled = true
            };

            eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };
            //eSplineCharts.options.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        protected ColumnCharts GetChartCfETimelinebySIMDData(List<BaseSPDataModel> listSchool, string stage, string subject) // query from database and return charts object
        {
            List<BaseSchoolProfile> temp = new List<BaseSchoolProfile>();
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            string gtype = "column";
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = listSchool[0].SchoolName;
            eColumnCharts.subtitle.text = subject + ": " +stage+" Levels by SIMD 2016 Quintiles ";
            eColumnCharts.yAxis.title.text = "% achieving expected CfE Levels";
            eColumnCharts.yAxis.max = 100;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchool != null && listSchool.Count > 0)
            {
                eColumnCharts.xAxis.categories = new List<string>() { "SIMD Q1 - Most Deprived", "SIMD Q2", "SIMD Q3", "SIMD Q4", "SIMD Q5 - Least Deprived" };
                foreach (var eSchool in listSchool)
                {
                    indexColor = 0;

                    if (!eSchool.SeedCode.Equals("1002"))
                    {

                        if (stage.Equals("P1"))
                        {
                            temp = eSchool.listSPCfElevel.Select(x => x.getP1EarlybySubjectAndSIMD(subject)).ToList();
                        }
                        else if (stage.Equals("P4"))
                        {
                            temp = eSchool.listSPCfElevel.Select(x => x.getP4FirstLevelbySubjectAndSIMD(subject)).ToList();
                        }
                        else
                        {
                            temp = eSchool.listSPCfElevel.Select(x => x.getP7SecondLevelbySubjectAndSIMD(subject)).ToList();
                        }

                        foreach (BaseSchoolProfile tempObj in temp)
                        {
                            eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                            {
                                type = gtype,
                                name = tempObj.YearInfo.academicyear,
                                data = tempObj.ListGenericSchoolData.Select(x=>(float?)Convert.ToDouble(x.Percent)).ToList(), 
                                //color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                                color = colors[indexColor]
                            });
                            indexColor++;
                        
                        }
                        
                    }
                }
            }

            eColumnCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eColumnCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eColumnCharts;
        }



        //Historical StudentStage data
        protected List<StudentStage> GetHistoricalStudentStageData(IGenericRepository2nd rpGeneric2nd, string sSchoolType, string seedcode, List<Year> listyear)
        {
            List<StudentStage> listStudentStage = new List<StudentStage>();
            List<GenericSchoolData> tempdata = new List<GenericSchoolData>();
            List<GenericSchoolData> foo = new List<GenericSchoolData>();
            StudentStage StudentStage = new StudentStage();

            Dictionary<string, string> DictStage = GetDicStage(rpGeneric2nd, sSchoolType);

            foreach (var item in DictStage)
            {

                foo.Add(new GenericSchoolData(item.Key, item.Value));

            }

            List<ViewObj> listViewObj = GetListViewObj(rpGeneric2nd, sSchoolType, "stage");

            if (seedcode.Equals("1002"))
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.GroupBy(x => x.code).Select(y => new GenericSchoolData
                    {
                        Code = y.Key.ToString(),
                        Name = DictStage[y.Key.ToString()],
                        count = y.Select(a => a.count).Sum(),
                        sCount = NumberFormatHelper.FormatNumber(y.Select(a => a.count).Sum(), 0).ToString(),
                        sum = total,
                        Percent = total != 0 ? (y.Select(a => a.count).Sum() * 100.00F / total) : 0.00F,
                        sPercent = total != 0 ? NumberFormatHelper.FormatNumber((y.Select(a => a.count).Sum() * 100.00F / total), 1).ToString() : NumberFormatHelper.FormatNumber((float)0.00, 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    StudentStage = new StudentStage();
                    StudentStage.YearInfo = year;
                    StudentStage.ListGenericSchoolData = groupedList;
                    StudentStage.totalschoolroll = total;
                    StudentStage.stotalschoolroll = NumberFormatHelper.FormatNumber(total, 0).ToString();
                    listStudentStage.Add(StudentStage);
                }
            }
            else
            {
                foreach (Year year in listyear)
                {
                    var listresult = listViewObj.Where(x => x.year.year.Equals(year.year) && x.seedcode.Equals(seedcode)).ToList();
                    int total = listresult.Select(s => s.count).Sum();
                    var groupedList = listresult.Select(y => new GenericSchoolData
                    {
                        Code = y.code,
                        Name = DictStage[y.code],
                        count = y.count,
                        sCount = NumberFormatHelper.FormatNumber(y.count, 0).ToString(),
                        sum = total,
                        Percent = total != 0 ? (y.count * 100.00F / total) : 0.00F,
                        sPercent = NumberFormatHelper.FormatNumber((total != 0 ? (y.count * 100.00F / total) : 0.00F), 1).ToString()
                    }).ToList();
                    groupedList.AddRange(foo.Where(x => groupedList.All(p1 => !p1.Code.Equals(x.Code))));
                    StudentStage = new StudentStage();
                    StudentStage.YearInfo = year;
                    StudentStage.ListGenericSchoolData = groupedList;
                    StudentStage.totalschoolroll = total;
                    StudentStage.stotalschoolroll = NumberFormatHelper.FormatNumber(total, 0).ToString();
                    listStudentStage.Add(StudentStage);
                }

            }

            return listStudentStage.OrderBy(x => x.YearInfo.year).ToList();
        }
        //Historical Free School Meal Registered data
        protected List<FreeSchoolMeal> GetHistoricalFSMData(IGenericRepository2nd rpGeneric2nd, string seedcode, List<Year> listyear, string schooltype)
        {
            List<FreeSchoolMeal> listFSM = new List<FreeSchoolMeal>();
            FreeSchoolMeal SPFSM = new FreeSchoolMeal();


            foreach (Year year in listyear)
            {
                SPFSM = new FreeSchoolMeal();
                var listResult = rpGeneric2nd.FindByNativeSQL("SELECT year,studentStage, stdroll, fsm, count FROM summary_fsm2 where schooltype= " + schooltype + " and year = " + year.year + " and seedcode = " + seedcode);
                if (listResult != null && listResult.Count >0 )
                {
                    foreach (var itemRow in listResult)
                    {
                        if (itemRow != null)
                        {
                            SPFSM = new FreeSchoolMeal();
                            GenericSchoolData tempdata = new GenericSchoolData();
                            tempdata.Code = itemRow[1].ToString();
                            tempdata.count = itemRow[3] == null ? 0 : Convert.ToInt32(itemRow[3].ToString());
                            tempdata.sum = itemRow[2]== null ? 0 : Convert.ToInt32(itemRow[2].ToString());
                            tempdata.Percent = itemRow[4] == null ? 0 : (float)Convert.ToDouble(itemRow[4].ToString());
                            tempdata.sPercent = itemRow[4] == null ? "n/a" : NumberFormatHelper.FormatNumber(Convert.ToDouble(itemRow[4].ToString()), 1).ToString();

                            SPFSM.year = year;
                            SPFSM.GenericSchoolData = tempdata;

                        }
                        //else {
                        //    SPFSM = new FreeSchoolMeal();
                        //    GenericSchoolData tempdata = new GenericSchoolData();
                        //    tempdata.Code = "P4-P7";
                        //    tempdata.Percent = 0.0F;
                        //    tempdata.sPercent = "n/a";

                        //    SPFSM.year = year;
                        //    SPFSM.GenericSchoolData = tempdata;
                        
                        //}
                        listFSM.Add(SPFSM);
                    }

                }
                else {
                    SPFSM = new FreeSchoolMeal();
                    GenericSchoolData tempdata = new GenericSchoolData();
                    tempdata.Code = schooltype.Equals("2") ? "P4-P7" : schooltype.Equals("3")? "S1-S6" :schooltype.Equals("4")? "SP": "All";
                    tempdata.Percent = 0.0F;
                    tempdata.sPercent = "n/a";
                    SPFSM.year = year;
                    SPFSM.GenericSchoolData = tempdata;
                    listFSM.Add(SPFSM);
                
                }

            }

            //if (listResult != null)
            //{
               
            //    foreach (var itemRow in listResult)
            //    {
            //        if (itemRow != null)
            //        {
            //            SPFSM = new FreeSchoolMeal();
            //            GenericSchoolData tempdata = new GenericSchoolData();
            //            tempdata.Code = itemRow[1].ToString();
            //            tempdata.count = itemRow[3].ToString() == null ? 0 : Convert.ToInt16(itemRow[3].ToString());
            //            tempdata.sum = itemRow[2].ToString() == null ? 0 : Convert.ToInt16(itemRow[2].ToString());
            //            tempdata.Percent = itemRow[4].ToString() == null ? 0 : (float)Convert.ToDouble(itemRow[4].ToString());
            //            tempdata.sPercent = itemRow[4].ToString() == null ? "n/a" : NumberFormatHelper.FormatNumber(Convert.ToDouble(itemRow[4].ToString()), 1).ToString();

            //            SPFSM.year = new Year(itemRow[0].ToString());
            //            SPFSM.GenericSchoolData = tempdata;
            //            //SPFSM.GenericSchoolData = new GenericSchoolData()
            //            //{
            //            //    Code = itemRow[1].ToString(),
            //            //    count = itemRow[3].ToString() == null ? 0 : Convert.ToInt16(itemRow[3].ToString()),
            //            //    Value = "",
            //            //    sum = itemRow[2].ToString()== null ? 0 : Convert.ToInt16(itemRow[2].ToString()),
            //            //    Name = "Free School Meals Registered",
            //            //    Percent = itemRow[4].ToString() == null ? 0 : Convert.ToInt16(itemRow[4].ToString()),
            //            //    sPercent = NumberFormatHelper.FormatNumber(itemRow[4].ToString() == null ? 0 : Convert.ToDouble(itemRow[4].ToString()), 1).ToString()
            //            //};
            //        }
            //        listFSM.Add(SPFSM);

            //    }
            
            //}


            


            return listFSM.OrderBy(x => x.year.year).ToList();
        }

        protected string[] ExportDataToXLSX(IGenericRepository2nd rpGeneric2nd, List<School> listSeedCode, string sYear, string tablename)
        {
            var listYear = GetListYear();
            var dtResult = new DataTable();

            foreach (School school in listSeedCode)
            {
                switch (tablename)
                {
                    case "nationality":
                        List<NationalityIdentity> listNationalityIdentity = GetHistoricalNationalityData(rpGeneric2nd, "2", school.seedcode, listYear);
                        //dtResult = ProcessExportDataFormat(listNationalityIdentity);
                        break;
                }
            }

            var workbook = new XLWorkbook();
            var dtNow = DateTime.Now.ToString("yyyyMMdd_HHmmss");


            var sWorksheetName = "test";
            var worksheet = workbook.Worksheets.Add(sWorksheetName);

            worksheet.Cell(1, 1).Value = "Test";
            //worksheet.Cell(2, 1).Value = eDeviceList.NameWithDesc;
            //worksheet.Cell(3, 1).Value = sDateTimeFrom + " - " + sDateTimeTo + " " + "[" + GetEnumDescription((AGGREGATE_TYPE)nAggregateType) + "]";
            //worksheet.Cell(4, 1).InsertTable(dtResult);

            //var nLastCellColumn = dtResult.Columns.Count;
            //worksheet.Range(1, 1, 1, nLastCellColumn).Merge();
            //worksheet.Range(2, 1, 2, nLastCellColumn).Merge();
            //worksheet.Range(3, 1, 3, nLastCellColumn).Merge();
            //worksheet.Range(4, 1, 4, nLastCellColumn).Merge();

            //worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

           // worksheet.Tables.FirstOrDefault().ShowAutoFilter = false;
            worksheet.Rows().AdjustToContents();
            worksheet.Columns().AdjustToContents();

            var sFileName = "test.xlsx";
            var sFilePath = HttpContext.Server.MapPath(@"~\download\" + sFileName);
            workbook.SaveAs(sFilePath);

            return new string[] { "download/" + sFileName, sFileName };
        }

        private DataTable ProcessExportDataFormat(IList<object[]> listData)
        {
            var dataResult = new DataTable();
            //try
            //{
            //    if (listData != null && listData.Count > 0)
            //    {
            //        var dicDIColumn = new Dictionary<int, DeviceParams>();
            //        if (listDeviceParams != null && listDeviceParams.Count > 0)
            //        {
            //            var dicDeviceParams = Session["SessionDicDeviceParams"] as Dictionary<string, DeviceParams>;
            //            for (var i = 0; i < listDeviceParams.Count + 1; i++)
            //            {
            //                dataResult.Columns.Add(i == 0 ? "Date Time" : listDeviceParams[i - 1].FldName);
            //            }
            //        }
            //        else
            //        {
            //            foreach (var itemColumn in listData[0])
            //            {
            //                dataResult.Columns.Add();
            //            }
            //        }
            //        foreach (var oRow in listData)
            //        {
            //            // date time
            //            oRow[0] = ProcessDataFormatDateTime(Convert.ToDateTime(oRow[0]), nAggregateType, eUserDataTableHdr);
            //            dataResult.Rows.Add(oRow);
            //        }
            //    }
            //    else
            //    {
            //        if (listDeviceParams != null && listDeviceParams.Count > 0)
            //        {
            //            for (var i = 0; i < listDeviceParams.Count + 1; i++)
            //            {
            //                dataResult.Columns.Add(i == 0 ? "Date Time" : listDeviceParams[i - 1].FldName);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex, ex.Message);
            //}
            return dataResult;
        }

        //Historical InCAS data
        protected List<SPCfElevel> GetHistoricalSecondaryCfeLevelData(IGenericRepository2nd rpGeneric2nd, string seedcode, string schooltype)
        {
            List<SPCfElevel> listSPCfelevel = new List<SPCfElevel>();
            SPCfElevel tSPCfelevel = new SPCfElevel();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();

            //get actual number 
            string query = "Select * from summary_secondary_cfe where seedcode =" + seedcode + " and SchoolType = " + schooltype;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tSPCfelevel = new SPCfElevel();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());
                        tSPCfelevel.seedcode = itemRow[1].ToString();

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        tSPCfelevel.ListThirdlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        tSPCfelevel.ListForthlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        tSPCfelevel.SIMDQ1_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        tSPCfelevel.SIMDQ1_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        tSPCfelevel.SIMDQ2_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tSPCfelevel.SIMDQ2_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[33])));
                        tSPCfelevel.SIMDQ3_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[34])));
                        tSPCfelevel.SIMDQ3_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[35])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[37])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[39])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[41])));
                        tSPCfelevel.SIMDQ4_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[36])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[38])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[40])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[42])));
                        tSPCfelevel.SIMDQ4_4level = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[43])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[45])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[47])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[49])));
                        tSPCfelevel.SIMDQ5_3Dlevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[44])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[46])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[48])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[50])));
                        tSPCfelevel.SIMDQ5_4level = temp;

                        listSPCfelevel.Add(tSPCfelevel);
                    }
                }
            }



            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
        }

        //Historical InCAS data
        protected List<SPCfElevel> GetHistoricalPrimaryCfeLevelData(IGenericRepository2nd rpGeneric2nd, string seedcode, string schooltype)
        {
            List<SPCfElevel> listSPCfelevel = new List<SPCfElevel>();
            SPCfElevel tSPCfelevel = new SPCfElevel();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();

            //get actual number 
            string query = "Select * from summary_primary_cfe where seedcode =" + seedcode + " and SchoolType = " + schooltype;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tSPCfelevel = new SPCfElevel();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());
                        tSPCfelevel.seedcode = itemRow[1].ToString();
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        tSPCfelevel.P1EarlyLevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        tSPCfelevel.P4FirstLevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        tSPCfelevel.P7SecondLevel = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        tSPCfelevel.P1EarlyLevelQ1 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        tSPCfelevel.P1EarlyLevelQ2 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tSPCfelevel.P1EarlyLevelQ3 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        tSPCfelevel.P1EarlyLevelQ4 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[33])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[34])));
                        tSPCfelevel.P1EarlyLevelQ5 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[35])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[36])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[37])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[38])));
                        tSPCfelevel.P4FirstLevelQ1 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[39])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[40])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[41])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[42])));
                        tSPCfelevel.P4FirstLevelQ2 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[43])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[44])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[45])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[46])));
                        tSPCfelevel.P4FirstLevelQ3 = temp;


                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[47])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[48])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[49])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[50])));
                        tSPCfelevel.P4FirstLevelQ4 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[51])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[52])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[53])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[54])));
                        tSPCfelevel.P4FirstLevelQ5 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[55])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[56])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[57])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[58])));
                        tSPCfelevel.P7SecondLevelQ1 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[59])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[60])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[61])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[62])));
                        tSPCfelevel.P7SecondLevelQ2 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[63])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[64])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[65])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[66])));
                        tSPCfelevel.P7SecondLevelQ3 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[67])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[68])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[69])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[70])));
                        tSPCfelevel.P7SecondLevelQ4 = temp;

                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("Reading", NumberFormatHelper.ConvertObjectToFloat(itemRow[71])));
                        temp.Add(new GenericSchoolData("Writing", NumberFormatHelper.ConvertObjectToFloat(itemRow[72])));
                        temp.Add(new GenericSchoolData("Listening & Talking", NumberFormatHelper.ConvertObjectToFloat(itemRow[73])));
                        temp.Add(new GenericSchoolData("Numeracy", NumberFormatHelper.ConvertObjectToFloat(itemRow[74])));
                        tSPCfelevel.P7SecondLevelQ5 = temp;

                        listSPCfelevel.Add(tSPCfelevel);
                    }
                }
            }

            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
        }

        //GetPrimaryCfeLevelDataforReport
        protected List<SPCfEReport> GetPrimaryCfeLevelDataforReport(IGenericRepository2nd rpGeneric2nd, string seedcode, string schooltype)
        {
            List<SPCfEReport> listSPCfelevel = new List<SPCfEReport>();
            SPCfEReport tSPCfelevel = new SPCfEReport();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            SPReport tempSPreport = new SPReport();

            //get actual number 
            string query = "Select * from summary_primary_cfe_report where seedcode =" + seedcode + " and SchoolType = " + schooltype;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tSPCfelevel = new SPCfEReport();
                        tSPCfelevel.P1 = new List<SPReport>();
                        tSPCfelevel.P4 = new List<SPReport>();
                        tSPCfelevel.P7 = new List<SPReport>();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ER";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P1.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "EW";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P1.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ELT";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P1.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "N";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P1.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ER";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "EW";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[33])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[34])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[35])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[36])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[37])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[38])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ELT";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[39])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[40])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[41])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[42])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[43])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[44])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "N";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[45])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[46])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[47])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[48])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[49])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[50])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P4.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ER";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[51])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[52])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[53])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[54])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[55])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[56])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P7.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "EW";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[57])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[58])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[59])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[60])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[61])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[62])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P7.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "ELT";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[63])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[64])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[65])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[66])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[67])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[68])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P7.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "N";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[69])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[70])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[71])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[72])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[73])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[74])));
                        tempSPreport.listdata = temp;
                        tSPCfelevel.P7.Add(tempSPreport);

                        listSPCfelevel.Add(tSPCfelevel);
                    }
                }
            }

            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
        }

        //GetPrimaryINCASDataforReport 
        protected List<SPCfEReport> GetPrimaryINCASDataforReport(IGenericRepository2nd rpGeneric2nd, string seedcode)
        {
            List<SPCfEReport> listSPCfelevel = new List<SPCfEReport>();
            SPCfEReport tSPCfelevel = new SPCfEReport();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            SPReport tempSPreport = new SPReport();
            List<SPReport> tmplistSPReport = new List<SPReport>();

            //get actual number 
            string query = "Select * from view_incas_report where seedcode =" + seedcode;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                tSPCfelevel = new SPCfEReport();
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tmplistSPReport = new List<SPReport>();
                        tSPCfelevel.year = new Year(itemRow[0].ToString());
                        tSPCfelevel.stdstage = itemRow[2].ToString();

                        tempSPreport = new SPReport();
                        tempSPreport.code = "DevAbil";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[3])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[4])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[5])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[6])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[7])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[8])));
                        tempSPreport.listdata = temp;
                        tmplistSPReport.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "GenMaths";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[9])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[10])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[11])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[12])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[13])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[14])));
                        tempSPreport.listdata = temp;
                        tmplistSPReport.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "MentArith";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[15])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[16])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[17])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[18])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[19])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[20])));
                        tempSPreport.listdata = temp;
                        tmplistSPReport.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "Reading";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[21])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[22])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[23])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[24])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[25])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[26])));
                        tempSPreport.listdata = temp;
                        tmplistSPReport.Add(tempSPreport);

                        tempSPreport = new SPReport();
                        tempSPreport.code = "Spelling";
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", NumberFormatHelper.ConvertObjectToFloat(itemRow[27])));
                        temp.Add(new GenericSchoolData("FSM", NumberFormatHelper.ConvertObjectToFloat(itemRow[28])));
                        temp.Add(new GenericSchoolData("LAC", NumberFormatHelper.ConvertObjectToFloat(itemRow[29])));
                        temp.Add(new GenericSchoolData("30M", NumberFormatHelper.ConvertObjectToFloat(itemRow[30])));
                        temp.Add(new GenericSchoolData("40M", NumberFormatHelper.ConvertObjectToFloat(itemRow[31])));
                        temp.Add(new GenericSchoolData("30L", NumberFormatHelper.ConvertObjectToFloat(itemRow[32])));
                        tempSPreport.listdata = temp;
                        tmplistSPReport.Add(tempSPreport);

                        switch (itemRow[2].ToString())
                        {
                            case "1":
                                tSPCfelevel.P2 = new List<SPReport>();
                                tSPCfelevel.P2 = tmplistSPReport;
                                break;
                            case "2":
                                tSPCfelevel.P3 = new List<SPReport>();
                                tSPCfelevel.P3 = tmplistSPReport;
                                break;
                            case "3": 
                                tSPCfelevel.P4 = new List<SPReport>();
                                tSPCfelevel.P4 = tmplistSPReport;
                                break;
                            case "4":
                                tSPCfelevel.P5 = new List<SPReport>();
                                tSPCfelevel.P5 = tmplistSPReport;
                                break;
                            case "5":
                                tSPCfelevel.P6 = new List<SPReport>();
                                tSPCfelevel.P6 = tmplistSPReport;
                                break; 
                        }
                    }
                }
                listSPCfelevel.Add(tSPCfelevel);

            }

            return listSPCfelevel.OrderBy(x => x.year.year).ToList(); ;
        }

        //GetPrimaryINCASDataforReport 
        protected List<SPExclusion> GetExclusionDataforReport(IGenericRepository2nd rpGeneric2nd, string seedcode)
        {
            List<SPExclusion> listSPExclusion = new List<SPExclusion>();
            List<GenericSchoolData> temp = new List<GenericSchoolData>();
            SPExclusion tempSPExclusion = new SPExclusion();

            //get actual number 
            string query = "Select * from view_exclusion_report where seedcode =" + seedcode;
            var listResult = rpGeneric2nd.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        tempSPExclusion = new SPExclusion();
                        tempSPExclusion.YearInfo = new Year(itemRow[0].ToString());
                        temp = new List<GenericSchoolData>();
                        temp.Add(new GenericSchoolData("ALL", itemRow[2] == null ? 0 : Convert.ToInt16(itemRow[2].ToString())));
                        temp.Add(new GenericSchoolData("FSM", itemRow[3] == null ? 0 : Convert.ToInt16(itemRow[3].ToString())));
                        temp.Add(new GenericSchoolData("LAC", itemRow[4] == null ? 0 : Convert.ToInt16(itemRow[4].ToString())));
                        tempSPExclusion.ListGenericSchoolData = temp;
                        listSPExclusion.Add(tempSPExclusion);

                    }
                }
            }

            return listSPExclusion.OrderBy(x => x.YearInfo.year).ToList(); ;
        }

        // Change text in 1.1	School Roll table   
        protected void ChangeTextInTBSchoolRoll(WordprocessingDocument doc, List<StudentStage> data, int schooltype, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.YearInfo.academicyear.Equals("2011/12"));
                string[] codes = new string[] {}; // using i to reference

                string[] years = new string[] {"2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                string sdata = "n/a";
                List<GenericSchoolData> tempdata;

                if (schooltype == 2)
                {
                    //primary schools
                    codes = new string[] { "P1", "P2", "P3", "P4", "P5", "P6", "P7" }; // using i to reference

                }
                else {
                    //secondary schools
                    codes = new string[] { "S1", "S2", "S3", "S4", "S5", "S6"}; // using i to reference
                
                }

                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                for (int i = 1; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {

                    // travel through each row from row 1.
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        //get list data by year
                        tempdata = data.Where(x => x.YearInfo.academicyear.Equals(years[i - 1])).Select(x => x.ListGenericSchoolData).First();

                        //the get data by code
                        if (j == row.Elements<TableCell>().Count() - 1)
                        {
                            sdata = data.Where(x => x.YearInfo.academicyear.Equals(years[i - 1])).Select(x => x.totalschoolroll).First().ToString();
                        }
                        else
                        {
                            sdata = tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.sCount).First();
                        }

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTB1: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 1.2	SIMD   
        protected void ChangeTextInTBSIMD(WordprocessingDocument doc, SPSIMD data, int tableid)
        {
            try
            {
                string[] codes = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }; // using i to reference
                string sdata = "n/a";
                List<GenericSchoolData> tempdata;

                //using (WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true))
                //{
                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);

                for (int i = 2; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    tempdata = data.ListGenericSchoolData;

                    // travel through each row from row 1.
                    TableRow row = table.Elements<TableRow>().ElementAt(i);

                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        //the get data by code
                        if (i == 2)
                        {
                            sdata = tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.sCount).First();
                        }
                        else
                        {
                            sdata = tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.sPercent).First();
                        }

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;

                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTB1: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 1.3 English as an additional language (EAL) table   
        protected void ChangeTextInTBEAL(WordprocessingDocument doc, List<LevelOfEnglish> data, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.YearInfo.academicyear.Equals("2011/12"));
                string[] codes = new string[] { "01", "02", "03", "04", "05", "EN", "LC", "NA" }; // using i to reference
                string[] years = new string[] { "2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                List<GenericSchoolData> tempdata;


                //using (WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true))
                //{
                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                for (int i = 1; i < table.Elements<TableRow>().Count(); i++)
                {

                    // travel through each row from row 1.
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        //get list data by year
                        tempdata = data.Where(x => x.YearInfo.academicyear.Equals(years[j - 1])).Select(x => x.ListGenericSchoolData).First();
                        //the get data by code
                        string sdata = tempdata.Where(x => x.Code.Equals(codes[i - 1])).Select(x => x.sPercent).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;

                    }
                }
                //}

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBEAL: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }
        }

        // Change text in 1.4 Additional Support needs (ASN) table   
        protected void ChangeTextInTBASN(WordprocessingDocument doc, List<StudentNeed> data, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.year.academicyear.Equals("2011/12"));
                string[] years = new string[] {"2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                string sdata = "n/a";

                //using (WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true))
                //{
                // Find the first table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);

                for (int i = 1; i < table.Elements<TableRow>().Count(); i++)
                {
                    // Find the second row (CSP) in the table.
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column from row 1.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {

                        //the get data by code
                        if (i == 1)
                        {
                            //get CSP data for CSP row
                            sdata = data.Where(s => s.year.academicyear.Equals(years[j - 1])).Select(x => x.CSP.sPercent).First();

                        }
                        else if (i == 2)
                        {
                            //get IEP data for IEP row
                            sdata = data.Where(s => s.year.academicyear.Equals(years[j - 1])).Select(x => x.IEP.sPercent).First();


                        }
                        else
                        {
                            //get IEP data for Childs Plan row
                            sdata = data.Where(s => s.year.academicyear.Equals(years[j - 1])).Select(x => x.ChildPlan.sPercent).First();

                        }
                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;
                    }
                }

                //}

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBASN: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }
        }

        // Change text in 1.5	Free School Meal
        protected void ChangeTextInTBFSM(WordprocessingDocument doc, List<FreeSchoolMeal> data, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.year.academicyear.Equals("2011/12"));
                string[] years = new string[] {"2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                string sdata = "n/a";
                GenericSchoolData tempdata;

                //using (WordprocessingDocument doc = WordprocessingDocument.Open(filepath, true))
                //{
                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 1; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {


                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        //get list data by year
                        tempdata = data.Where(x => x.year.academicyear.Equals(years[i - 1])).Select(x => x.GenericSchoolData).First();

                        //the get data by code
                        if (j == 1)
                        {
                            sdata = tempdata.sum.ToString();
                        }
                        else
                        {
                            sdata = tempdata.sPercent;
                        }

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata;

                        //}
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBFSM: " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 1.6 AAE:5 Year trend
        protected void ChangeTextInTBAAETrend(WordprocessingDocument doc, List<SPAttendance> data, int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.YearInfo.academicyear.Equals("2011/12"));
                string[] names = new string[] { "Attendance", "Authorised Absence", "Unauthorised Absence", "Absense due to Exclusion" }; // using i to reference
                string[] years = new string[] { "2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                List<GenericSchoolData> tempdata;

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 2; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    //get list data by year
                    tempdata = data.Where(x => x.YearInfo.academicyear.Equals(years[i - 2])).Select(x => x.ListGenericSchoolData).FirstOrDefault();
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        string sdata = tempdata == null ? "--" : tempdata.Where(x => x.Name.Equals(names[j - 1])).Select(x => x.sPercent).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata.Equals("n/a") ? "--" : sdata.Equals("0.0") ? "-" : sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBAAETrend : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 1.8 CfELevel by FSM and LAC
        protected void ChangeTextInTBCfeLevel(WordprocessingDocument doc, List<SPReport> data, int tableid)
        {
            try
            {
                // code 0 = "Temporary Exclusions", 1="Removed From Register", 2="Number of days per 1000 pupils lost to exclusions" 
                string[] datasets = new string[] { "ER", "EW", "ELT", "N" }; // using i to reference
                string[] codes = new string[] { "ALL", "FSM", "LAC", "30M", "40M", "30L" }; // using j to reference
                List<GenericSchoolData> tempdata;

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 3; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    //get list data by dataset names

                    tempdata = data.Where(x => x.code.Equals(datasets[i - 3])).Select(x => x.listdata).First();
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        string sdata = tempdata.Where(x => x.Code.Equals(codes[j - 1])).Select(x => x.sPercent).First();

                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata.Equals("n/a")?"#":sdata.Equals("0.0")?"-":sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBExclusionTrend : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

        // Change text in 1.7 Exclusions:5 Year trend
        protected void ChangeTextInTBExclusionTrend(WordprocessingDocument doc, List<SPExclusion> data,int tableid)
        {
            try
            {
                //remove 2011/12 data from list
                data.RemoveAll(s => s.YearInfo.academicyear.Equals("2011/12"));
                // code 0 = "Temporary Exclusions", 1="Removed From Register", 2="Number of days per 1000 pupils lost to exclusions" 
                string[] years = new string[] { "2013/14", "2014/15", "2015/16", "2016/17", "2017/18" }; // using j to reference
                List<GenericSchoolData> tempdata;
                string sdata = "n/a";

                // Find the forth table in the document.
                Table table = doc.MainDocumentPart.Document.Body.Elements<Table>().ElementAt(tableid);
                // travel through each row from row 1.
                for (int i = 1; i < table.Elements<TableRow>().Count(); i++) //loop row/year
                {
                    //get list data by year
                    tempdata = data.Where(x => x.YearInfo.academicyear.Equals(years[i - 1])).Select(x => x.ListGenericSchoolData).FirstOrDefault();
                    TableRow row = table.Elements<TableRow>().ElementAt(i);
                    // travel through each column in row i.
                    for (int j = 1; j < row.Elements<TableCell>().Count(); j++)
                    {
                        if (j == 1)
                        {
                            sdata = tempdata.Where(x => x.Code.Equals("0")).Select(x => x.sCount).FirstOrDefault();
                        }
                        else if (j == 2)
                        {
                            sdata = tempdata.Where(x => x.Code.Equals("1")).Select(x => x.sCount).FirstOrDefault();
                        }
                        else
                        {
                            // Number of days Lost
                            sdata = tempdata == null ? "--" : tempdata.Where(x => x.Code.Equals("2")).Select(x => x.sPercent).FirstOrDefault() == null ? "n/a" : tempdata.Where(x => x.Code.Equals("2")).Select(x => x.sPercent).First();
                        }
                        // travel through each column in each row.
                        TableCell cell = row.Elements<TableCell>().ElementAt(j);
                        // Find the first paragraph in the table cell.
                        Paragraph p = cell.Elements<Paragraph>().First();

                        // Find the first run in the paragraph.
                        Run r = p.Elements<Run>().First();

                        // Set the text for the run.
                        Text t = r.Elements<Text>().First();
                        t.Text = sdata.Equals("n/a") ? "--" : sdata.Equals("0.0") || sdata.Equals("0") ? "-" : sdata;
                    }
                }

            }
            catch (Exception ex)
            {
                var sErrorMessage = "Error in ChangeTextInTBExclusionTrend : " + ex.Message + (ex.InnerException != null ? ", More Detail : " + ex.InnerException.Message : "");
                log.Error(ex.Message, ex);
            }

        }

    }
}