using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Mappings
{
    public class DatahubDataAberdeenMap : ClassMap<DatahubDataAberdeen>
    {
        public DatahubDataAberdeenMap()
        {
            Table("datahubdata_aberdeen");
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Cohort);
            Map(x => x.Forename);
            Map(x => x.Surname);
            Map(x => x.Preferred_Forename);
            Map(x => x.CSS_Address);
            Map(x => x.CSS_Postcode);
            Map(x => x.LA_Address);
            Map(x => x.LA_Postcode);
            Map(x => x.Telephone_Number);
            Map(x => x.Date_of_Birth);
            Map(x => x.Age);
            Map(x => x.Gender);
            Map(x => x.SDS_Client_Ref);
            Map(x => x.Scottish_Candidate_Number);
            Map(x => x.Statutory_Leave_Date);
            Map(x => x.SEED_Code);
            Map(x => x.School_Name);
            Map(x => x.School_MIS_Reference);
            Map(x => x.Start_Date);
            Map(x => x.Anticipated_School_Leaving_Date);
            Map(x => x.Actual_Date_Left_School);
            Map(x => x.School_Year_Group);
            Map(x => x.School_Roll_Status_Code);
            Map(x => x.School_History_Source);
            Map(x => x.Preferred_Occupation);
            Map(x => x.Preferred_Occupation_Source);
            Map(x => x.Preferred_Route);
            Map(x => x.Preferred_Route_Source);
            Map(x => x.Current_Status);
            Map(x => x.Status_Source);
            Map(x => x.Conditional_Status);
            Map(x => x.Status_Start_Date);
            Map(x => x.Organisation_Name);
            Map(x => x.Course_Title);
            Map(x => x.Course_Level);
            Map(x => x.Employer_Name);
            Map(x => x.Job_Title);
            Map(x => x.End_Date);
            Map(x => x.Weeks_since_last_Pos_Status);
            Map(x => x.Last_Positive_Status);
            Map(x => x.Last_Engagement_with_SDS);
            Map(x => x.Benefit_Types);
            Map(x => x.Benefit_Source);
            Map(x => x.Looked_After_Status);
            Map(x => x.Looked_After_Source);
            Map(x => x.Young_Carer);
            Map(x => x.Young_Carer_Source);
            Map(x => x.ASN);
            Map(x => x.ASN_Source);
            Map(x => x.IEP);
            Map(x => x.IEP_Source);
            Map(x => x.CSP);
            Map(x => x.CSP_Source);
            Map(x => x.Transition_Plan);
            Map(x => x.Transition_Plan_Source);
            Map(x => x.Childs_Plan);
            Map(x => x.Childs_Plan_Source);
            Map(x => x.Data_Month);
            Map(x => x.Data_Year);
        }
    }
}
