using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class SchStudentMap : ClassMap<SchStudent>
    {
        public SchStudentMap()
        {
            Table("sch_Student_t");
            Id(x => x.RecordId).Column("RecordId");
            Map(x => x.LaCode);
            Map(x => x.SeedCode);
            Map(x => x.StudentId);
            Map(x => x.PostCode);
            Map(x => x.Gender);
            Map(x => x.StudentStage);
            Map(x => x.StudentStatus);
            Map(x => x.DateOfBirth);
            Map(x => x.ScottishCandidateNumber);
            Map(x => x.AdmissionDate);
            Map(x => x.EthnicBackground);
            Map(x => x.NationalIdentity);
            Map(x => x.AsylumStatus);
            Map(x => x.FreeSchoolMealRegistered);
            Map(x => x.StudentLookedAfter);
            Map(x => x.ResponsibleAuthority);
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
            Map(x => x.MainHomeLanguage);
            Map(x => x.LevelOfEnglish);
        }
    }
}
