using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPReport
    {
        public string code;
        public List<GenericSchoolData> listdata { get; set; }
    }
}
