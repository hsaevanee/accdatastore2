using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "seriesColumnCharts")]
    public class series
    {
        public string type { get; set; }
        public string name { get; set; }
        public List<float?> data { get; set; }
        public string color { get; set; }
    }
}