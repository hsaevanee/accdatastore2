using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class GenericSPFSM:GenericSchoolData
    {
        public string Studentstage { get; set; }

        public GenericSPFSM(string studentstage, string code, int count) {
            this.Studentstage = studentstage;
            this.Code = code;
            this.count = count;
        
        }
        public GenericSPFSM( )
        {
 

        }
    }
}
