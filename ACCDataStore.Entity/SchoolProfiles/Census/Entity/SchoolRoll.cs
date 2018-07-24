using ACCDataStore.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class SchoolRoll
    {
        public Year year {get;set;}
        public int schoolroll { get; set; }
        public int capacity { get; set; }
        public float percent { get; set; }
        public string spercent
        {
            get
            {
                return NumberFormatHelper.FormatNumber(this.percent, 1).ToString(); ;
            }
            set
            {
                this.spercent = value;
            }
        }
        public string sschoolroll { get; set; }
    }
}
