using ACCDataStore.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class GenericData
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public float? Percent { get; set; }
        public int count {get;set;}
        public int sum { get; set; }
        public string sPercent { get; set; }
        public string scount { get; set; }

        public GenericData()
        {
            this.Percent = 0.0F;
            this.sPercent = "0.0";
        }

        public GenericData(string code, int count, int sum) {
            this.Code = code;
            this.count = count;
            this.Percent = this.count*100.0F/this.sum;
            this.sPercent = GetStringPercent();
        }

        public GenericData(string code, int count)
        {
            this.Code = code;
            this.count = count;
            this.Percent = 0.0F;
            this.sPercent = "0.0";
        }

        public GenericData(string code, string Name)
        {
            this.Code = code;
            this.Name = Name;
            this.Percent = 0.0F;
            this.sPercent = "0.0";
        }

        public GenericData(string code, float? percent)
        {
            this.Code = code;
            this.Percent = percent;
            this.sPercent = GetStringPercent();
        }

        public string GetStringPercent() {
            return NumberFormatHelper.FormatNumber(this.Percent,1,"n/a").ToString();
        }
    }
}
