using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.SplineCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "yAxisSplineCharts")]
    public class yAxis
    {
        public float? min { get; set; }
        public float? max { get; set; }
        public title title { get; set; }
        public bool opposite { get; set; }
        public float? tickInterval { get; set; }
        //public labels labels { get; set; }
    }
}