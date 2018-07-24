using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "backgroundColorGeneric")]
    public class backgroundColor
    {
        public linearGradient linearGradient { get; set; }
        public List<List<object>> stops { get; set; }
    }
}