using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "chartAngularGauge")]
    public class chart : ACCDataStore.Entity.RenderObject.Charts.Generic.chart
    {
        public string plotBackgroundColor { get; set; }
        public string plotBackgroundImage { get; set; }
        public int? plotBorderWidth { get; set; }
        public bool? plotShadow { get; set; }
    }
}