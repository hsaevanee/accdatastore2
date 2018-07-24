using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class PrimaryCfElevel  
    {
        public Year year;
        public string seedcode;
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
    }
}
