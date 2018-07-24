using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class StudentNeed
    {
        public Year year { get; set; }
        public GenericSchoolData IEP { get; set; }
        public GenericSchoolData CSP { get; set; }
        public GenericSchoolData ChildPlan { get; set; }
    }
}
