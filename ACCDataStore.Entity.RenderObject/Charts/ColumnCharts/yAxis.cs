using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "yAxisColumnCharts")]
    public class yAxis
    {
        public int? min { get; set; }
        public int? max { get; set; }
        public title title { get; set; }
        public float? tickInterval { get; set; }
        //public labels labels { get; set; }
    }
}