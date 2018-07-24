using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "gaugeAngularGauge")]
    public class gauge
    {
        public dial dial { get; set; }
        public bool animation { get; set; }
    }
}