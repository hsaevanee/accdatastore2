using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.StackedColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "xAxisStackedColumnCharts")]
    public class xAxis
    {
        public List<string> categories { get; set; }
        public bool crosshair { get; set; }
        //public labels labels { get; set; } // may cause error
    }
}
