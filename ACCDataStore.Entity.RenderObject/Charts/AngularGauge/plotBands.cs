using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "plotBandsAngularGauge")]
    public class plotBands
    {
        public int? from { get; set; }
        public int? to { get; set; }
        public string color { get; set; }
        public string outerRadius { get; set; }
        public string innerRadius { get; set; }
    }
}