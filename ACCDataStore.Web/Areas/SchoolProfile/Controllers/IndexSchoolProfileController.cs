using ACCDataStore.Repository;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ACCDataStore.Entity;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.EthnicBackground;
using ACCDataStore.Web.Areas.SchoolProfile.ViewModels.IndexSchoolProfile;
using ACCDataStore.Entity.SchoolProfile;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class IndexSchoolProfileController : BaseSchoolProfileController
    {
        // GET: SchoolProfile/IndexSchoolProfile
        private static ILog log = LogManager.GetLogger(typeof(IndexSchoolProfileController));

        private readonly IGenericRepository rpGeneric;

        public IndexSchoolProfileController(IGenericRepository rpGeneric)
        {
            this.rpGeneric = rpGeneric;
        }

        //private void SetDefaultCounter()
        //{
        //    var eGeneralCounter = new GeneralCounter();
        //    eGeneralCounter.Module1Counter = 10;
        //    eGeneralCounter.Module2Counter = 20;
        //    eGeneralCounter.SiteCounter = 100;
        //    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralCounter, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));
        //}

        //public ActionResult Index()
        //{
        //    //SetDefaultCounter();
        //    var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
        //    eGeneralSettings.SiteCounter++;
        //    TS.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

        //    //var vmIndex = new IndexViewModel();
        //    //var result = this.rpGeneric.FindAll<StudentSIMD>();
        //    var vmIndexSchoolProfile = new IndexSchoolProfileViewModel();

        //    var sSchoolName = Request["selectedschoolname"];

        //    var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

        //    List<string> fooList = listResult.OfType<string>().ToList();

        //    vmIndexSchoolProfile.ListSchoolNameData = fooList;

        //    if (sSchoolName == null)
        //    {
        //        sSchoolName = fooList[0];
        //    }

        //    vmIndexSchoolProfile.selectedschoolname = sSchoolName;
        //    vmIndexSchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName);
        //    vmIndexSchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName);
        //    vmIndexSchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName, new List<string>(new string[] { "2012" }));
        //    vmIndexSchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName);
        //    vmIndexSchoolProfile.ListLevelENData = GetLevelENDatabySchoolname(this.rpGeneric, sSchoolName);            
        //    List<string> TempCode = new List<string>();

        //    listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");

        //    if (listResult != null)
        //    {
        //        foreach (var itemRow in listResult)
        //        {
        //            if (itemRow != null)
        //            {
        //                TempCode.Add(itemRow.ToString());
        //            }
        //        }
        //    }


        //    vmIndexSchoolProfile.ListSIMDCode = TempCode;


        //    listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

        //    fooList = listResult.OfType<string>().ToList();

        //    vmIndexSchoolProfile.ListEthnicCode = fooList;

        //    vmIndexSchoolProfile.DicEthnicBG = GetDicEhtnicBG();


        //    listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

        //    fooList = listResult.OfType<string>().ToList();
        //    vmIndexSchoolProfile.ListNationalityCode = fooList;
        //    vmIndexSchoolProfile.DicNational = GetDicNational();

        //    listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

        //    fooList = listResult.OfType<string>().ToList();
        //    vmIndexSchoolProfile.ListStageCode = fooList;

        //    return View("index", vmIndexSchoolProfile);



        //}

        public ActionResult Index()
        {
            //SetDefaultCounter();
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.SiteCounter++;
            ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

            //var vmIndex = new IndexViewModel();
            //var result = this.rpGeneric.FindAll<StudentSIMD>();
            var vmIndexSchoolProfile = new IndexSchoolProfileViewModel();

            var sSchoolName = Request["selectedschoolname"];

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            List<string> fooList = listResult.OfType<string>().ToList();

            vmIndexSchoolProfile.ListSchoolNameData = fooList;

            if (sSchoolName == null)
            {
                sSchoolName = fooList[0];
            }

            vmIndexSchoolProfile.selectedschoolname = sSchoolName;
            vmIndexSchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName);
            vmIndexSchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName);
            vmIndexSchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName, new List<string>(new string[] { "2012" }));
            vmIndexSchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName);
            vmIndexSchoolProfile.ListLevelENData = GetLevelENDatabySchoolname(this.rpGeneric, sSchoolName);
            List<string> TempCode = new List<string>();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        TempCode.Add(itemRow.ToString());
                    }
                }
            }


            vmIndexSchoolProfile.ListSIMDCode = TempCode;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmIndexSchoolProfile.ListEthnicCode = fooList;

            vmIndexSchoolProfile.DicEthnicBG = GetDicEhtnicBG();


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmIndexSchoolProfile.ListNationalityCode = fooList;
            vmIndexSchoolProfile.DicNational = GetDicNational();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmIndexSchoolProfile.ListStageCode = fooList;

            return View("index", vmIndexSchoolProfile);



        }

        public ActionResult Compareable()
        {
            //SetDefaultCounter();
            var eGeneralSettings = ACCDataStore.Core.Helper.ConvertHelper.XmlFile2Object(HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"), typeof(GeneralCounter)) as GeneralCounter;
            eGeneralSettings.SchProfilepgCounter++;
            ACCDataStore.Core.Helper.ConvertHelper.Object2XmlFile(eGeneralSettings, HttpContext.Server.MapPath("~/Config/GeneralSettings.xml"));

            var vmIndex2SchoolProfile = new Index2SchoolProfileViewModel();

            List<SchStudent> listdata = this.rpGeneric.FindAll<ACCDataStore.Entity.SchStudent>().ToList();

            List<SchPrimarySchool> PrimarySchoollistdata = this.rpGeneric.FindAll<ACCDataStore.Entity.SchPrimarySchool>().ToList();

            List<string> fooList = PrimarySchoollistdata.Select(x => x.Name).Distinct().ToList();

            var listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW Name FROM test_3 group by Name");

            //List<string> fooList = listResult.OfType<string>().ToList();

            vmIndex2SchoolProfile.ListSchoolNameData = fooList;
            vmIndex2SchoolProfile.ListSchoolNameData2 = fooList;
            var sSchoolName1 = Request["selectedschoolname"];

            var sSchoolName2 = Request["selectedschoolname2"];

            if (sSchoolName1 == null && sSchoolName2 ==null)
            {
                sSchoolName1 = fooList[0];
                sSchoolName2 = fooList[1];
            }

            // Data for sSchoolName1

            vmIndex2SchoolProfile.selectedschoolname = sSchoolName1;
            vmIndex2SchoolProfile.ListEthnicData = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName1);
            vmIndex2SchoolProfile.ListNationalityData = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName1);
            vmIndex2SchoolProfile.ListSIMDData = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName1, new List<string>(new string[] { "2012" }));
            vmIndex2SchoolProfile.ListStdStageData = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName1);
            vmIndex2SchoolProfile.ListLevelENData = GetLevelENDatabySchoolname(this.rpGeneric, sSchoolName1);
            vmIndex2SchoolProfile.ListFSMData = GetFSMDatabySchoolname(this.rpGeneric, sSchoolName1);
            //List<CurriculumObj> temp = GetCurriculumDatabySchoolname(this.rpGeneric, sSchoolName1);

                // Data for sSchoolName2

            vmIndex2SchoolProfile.selectedschoolname2 = sSchoolName2;
            vmIndex2SchoolProfile.ListEthnicData2 = GetEthnicityDatabySchoolname(this.rpGeneric, sSchoolName2);
            vmIndex2SchoolProfile.ListNationalityData2 = GetNationalityDatabySchoolname(this.rpGeneric, sSchoolName2);
            vmIndex2SchoolProfile.ListSIMDData2 = GetSIMDDatabySchoolname(this.rpGeneric, sSchoolName2, new List<string>(new string[] { "2012" }));
            vmIndex2SchoolProfile.ListStdStageData2 = GetStudentStageDatabySchoolname(this.rpGeneric, sSchoolName2);
            vmIndex2SchoolProfile.ListLevelENData2 = GetLevelENDatabySchoolname(this.rpGeneric, sSchoolName2);


            List<string> TempCode = new List<string>();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW SIMD_2012_decile FROM test_3 group by SIMD_2012_decile");
            
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    if (itemRow != null)
                    {
                        TempCode.Add(itemRow.ToString());
                    }
                }
            }


            vmIndex2SchoolProfile.ListSIMDCode = TempCode;


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW EthnicBackground FROM test_3 group by EthnicBackground");

            fooList = listResult.OfType<string>().ToList();

            vmIndex2SchoolProfile.ListEthnicCode = fooList;

            vmIndex2SchoolProfile.DicEthnicBG = GetDicEhtnicBG();


            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW NationalIdentity FROM test_3 group by NationalIdentity");

            fooList = listResult.OfType<string>().ToList();
            vmIndex2SchoolProfile.ListNationalityCode = fooList;
            vmIndex2SchoolProfile.DicNational = GetDicNational();

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW StudentStage FROM test_3 group by StudentStage");

            fooList = listResult.OfType<string>().ToList();
            vmIndex2SchoolProfile.ListStageCode = fooList;

            listResult = this.rpGeneric.FindSingleColumnByNativeSQL("SELECT DISTINCTROW LevelOfEnglish FROM test_3 group by LevelOfEnglish");

            fooList = listResult.OfType<string>().ToList();
            vmIndex2SchoolProfile.ListLevelENCode = fooList;
            vmIndex2SchoolProfile.DicLevelEN = GetDicLevelEnglish();

            vmIndex2SchoolProfile.ListWiderAchievementData = GetWiderAchievementdata(this.rpGeneric);
            return View("index2", vmIndex2SchoolProfile);           
        }
    }
}