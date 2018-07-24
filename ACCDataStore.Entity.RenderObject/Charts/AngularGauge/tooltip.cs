using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "tooltipAngularGauge")]
    public class tooltip
    {
        public string valueSuffix { get; set; }
    }
}