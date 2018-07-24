using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPPIPS
    {
        public Year year;
        public List<GenericSchoolData> ListGenericSchoolData { get; set; }
        //public List<GenericSchoolData> ListGenericSchoolEnd { get; set; }
    }
}
