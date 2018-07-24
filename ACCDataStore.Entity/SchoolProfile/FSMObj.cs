using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity
{
    public class FSMObj : BaseEntity
    {        
        public string schoolname { get; set; }
        public double schoolroll { get; set; }
        public double registeredFSMInSchool { get; set; }
        public double PercentageRegisteredInSchool { get; set; }


        public FSMObj(string schoolname)
        {
            this.schoolname = schoolname;            
        }

        public FSMObj()
        {
            this.schoolname = null;
            this.schoolroll = 0;
            this.registeredFSMInSchool = 0;
            this.PercentageRegisteredInSchool = 0;
        }
    }
}
