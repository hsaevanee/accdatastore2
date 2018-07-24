using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.SplineCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "seriesSplineCharts")]
    public class series
    {
        public string name { get; set; }
        public List<float?> data { get; set; }
        public string color { get; set; }
        public int lineWidth { get; set; }
        public int yAxis { get; set; }
        public bool visible { get; set; }
        
    }
}