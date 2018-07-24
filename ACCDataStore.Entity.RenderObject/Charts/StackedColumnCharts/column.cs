using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.StackedColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "columnStackedColumnCharts")]
    public class column
    {
        public float? pointPadding { get; set; }
        public int borderWidth { get; set; }
        public string stacking { get; set; }
        //public dataLabels dataLabels { get; set; }
        public bool animation { get; set; }
    }
}
