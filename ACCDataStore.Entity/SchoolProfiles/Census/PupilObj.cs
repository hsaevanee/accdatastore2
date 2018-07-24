using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class PupilObj:BaseEntity
    {
        public virtual Costcentre Costcentre { get; set; }
        public virtual string StudentID { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Stage { get; set; }
        public virtual string RegistrationClass { get; set; }
        public virtual DateTime DOB { get; set; }
        public virtual string CandidateNumber { get; set; }
        public virtual DateTime AdmissionDate { get; set; }
        public virtual EthnicBackground EthnicBackground { get; set; }
        //public virtual string EthnicBackground { get; set; }
        //public virtual string NationalIdentity { get; set; }
        public virtual NationalIdentity NationalIdentity { get; set; }
        public virtual string AsylumStatus { get; set; }
        public virtual string FreeSchoolMeal { get; set; }
        public virtual string LookedAfter { get; set; }
        public virtual string LookedAfterBy { get; set; }
        public virtual string Status { get; set; }
        public virtual string BaseSchool { get; set; }
        public virtual string ParentLA { get; set; }
        public virtual string AttendanceMode { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string MainstreamInt { get; set; }
        public virtual string SpecialAtt { get; set; }
        public virtual string ForeName { get; set; }
        public virtual string SurName { get; set; }
        public virtual string PhysicalAdaptation { get; set; }
        public virtual string CurriculumAdaptation { get; set; }
        public virtual string CommunicationAdaptation { get; set; }
        public virtual string GaelicEducation { get; set; }
        public virtual EnglishLevel EnglishLevel { get; set; }
        //public virtual string LevelofEnglish { get; set; }
        public virtual string MainHomeLanguage { get; set; }
        public virtual string Knownam { get; set; }
        public virtual string Property { get; set; }
        public virtual string Street { get; set; }
        public virtual string Locality { get; set; }
        public virtual string Town { get; set; }
        public virtual string County { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string PreviousSchool { get; set; }
    }
}
