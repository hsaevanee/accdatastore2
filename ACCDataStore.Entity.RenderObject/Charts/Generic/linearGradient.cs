using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "linearGradientGeneric")]
    public class linearGradient
    {
        public float? x1 { get; set; }
        public float? x2 { get; set; }
        public float? y1 { get; set; }
        public float? y2 { get; set; }
    }
}