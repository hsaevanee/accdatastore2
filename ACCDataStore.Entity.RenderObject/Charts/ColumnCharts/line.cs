using ACCDataStore.Entity.RenderObject.Charts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "lineColumnCharts")]
    public class line
    {
        public marker marker { get; set; }
        public bool animation { get; set; }
    }
}
