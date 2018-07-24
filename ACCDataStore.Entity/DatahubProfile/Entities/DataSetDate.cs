using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class DataSetDate: BaseEntity
    {
        public string name { get; set; }
        public string code { get; set; }
        public string month { get; set; }
        public string year { get; set; }

        public DataSetDate(string name, string code, string month, string year)
        {
            this.name =name;
            this.code = code;
            this.month = month;
            this.year = year;

        }

        public object GetJson()
        {
            return new
            {
                Name = this.name,
                Code = this.code
            };
        }
    }
}
