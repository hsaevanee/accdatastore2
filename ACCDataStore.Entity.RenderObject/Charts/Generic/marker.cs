using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "markerGeneric")]
    public class marker
    {
        public bool enabled { get; set; }
        public string symbol { get; set; }
        public int radius { get; set; }
    }
}
