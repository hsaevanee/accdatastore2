using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SPCfEReport
    {
        //use by CfE, INCAS data
        public Year year;
        public string stdstage { get; set; }
        public List<SPReport> P1 { get; set; }
        public List<SPReport> P2 { get; set; }
        public List<SPReport> P3 { get; set; }
        public List<SPReport> P4 { get; set; }
        public List<SPReport> P5 { get; set; }
        public List<SPReport> P6 { get; set; }
        public List<SPReport> P7 { get; set; }
        public List<SPReport> S34 { get; set; }
        public List<SPReport> S4 { get; set; }
    }
}
