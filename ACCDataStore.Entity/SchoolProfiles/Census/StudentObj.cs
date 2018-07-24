using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class StudentObj : BaseEntity
    {
        public virtual int ID { get; set; }
        //public virtual string LaCode { get; set; }
        public virtual int SeedCode { get; set; }
        public virtual string StudentID { get; set; }
        public virtual string ForeName { get; set; }
        public virtual string SurName { get; set; }
        //public virtual string PreferredForename { get; set; }
        //public virtual string PreferredSurname { get; set; }
        //public virtual string PreviousForename { get; set; }
        //public virtual string PreviousSurname { get; set; }
        //public virtual string MiddleName { get; set; }
        //public virtual string AddressLine1 { get; set; }
        //public virtual string AddressLine2 { get; set; }
        //public virtual string AddressLine3 { get; set; }
        //public virtual string AddressLine4 { get; set; }
        //public virtual string AddressLine5 { get; set; }
        //public virtual string AddressLine6 { get; set; }
        public virtual string PostCode { get; set; }
        //public virtual string StudentTelephone { get; set; }
        //public virtual int LastSchoolAttended { get; set; }
        //public virtual int PrimarySchoolAttended { get; set; }
        //public virtual string RegistrationClass { get; set; }
        public virtual string Gender { get; set; }
        public virtual string StudentStage { get; set; }
        public virtual string StudentStatus { get; set; }
        //public virtual DateTime DateofBirth { get; set; }
        public virtual string ScottishCandidateNumber { get; set; }
        //public virtual DateTime AdmissionDate { get; set; }
        public virtual string EthnicBackground { get; set; }
        public virtual string NationalIdentity { get; set; }
        //public virtual string AsylumStatus { get; set; }
        public virtual string FreeSchoolMealRegistered { get; set; }
        public virtual string StudentLookedAfter { get; set; }
        //public virtual string ResponsibleAuthority { get; set; }
        //public virtual string BaseSchoolCode { get; set; }
        //public virtual string ParentLaCode { get; set; }
        public virtual string ModeofAttendance { get; set; }
        public virtual string IepIndicator { get; set; }
        public virtual string RonIndicator { get; set; }
        //public virtual string MainstreamIntegration { get; set; }
        //public virtual string AttendanceSsu { get; set; }
        //public virtual string MainDifficultyInLearning { get; set; }
        public virtual string CSPIndicator { get; set; }
        //public virtual string AdditionalSupportText { get; set; }
        //public virtual string DeclaredDisabled { get; set; }
        //public virtual string AssessedDisabled { get; set; }
        //public virtual string PhysicalAdaptation { get; set; }
        //public virtual string CurriculumAdaptation { get; set; }
        //public virtual string CommunicationAdaptation { get; set; }
        //public virtual string DisabilityText { get; set; }
        //public virtual string GaelicEducation { get; set; }
        public virtual string MainHomeLanguage { get; set; }
        public virtual string LevelOfEnglish { get; set; }
        public virtual int SIMD_Decile { get; set; }

    }

    public class SchStudent2008 : StudentObj
    {
    }

    public class SchStudent2009: StudentObj
    {
    }

    public class SchStudent2010 : StudentObj
    {
    }

    public class SchStudent2011 : StudentObj
    {
    }

    public class SchStudent2012 : StudentObj
    {
    }

    public class SchStudent2013 : StudentObj
    {
    }

    public class SchStudent2014 : StudentObj
    {
    }

    public class SchStudent2015 : StudentObj
    {
    }

    public class SchStudent2016 : StudentObj
    {
    }
}
