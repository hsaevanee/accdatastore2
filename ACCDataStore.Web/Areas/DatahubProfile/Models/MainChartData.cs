using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class MainChartData
    {
        public object totals { get; set; }
        public List<object> selected { get; set; }
        public long benchmarkResults { get; set; }
    }
}