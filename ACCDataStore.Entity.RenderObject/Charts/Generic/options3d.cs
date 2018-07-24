using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "options3dGeneric")]
    public class options3d
    {
        public bool enabled { get; set; }
        public int? alpha { get; set; }
        public int? beta { get; set; }
    }
}
