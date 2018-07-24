using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Mapping
{
    public class SchStudent2008Map : ClassMap<SchStudent2008>
    {
        public SchStudent2008Map()
        {
            Table("sch_student_t_2008");
            Id(x => x.ID).Column("ID");
            //Map(x => x.LaCode);
            Map(x => x.SeedCode);
            Map(x => x.StudentID);
            Map(x => x.ForeName);
            Map(x => x.SurName);
            //Map(x => x.PreferredForename);
            //Map(x => x.PreferredSurname);
            //Map(x => x.PreviousForename);
            //Map(x => x.PreviousSurname);
            //Map(x => x.MiddleName);
            //Map(x => x.AddressLine1);
            //Map(x => x.AddressLine2);
            //Map(x => x.AddressLine3);
            //Map(x => x.AddressLine4);
            //Map(x => x.AddressLine5);
            //Map(x => x.AddressLine6);
            Map(x => x.PostCode);
            //Map(x => x.StudentTelephone);
            //Map(x => x.LastSchoolAttended);
            //Map(x => x.PrimarySchoolAttended);
            //Map(x => x.RegistrationClass);
            Map(x => x.Gender);
            Map(x => x.StudentStage);
            Map(x => x.StudentStatus);
            //Map(x => x.DateofBirth);
            Map(x => x.ScottishCandidateNumber);
            // Map(x => x.AdmissionDate);
            Map(x => x.EthnicBackground);
            Map(x => x.NationalIdentity);
            //Map(x => x.AsylumStatus);
            Map(x => x.FreeSchoolMealRegistered);
            Map(x => x.StudentLookedAfter);
            //Map(x => x.ResponsibleAuthority);
            //Map(x => x.BaseSchoolCode);
            //Map(x => x.ParentLaCode);
            Map(x => x.ModeofAttendance);
            Map(x => x.IepIndicator);
            Map(x => x.RonIndicator);
            //Map(x => x.MainstreamIntegration);
            Map(x => x.CSPIndicator);
            //Map(x => x.AdditionalSupportText);
            //Map(x => x.DeclaredDisabled);
            //Map(x => x.AssessedDisabled);
            //Map(x => x.PhysicalAdaptation);
            //Map(x => x.CurriculumAdaptation);
            //Map(x => x.CommunicationAdaptation);
            //Map(x => x.DisabilityText);
            //Map(x => x.GaelicEducation);
            Map(x => x.MainHomeLanguage);
            Map(x => x.LevelOfEnglish);
            Map(x => x.SIMD_Decile);

        }


    }
}
