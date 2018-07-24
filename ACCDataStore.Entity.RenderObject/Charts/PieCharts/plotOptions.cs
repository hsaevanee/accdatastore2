using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "plotOptionsPieCharts")]
    public class plotOptions
    {
        public pie pie { get; set; }
    }
}
