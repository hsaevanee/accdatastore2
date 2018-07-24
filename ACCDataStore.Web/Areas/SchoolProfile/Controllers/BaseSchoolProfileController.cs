using ACCDataStore.Entity;
using ACCDataStore.Entity.SchoolProfile;
using ACCDataStore.Repository;
using ACCDataStore.Web.Controllers;
using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.SchoolProfile.Controllers
{
    public class BaseSchoolProfileController : BaseController
    {
        private static ILog log = LogManager.GetLogger(typeof(BaseSchoolProfileController));

        //private readonly IGenericRepository rpGeneric;

        //public BaseSchoolProfileController(IGenericRepository rpGeneric)
        //{
        //    this.rpGeneric = rpGeneric;
        //}

        //public BaseSchoolProfileController()
        //{
        //   // this.rpGeneric = rpGeneric;
        //}

        protected List<EthnicObj> GetEthnicityDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetEthnicityData in BaseSchoolProfileController==> ");

            //List<EthnicObj> listDataseries = new List<EthnicObj>();
            List<EthnicObj> listtemp = new List<EthnicObj>();
            //List<EthnicObj> listtemp1 = new List<EthnicObj>();
            EthnicObj tempEthnicObj = new EthnicObj();

            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select EthnicBackground,Gender,(Count(EthnicBackground)* 100 / (Select Count(*) From sch_Student_t))  From sch_Student_t  Group By EthnicBackground, Gender  ");
            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Ethniccode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Ethniccode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempEthnicObj = new EthnicObj();
                        foreach (var itemRow in templist2)
                        {
                            tempEthnicObj.EthinicCode = Convert.ToString(itemRow[0]);
                            tempEthnicObj.EthinicName = GetDicEhtnicBG().ContainsKey(tempEthnicObj.EthinicCode) ? GetDicEhtnicBG()[tempEthnicObj.EthinicCode] : "NO NAME";

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempEthnicObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempEthnicObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempEthnicObj);
                    }
                }
            }


            //% for specific schoolname
            string query = " Select EthnicBackground,Gender, (Count(EthnicBackground)* 100 /";
            query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By EthnicBackground, Gender ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                // need to select only the Ethniccode that appear for this specific school
                //var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                //foreach (var Ethniccode in DistinctItems)
                //{
                //    tempEthnicObj = listtemp.Find(x => x.EthinicCode.Equals(Ethniccode.Key));
                //    if (tempEthnicObj != null)
                //        listDataseries.Add(tempEthnicObj);
                //}


                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.EthinicCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempEthnicObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempEthnicObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempEthnicObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        //listDataseries.Add(tempEthnicObj);
                    }
                }
            }

            foreach (var itemRow in listtemp)
            {
                tempEthnicObj = itemRow;
                tempEthnicObj.PercentageInSchool = tempEthnicObj.PercentageFemaleInSchool + tempEthnicObj.PercentageMaleInSchool;
                tempEthnicObj.PercentageAllSchool = tempEthnicObj.PercentageFemaleAllSchool + tempEthnicObj.PercentageMaleAllSchool;
            }

            return listtemp;
        }

        protected Dictionary<string, string> GetDicNational()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "Scottish");
            dicNational.Add("02", "English");
            dicNational.Add("03", "Northern Irish");
            dicNational.Add("04", "Welsh");
            dicNational.Add("05", "British");
            dicNational.Add("99", "Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("98", "Not Known");
            return dicNational;
        }
        
        protected Dictionary<string, string> GetDicEhtnicBG()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "White – Scottish");
            dicNational.Add("02", "African – African / Scottish / British");
            dicNational.Add("03", "Caribbean or Black – Caribbean / British / Scottish");
            dicNational.Add("05", "Asian – Indian/British/Scottish");
            dicNational.Add("06", "Asian – Pakistani / British / Scottish");
            dicNational.Add("07", "Asian –Bangladeshi / British / Scottish");
            dicNational.Add("08", "Asian – Chinese / British / Scottish");
            dicNational.Add("09", "White – Other");
            dicNational.Add("10", "Not Disclosed");
            dicNational.Add("12", "Mixed or multiple ethnic groups");
            dicNational.Add("17", "Asian – Other");
            dicNational.Add("19", "White – Gypsy/Traveller");
            dicNational.Add("21", "White – Other British");
            dicNational.Add("22", "White – Irish");
            dicNational.Add("23", "White – Polish");
            dicNational.Add("24", "Caribbean or Black – Other");
            dicNational.Add("25", "African – Other");
            dicNational.Add("27", "Other – Arab");
            dicNational.Add("98", "Not Known");
            dicNational.Add("99", "Other – Other");
            return dicNational;
        }

        protected Dictionary<string, string> GetDicGender()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("F", "Female");
            dicNational.Add("M", "Male");
            dicNational.Add("T", "Total");
            return dicNational;
        }

        protected Dictionary<string, string> GetDicLevelEnglish()
        {
            var dicNational = new Dictionary<string, string>();
            dicNational.Add("01", "New to English");
            dicNational.Add("02", "Early Acquisition");
            dicNational.Add("03", "Developing Competence");
            dicNational.Add("04", "Competent");
            dicNational.Add("05", "Fluent");
            dicNational.Add("EN", "English as 'first language'");          
            dicNational.Add("LC", "Limited Communication");           
            dicNational.Add("NA", "Not Assessed");
            return dicNational;
        }


        protected Dictionary<string, string[]> GetDicGenderWithSelected(List<string> listSelectedGender)
        {
            var dicNational = new Dictionary<string, string[]>();
            if (listSelectedGender != null)
            {
                if (listSelectedGender.FirstOrDefault(x => x.Equals("F")) != null)
                {
                    dicNational.Add("F", new string[] { "Female", "checked" });
                }
                else
                {
                    dicNational.Add("F", new string[] { "Female", "" });
                }
                if (listSelectedGender.FirstOrDefault(x => x.Equals("M")) != null)
                {
                    dicNational.Add("M", new string[] { "Male", "checked" });
                }
                else
                {
                    dicNational.Add("M", new string[] { "Male", "" });
                }
                if (listSelectedGender.FirstOrDefault(x => x.Equals("T")) != null)
                {
                    dicNational.Add("T", new string[] { "Total", "checked" });
                }
                else
                {
                    dicNational.Add("T", new string[] { "Total", "" });
                }
            }
            else
            {
                dicNational.Add("F", new string[] { "Female", "checked" });
                dicNational.Add("M", new string[] { "Male", "checked" });
                dicNational.Add("T", new string[] { "Total", "checked" });
            }
            return dicNational;
        }

        protected List<NationalityObj> GetNationalityDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetNationalityData ==> ");

            List<NationalityObj> listDataseries = new List<NationalityObj>();
            List<NationalityObj> listtemp = new List<NationalityObj>();
            NationalityObj tempNationalObj = new NationalityObj();


            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select NationalIdentity,Gender, (Count(NationalIdentity)* 100 / (Select Count(*) From sch_Student_t))  From sch_Student_t  Group By NationalIdentity, Gender ");
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempNationalObj = new NationalityObj();
            //        tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
            //        tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";
            //        tempNationalObj.PercentageAllSchool = Convert.ToDouble(itemRow[1]);
            //        listtemp.Add(tempNationalObj);
            //    }
            //}
            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var Nationalcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(Nationalcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new NationalityObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.IdentityName = GetDicNational().ContainsKey(tempNationalObj.IdentityCode) ? GetDicNational()[tempNationalObj.IdentityCode] : "NO NAME";

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

            //% for specific schoolname
            string query = " Select NationalIdentity, Gender, (Count(NationalIdentity)* 100 /";
            query += " (Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By NationalIdentity, Gender ";

            //Select Count(*)  from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in ('Brimmond Primary School')
            listResult = rpGeneric.FindByNativeSQL(query);

            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Convert.ToString(itemRow[0])));
            //        tempNationalObj.PercentageInSchool = Convert.ToDouble(itemRow[1]);

            //        listDataseries.Add(tempNationalObj);

            //    }
            //}


            //return listDataseries;
            if (listResult != null)
            {
                // need to select only the Ethniccode that appear for this specific school
                //var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                //foreach (var Ethniccode in DistinctItems)
                //{
                //    tempNationalObj = listtemp.Find(x => x.IdentityCode.Equals(Ethniccode.Key));
                //    if (tempNationalObj != null)
                //        listDataseries.Add(tempNationalObj);
                //}


                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.IdentityCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempNationalObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempNationalObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempNationalObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        //listDataseries.Add(tempEthnicObj);
                    }
                }
            }

            foreach (var itemRow in listtemp)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageInSchool = tempNationalObj.PercentageFemaleInSchool + tempNationalObj.PercentageMaleInSchool;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listtemp;
        }

        protected List<SIMDObj> GetSIMDDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname, List<string> myear)
        {
            Console.Write("GetSIMDDatabySchoolname in BaseSchoolProfileController ==> ");

            List<SIMDObj> listDataseries = new List<SIMDObj>();
            List<SIMDObj> listtemp = new List<SIMDObj>();
            SIMDObj tempSIMDObj = new SIMDObj();
            
            //should loop through myear

            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select SIMD_2012_decile, (Count(*)* 100 / (Select Count(*) From test_3))  From test_3  Group By SIMD_2012_decile ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = new SIMDObj();
                    tempSIMDObj.SIMDCode = Convert.ToString(itemRow[0]);
                    tempSIMDObj.PercentageAllSchool2012 = Convert.ToDouble(itemRow[1]);
                    listtemp.Add(tempSIMDObj);
                }
            }

            listResult = rpGeneric.FindByNativeSQL("Select SIMD_2009_decile, (Count(*)* 100 / (Select Count(*) From test_3))  From test_3  Group By SIMD_2009_decile ");
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
                    tempSIMDObj.PercentageAllSchool2009 = Convert.ToDouble(itemRow[1]);
                }
            }

            string query = " Select SIMD_2012_decile, (Count(*)* 100 /";
            query += " (Select Count(*) From test_3 where Name in (\"" + mSchoolname + "\")))";
            query += " From test_3 where Name in (\"" + mSchoolname + "\") Group By SIMD_2012_decile ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
                    tempSIMDObj.PercentageInSchool2012 = Convert.ToDouble(itemRow[1]);



                }
            }

            query = " Select SIMD_2009_decile, (Count(*)* 100 /";
            query += " (Select Count(*) From test_3 where Name in (\"" + mSchoolname + "\")))";
            query += " From test_3 where Name in (\"" + mSchoolname + "\") Group By SIMD_2009_decile ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
                    tempSIMDObj.PercentageInSchool2009 = Convert.ToDouble(itemRow[1]);



                }
            }







            //% for All school
            //var listResult = rpGeneric.FindByNativeSQL("Select SIMD_2012_decile, (Count(SIMD_2012_decile)* 100 / (Select Count(*) From test_3))  From test_3  Group By SIMD_2012_decile ");
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempSIMDObj = new SIMDObj();
            //        tempSIMDObj.SIMDCode = Convert.ToString(itemRow[0]);
            //        tempSIMDObj.PercentageAllSchool2012 = Convert.ToDouble(itemRow[1]);
            //        listtemp.Add(tempSIMDObj);
            //    }
            //}


            ////% for specific schoolname
            //string query = " Select SIMD_2012_decile, (Count(SIMD_2012_decile)* 100 /";
            //query += " (Select Count(*) From test_3 where Name in ('" + mSchoolname + " ')))";
            //query += " From test_3 where Name in ('" + mSchoolname + " ') Group By SIMD_2012_decile ";

            //listResult = rpGeneric.FindByNativeSQL(query);
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        tempSIMDObj = listtemp.Find(x => x.SIMDCode.Equals(Convert.ToString(itemRow[0])));
            //        tempSIMDObj.PercentageInSchool2012 = Convert.ToDouble(itemRow[1]);

            //        listDataseries.Add(tempSIMDObj);

            //    }
            //}
            return listtemp;
        }

        protected List<StdStageObj> GetStudentStageDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetStdStageDatabySchoolname in BaseSchoolProfileController==> ");

            List<StdStageObj> listDataseries = new List<StdStageObj>();
            List<StdStageObj> listtemp = new List<StdStageObj>();
            List<StdStageObj> listtemp1 = new List<StdStageObj>();
            StdStageObj tempStdStageObj = new StdStageObj();

            //% for All school
            //var listResult = rpGeneric.FindByNativeSQL("Select StudentStage,Gender,(Count(*)* 100 / (Select Count(*) From test_3))  From test_3  Group By StudentStage, Gender ");
            var listResult = rpGeneric.FindByNativeSQL("Select StudentStage,Gender,(Count(*)* 100 / (Select Count(*) From sch_Student_t))  From sch_Student_t  Group By StudentStage, Gender ");
            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var StdStagecode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(StdStagecode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempStdStageObj = new StdStageObj();
                        foreach (var itemRow in templist2)
                        {
                            tempStdStageObj.StageCode = Convert.ToString(itemRow[0]);                            

                            //tempEthnicObj.EthnicGender = Convert.ToString(itemRow[1]);
                            if ("F".Equals(Convert.ToString(itemRow[1])))
                            {
                                tempStdStageObj.PercentageFemaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }
                            else
                            {
                                tempStdStageObj.PercentageMaleAllSchool = Convert.ToDouble(itemRow[2]);
                            }

                        }

                        listtemp.Add(tempStdStageObj);
                    }
                }
            }


            //% for specific schoolname
            // old query
            //string query = " Select StudentStage,Gender, (Count(*)* 100 /";
            //query += " (Select Count(*) From test_3 where Name in (\"" + mSchoolname + "\")))";
            //query += " From test_3 where Name in (\"" + mSchoolname + "\") Group By StudentStage, Gender ";

            string query = " Select StudentStage,Gender, (Count(*)* 100 /";
            query += " (Select Count(*) from sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By StudentStage, Gender ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                // need to select only the Ethniccode that appear for this specific school
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var StdStagecode in DistinctItems)
                {
                    tempStdStageObj = listtemp.Find(x => x.StageCode.Equals(StdStagecode.Key));
                    if (tempStdStageObj != null)
                        listDataseries.Add(tempStdStageObj);
                }


                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.StageCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempStdStageObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempStdStageObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempStdStageObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        //listDataseries.Add(tempEthnicObj);
                    }
                }
            }

            foreach (var itemRow in listDataseries)
            {
                tempStdStageObj = itemRow;
                tempStdStageObj.PercentageInSchool = tempStdStageObj.PercentageFemaleInSchool + tempStdStageObj.PercentageMaleInSchool;
                tempStdStageObj.PercentageAllSchool = tempStdStageObj.PercentageFemaleAllSchool + tempStdStageObj.PercentageMaleAllSchool;
            }

            return listDataseries;
        }


        protected List<NationalityObj> GetLevelENDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetLevelENDatabySchoolname ==> ");

            //List<NationalityObj> listDataseries = new List<NationalityObj>();
            List<NationalityObj> listtemp = new List<NationalityObj>();
            NationalityObj tempNationalObj = new NationalityObj();


            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select LevelOfEnglish,Gender, (Count(*)* 100 / (Select Count(*) From sch_Student_t))  From sch_Student_t  Group By LevelOfEnglish, Gender ");

            if (listResult != null)
            {
                var DistinctItems = listResult.GroupBy(x => x.ElementAt(0).ToString()).ToList();

                foreach (var LevelENcode in DistinctItems)
                {
                    var templist2 = (from a in listResult where a.ElementAt(0).ToString().Equals(LevelENcode.Key) select a).ToList();

                    if (templist2.Count != 0)
                    {
                        tempNationalObj = new NationalityObj();
                        foreach (var itemRow in templist2)
                        {
                            tempNationalObj.IdentityCode = Convert.ToString(itemRow[0]);
                            tempNationalObj.IdentityName = GetDicLevelEnglish().ContainsKey(tempNationalObj.IdentityCode) ? GetDicLevelEnglish()[tempNationalObj.IdentityCode] : "NO NAME";

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

            //% for specific schoolname
            string query = " Select LevelOfEnglish, Gender, (Count(*)* 100 /";
            query += " (Select Count(*) From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\")))";
            query += " From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode where t2.Name in (\"" + mSchoolname + "\") Group By LevelOfEnglish, Gender ";

            listResult = rpGeneric.FindByNativeSQL(query);
            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.IdentityCode.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempNationalObj = x[0];
                        if ("F".Equals(Convert.ToString(itemRow[1])))
                        {
                            tempNationalObj.PercentageFemaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                        else
                        {
                            tempNationalObj.PercentageMaleInSchool = Convert.ToDouble(itemRow[2]);
                        }
                    }
                }
            }

            foreach (var itemRow in listtemp)
            {
                tempNationalObj = itemRow;
                tempNationalObj.PercentageInSchool = tempNationalObj.PercentageFemaleInSchool + tempNationalObj.PercentageMaleInSchool;
                tempNationalObj.PercentageAllSchool = tempNationalObj.PercentageFemaleAllSchool + tempNationalObj.PercentageMaleAllSchool;
            }

            return listtemp;
        }

        protected List<FSMObj> GetFSMDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname)
        {
            Console.Write("GetFSMDatabySchoolname in BaseSchoolProfileController ==> ");

            List<FSMObj> listtemp = new List<FSMObj>();
            FSMObj tempFSMObj = new FSMObj();


            //% for All school
            var listResult = rpGeneric.FindByNativeSQL("Select t2.Name, count (t1.FreeSchoolMealRegistered) From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on  t1.SeedCode = t2.SeedCode  Group By Name");

            if (listResult != null)
            {

                foreach (var itemRow in listResult)
                {
                    tempFSMObj = new FSMObj();
                    tempFSMObj.schoolname = Convert.ToString(itemRow[0]);
                    //tempFSMObj. = Convert.ToBoolean(Convert.ToInt16(tempFSMObj.IdentityCode)) ? "Registered" : "Not Registered";
                    tempFSMObj.schoolroll = Convert.ToInt16(itemRow[1]);
                    listtemp.Add(tempFSMObj);
                }
            }

            //% for Registered Schoolmeal in each school
            listResult = rpGeneric.FindByNativeSQL("Select t2.Name, count (t1.FreeSchoolMealRegistered) From sch_Student_t t1 INNER JOIN sch_PrimarySchool_t  t2 on t1.SeedCode = t2.SeedCode  Where t1.FreeSchoolMealRegistered='1' Group By Name");

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {
                    var x = (from a in listtemp where a.schoolname.Equals(Convert.ToString(itemRow[0])) select a).ToList();
                    if (x.Count != 0)
                    {
                        tempFSMObj = x[0];
                        tempFSMObj.registeredFSMInSchool = Convert.ToDouble(itemRow[1]);
                        tempFSMObj.PercentageRegisteredInSchool = (tempFSMObj.registeredFSMInSchool / tempFSMObj.schoolroll) * 100;
                    }
                }
            }
            return listtemp;
        }

        protected List<CurriculumObj> GetCurriculumDatabySchoolname(IGenericRepository rpGeneric, string mSchoolname, string colname)
        {
            Console.Write("GetCurriculumDatabySchoolname ==> ");

            List<CurriculumObj> listtemp = new List<CurriculumObj>();
            CurriculumObj tempCurriculumObj = new CurriculumObj();
            
            //% for All school
            var listTempStage = new List<string>() { "P1", "P2", "P3", "P4", "P5", "P6", "P7" };

            foreach (var item in listTempStage)
            {
                listtemp.Add(new CurriculumObj(item, "M"));
                listtemp.Add(new CurriculumObj(item, "F"));
                listtemp.Add(new CurriculumObj(item, "T"));

            }

            //get stage and gender from database but it is included SP stage
            //var listResult = rpGeneric.FindByNativeSQL("Select distinct StudentStage,Gender From sch_Student_t  Group By StudentStage, Gender ");
            //if (listResult != null)
            //{
            //    foreach (var itemRow in listResult)
            //    {
            //        listtemp.Add(new CurriculumObj(Convert.ToString(itemRow[0]), Convert.ToString(itemRow[1])));
            //    }
            //}

            //List<CurriculumObj> listtemp = listtempdata.OrderBy(x => x.stage).ToList();
 
            string query = " Select StudentStage, Gender, " + colname + ", Count(*)";            
            query += " From test_3 where Name in (\"" + mSchoolname + "\") Group By StudentStage, Gender," + colname;


            var listResult = rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
                {
                    foreach (var itemRow in listResult)
                    {

                        var x = (from a in listtemp where a.stage.Equals(Convert.ToString(itemRow[0])) && a.gender.Equals(Convert.ToString(itemRow[1])) select a).ToList();
                        if (x.Count != 0)
                        {

                            tempCurriculumObj = x[0];                          
                            if (itemRow[2] == null)
                            {
                                tempCurriculumObj.blank = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Early"))
                            {
                                tempCurriculumObj.early = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Early Consolidating"))
                            {
                                tempCurriculumObj.earlyconsolidating = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Early Developing"))
                            {
                                tempCurriculumObj.earlydeveloping = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Early Secure"))
                            {
                                tempCurriculumObj.earlysecure = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("First Consolidating"))
                            {
                                tempCurriculumObj.firstconsolidating = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("First Developing"))
                            {
                                tempCurriculumObj.firstdeveloping = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("First Secure"))
                            {
                                tempCurriculumObj.firstsecure = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Second Consolidating"))
                            {
                                tempCurriculumObj.secondconsolidating = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Second Developing"))
                            {
                                tempCurriculumObj.seconddeveloping = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Second Secure"))
                            {
                                tempCurriculumObj.secondsecure = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Third Consolidating"))
                            {
                                tempCurriculumObj.thirdconsolidating = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Third Developing"))
                            {
                                tempCurriculumObj.thirddeveloping = Convert.ToDouble(itemRow[3]);
                            }
                            else if (Convert.ToString(itemRow[2]).Equals("Third Secure"))
                            {
                                tempCurriculumObj.thirdsecure = Convert.ToDouble(itemRow[3]);
                            }
                        }
                    }
                }


            query = " Select StudentStage, Gender, Count(*) From test_3 where Name in (\"" + mSchoolname + "\") Group by StudentStage ,Gender";


            listResult = rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {

                    var x = (from a in listtemp where a.stage.Equals(Convert.ToString(itemRow[0])) && a.gender.Equals(Convert.ToString(itemRow[1])) select a).ToList();
                    if (x.Count != 0)
                    {

                        tempCurriculumObj = x[0];
                        tempCurriculumObj.sumpupils = Convert.ToDouble(itemRow[2]);
                    }


                }

            }

            //query = " Select StudentStage, " + colname + ", Count(*)";
            //query += " From test_3 where DataZone in (\"" + pZonecode + "\")  Group By StudentStage," + colname;

            query = " Select StudentStage, " + colname + ", Count(*)";
            query += " From test_3 where Name in (\"" + mSchoolname + "\") Group By StudentStage," + colname;


            listResult = rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {

                    var x = (from a in listtemp where a.stage.Equals(Convert.ToString(itemRow[0])) && a.gender.Equals("T") select a).ToList();
                    if (x.Count != 0)
                    {

                        tempCurriculumObj = x[0];


                        if (itemRow[1] == null)
                        {
                            tempCurriculumObj.blank = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Early"))
                        {
                            tempCurriculumObj.early = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Early Consolidating"))
                        {
                            tempCurriculumObj.earlyconsolidating = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Early Developing"))
                        {
                            tempCurriculumObj.earlydeveloping = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Early Secure"))
                        {
                            tempCurriculumObj.earlysecure = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("First Consolidating"))
                        {
                            tempCurriculumObj.firstconsolidating = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("First Developing"))
                        {
                            tempCurriculumObj.firstdeveloping = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("First Secure"))
                        {
                            tempCurriculumObj.firstsecure = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Second Consolidating"))
                        {
                            tempCurriculumObj.secondconsolidating = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Second Developing"))
                        {
                            tempCurriculumObj.seconddeveloping = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Second Secure"))
                        {
                            tempCurriculumObj.secondsecure = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Third Consolidating"))
                        {
                            tempCurriculumObj.thirdconsolidating = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Third Developing"))
                        {
                            tempCurriculumObj.thirddeveloping = Convert.ToDouble(itemRow[2]);
                        }
                        else if (Convert.ToString(itemRow[1]).Equals("Third Secure"))
                        {
                            tempCurriculumObj.thirdsecure = Convert.ToDouble(itemRow[2]);
                        }
                    }

                }
                // listtemp.Add(tempCurriculumObj);
            }

            query = " Select StudentStage, Count(*) From test_3 where Name in (\"" + mSchoolname + "\") Group by StudentStage";


            listResult = rpGeneric.FindByNativeSQL(query);

            if (listResult != null)
            {
                foreach (var itemRow in listResult)
                {

                    var x = (from a in listtemp where a.stage.Equals(Convert.ToString(itemRow[0])) && a.gender.Equals("T")  select a).ToList();
                    if (x.Count != 0)
                    {

                        tempCurriculumObj = x[0];
                        tempCurriculumObj.sumpupils = Convert.ToDouble(itemRow[1]);
                    }


                }

            }

            List<CurriculumObj> listtemp1 = listtemp.OrderBy(x => x.stage).ToList();

            foreach (var itemRow in listtemp1)
            {
                tempCurriculumObj = itemRow;
                tempCurriculumObj.blank = (tempCurriculumObj.blank * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.early = (tempCurriculumObj.early * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.earlysecure = (tempCurriculumObj.earlysecure * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.earlyconsolidating = (tempCurriculumObj.earlyconsolidating * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.earlydeveloping = (tempCurriculumObj.earlydeveloping * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.firstdeveloping = (tempCurriculumObj.firstdeveloping * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.firstconsolidating = (tempCurriculumObj.firstconsolidating * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.firstsecure = (tempCurriculumObj.firstsecure * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.seconddeveloping = (tempCurriculumObj.seconddeveloping * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.secondconsolidating = (tempCurriculumObj.secondconsolidating * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.secondsecure = (tempCurriculumObj.secondsecure * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.thirddeveloping = (tempCurriculumObj.thirddeveloping * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.thirdconsolidating = (tempCurriculumObj.thirdconsolidating * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.thirdsecure = (tempCurriculumObj.thirdsecure * 100) / tempCurriculumObj.sumpupils;
                tempCurriculumObj.grandtotal = tempCurriculumObj.blank + tempCurriculumObj.early + tempCurriculumObj.earlysecure + tempCurriculumObj.earlydeveloping + tempCurriculumObj.earlyconsolidating + tempCurriculumObj.firstsecure + tempCurriculumObj.firstdeveloping + tempCurriculumObj.firstconsolidating + tempCurriculumObj.secondsecure + tempCurriculumObj.seconddeveloping + tempCurriculumObj.secondconsolidating + tempCurriculumObj.thirddeveloping + tempCurriculumObj.thirdconsolidating + tempCurriculumObj.thirdsecure;

            }

            return listtemp1;
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
                    award2013 = x.Sum(y =>y.award2013),
                    award2014 = x.Sum(y => y.award2014),
                    award2015 = x.Sum(y => y.award2015),
                }).ToList();

            }

            temp = temp.OrderByDescending(x => x.age_range).ThenBy(x => x.awardname).ToList();
 
            return temp;
        }
 
    }
}