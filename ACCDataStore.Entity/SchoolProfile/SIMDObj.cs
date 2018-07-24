using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class SIMDObj:BaseEntity
    {
        public SIMDObj(string sSIMDcode)
        {
            this.SIMDCode = sSIMDcode;
            this.PercentageInSchool2009 = 0;
            this.PercentageAllSchool2009 = 0;
            this.PercentageInSchool2012 = 0;
            this.PercentageAllSchool2012 = 0;
        }

        public SIMDObj()
        {
            this.SIMDCode = null;
            this.PercentageInSchool2009 = 0;
            this.PercentageAllSchool2009 = 0;
            this.PercentageInSchool2012 = 0;
            this.PercentageAllSchool2012 = 0;
        }
        public string SIMDCode { get; set; }
        public double PercentageInSchool2009 { get; set; }
        public double PercentageAllSchool2009 { get; set; }
        public double PercentageInSchool2012 { get; set; }
        public double PercentageAllSchool2012 { get; set; }
    }
    
}
