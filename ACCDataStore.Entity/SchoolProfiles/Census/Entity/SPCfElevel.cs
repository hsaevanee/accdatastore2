using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPCfElevel  
    {
        public Year year;
        public string seedcode;
        public List<GenericSchoolData> ListThirdlevel { get; set; }
        public List<GenericSchoolData> ListForthlevel{ get; set; }
        public List<GenericSchoolData> ListThirdlevel_no { get; set; }
        public List<GenericSchoolData> ListForthlevel_no { get; set; }
        public List<GenericSchoolData> SIMDQ1_3Dlevel { get; set; }
        public List<GenericSchoolData> SIMDQ2_3Dlevel { get; set; }
        public List<GenericSchoolData> SIMDQ3_3Dlevel { get; set; }
        public List<GenericSchoolData> SIMDQ4_3Dlevel { get; set; }
        public List<GenericSchoolData> SIMDQ5_3Dlevel { get; set; }
        public List<GenericSchoolData> SIMDQ1_4level { get; set; }
        public List<GenericSchoolData> SIMDQ2_4level { get; set; }
        public List<GenericSchoolData> SIMDQ3_4level { get; set; }
        public List<GenericSchoolData> SIMDQ4_4level { get; set; }
        public List<GenericSchoolData> SIMDQ5_4level { get; set; }

        public List<GenericSchoolData> P1EarlyLevel { get; set; }
        public List<GenericSchoolData> P4FirstLevel { get; set; }
        public List<GenericSchoolData> P7SecondLevel { get; set; }

        public List<GenericSchoolData> P1EarlyLevelQ1 { get; set; }
        public List<GenericSchoolData> P1EarlyLevelQ2 { get; set; }
        public List<GenericSchoolData> P1EarlyLevelQ3 { get; set; }
        public List<GenericSchoolData> P1EarlyLevelQ4 { get; set; }
        public List<GenericSchoolData> P1EarlyLevelQ5 { get; set; }

        public List<GenericSchoolData> P4FirstLevelQ1 { get; set; }
        public List<GenericSchoolData> P4FirstLevelQ2 { get; set; }
        public List<GenericSchoolData> P4FirstLevelQ3 { get; set; }
        public List<GenericSchoolData> P4FirstLevelQ4 { get; set; }
        public List<GenericSchoolData> P4FirstLevelQ5 { get; set; }

        public List<GenericSchoolData> P7SecondLevelQ1 { get; set; }
        public List<GenericSchoolData> P7SecondLevelQ2 { get; set; }
        public List<GenericSchoolData> P7SecondLevelQ3 { get; set; }
        public List<GenericSchoolData> P7SecondLevelQ4 { get; set; }
        public List<GenericSchoolData> P7SecondLevelQ5 { get; set; }

        public List<GenericSchoolData> P1EarlyLevel_no { get; set; }
        public List<GenericSchoolData> P4FirstLevel_no { get; set; }
        public List<GenericSchoolData> P7SecondLevel_no { get; set; }

        //public List<GenericSchoolData> Reading_3betterSIMDQ { get; set; }
        //public List<GenericSchoolData> Reading_4SIMDQ { get; set; }
        //public List<GenericSchoolData> Writing_3betterSIMDQ { get; set; }
        //public List<GenericSchoolData> Writing_4SIMDQ { get; set; }
        //public List<GenericSchoolData> ELT_3betterSIMDQ { get; set; }
        //public List<GenericSchoolData> ELT_4SIMDQ { get; set; }
        //public List<GenericSchoolData> Numeracy_3betterSIMDQ { get; set; }
        //public List<GenericSchoolData> Numeracy_4SIMDQ { get; set; }

        public BaseSchoolProfile getP1EarlybySubjectAndSIMD(string subject)
        {

            BaseSchoolProfile temp = new BaseSchoolProfile();
            temp.YearInfo = year;
            //temp.ListGenericSchoolData = new List<GenericSchoolData>();
            temp.ListGenericSchoolData.Add(P1EarlyLevelQ1.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P1EarlyLevelQ2.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P1EarlyLevelQ3.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P1EarlyLevelQ4.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P1EarlyLevelQ5.Where(x => x.Code.Equals(subject)).FirstOrDefault());
 
            return temp;
        
        
        }

        public BaseSchoolProfile getP4FirstLevelbySubjectAndSIMD(string subject)
        {

            BaseSchoolProfile temp = new BaseSchoolProfile();
            temp.YearInfo = year;
            temp.ListGenericSchoolData.Add(P4FirstLevelQ1.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P4FirstLevelQ2.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P4FirstLevelQ3.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P4FirstLevelQ4.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P4FirstLevelQ5.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            
            return temp;
        }

        public BaseSchoolProfile getP7SecondLevelbySubjectAndSIMD(string subject)
        {

            BaseSchoolProfile temp = new BaseSchoolProfile();
            temp.YearInfo = year;

            temp.ListGenericSchoolData.Add(P7SecondLevelQ1.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P7SecondLevelQ2.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P7SecondLevelQ3.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P7SecondLevelQ4.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(P7SecondLevelQ5.Where(x => x.Code.Equals(subject)).FirstOrDefault());

            return temp;
        }

        public BaseSchoolProfile getS3ThirdLevelbySubjectAndSIMD(string subject)
        {

            BaseSchoolProfile temp = new BaseSchoolProfile();
            temp.YearInfo = year;

            temp.ListGenericSchoolData.Add(SIMDQ1_3Dlevel.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ2_3Dlevel.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ3_3Dlevel.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ4_3Dlevel.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ5_3Dlevel.Where(x => x.Code.Equals(subject)).FirstOrDefault());

            return temp;
        }
        public BaseSchoolProfile getS3ForthLevelbySubjectAndSIMD(string subject)
        {

            BaseSchoolProfile temp = new BaseSchoolProfile();
            temp.YearInfo = year;

            temp.ListGenericSchoolData.Add(SIMDQ1_4level.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ2_4level.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ3_4level.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ4_4level.Where(x => x.Code.Equals(subject)).FirstOrDefault());
            temp.ListGenericSchoolData.Add(SIMDQ5_4level.Where(x => x.Code.Equals(subject)).FirstOrDefault());

            return temp;
        }
    }
}
