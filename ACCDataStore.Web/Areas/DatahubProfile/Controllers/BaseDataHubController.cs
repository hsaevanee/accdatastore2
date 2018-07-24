using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Entity.DatahubProfile.Entities;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Entity.SchoolProfiles;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.DatahubProfile.Controllers
{
    public class BaseDataHubController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseDataHubController));

        protected IList<DataSetDate> GetListDataSetDate()
        {
            List<DataSetDate> temp = new List<DataSetDate>();
            temp.Add(new DataSetDate("October 2016", "102016", "10","2016"));
            temp.Add(new DataSetDate("November 2016", "112016","11","2016"));
            temp.Add(new DataSetDate("December 2016", "122016", "12", "2016"));
            temp.Add(new DataSetDate("January 2017", "12017", "1", "2017"));
            temp.Add(new DataSetDate("February 2017", "22017", "2", "2017"));
            temp.Add(new DataSetDate("November 2017", "112017", "11", "2017"));
            temp.Add(new DataSetDate("December 2017", "122017", "12", "2017"));
            temp.Add(new DataSetDate("January 2018", "12018", "1", "2018"));
            temp.Add(new DataSetDate("February 2018", "22018", "2", "2018"));
            temp.Add(new DataSetDate("March 2018", "32018", "3", "2018"));
            temp.Add(new DataSetDate("April 2018", "42018", "4", "2018"));
            return temp;

        }


        protected IList<DatahubCentre> GetListSchoolname()
        {
            List<DatahubCentre> temp = new List<DatahubCentre>();
            temp.Add(new DatahubCentre("5244439", "Aberdeen Grammar School", "1"));
            temp.Add(new DatahubCentre("5235634", "Bridge Of Don Academy", "1"));
            temp.Add(new DatahubCentre("5234034", "Bucksburn Academy", "1"));
            temp.Add(new DatahubCentre("5248744", "Cordyce School", "1"));
            temp.Add(new DatahubCentre("5235839", "Cults Academy", "1"));
            temp.Add(new DatahubCentre("5243335", "Dyce Academy", "1"));
            temp.Add(new DatahubCentre("5243238", "Harlaw Academy", "1"));
            temp.Add(new DatahubCentre("5243432", "Hazlehead Academy", "1"));
            temp.Add(new DatahubCentre("5244943", "Hazlewood School", "1"));
            temp.Add(new DatahubCentre("5243831", "Kincorth Academy", "1"));
            temp.Add(new DatahubCentre("5244234", "Northfield Academy", "1"));
            temp.Add(new DatahubCentre("5246237", "Oldmachar Academy", "1"));
            temp.Add(new DatahubCentre("5246431", "St Machar Academy", "1"));
            temp.Add(new DatahubCentre("5244838", "Torry Academy", "1"));
            return temp.OrderBy(x => x.name).ToList(); 

        }

        protected IList<Status> GetListPositiveDestinations()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("112","School Pupil"));
            temp.Add(new Status("113","School Pupil - In Transition"));
            temp.Add(new Status("101","Activity Agreement"));
            temp.Add(new Status("102","Employability Fund Stage 2"));
            temp.Add(new Status("103","Employability Fund Stage 3"));
            temp.Add(new Status("104","Employability Fund Stage 4"));
            temp.Add(new Status("105","Full-Time Employment"));
            temp.Add(new Status("106","Further Education"));
            temp.Add(new Status("107","Higher Education"));
            temp.Add(new Status("108","Modern Apprenticeship"));
            temp.Add(new Status("110","Part-Time Employment"));
            temp.Add(new Status("111","Personal/ Skills Development"));
            temp.Add(new Status("114","Self-Employed"));
            //temp.Add(new Status("","Training (non ntp)"));
            temp.Add(new Status("115","Voluntary Work"));
            temp.Add(new Status("116","PSD (Social & Health)"));
            temp.Add(new Status("117","PSD (Employability)"));
            temp.Add(new Status("109","Other Formal Training"));

            return temp;

        }

        protected IList<Status> GetListEducation()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("112", "School Pupil"));
            temp.Add(new Status("113", "School Pupil - In Transition"));
            temp.Add(new Status("106", "Further Education"));
            temp.Add(new Status("107", "Higher Education"));
            return temp;

        }

        protected IList<Status> GetListEmployment()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("105", "Full-Time Employment"));
            temp.Add(new Status("108", "Modern Apprenticeship"));
            temp.Add(new Status("110", "Part-Time Employment"));
            temp.Add(new Status("114", "Self-Employed"));
            return temp;

        }

        protected IList<Status> GetListTrainingAndOtherDevelopment()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("101", "Activity Agreement"));
            temp.Add(new Status("102", "Employability Fund Stage 2"));
            temp.Add(new Status("103", "Employability Fund Stage 3"));
            temp.Add(new Status("104", "Employability Fund Stage 4"));
            temp.Add(new Status("111", "Personal/ Skills Development"));
            temp.Add(new Status("115", "Voluntary Work"));
            temp.Add(new Status("116", "PSD (Social & Health)"));
            temp.Add(new Status("117", "PSD (Employability)"));
            temp.Add(new Status("109", "Other Formal Training"));
            return temp;

        }

        protected IList<Status> GetListNonPositiveDestinations()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("001", "Custody"));
            temp.Add(new Status("002", "Economically Inactive"));
            temp.Add(new Status("003", "Unavailable - Ill Health"));
            temp.Add(new Status("004", "Unemployed"));
            temp.Add(new Status("005", "Economically Inactive - TOTT"));
            return temp;

        }

        protected IList<Status> GetListNPUnemployedSeeking()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("004", "Unemployed"));
            return temp;

        }

        protected IList<Status> GetListNPUnemployedNotSeeking()
        {
            List<Status> temp = new List<Status>();
            temp.Add(new Status("001", "Custody"));
            temp.Add(new Status("002", "Economically Inactive"));
            temp.Add(new Status("003", "Unavailable - Ill Health"));
            temp.Add(new Status("005", "Economically Inactive - TOTT"));
            return temp;

        }

        protected IList<DatahubCentre> GetListNeighbourhoodsname(IGenericRepository2nd rpGeneric2nd)
        {
            List<DatahubCentre> temp = new List<DatahubCentre>();
            var listneighbourhoods = rpGeneric2nd.FindByNativeSQL("Select distinct Ref_No, Neighbourhood from Neighbourhood_Postcodes1");
            if (listneighbourhoods != null)
            {
                foreach (var item in listneighbourhoods)
                {
                    if (item != null)
                    {
                        temp.Add(new DatahubCentre(item[0].ToString(), item[1].ToString(),"2"));
                    }

                }
            }
            return temp.OrderBy(x => x.name).ToList();

        }

        protected List<PupilsDataHubObj> GetDatahubdatabyNeighbourhoods(IGenericRepository2nd rpGeneric2nd, string neighbourhood_RefNO, string sYear)
        {
            //var listpupilsdata = this.rpGeneric.FindAll<ACCDataStore.Entity.DatahubProfile.DatahubDataObj>();
            List<PupilsDataHubObj> listpupilsdata = Getlistpupil(rpGeneric2nd, sYear);

            var listneighbourhooddata = rpGeneric2nd.FindAll<ACCDataStore.Entity.DatahubProfile.NeighbourhoodObj>();
            var listdata = new List<PupilsDataHubObj>();
            if (neighbourhood_RefNO != null)
            {
                listdata = (from a in listpupilsdata join b in listneighbourhooddata on a.CSS_Postcode equals b.CSS_Postcode where b.Ref_No.Contains(neighbourhood_RefNO) select a).ToList();

            }
            return listdata;
        }

        protected List<PupilsDataHubObj> Getlistpupil(IGenericRepository2nd rpGeneric2nd, string sYear)
        {

            //List<PupilsDataHubObj> listdata = rpGeneric2nd.FindAll<PupilsDataHubObj>().Where(x => x.Data_Date.Equals(sYear)).ToList<PupilsDataHubObj>();
            List<PupilsDataHubObj> listdata = GetdatafromDB(rpGeneric2nd, sYear);

            //List<PupilsDataHubObj> pupilsmoveoutScotland = listdata.Where(x => x.Current_Status.ToLower().Equals("moved outwith scotland")).ToList();

            List<PupilsDataHubObj> pupilsAge15 = listdata.Where(x => x.Age == 15).ToList();

            List<PupilsDataHubObj> listResult = listdata.Except(pupilsAge15).ToList();


            return listResult;
        }

        protected virtual List<PupilsDataHubObj> GetdatafromDB(IGenericRepository2nd rpGeneric2nd, string datasetDate)
        {
            List<PupilsDataHubObj> listResult = new List<PupilsDataHubObj>();
            string query = "";
            switch (datasetDate)
            {
                case "102016":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_octorber";
                    break;
                case "112016":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_november where data_date = 112016";
                    break;
                case "122016":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_december";
                    break;
                case "12017":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_january where data_date = 12017";
                    break;
                case "12018":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_january where data_date = 12018";
                    break;
                case "22017":
                    //to calculate IEP CSP
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_february";
                    break;
                case "22018":
                    //to calculate IEP CSP
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_february where data_date = 22018";
                    break;
                case "32018":
                    //to calculate IEP CSP
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_march where data_date = 32018";
                    break;
                case "42018":
                    //to calculate IEP CSP
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_april where data_date = 42018";
                    break;
                case "5":
                    //to calculate IEP CSP
                    query = "Select * from datahub_may";
                    break;
                case "6":
                    //to calculate IEP CSP
                    query = "Select * from datahub_june";
                    break;
                case "7":
                    //to calculate IEP CSP
                    query = "Select * from datahub_july";
                    break;
                case "8":
                    //to calculate IEP CSP
                    query = "Select * from datahub_august";
                    break;
                case "9":
                    //to calculate IEP CSP
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref   from datahub_september";
                    break;
                case "112017":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_november where data_date = 112017 ";
                    break;
                case "122017":
                    query = "Select Cohort, Forename, Surname, Age, Gender, CSS_Address, CSS_Postcode, Telephone_Number, SEED_Code, Current_Status, status_code, SDS_Client_Ref  from datahub_december where data_date = 122017 ";
                    break;
            }

            var listtemp = rpGeneric2nd.FindByNativeSQL(query);
            foreach (var itemrow in listtemp)
            {
                if (itemrow != null)
                {
                    PupilsDataHubObj temp = new PupilsDataHubObj();
                    temp.Cohort = itemrow[0]==null? "n/a" : itemrow[0].ToString();
                    temp.Forename = itemrow[1] == null ? "n/a" : itemrow[1].ToString();
                    temp.Surname = itemrow[2] == null ? "n/a" : itemrow[2].ToString();
                    temp.Age = itemrow[3] == null ? 0 : Convert.ToInt16(itemrow[3].ToString());
                    temp.Gender =  itemrow[4] == null ? "n/a": itemrow[4].ToString();
                    temp.CSS_Address = itemrow[5] == null ? "n/a" : itemrow[5].ToString();
                    temp.CSS_Postcode = itemrow[6] == null ? "n/a" : itemrow[6].ToString();
                    temp.Telephone_Number = itemrow[7] == null ? "n/a" : itemrow[7].ToString();
                    temp.SEED_Code = itemrow[8] == null ? "n/a" : itemrow[8].ToString();
                    temp.Current_Status = itemrow[9] == null ? "n/a" : itemrow[9].ToString();
                    temp.status_code = itemrow[10] == null ? "n/a" : itemrow[10].ToString();
                    temp.SDS_Client_Ref = itemrow[11] == null ? "n/a" : itemrow[11].ToString();
                    listResult.Add(temp);
                }
            }


            return listResult;

        }

        protected List<SummaryDHdata> GetSummaryData(IGenericRepository2nd rpGeneric2nd, string seedcode) {

            List<SummaryDHdata> listResult = new List<SummaryDHdata>();
            List<DataSetDate> dataset = GetListDataSetDate().ToList();
            var listtemp = rpGeneric2nd.FindByNativeSQL("select * from datahub_summary where TRIM(SeedCode) like '" + seedcode +"'");

            foreach (var itemrow in listtemp)
            {
                SummaryDHdata temp = new SummaryDHdata();
                if (itemrow != null)
                {
                    temp.year = Convert.ToInt16(itemrow[0].ToString());
                    temp.month = Convert.ToInt16(itemrow[1].ToString());
                    temp.seedcode = itemrow[2].ToString();
                    temp.listdata = new List<GenericData>();
                    temp.listdata.Add(new GenericData("Participating Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[4])));
                    temp.listdata.Add(new GenericData("Non-Participating Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[5])));
                    temp.listdata.Add(new GenericData("Unknown Destination", NumberFormatHelper.ConvertObjectToFloat(itemrow[6])));
                    temp.sdataset = dataset.Where(x => x.month.Equals(temp.month.ToString()) && x.year.Equals(temp.year.ToString())).FirstOrDefault();
                    listResult.Add(temp);
                }
            }

            return listResult.OrderBy(x=>x.year).ThenBy(x=>x.month).ToList();
        }

        // Pupils by Ages Chart
        protected ColumnCharts GetChartPupilsAges(List<DataHubData> listSchoolData) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;
            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = "Pupils by Ages ";
            eColumnCharts.yAxis.title.text = "% of pupils";
            eColumnCharts.yAxis.min = 0;
            eColumnCharts.yAxis.max = 100;
            eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchoolData != null && listSchoolData.Count > 0)
            {
                eColumnCharts.xAxis.categories = listSchoolData[0].ListPupilsbyAges.Select(x => x.Code).ToList();
                foreach (var eSchool in listSchoolData)
                {
                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = eSchool.ListPupilsbyAges.Select(x => x.sPercent.Equals("n/a") ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                        //data = eSchool.ListPupilsbyAges.Select(x => (float?)Convert.ToDouble(x.Percent)).ToList(),
                        color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
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


        // Overall Destinations Chart
        protected ColumnCharts GetChartbyDestinations(List<DataHubData> listSchoolData, string destination) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;

            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = destination;
            eColumnCharts.yAxis.title.text = "% of pupils";
            eColumnCharts.yAxis.min = 0;
            eColumnCharts.yAxis.max = 60;
            eColumnCharts.yAxis.tickInterval = 10;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchoolData != null && listSchoolData.Count > 0)
            {
                if (destination.Equals("Participating"))
                {
                    eColumnCharts.xAxis.categories = listSchoolData[0].PositiveDestinations.Select(x => x.Name).ToList();

                }
                else
                {

                    eColumnCharts.xAxis.categories = listSchoolData[0].NonPositiveDestinations.Select(x => x.Name).ToList();
                }

                foreach (var eSchool in listSchoolData)
                {
                    if (destination.Equals("Participating"))
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSchool.SchoolName,
                            data = eSchool.PositiveDestinations.Select(x => x.sPercent.Equals("n/a") ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;

                    }
                    else
                    {

                        eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                        {
                            name = eSchool.SchoolName,
                            data = eSchool.NonPositiveDestinations.Select(x => x.sPercent.Equals("n/a") ? null : (float?)Convert.ToDouble(x.Percent)).ToList(),
                            color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
                        });
                        indexColor++;

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

        // Overall Destinations Chart
        protected SplineCharts GetChartTimelieDestinations(List<DataHubData> listSchoolData) // query from database and return charts object
        {

            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;

            var eSplineCharts = new SplineCharts();
            eSplineCharts.SetDefault(false);
            eSplineCharts.title.text = "Participation Timeline";
            eSplineCharts.yAxis.title.text = "% of pupils";

            eSplineCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series>();


            if (listSchoolData != null && listSchoolData.Count > 0)
            {
                eSplineCharts.xAxis.categories = listSchoolData[1].listsumarydestination.Select(x => x.sdataset.name).ToList(); // year on xAxis
                eSplineCharts.yAxis.title = new Entity.RenderObject.Charts.Generic.title() { text = "% of pupils" };

                foreach (var eSchool in listSchoolData)
                {
                    var listSeriesStart = eSchool.listsumarydestination.Select(x => (float?)x.listdata[0].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Participating",
                        color = colors[indexColor],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexColor++;

                    listSeriesStart = eSchool.listsumarydestination.Select(x => (float?)x.listdata[1].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Not participating",
                        color = colors[indexColor],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexColor++;

                    listSeriesStart = eSchool.listsumarydestination.Select(x => (float?)x.listdata[2].Percent).ToList();

                    eSplineCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.SplineCharts.series()
                    {
                        name = eSchool.SchoolName + " Unknown",
                        color = colors[indexColor],
                        lineWidth = 2,
                        data = listSeriesStart,
                        visible = true
                    });

                    indexColor++;

                }
            }

            eSplineCharts.exporting = new ACCDataStore.Entity.RenderObject.Charts.Generic.exporting()
            {
                enabled = true,
                filename = "export"
            };

            eSplineCharts.chart.options3d = new Entity.RenderObject.Charts.Generic.options3d() { enabled = true, alpha = 10, beta = 10 }; // enable 3d charts

            return eSplineCharts;
        }

        // Overall Destinations Chart
        protected ColumnCharts GetChartOverallDestinations(List<DataHubData> listSchoolData) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;

            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = " Destinations";
            eColumnCharts.yAxis.title.text = "% of pupils";
            eColumnCharts.yAxis.min = 0;
            eColumnCharts.yAxis.max = 100;
            eColumnCharts.yAxis.tickInterval = 20;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchoolData != null && listSchoolData.Count > 0)
            {

                eColumnCharts.xAxis.categories = new List<string>() { "Participating", "Not Participating", "Unknown" };

                foreach (var eSchool in listSchoolData)
                {
                    List<float?> data = new List<float?>() { };
                    data.Add(eSchool.PositiveDestinations.Select(x => x.Percent).Sum());
                    data.Add(eSchool.NonPositiveDestinations.Select(x => x.Percent).Sum());
                    data.Add(eSchool.Unknown.Percent);

                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = data,
                        color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
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

        // Group Destinations Chart
        protected ColumnCharts GetChartbyGroupDestinations(List<DataHubData> listSchoolData) // query from database and return charts object
        {
            string[] colors = new string[] { "#50B432", "#24CBE5", "#f969e8", "#DDDF00", "#64E572", "#FF9655", "#FFF263", "#6AF9C4" };
            int indexColor = 0;

            var eColumnCharts = new ColumnCharts();
            eColumnCharts.SetDefault(false);
            eColumnCharts.title.text = " Destinations by Key Results";
            eColumnCharts.yAxis.title.text = "% of pupils";
            eColumnCharts.yAxis.min = 0;
            eColumnCharts.yAxis.max = 60;
            eColumnCharts.yAxis.tickInterval = 10;

            eColumnCharts.series = new List<ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series>();
            if (listSchoolData != null && listSchoolData.Count > 0)
            {

                eColumnCharts.xAxis.categories = new List<string>() { "Education", "Employment", "Training & Other Development", "Unemployed Seeking", "Unemployed not seeking", "Unknown" };

                foreach (var eSchool in listSchoolData)
                {
                    List<float?> data = new List<float?>() { };
                    data.Add((float?)eSchool.listPEducationGroup.checkSumPercentage);
                    data.Add((float?)eSchool.listPEmploymentGroup.checkSumPercentage);
                    data.Add((float?)eSchool.listPTrainingGroup.checkSumPercentage);
                    data.Add((float?)eSchool.listNPUnemployedSeekingGroup.checkSumPercentage);
                    data.Add((float?)eSchool.listNPUnemployedNotSeekingGroup.checkSumPercentage);
                    data.Add(eSchool.Unknown.Percent);

                    eColumnCharts.series.Add(new ACCDataStore.Entity.RenderObject.Charts.ColumnCharts.series()
                    {
                        name = eSchool.SchoolName,
                        data = data,
                        color = eSchool.SeedCode.Equals("1002") ? "#058DC7" : colors[indexColor]
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

    }
}