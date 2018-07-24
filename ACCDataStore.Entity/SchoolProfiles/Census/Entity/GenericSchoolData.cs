using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACCDataStore.Core.Helper;

namespace ACCDataStore.Entity.SchoolProfiles.Census.Entity
{
    public class GenericSchoolData
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public float? Percent { get; set; }
        public int count {get;set;}
        public int sum { get; set; }
        public string sPercent { get; set; }
        public string sCount { get; set; }
        public string color { get; set; }

        public GenericSchoolData()
        {
            this.Percent = 0.0F;
            this.sPercent = "0.0";
            this.color = "‎#000000";
        }

        public GenericSchoolData(string code, int count) {
            this.Code = code;
            this.count = count;
            this.Percent = 0.0F;
            this.sPercent = "0.0";
            this.sCount = NumberFormatHelper.FormatNumber(count, 0, "n/a").ToString();
            this.color = "‎#000000";
        }

        public GenericSchoolData(string code, string Name)
        {
            this.Code = code;
            this.Name = Name;
            this.Percent = 0.0F;
            this.sPercent = "0.0";
            this.count = 0;
            this.sCount = "0";
            this.color = "‎#000000";
        }

        public GenericSchoolData(string code, float? percent)
        {
            this.Code = code;
            this.Percent = percent;
            this.sPercent = GetStringPercent();
            this.color = "‎#000000";
        }

        public string GetStringPercent() {
            return NumberFormatHelper.FormatNumber(this.Percent,1,"n/a").ToString();
        }
    }
}
