using ACCDataStore.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class SDSGroup
    {
        public string name;
        public string code;
        public double checkSumPercentage;
        public int checkSumCount;
        public List<GenericData> listdata { get; set; }

        public SDSGroup(string code, string name) {
            this.code = code;
            this.name = name;
            this.checkSumPercentage = 0.0;
            this.checkSumCount = 0;
            this.listdata = new List<GenericData>();      
        }

        public double getcheckSumPercentage() {

            return Convert.ToDouble(NumberFormatHelper.FormatNumber(this.listdata.Select(x => Convert.ToDouble(x.Percent)).Sum(), 1));
        }

        public int getcheckSumCount()
        {

            return this.listdata.Select(x => Convert.ToInt32(x.count)).Sum(); 
        }

    }
}
