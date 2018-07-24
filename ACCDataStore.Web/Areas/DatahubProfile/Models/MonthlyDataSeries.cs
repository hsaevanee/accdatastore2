using ACCDataStore.Entity.DatahubProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class MonthlyDataSeries
    {
        public List<DatahubDataObj> month1 { get; set; }
        public List<DatahubDataObj> month2 { get; set; }
        public List<DatahubDataObj> month3 { get; set; }
        public List<DatahubDataObj> month4 { get; set; }
        public List<DatahubDataObj> month5 { get; set; }
        public List<DatahubDataObj> month6 { get; set; }
        public List<DatahubDataObj> month7 { get; set; }
        public List<DatahubDataObj> month8 { get; set; }
        public List<DatahubDataObj> month9 { get; set; }
        public List<DatahubDataObj> month10 { get; set; }
        public List<DatahubDataObj> month11 { get; set; }
        public List<DatahubDataObj> month12 { get; set; }
    }
}