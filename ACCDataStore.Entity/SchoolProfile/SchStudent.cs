using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class SchStudent : BaseEntity
    {
        public virtual int RecordId { get; set; }
        public virtual string LaCode { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string StudentId { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Gender { get; set; }
        public virtual string StudentStage { get; set; }
        public virtual string StudentStatus { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual string ScottishCandidateNumber { get; set; }
        public virtual DateTime AdmissionDate { get; set; }
        public virtual string EthnicBackground { get; set; }
        public virtual string NationalIdentity { get; set; }
        public virtual string AsylumStatus { get; set; }
        public virtual string FreeSchoolMealRegistered { get; set; }
        public virtual string StudentLookedAfter { get; set; }
        public virtual string ResponsibleAuthority { get; set; }
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
        public virtual string MainHomeLanguage { get; set; }
        public virtual string LevelOfEnglish { get; set; }
    }
}
