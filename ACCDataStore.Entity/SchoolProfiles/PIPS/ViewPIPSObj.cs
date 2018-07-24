using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles
{
    public class ViewPIPSObj:BaseEntity
    {
        public Year year {get; set;}
        public string seedcode { get; set; }
        public double sreading { get; set; }
        public double smath { get; set; }
        public double sphonics { get; set; }
        public double stotal { get; set; }
        public double ereading { get; set; }
        public double emath { get; set; }
        public double ephonics { get; set; }
        public double etotal { get; set; }
       
    }
}
