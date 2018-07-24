using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "titleGeneric")]
    public class title
    {
        public string text { get; set; }
        //public style style { get; set; }
    }
}