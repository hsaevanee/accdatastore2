
using ACCDataStore.Entity.SchoolProfiles;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.Mapping.MSAccess
{
    public class PupilMap : ClassMap<PupilObj>
    {
        public PupilMap()
        {
            Table("sx_pupil"); //from Scotxed_15 database
            Id(x => x.StudentID);
            References(x => x.Costcentre, "CostCentreKey");
            //Map(x => x.CostCentreKey);
            Map(x => x.StudentID);
            Map(x => x.Gender);
            Map(x => x.Stage);
            Map(x => x.RegistrationClass);
            Map(x => x.DOB);
            Map(x => x.CandidateNumber);
            Map(x => x.AdmissionDate);
            References(x => x.EthnicBackground, "EthnicBackground");
            //Map(x => x.EthnicBackground);
            References(x => x.NationalIdentity, "NationalIdentity");
            //Map(x => x.NationalIdentity);
            Map(x => x.AsylumStatus);
            Map(x => x.FreeSchoolMeal);
            Map(x => x.LookedAfter);
            Map(x => x.LookedAfterBy);
            Map(x => x.Status);
            Map(x => x.BaseSchool);
            Map(x => x.ParentLA);
            Map(x => x.AttendanceMode);
            Map(x => x.PostCode);
            Map(x => x.MainstreamInt);
            Map(x => x.SpecialAtt);
            Map(x => x.ForeName);
            Map(x => x.SurName);
            Map(x => x.PhysicalAdaptation);
            Map(x => x.CurriculumAdaptation);
            Map(x => x.CommunicationAdaptation);
            Map(x => x.GaelicEducation);
            References(x => x.EnglishLevel, "LevelofEnglish");
            //Map(x => x.LevelofEnglish);
            Map(x => x.MainHomeLanguage);
            Map(x => x.Knownam);
            Map(x => x.Property);
            Map(x => x.Street);
            Map(x => x.Locality);
            Map(x => x.Town);
            Map(x => x.County);
            Map(x => x.Telephone);
            Map(x => x.PreviousSchool);
        }
    }
}
