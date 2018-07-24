using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "dataLabelsPieCharts")]
    public class dataLabels
    {
        public bool enabled { get; set; }

        [XmlIgnore]
        public string format { get; set; }
        [XmlElement("format")]
        [IgnoreDataMember]
        public XmlCDataSection formatCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(format);
            }
            set
            {
                format = value.Value;
            }
        }
    }
}
