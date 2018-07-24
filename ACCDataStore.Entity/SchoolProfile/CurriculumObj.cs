using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfile
{
    public class CurriculumObj:BaseEntity
    {        
        public string stage { get; set; }
        public string gender { get; set; }
        public double sumpupils { get; set; } // Total number of pupils in this stage
        public double early { get; set; }
        public double earlyconsolidating { get; set; }
        public double earlydeveloping { get; set; }
        public double earlysecure { get; set; }
        public double firstconsolidating { get; set; }
        public double firstdeveloping { get; set; }
        public double firstsecure { get; set; }
        public double seconddeveloping { get; set; }
        public double secondconsolidating { get; set; }
        public double secondsecure { get; set; }
        public double thirddeveloping { get; set; }
        public double thirdconsolidating { get; set; }
        public double thirdsecure { get; set; }
        public double blank { get; set; }
        public double grandtotal { get; set; }

        public CurriculumObj(string stage, string gender)
        {
            this.stage = stage;
            this.gender = gender;
            this.sumpupils = 0;
            this.earlydeveloping = 0;
            this.earlysecure = 0;
            this.firstconsolidating = 0;
            this.firstdeveloping = 0;
            this.firstsecure = 0;
            this.seconddeveloping = 0;
            this.secondconsolidating = 0;
            this.secondsecure = 0;
            this.blank = 0;
            this.grandtotal = 0;
        }

        public CurriculumObj()
        {
            this.stage = null;
            this.gender = null;
            this.sumpupils = 0;
            this.earlydeveloping = 0;
            this.earlysecure = 0;
            this.firstconsolidating = 0;
            this.firstdeveloping = 0;
            this.firstsecure = 0;
            this.seconddeveloping = 0;
            this.secondconsolidating = 0;
            this.secondsecure = 0;
            this.blank = 0;
            this.grandtotal = 0;

        }
    }
}
