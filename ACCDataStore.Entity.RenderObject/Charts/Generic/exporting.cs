using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "exportingGeneric")]
    public class exporting
    {
        public bool enabled { get; set; }
        public string filename { get; set; }
        public string url { get; set; }
        public int sourceHeight { get; set; }
        public int sourceWidth { get; set; }
    }
}
