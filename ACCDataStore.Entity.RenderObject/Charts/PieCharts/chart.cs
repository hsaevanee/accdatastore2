using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "chartPieCharts")]
    public class chart : ACCDataStore.Entity.RenderObject.Charts.Generic.chart
    {
        public string plotBackgroundColor { get; set; }
        public int? plotBorderWidth { get; set; }
        public bool? plotShadow { get; set; }
    }
}