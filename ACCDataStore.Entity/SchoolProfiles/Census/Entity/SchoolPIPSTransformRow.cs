using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SchoolPIPSTransformRow
    {
        public School SPSchool{ get; set; }
        public Year Year { get; set; }
        public List<GenericSchoolData> ListGenericSchoolData { get; set; }

        public string SchoolName { get; set; }
    }
}
