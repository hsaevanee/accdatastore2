using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "backgroundAngularGauge")]
    public class background
    {
        public backgroundColor backgroundColor { get; set; }
        public int? borderWidth { get; set; }
        public string outerRadius { get; set; }
        public string innerRadius { get; set; }
    }
}