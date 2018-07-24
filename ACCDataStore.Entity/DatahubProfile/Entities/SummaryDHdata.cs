using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACCDataStore.Entity.DatahubProfile.Entities
{
    public class SummaryDHdata
    {
        public int year;
        public int month;
        public string smonth;
        public string seedcode;
        public string centrename;
        public List<GenericData> listdata;
        public DataSetDate sdataset;
    }
}
