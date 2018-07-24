using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.Generic
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "dateTimeLabelFormatsGeneric")]
    public class dateTimeLabelFormats
    {
        public string second { get; set; }
        public string minute { get; set; }
        public string hour { get; set; }
        public string day { get; set; }
        public string week { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
}
