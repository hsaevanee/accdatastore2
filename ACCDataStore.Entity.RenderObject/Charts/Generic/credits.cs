using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "creditsGeneric")]
    public class credits
    {
        public bool enabled { get; set; }
    }
}