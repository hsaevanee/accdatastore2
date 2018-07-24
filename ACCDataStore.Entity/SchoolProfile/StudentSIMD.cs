using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class StudentSIMD : BaseEntity
    {
        public virtual int ID { get; set; }
        public virtual string SchName { get; set; }
        public virtual string Test2_Postcode { get; set; }
        public virtual string Gender { get; set; }
        public virtual string StudentStage { get; set; }
        public virtual string StudentStatus { get; set; }
        public virtual DateTime DateOfBirth{ get; set; }
        public virtual DateTime AdmissionDate { get; set; }
        public virtual string EthnicBackground { get; set; }
        public virtual string NationalIdentity { get; set; }
        public virtual string AsylumStatus { get; set; }
        public virtual string FreeSchoolMealRegistered { get; set; }
        public virtual string BaseSchoolCode { get; set; }
        public virtual string ParentLaCode { get; set; }
        public virtual string ModeOfAttendance { get; set; }
        public virtual string IepIndicator { get; set; }
        public virtual string RonIndicator { get; set; }
        public virtual string MainstreamIntegration { get; set; }
        public virtual string AttendanceSsu { get; set; }
        public virtual string MainDifficultyInLearning { get; set; }
        public virtual string CSPIndicator { get; set; }
        public virtual string AdditionalSupportText { get; set; }
        public virtual string DeclaredDisabled { get; set; }
        public virtual string AssessedDisabled { get; set; }
        public virtual string PhysicalAdaptation { get; set; }
        public virtual string CurriculumAdaptation { get; set; }
        public virtual string CommunicationAdaptation { get; set; }
        public virtual string DisabilityText { get; set; }
        public virtual string GaelicEducation { get; set; }
        public virtual string LevelOfEnglish { get; set; }
        public virtual string Literacy_Primary { get; set; }
        public virtual string Reading { get; set; }
        public virtual string Writing { get; set; }
        public virtual string L_and_T { get; set; }
        public virtual string Numeracy_Primary { get; set; }
        public virtual string NMM { get; set; }
        public virtual string SPM { get; set; }
        public virtual string IH { get; set; }
        public virtual string Adm_date { get; set; }
        public virtual string Lv_Date { get; set; }
        public virtual string In_Care_Curre { get; set; }
        public virtual string In_whilst_at { get; set; }
        public virtual string In_care_locati { get; set; }
        public virtual string CityandShire_Postcode { get; set; }
        public virtual string DataZone { get; set; }
        public virtual int SIMD_2012_rank{ get; set; }
        public virtual int SIMD_2012_quintile { get; set; }
        public virtual int SIMD_2012_decile { get; set; }
        public virtual int SIMD_2012_vigintile { get; set; }
        public virtual int SIMD_2009_rank { get; set; }
        public virtual int SIMD_2009_quintile { get; set; }
        public virtual int SIMD_2009_decile { get; set; }
        public virtual int SIMD_2009_vigintile { get; set; }
        public virtual int Datazone_Population_2010 { get; set; }
        public virtual string CHP_Population_Weighted_Vigintile_2012 { get; set; }
        public virtual string HB_Population_Weighted_Vigintile_2012 { get; set; }
        public virtual string Scotland_Population_Weighted_Vigintile_2012 { get; set; }
        public virtual string IntZone { get; set; }
        public virtual string IntZone_Name { get; set; }
        public virtual string LA_Name { get; set; }
        public virtual string CHP_Name { get; set; }
        public virtual string HB_Code { get; set; }
        public virtual string UR6_Desc { get; set; }
        public virtual string SplitInd { get; set; }

    }
}
