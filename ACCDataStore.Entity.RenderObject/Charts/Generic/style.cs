using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "styleGeneric")]
    public class style
    {
        public string fontFamily { get; set; }
        public string fontSize { get; set; }
        public string fontWeight { get; set; }
        public string color { get; set; }
        public string textShadow { get; set; }
    }
}