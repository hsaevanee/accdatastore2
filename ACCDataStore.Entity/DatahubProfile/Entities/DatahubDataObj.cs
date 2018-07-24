using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile
{
    public class DatahubDataObj : BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual string Cohort { get; set; }
        public virtual string Forename { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Preferred_Forename { get; set; }
        public virtual string CSS_Address { get; set; }
        public virtual string CSS_Postcode { get; set; }
        public virtual string LA_Address { get; set; }
        public virtual string LA_Postcode { get; set; }
        public virtual string Telephone_Number { get; set; }
        public virtual DateTime Date_of_Birth { get; set; }
        public virtual int Age { get; set; }
        public virtual string Gender { get; set; }
        public virtual string SDS_Client_Ref { get; set; }
        public virtual string Scottish_Candidate_Number { get; set; }
        public virtual DateTime Statutory_Leave_Date { get; set; }
        public virtual string SEED_Code { get; set; }
        public virtual string School_Name { get; set; }
        public virtual string School_MIS_Reference { get; set; }
        public virtual DateTime Start_Date { get; set; }
        public virtual DateTime Anticipated_School_Leaving_Date { get; set; }
        public virtual DateTime Actual_Date_Left_School { get; set; }
        public virtual string School_Year_Group { get; set; }
        public virtual string School_Roll_Status_Code { get; set; }
        public virtual string School_History_Source { get; set; }
        public virtual string Preferred_Occupation { get; set; }
        public virtual string Preferred_Occupation_Source { get; set; }
        public virtual string Preferred_Route { get; set; }
        public virtual string Preferred_Route_Source { get; set; }
        public virtual string Current_Status { get; set; }
        public virtual string Status_Source { get; set; }
        public virtual string Conditional_Status { get; set; }
        public virtual string Status_Start_Date { get; set; }
        public virtual string Organisation_Name { get; set; }
        public virtual string Course_Title { get; set; }
        public virtual string Course_Level { get; set; }
        public virtual string Employer_Name { get; set; }
        public virtual string Job_Title { get; set; }
        public virtual DateTime End_Date { get; set; }
        public virtual string Weeks_since_last_Pos_Status { get; set; }
        public virtual string Last_Positive_Status { get; set; }
        public virtual DateTime Last_Engagement_with_SDS { get; set; }
        public virtual string Benefit_Types { get; set; }
        public virtual string Benefit_Source { get; set; }
        public virtual string Looked_After_Status { get; set; }
        public virtual string Looked_After_Source { get; set; }
        public virtual string Young_Carer { get; set; }
        public virtual string Young_Carer_Source { get; set; }
        public virtual string ASN { get; set; }
        public virtual string ASN_Source { get; set; }
        public virtual string IEP { get; set; }
        public virtual string IEP_Source { get; set; }
        public virtual string CSP { get; set; }
        public virtual string CSP_Source { get; set; }
        public virtual string Transition_Plan { get; set; }
        public virtual string Transition_Plan_Source { get; set; }
        public virtual string Childs_Plan { get; set; }
        public virtual string Childs_Plan_Source { get; set; }
        public virtual int Data_Month { get; set; }
        public virtual int Data_Year { get; set; }
    }

    public class DatahubDataGlasgow : DatahubDataObj { }
    public class DatahubDataAberdeen : DatahubDataObj { }
    public class DatahubDataAberdeenshire : DatahubDataObj { }
}
