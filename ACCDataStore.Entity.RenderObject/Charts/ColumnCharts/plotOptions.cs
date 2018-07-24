using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "plotOptionsColumnCharts")]
    public class plotOptions
    { 
        public column column { get; set; }
        public spline spline { get; set; }
        public line line { get; set; }
    }
}