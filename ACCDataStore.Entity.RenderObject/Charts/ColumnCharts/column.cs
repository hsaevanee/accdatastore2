using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "columnColumnCharts")]
    public class column
    {
        public float? pointPadding { get; set; }
        public int borderWidth { get; set; }
        public string color { get; set; }
        public bool animation { get; set; }
    }
}
