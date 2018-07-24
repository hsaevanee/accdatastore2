using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class StudentSIMDMap : ClassMap<StudentSIMD>
    {
        public StudentSIMDMap() {
            Table("test_3");
            Id(x => x.ID).Column("Id");
            Map(x => x.SchName).Column("Name");
            Map(x => x.Test2_Postcode).Column("Test_2_Postcode");
            Map(x => x.Gender).Column("Gender");
            Map(x => x.StudentStage);
            Map(x => x.StudentStatus);
            Map(x => x.DateOfBirth);
            Map(x => x.AdmissionDate);
            Map(x => x.EthnicBackground);
            Map(x => x.NationalIdentity); 
            Map(x => x.FreeSchoolMealRegistered);
            Map(x => x.BaseSchoolCode);
            Map(x => x.ParentLaCode);
            Map(x => x.ModeOfAttendance);
            Map(x => x.IepIndicator);
            Map(x => x.RonIndicator);
            Map(x => x.MainstreamIntegration);
            Map(x => x.AttendanceSsu);
            Map(x => x.MainDifficultyInLearning);
            Map(x => x.CSPIndicator);
            Map(x => x.AdditionalSupportText);
            Map(x => x.DeclaredDisabled);
            Map(x => x.AssessedDisabled);
            Map(x => x.PhysicalAdaptation);
            Map(x => x.CurriculumAdaptation);
            Map(x => x.CommunicationAdaptation);
            Map(x => x.DisabilityText);
            Map(x => x.GaelicEducation);
            Map(x => x.LevelOfEnglish);
            Map(x => x.Literacy_Primary).Column("Literacy_Primary");
            Map(x => x.Reading);
            Map(x => x.Writing);
            Map(x => x.L_and_T).Column("L_and_T");
            Map(x => x.Numeracy_Primary).Column("Numeracy_Primary");
            Map(x => x.NMM);
            Map(x => x.SPM);
            Map(x => x.IH);
            Map(x => x.Adm_date).Column("Adm_date");
            Map(x => x.Lv_Date).Column("Lv_Date");
            Map(x => x.In_Care_Curre).Column("In_Care_Curre");
            Map(x => x.In_whilst_at).Column("In_whilst_at");
            Map(x => x.In_care_locati).Column("In_care_locati");
            Map(x => x.CityandShire_Postcode).Column("CityAndShire_Postcode");
            Map(x => x.DataZone);
            Map(x => x.SIMD_2012_rank).Column("SIMD_2012_rank");
            Map(x => x.SIMD_2012_quintile).Column("SIMD_2012_quintile");
            Map(x => x.SIMD_2012_decile).Column("SIMD_2012_decile");
            Map(x => x.SIMD_2012_vigintile).Column("SIMD_2012_vigintile");
            Map(x => x.SIMD_2009_rank).Column("SIMD_2009_rank");
            Map(x => x.SIMD_2009_quintile).Column("SIMD_2009_quintile");
            Map(x => x.SIMD_2009_decile).Column("SIMD_2009_decile");
            Map(x => x.SIMD_2009_vigintile).Column("SIMD_2009_vigintile");
            Map(x => x.Datazone_Population_2010).Column("Datazone_Population_2010");
            Map(x => x.CHP_Population_Weighted_Vigintile_2012).Column("CHP_Population_Weighted_Vigintile_2012");
            Map(x => x.HB_Population_Weighted_Vigintile_2012).Column("HB_Population_Weighted_Vigintile_2012");
            Map(x => x.Scotland_Population_Weighted_Vigintile_2012).Column("Scotland_Population_Weighted_Vigintile_2012");
            Map(x => x.IntZone);
            Map(x => x.IntZone_Name);
            Map(x => x.LA_Name);
            Map(x => x.CHP_Name);
            Map(x => x.HB_Code);
            Map(x => x.UR6_Desc);
            Map(x => x.SplitInd);
        }
    }
}
