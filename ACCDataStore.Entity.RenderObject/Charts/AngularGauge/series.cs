using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "seriesAngularGauge")]
    public class series
    {
        public string name { get; set; }
        public List<float?> data { get; set; }
        public tooltip tooltip { get; set; }
        public dataLabels dataLabels { get; set; }
    }
}