using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.StackedColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "seriesStackedColumnCharts")]
    public class series
    {
        public string name { get; set; }
        public List<float?> data { get; set; }
        public string color { get; set; }
    }
}