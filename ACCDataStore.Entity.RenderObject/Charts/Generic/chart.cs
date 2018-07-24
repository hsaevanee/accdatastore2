using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "chartGeneric")]
    public class chart
    {
        public string type { get; set; }
        //public int margin { get; set; } // may cause error
        public string zoomType { get; set; }
        public options3d options3d { get; set; }
    }
}