using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "dataLabelsGeneric")]
    public class dataLabels
    {
        public bool enabled { get; set; }
        public string color { get; set; }
        public style style { get; set; }
    }
}