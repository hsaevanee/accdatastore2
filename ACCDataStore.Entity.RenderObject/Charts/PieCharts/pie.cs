using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "piePieCharts")]
    public class pie
    {
        public bool allowPointSelect { get; set; }
        public string cursor { get; set; }
        public int? depth { get; set; }
        public dataLabels dataLabels { get; set; }
        public bool animation { get; set; }
    }
}
