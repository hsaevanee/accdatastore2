using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPInCAS
    {
        public Year year;
        public int yeargroup;
        public List<GenericSchoolData> ListGenericAgeAtTest { get; set; }
        public List<GenericSchoolData> ListGenericAgeEquiv { get; set; }
        public List<GenericSchoolData> ListGenericAgeDiffrence { get; set; }
        public List<GenericSchoolData> ListGenericStandardised { get; set; }

        public List<GenericSchoolData> ListGenericDevAbil { get; set; }
        public List<GenericSchoolData> ListGenericReading { get; set; }
        public List<GenericSchoolData> ListGenericSpelling { get; set; }
        public List<GenericSchoolData> ListGenericGenMath{ get; set; }
        public List<GenericSchoolData> ListGenericMentArith { get; set; }

    }
}
