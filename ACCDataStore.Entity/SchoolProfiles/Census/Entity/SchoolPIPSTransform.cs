using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SchoolPIPSTransform
    {
        public List<GenericSchoolData> ListColumnInfo { get; set; }
        public List<SchoolPIPSTransformRow> ListSchoolPIPSTransformRow { get; set; }
    }
}
