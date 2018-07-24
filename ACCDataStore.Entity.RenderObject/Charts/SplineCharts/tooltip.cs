using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.SplineCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "tooltipSplineCharts")]
    public class tooltip
    {
        public crosshairs crosshairs { get; set; }
        public bool shared { get; set; }
        public string headerFormat { get; set; }
        public int valueDecimals { get; set; }
    }
}