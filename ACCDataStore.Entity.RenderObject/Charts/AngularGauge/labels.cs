using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "labelsAngularGauge")]
    public class labels
    {
        public int? step { get; set; }
        public string rotation { get; set; }
        public style style { get; set; }
    }
}