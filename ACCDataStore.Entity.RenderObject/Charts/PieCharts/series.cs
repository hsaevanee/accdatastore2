using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "seriesPieCharts")]
    public class series
    {
        public string name { get; set; }
        public bool colorByPoint { get; set; }
        public List<dataItem> data { get; set; }
    }
}