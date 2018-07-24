using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.StackedColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "yAxisStackedColumnCharts")]
    public class yAxis
    {
        public int? min { get; set; }
        public int? max { get; set; }
        public title title { get; set; }
        //public labels labels { get; set; } // may cause error
    }
}