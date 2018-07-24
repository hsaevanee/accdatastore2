using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "tooltipColumnCharts")]
    public class tooltip
    {
        [XmlIgnore]
        public string headerFormat { get; set; }
        [XmlElement("headerFormat")]
        [IgnoreDataMember]
        public XmlCDataSection headerFormatCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(headerFormat);
            }
            set
            {
                headerFormat = value.Value;
            }
        }
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
        [XmlIgnore]
        public string footerFormat { get; set; }
        [XmlElement("footerFormat")]
        [IgnoreDataMember]
        public XmlCDataSection footerFormatCDATA
        {
            get
            {
                return new XmlDocument().CreateCDataSection(footerFormat);
            }
            set
            {
                footerFormat = value.Value;
            }
        }
        public bool shared { get; set; }
        public bool useHTML { get; set; }
    }
}