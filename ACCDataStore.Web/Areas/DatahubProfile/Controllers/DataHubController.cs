using ACCDataStore.Core.Helper;
using ACCDataStore.Entity;
using ACCDataStore.Entity.DatahubProfile;
using ACCDataStore.Entity.DatahubProfile.Entities;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.SplineCharts;
using ACCDataStore.Helpers.ORM;
using ACCDataStore.Helpers.ORM.Helpers.Security;
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
    public class DataHubController : BaseDataHubController
    {
        private static ILog log = LogManager.GetLogger(typeof(DataHubController));

        private readonly IGenericRepository2nd rpGeneric2nd;

        public DataHubController(IGenericRepository2nd rpGeneric2nd)
        {
            this.rpGeneric2nd = rpGeneric2nd;
        }

        // GET: DatahubProfile/DataHub
        [DatahubAuthentication]
        [Transactional]
        public virtual ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("DatahubProfile/DataHub/GetCondition")]
        public virtual JsonResult GetCondition()
        {
            try
            {
                var listSchool = GetListSchoolname();
                var listNeighbourhoodName = GetListNeighbourhoodsname(rpGeneric2nd);
                var listdatasetdate = GetListDataSetDate();
                var listReport = new[] { new { Code = "01", Name = "Participation Measure by Status" }, new { Code = "02", Name = "Participation Measure by Status Grouping" }, new { Code = "03", Name = "Participation Measure by Age" }, new { Code = "04", Name = "Participation Measure by Gender" } }.ToList();

                object oResult = null;

                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()),
                    ListNeighbourhood = listNeighbourhoodName.Select(x => x.GetJson()),
                    ListDataset = listdatasetdate.Select(x => x.GetJson()),
                    DatasetSelected = listdatasetdate.Where(x => x.code.Equals("42018")).Select(x => x.GetJson()).First(),
                    ListSchoolSelected = listSchool.Where(x => x.seedcode.Equals("5244439")).Select(x => x.GetJson()),
                    ListReport = listReport,
                    ReportsetSelected = listReport.Where(x => x.Code.Equals("01")).First(),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        [HttpGet]
        [Route("DatahubProfile/Datahub/GetData")]
        public JsonResult GetData([System.Web.Http.FromUri] List<string> listSeedCode, [System.Web.Http.FromUri] List<string> ListNeighbourhoodCode, [System.Web.Http.FromUri] string sYear, [System.Web.Http.FromUri] string sReport) // get selected list of school's id
        {
            try
            {
                object oResult = null;
                List<DataHubData> listSchoolData = new List<DataHubData>();

                var listSchool = GetListSchoolname();
                var listNeighbourhoodName = GetListNeighbourhoodsname(rpGeneric2nd);
                var listdatasetdate = GetListDataSetDate();
                var listReport = new[] { new { Code = "01", Name = "Participation Measure by Status" }, new { Code = "02", Name = "Participation Measure by Status Grouping" }, new { Code = "03", Name = "Participation Measure by Age" }, new { Code = "04", Name = "Participation Measure by Gender" } }.ToList();


                List<DatahubCentre> ListSchoolSelected = listSeedCode != null && listSeedCode.Count > 0 ? listSchool.Where(x => listSeedCode.Contains(x.seedcode)).ToList() : null;
                List<DatahubCentre> ListNeighbourSelected = ListNeighbourhoodCode != null && ListNeighbourhoodCode.Count > 0 ? listNeighbourhoodName.Where(x => ListNeighbourhoodCode.Contains(x.seedcode)).ToList() : null;

                if (ListSchoolSelected != null && ListSchoolSelected.Count > 0){
                    switch (sReport)
                    {
                        case "01":
                            listSchoolData = GetDataHubSchoolDatabyStatuses(ListSchoolSelected, sYear);
                            break;
                        case "02":
                            Console.WriteLine("Case 2");
                            break;
                        case "03":
                            Console.WriteLine("Default case");
                            break;
                        case "04":
                            Console.WriteLine("Default case");
                            break;
                    }

                }
                else if (ListNeighbourSelected != null && ListNeighbourSelected.Count > 0) {
                    listSchoolData = GetDataHubNeighbourhoodDatabyStatuses(ListNeighbourSelected, sYear);
                }
                   
                //var listNeighData = GetDataHubData(ListNeighbourSelected);

                oResult = new
                {
                    ListSchool = listSchool.Select(x => x.GetJson()), // all school
                    ListNeighbourhood = listNeighbourhoodName.Select(x => x.GetJson()),
                    ListDataset = listdatasetdate.Select(x => x.GetJson()),
                    DatasetSelected = listdatasetdate.Where(x => x.code.Equals(sYear)).Select(x => x.GetJson()).First(),
                    ListSchoolSelected = ListSchoolSelected!=null? ListSchoolSelected.Where(x => !x.seedcode.Equals("1002")).Select(x => x.GetJson()):null, // set selected list of school
                    ListNeighbourSelected = ListNeighbourSelected != null ? ListNeighbourSelected.Where(x => !x.seedcode.Equals("1002")).Select(x => x.GetJson()) : null, // set selected list of school
                    ListingData = listSchoolData, // table data
                    ChartData = GetChartData(listSchoolData),
                    ListReport = listReport,
                    ReportsetSelected = listReport.Where(x => x.Code.Equals(sReport)).First(),
                };

                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ThrowJsonError(ex);
            }
        }

        protected List<DataHubData> GetDataHubSchoolDatabyStatuses(List<DatahubCentre> tListSchoolSelected, string sYear)
        {

            var listSchoolData = new List<DataHubData>();
            DataHubData tempSchool = new DataHubData();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new DatahubCentre("1002", "Aberdeen City","1"));
            List<PupilsDataHubObj> listpupils = Getlistpupil(rpGeneric2nd, sYear);

           // List<PupilsDataHubObj> listpupilsNeighbour = GetDatahubdatabyNeighbourhoods(rpGeneric2nd);

            List<PupilsDataHubObj> templistpupils = new  List<PupilsDataHubObj>();

            IList<Status> listpositivedestination = GetListPositiveDestinations();

            IList<Status> listnonpositivedestination = GetListNonPositiveDestinations();
            IList<Status> listPEducationGroup = GetListEducation();
            IList<Status> listPEmploymentGroup = GetListEmployment();
            IList<Status> listPTrainingGroup = GetListTrainingAndOtherDevelopment();

            IList<Status> listNPSeekingGroup = GetListNPUnemployedSeeking();
            IList<Status> listNPNotSeekingGroup = GetListNPUnemployedNotSeeking();

            foreach (DatahubCentre centre in tListSchoolSelected)
            {
                if (!centre.seedcode.Equals("1002"))
                {
                    templistpupils = listpupils.Where(x => (x.SEED_Code != null && x.SEED_Code.Equals(centre.seedcode))).ToList();
                }
                else {
                    templistpupils = listpupils;
                
                }
 

                tempSchool = new DataHubData();
                tempSchool.SeedCode = centre.seedcode;
                tempSchool.SchoolName = centre.name;
                tempSchool.Centretype = centre.centretype;
                tempSchool.AllClients = new GenericData("AllClients", templistpupils.Count());
                tempSchool.Females = GetGender(templistpupils, "Female");
                tempSchool.Males = GetGender(templistpupils, "Male");
                tempSchool.ListPupilsbyAges = GetListPupilsbyAges(templistpupils);
                tempSchool.Pupil16 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils16")).FirstOrDefault();
                tempSchool.Pupil17 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils17")).FirstOrDefault();
                tempSchool.Pupil18 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils18")).FirstOrDefault();
                tempSchool.Pupil19 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils19")).FirstOrDefault();
                tempSchool.PositiveDestinations = GetDestination(centre.seedcode, templistpupils, listpositivedestination);
                tempSchool.NonPositiveDestinations = GetDestination(centre.seedcode, templistpupils, listnonpositivedestination);
                tempSchool.listPEducationGroup = new SDSGroup("SGP01", "Education");
                tempSchool.listPEducationGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPEducationGroup);
                tempSchool.listPEducationGroup.checkSumCount = tempSchool.listPEducationGroup.getcheckSumCount();
                tempSchool.listPEducationGroup.checkSumPercentage = tempSchool.listPEducationGroup.getcheckSumPercentage();
                tempSchool.listPEmploymentGroup = new SDSGroup("SGP02", "Employment");
                tempSchool.listPEmploymentGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPEmploymentGroup);
                tempSchool.listPEmploymentGroup.checkSumCount = tempSchool.listPEmploymentGroup.getcheckSumCount();
                tempSchool.listPEmploymentGroup.checkSumPercentage = tempSchool.listPEmploymentGroup.getcheckSumPercentage();
                tempSchool.listPTrainingGroup = new SDSGroup("SGP03", "Training & Other Development");
                tempSchool.listPTrainingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPTrainingGroup);
                tempSchool.listPTrainingGroup.checkSumCount = tempSchool.listPTrainingGroup.getcheckSumCount();
                tempSchool.listPTrainingGroup.checkSumPercentage = tempSchool.listPTrainingGroup.getcheckSumPercentage();
                tempSchool.listNPUnemployedSeekingGroup = new SDSGroup("SGNP01", "Unemployed Seeking");
                tempSchool.listNPUnemployedSeekingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listNPSeekingGroup);
                tempSchool.listNPUnemployedSeekingGroup.checkSumCount = tempSchool.listNPUnemployedSeekingGroup.getcheckSumCount();
                tempSchool.listNPUnemployedSeekingGroup.checkSumPercentage = tempSchool.listNPUnemployedSeekingGroup.getcheckSumPercentage();
                tempSchool.listNPUnemployedNotSeekingGroup = new SDSGroup("SGNP02", "Unemployed not seeking");
                tempSchool.listNPUnemployedNotSeekingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listNPNotSeekingGroup);
                tempSchool.listNPUnemployedNotSeekingGroup.checkSumCount = tempSchool.listNPUnemployedNotSeekingGroup.getcheckSumCount();
                tempSchool.listNPUnemployedNotSeekingGroup.checkSumPercentage = tempSchool.listNPUnemployedNotSeekingGroup.getcheckSumPercentage();
                tempSchool.Unknown = GetUnknown(templistpupils);
                tempSchool.MovedOutwithScotland = GetMovedOutwithScotland(templistpupils);
                tempSchool.listsumarydestination = GetSummaryData(rpGeneric2nd, centre.seedcode);
                tempSchool.Participating_count = tempSchool.listPEducationGroup.checkSumCount + tempSchool.listPEmploymentGroup.checkSumCount + tempSchool.listPTrainingGroup.checkSumCount;
                tempSchool.Participating_Percent = Math.Round(tempSchool.listPEducationGroup.checkSumPercentage + tempSchool.listPEmploymentGroup.checkSumPercentage + tempSchool.listPTrainingGroup.checkSumPercentage,2);
                tempSchool.NotParticipating_count = tempSchool.listNPUnemployedSeekingGroup.checkSumCount + tempSchool.listNPUnemployedNotSeekingGroup.checkSumCount;
                tempSchool.NotParticipating_Percent =  Math.Round(tempSchool.listNPUnemployedSeekingGroup.checkSumPercentage + tempSchool.listNPUnemployedNotSeekingGroup.checkSumPercentage,2);

                listSchoolData.Add(tempSchool);
            }
            return listSchoolData;
        }

        protected List<DataHubData> GetDataHubNeighbourhoodDatabyStatuses(List<DatahubCentre> tListSchoolSelected, string sYear)
        {

            var listSchoolData = new List<DataHubData>();
            DataHubData tempSchool = new DataHubData();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new DatahubCentre("1002", "Aberdeen City", "2"));

            List<PupilsDataHubObj> templistpupils = new List<PupilsDataHubObj>();

            IList<Status> listpositivedestination = GetListPositiveDestinations();

            IList<Status> listnonpositivedestination = GetListNonPositiveDestinations();
            IList<Status> listPEducationGroup = GetListEducation();
            IList<Status> listPEmploymentGroup = GetListEmployment();
            IList<Status> listPTrainingGroup = GetListTrainingAndOtherDevelopment();

            IList<Status> listNPSeekingGroup = GetListNPUnemployedSeeking();
            IList<Status> listNPNotSeekingGroup = GetListNPUnemployedNotSeeking();

            foreach (DatahubCentre centre in tListSchoolSelected)
            {
                if (centre.seedcode.Equals("1002"))
                {
                    templistpupils = Getlistpupil(rpGeneric2nd, sYear);
                }
                else
                {
                    templistpupils = GetDatahubdatabyNeighbourhoods(rpGeneric2nd, centre.seedcode, sYear);

                }


                tempSchool = new DataHubData();
                tempSchool.SeedCode = centre.seedcode;
                tempSchool.SchoolName = centre.name;
                tempSchool.Centretype = centre.centretype;
                tempSchool.AllClients = new GenericData("AllClients", templistpupils.Count());
                tempSchool.Females = GetGender(templistpupils, "Female");
                tempSchool.Males = GetGender(templistpupils, "Male");
                tempSchool.ListPupilsbyAges = GetListPupilsbyAges(templistpupils);
                tempSchool.Pupil16 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils16")).FirstOrDefault();
                tempSchool.Pupil17 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils17")).FirstOrDefault();
                tempSchool.Pupil18 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils18")).FirstOrDefault();
                tempSchool.Pupil19 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils19")).FirstOrDefault();
                tempSchool.PositiveDestinations = GetDestination(centre.seedcode, templistpupils, listpositivedestination);
                tempSchool.NonPositiveDestinations = GetDestination(centre.seedcode, templistpupils, listnonpositivedestination);
                tempSchool.listPEducationGroup = new SDSGroup("SGP01", "Education");
                tempSchool.listPEducationGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPEducationGroup);
                tempSchool.listPEducationGroup.checkSumCount = tempSchool.listPEducationGroup.getcheckSumCount();
                tempSchool.listPEducationGroup.checkSumPercentage = tempSchool.listPEducationGroup.getcheckSumPercentage();
                tempSchool.listPEmploymentGroup = new SDSGroup("SGP02", "Employment");
                tempSchool.listPEmploymentGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPEmploymentGroup);
                tempSchool.listPEmploymentGroup.checkSumCount = tempSchool.listPEmploymentGroup.getcheckSumCount();
                tempSchool.listPEmploymentGroup.checkSumPercentage = tempSchool.listPEmploymentGroup.getcheckSumPercentage();
                tempSchool.listPTrainingGroup = new SDSGroup("SGP03", "Training & Other Development");
                tempSchool.listPTrainingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPTrainingGroup);
                tempSchool.listPTrainingGroup.checkSumCount = tempSchool.listPTrainingGroup.getcheckSumCount();
                tempSchool.listPTrainingGroup.checkSumPercentage = tempSchool.listPTrainingGroup.getcheckSumPercentage();
                tempSchool.listNPUnemployedSeekingGroup = new SDSGroup("SGNP01", "Unemployed Seeking");
                tempSchool.listNPUnemployedSeekingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listNPSeekingGroup);
                tempSchool.listNPUnemployedSeekingGroup.checkSumCount = tempSchool.listNPUnemployedSeekingGroup.getcheckSumCount();
                tempSchool.listNPUnemployedSeekingGroup.checkSumPercentage = tempSchool.listNPUnemployedSeekingGroup.getcheckSumPercentage();
                tempSchool.listNPUnemployedNotSeekingGroup = new SDSGroup("SGNP02", "Unemployed not seeking");
                tempSchool.listNPUnemployedNotSeekingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listNPNotSeekingGroup);
                tempSchool.listNPUnemployedNotSeekingGroup.checkSumCount = tempSchool.listNPUnemployedNotSeekingGroup.getcheckSumCount();
                tempSchool.listNPUnemployedNotSeekingGroup.checkSumPercentage = tempSchool.listNPUnemployedNotSeekingGroup.getcheckSumPercentage();

                tempSchool.Unknown = GetUnknown(templistpupils);
                tempSchool.MovedOutwithScotland = GetMovedOutwithScotland(templistpupils);
                tempSchool.listsumarydestination = GetSummaryData(rpGeneric2nd, centre.seedcode);
                listSchoolData.Add(tempSchool);
            }
            return listSchoolData;
        }

        protected List<DataHubData> GetDataHubSchoolDatabyGroup(List<DatahubCentre> tListSchoolSelected, string sYear)
        {

            var listSchoolData = new List<DataHubData>();
            DataHubData tempSchool = new DataHubData();

            //add Aberdeen Primary School data
            tListSchoolSelected.Add(new DatahubCentre("1002", "Aberdeen City", "1"));
            List<PupilsDataHubObj> listpupils = Getlistpupil(rpGeneric2nd, sYear);

            // List<PupilsDataHubObj> listpupilsNeighbour = GetDatahubdatabyNeighbourhoods(rpGeneric2nd);

            List<PupilsDataHubObj> templistpupils = new List<PupilsDataHubObj>();

            IList<Status> listpositivedestination = GetListPositiveDestinations();

            IList<Status> listnonpositivedestination = GetListNonPositiveDestinations();
            IList<Status> listPEducationGroup = GetListEducation();
            IList<Status> listPEmploymentGroup = GetListEmployment();
            IList<Status> listPTrainingGroup = GetListTrainingAndOtherDevelopment();

            IList<Status> listNPSeekingGroup = GetListNPUnemployedSeeking();
            IList<Status> listNPNotSeekingGroup = GetListNPUnemployedNotSeeking();

            foreach (DatahubCentre centre in tListSchoolSelected)
            {
                if (!centre.seedcode.Equals("1002"))
                {
                    templistpupils = listpupils.Where(x => (x.SEED_Code != null && x.SEED_Code.Equals(centre.seedcode))).ToList();
                }
                else
                {
                    templistpupils = listpupils;

                }


                tempSchool = new DataHubData();
                tempSchool.SeedCode = centre.seedcode;
                tempSchool.SchoolName = centre.name;
                tempSchool.Centretype = centre.centretype;
                tempSchool.AllClients = new GenericData("AllClients", templistpupils.Count());
                tempSchool.Females = GetGender(templistpupils, "Female");
                tempSchool.Males = GetGender(templistpupils, "Male");
                tempSchool.ListPupilsbyAges = GetListPupilsbyAges(templistpupils);
                tempSchool.Pupil16 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils16")).FirstOrDefault();
                tempSchool.Pupil17 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils17")).FirstOrDefault();
                tempSchool.Pupil18 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils18")).FirstOrDefault();
                tempSchool.Pupil19 = tempSchool.ListPupilsbyAges.Where(x => x.Code.Equals("Pupils19")).FirstOrDefault();
                tempSchool.PositiveDestinations = GetDestination(centre.seedcode, templistpupils, listpositivedestination);
                tempSchool.NonPositiveDestinations = GetDestination(centre.seedcode, templistpupils, listnonpositivedestination);
                tempSchool.listPEducationGroup = new SDSGroup("SGP01", "Education");
                tempSchool.listPEducationGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPEducationGroup);
                tempSchool.listPEducationGroup.checkSumCount = tempSchool.listPEducationGroup.getcheckSumCount();
                tempSchool.listPEducationGroup.checkSumPercentage = tempSchool.listPEducationGroup.getcheckSumPercentage();
                tempSchool.listPEmploymentGroup = new SDSGroup("SGP02", "Employment");
                tempSchool.listPEmploymentGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPEmploymentGroup);
                tempSchool.listPEmploymentGroup.checkSumCount = tempSchool.listPEmploymentGroup.getcheckSumCount();
                tempSchool.listPEmploymentGroup.checkSumPercentage = tempSchool.listPEmploymentGroup.getcheckSumPercentage();
                tempSchool.listPTrainingGroup = new SDSGroup("SGP03", "Training & Other Development");
                tempSchool.listPTrainingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listPTrainingGroup);
                tempSchool.listPTrainingGroup.checkSumCount = tempSchool.listPTrainingGroup.getcheckSumCount();
                tempSchool.listPTrainingGroup.checkSumPercentage = tempSchool.listPTrainingGroup.getcheckSumPercentage();
                tempSchool.listNPUnemployedSeekingGroup = new SDSGroup("SGNP01", "Unemployed Seeking");
                tempSchool.listNPUnemployedSeekingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listNPSeekingGroup);
                tempSchool.listNPUnemployedSeekingGroup.checkSumCount = tempSchool.listNPUnemployedSeekingGroup.getcheckSumCount();
                tempSchool.listNPUnemployedSeekingGroup.checkSumPercentage = tempSchool.listNPUnemployedSeekingGroup.getcheckSumPercentage();
                tempSchool.listNPUnemployedNotSeekingGroup = new SDSGroup("SGNP02", "Unemployed not seeking");
                tempSchool.listNPUnemployedNotSeekingGroup.listdata = GetDestination(centre.seedcode, templistpupils, listNPNotSeekingGroup);
                tempSchool.listNPUnemployedNotSeekingGroup.checkSumCount = tempSchool.listNPUnemployedNotSeekingGroup.getcheckSumCount();
                tempSchool.listNPUnemployedNotSeekingGroup.checkSumPercentage = tempSchool.listNPUnemployedNotSeekingGroup.getcheckSumPercentage();
                tempSchool.Unknown = GetUnknown(templistpupils);
                tempSchool.MovedOutwithScotland = GetMovedOutwithScotland(templistpupils);
                tempSchool.listsumarydestination = GetSummaryData(rpGeneric2nd, centre.seedcode);
                listSchoolData.Add(tempSchool);
            }
            return listSchoolData;
        }
        protected GenericData GetGender(List<PupilsDataHubObj> listpupils, string gender)
        {

            GenericData data = new GenericData();
            data.Code = gender;
            data.count = listpupils.Where(x => x.Gender.Equals(gender)).ToList().Count();
            data.sum = listpupils.Count();
            data.Percent = data.sum != 0 ? data.count * 100.0F / data.sum : 0.0f;
            //data.sPercent = NumberFormatHelper.FormatNumber(data.Percent, 1, "n/a").ToString();
            data.sPercent = double.IsNaN(data.count * 100.0F / data.sum) ? "n/a" : NumberFormatHelper.FormatNumber(data.Percent, 1).ToString();
            data.scount = data.sPercent.Equals("n/a") ? "n/a" : data.count.ToString();

            return data;
        }

        protected GenericData GetUnknown(List<PupilsDataHubObj> listpupils)
        {
            List<PupilsDataHubObj> pupilsmoveoutScotland = listpupils.Where(x => x.status_code.Equals("000")).ToList();

            listpupils = listpupils.Except(pupilsmoveoutScotland).ToList();

            GenericData data = new GenericData();
            data.Code = "Unknown";
            data.count = listpupils.Where(x => x.status_code.Equals("999")).ToList().Count;
            data.sum = listpupils.Count();
            data.Percent = data.sum != 0? data.count * 100.0F / data.sum : 0.0f;
            //data.sPercent = NumberFormatHelper.FormatNumber(data.Percent, 1).ToString();
            data.sPercent = double.IsNaN(data.count * 100.0F / data.sum) ? "n/a" : NumberFormatHelper.FormatNumber(data.Percent, 1).ToString();
            data.scount = data.sPercent.Equals("n/a") ? "n/a" : data.count.ToString();

            return data;
        }

        protected GenericData GetMovedOutwithScotland(List<PupilsDataHubObj> listpupils)
        {

            GenericData data = new GenericData();
            data.Code = "MovedOutwithScotland";
            data.count = listpupils.Where(x => (x.Current_Status != null && x.Current_Status.ToLower().Equals("moved outwith scotland"))).ToList().Count;
            data.sum = listpupils.Count();
            data.Percent = data.sum != 0 ? data.count * 100.0F / data.sum : 0.0f;
            //data.sPercent = NumberFormatHelper.FormatNumber(data.Percent, 1).ToString();
            data.sPercent = double.IsNaN(data.count * 100.0F / data.sum) ? "n/a" : NumberFormatHelper.FormatNumber(data.Percent, 1).ToString();
            data.scount = data.sPercent.Equals("n/a") ? "n/a" : data.count.ToString();
            return data;
        }

        protected List<GenericData> GetListPupilsbyAges(List<PupilsDataHubObj> listpupils)
        {
            List<GenericData> listdata = new List<GenericData>();

            List<int> ages = new List<int>() { 16, 17, 18, 19 };

            GenericData tempobj = new GenericData();

            foreach (int age in ages)
            {
                tempobj = GetPupilsAges(listpupils, age);
                listdata.Add(tempobj);
            }

            return listdata;
            
        }

        protected GenericData GetPupilsAges(List<PupilsDataHubObj> listpupils, int age)
        {

            GenericData data = new GenericData();
            data.Code = "Pupils"+age;
            data.count = listpupils.Where(x => x.Age==age).ToList().Count;
            data.sum = listpupils.Count();
            data.Percent = data.sum != 0 ? data.count * 100.0F / data.sum : 0.0f;
            data.sPercent = double.IsNaN(data.count * 100.0F / data.sum)? "n/a" : NumberFormatHelper.FormatNumber(data.Percent, 1).ToString();
            data.scount = data.sPercent.Equals("n/a") ? "n/a" : data.count.ToString();
            return data;
        }

        protected List<GenericData> GetDestination(string seedcode, List<PupilsDataHubObj> listpupils, IList<Status> destinations)
        {

            List<PupilsDataHubObj> pupilsmoveoutScotland = listpupils.Where(x => x.Current_Status.ToLower().Equals("moved outwith scotland")).ToList();

            listpupils = listpupils.Except(pupilsmoveoutScotland).ToList();

            List<GenericData> listdata = new List<GenericData>();

            GenericData tempobj = new GenericData();

            foreach (Status item in destinations)
            {
                tempobj = new GenericData();
                tempobj.Code = item.code;
                tempobj.Name = item.name;
                tempobj.count = listpupils.Where(x => (x.Current_Status != null && x.status_code.Equals(item.code))).ToList().Count;
                tempobj.sum = listpupils.Count();
                tempobj.Percent = tempobj.sum != 0 ? tempobj.count * 100.0F / tempobj.sum : 0.0f; ;
                tempobj.sPercent = double.IsNaN(tempobj.count * 100.0F / tempobj.sum) ? "n/a" : NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                tempobj.scount = tempobj.sPercent.Equals("n/a") ? "n/a" : tempobj.count.ToString();
                //tempobj.sPercent = NumberFormatHelper.FormatNumber(tempobj.Percent, 1).ToString();
                listdata.Add(tempobj);
            }
            return listdata;
        }

        private ACCDataStore.Entity.DatahubProfile.Entities.ChartData GetChartData(List<DataHubData> listSchoolData)
        {

            ChartData chartdata = new ChartData();

                chartdata.ChartPupilsAges = GetChartPupilsAges(listSchoolData);
                chartdata.ChartPositiveDestinations = GetChartbyDestinations(listSchoolData, "Participating");
                chartdata.ChartNonPositiveDestinations = GetChartbyDestinations(listSchoolData, "Not Participating");
                chartdata.ChartOverallDestinations = GetChartOverallDestinations(listSchoolData);
                chartdata.ChartTimelineDestinations = GetChartTimelieDestinations(listSchoolData);
                chartdata.ChartGroupDestinations = GetChartbyGroupDestinations(listSchoolData);
 
 

            return chartdata;
        }
   
        [DatahubAuthentication]
        [Transactional]
        [HttpGet]
        [Route("DatahubProfile/Datahub/GetListPupils")]
        public JsonResult GetListPupils([System.Web.Http.FromUri] string seedcode,[System.Web.Http.FromUri] string centretype,  [System.Web.Http.FromUri] string dataname, [System.Web.Http.FromUri] string sYear)
        {
            try
            {



                Users temp = Session["SessionUser"] as Users;
                List<PupilsDataHubObj> templistpupils = new List<PupilsDataHubObj>();
                List<PupilsDataHubObj> listpupils = new List<PupilsDataHubObj>();
                DatahubCentre centreName = new DatahubCentre();

                if (centretype.Equals("1")) { 
                //get list all pupils data by school
                    if (seedcode.Equals("1002"))
                    {
                        templistpupils = Getlistpupil(rpGeneric2nd, sYear);
                        centreName = new DatahubCentre("1002", "Aberdeen City", "1");
                    }
                    else
                    {
                        templistpupils = Getlistpupil(rpGeneric2nd, sYear).Where(x => (x.SEED_Code != null && x.SEED_Code.Equals(seedcode))).ToList(); ;
                        centreName = GetListSchoolname().Where(x => x.seedcode.Equals(seedcode)).First();
                    }               
                }else if(centretype.Equals("2")){
                //get list pupils data by neighbourhood
                    if (seedcode.Equals("1002"))
                    {
                        templistpupils = Getlistpupil(rpGeneric2nd, sYear);
                        centreName = new DatahubCentre("1002", "Aberdeen City", "2");
                    }
                    else
                    {
                        templistpupils = GetDatahubdatabyNeighbourhoods(rpGeneric2nd, seedcode, sYear);
                        centreName = GetListNeighbourhoodsname(rpGeneric2nd).Where(x=>x.seedcode.Equals(seedcode)).First();                       
                    }

                }


                switch (dataname.ToLower())
                {
                    case "allclients":
                        listpupils = (from a in templistpupils where !a.SDS_Client_Ref.Equals("n/a") select a).ToList();
                        break;
                    case "male":
                        listpupils = (from a in templistpupils where a.Gender.ToLower().Equals("male") select a).ToList();                         
                        break;
                    case "female":
                        listpupils = (from a in templistpupils where a.Gender.ToLower().Equals("female") select a).ToList();
                        break;
                    case "pupils16":
                        listpupils = (from a in templistpupils where a.Age == 16 select a).ToList();
                        break;
                    case "pupils17":
                        listpupils = (from a in templistpupils where a.Age == 17 select a).ToList();
                        break;
                    case "pupils18":
                        listpupils = (from a in templistpupils where a.Age == 18 select a).ToList();
                        break;
                    case "pupils19":
                        listpupils = (from a in templistpupils where a.Age == 19 select a).ToList();
                        break;
                    case "movedoutwithscotland":
                        listpupils = (from a in templistpupils where a.status_code.ToLower().Equals("000") select a).ToList();
                        break;
                    default:
                        listpupils = (from a in templistpupils where a.status_code.ToLower().Equals(dataname.ToLower()) select a).ToList();
                        break;

                }

                var listdatasetdate = GetListDataSetDate();

                object oResult = null;

                oResult = new
                {
                    ListPupils = listpupils.Select(x => new
                    {
                        Forename = x.Forename,
                        Surname = x.Surname,
                        Age = x.Age,
                        Gender = x.Gender,
                        CSS_Address = x.CSS_Address,
                        CSS_Postcode = x.CSS_Postcode,
                        Telephone = x.Telephone_Number,
                    }).OrderBy(x=>x.Age).ThenBy(x=>x.Forename),
                    DatasetSelected = listdatasetdate.Where(x => x.code.Equals(sYear)).Select(x => x.GetJson()).First(),
                    DataTitle = dataname,
                    CentreName = centreName
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