using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "dialAngularGauge")]
    public class dial
    {
        public string radius { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public int? borderWidth { get; set; }
        public int? baseWidth { get; set; }
        public int? topWidth { get; set; }
        public string baseLength { get; set; }
        public string rearLength { get; set; }
    }
}