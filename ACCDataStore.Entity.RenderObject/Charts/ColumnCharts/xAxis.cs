using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "xAxisColumnCharts")]
    public class xAxis
    {
        public List<string> categories { get; set; }
        //public labels labels { get; set; }
        public bool crosshair { get; set; }
    }
}
