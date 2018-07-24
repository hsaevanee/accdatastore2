using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "paneAngularGauge")]
    public class pane
    {
        public int? startAngle { get; set; }
        public int? endAngle { get; set; }
        public List<background> background { get; set; }

    }
}