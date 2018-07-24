using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles 
{
    public class ViewObj:BaseEntity
    {
        public Year year { get; set; }
        public string seedcode { get; set; }
        public string schooltype { get; set; }
        public string code { get; set; }
        public int count { get; set; }
        public int schoolroll { get; set; }

        public ViewObj(string year, string seedcode, string schooltype, string code, int count) {
            this.year = new Year(year);
            this.seedcode = seedcode;
            this.schooltype = schooltype;
            this.code = code;
            this.count = count;
            this.schoolroll = 0;

        
        }
        public ViewObj() {
            this.count = 0;
            this.schoolroll = 0;
        }

        public ViewObj(string code, int count)
        {
            this.code = code;
            this.count = 0;
        }

        public double getPercentage()
        {
            return (this.count / this.schoolroll) * 100.00;
        }

    }
}
