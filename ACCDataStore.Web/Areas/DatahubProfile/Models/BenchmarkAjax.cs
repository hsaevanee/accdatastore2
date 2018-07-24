using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACCDataStore.Web.Areas.DatahubProfile.Models
{
    public class BenchmarkAjax
    {
        public List<PosNegSchoolList> data { get; set; }
        public List<HistogramSeriesData> chart { get; set; }
        public object councilAverage { get; set; }
        public long benchmarkResults { get; set; }
    }
}