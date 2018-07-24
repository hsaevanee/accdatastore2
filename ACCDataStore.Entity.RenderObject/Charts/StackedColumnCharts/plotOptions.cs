using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.StackedColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "plotOptionsStackedColumnCharts")]
    public class plotOptions
    { 
        public column column { get; set; }
    }
}