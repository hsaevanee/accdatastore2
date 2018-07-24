using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class StdStageObj:BaseEntity
    {
        public StdStageObj(string sStagecode)
        {
            this.StageCode = sStagecode;
        }

        public StdStageObj()
        {
            this.StageCode = null;
            this.PercentageFemaleInSchool = 0;
            this.PercentageFemaleAllSchool = 0;
            this.PercentageMaleInSchool = 0;
            this.PercentageMaleAllSchool = 0;
            this.PercentageInSchool = 0;
            this.PercentageAllSchool = 0;
        }
        public string StageCode { get; set; }
        public double PercentageFemaleInSchool { get; set; }
        public double PercentageFemaleAllSchool { get; set; }
        public double PercentageInSchool { get; set; }
        public double PercentageAllSchool { get; set; }
        public double PercentageMaleInSchool { get; set; }
        public double PercentageMaleAllSchool { get; set; }   
    

    }
}
