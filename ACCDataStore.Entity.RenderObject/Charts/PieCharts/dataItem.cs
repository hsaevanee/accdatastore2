using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "dataItemPieCharts")]
    public class dataItem
    {
        public string name { get; set; }
        public float? y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }
    }
}
