using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.SplineCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "xAxisSplineCharts")]
    public class xAxis
    {
        public string type { get; set; }
        public dateTimeLabelFormats dateTimeLabelFormats { get; set; }
        public title title { get; set; }
        public int gridLineWidth { get; set; }
        public List<string> categories { get; set; }
    }
}
