using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "tooltipPieCharts")]
    public class tooltip
    {
        [XmlIgnore]
        public string pointFormat { get; set; }
        [XmlElement("pointFormat")]
        [IgnoreDataMember]
        public XmlCDataSection pointFormatCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(pointFormat);
            }
            set
            {
                pointFormat = value.Value;
            }
        }
    }
}