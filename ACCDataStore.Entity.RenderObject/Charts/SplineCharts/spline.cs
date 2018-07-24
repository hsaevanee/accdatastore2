using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.SplineCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "splineSplineCharts")]
    public class spline
    {
        public marker marker { get; set; }
        public bool animation { get; set; }
    }
}
