using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "yAxisAngularGauge")]
    public class yAxis
    {
        public int? min { get; set; }
        public int? max { get; set; }
        public string minorTickInterval { get; set; }
        public int? minorTickWidth { get; set; }
        public int? minorTickLength { get; set; }
        public string minorTickPosition { get; set; }
        public string minorTickColor { get; set; }
        public int? tickPixelInterval { get; set; }
        public int? tickWidth { get; set; }
        public string tickPosition { get; set; }
        public int? tickLength { get; set; }
        public string tickColor { get; set; }
        public labels labels { get; set; }
        public title title { get; set; }
        public List<plotBands> plotBands { get; set; }
    }
}