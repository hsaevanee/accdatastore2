using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPMiDYISReport
    {
        public Year year;
        public string school;
        public List<SPReport> listdata { get; set; }
    }
}
