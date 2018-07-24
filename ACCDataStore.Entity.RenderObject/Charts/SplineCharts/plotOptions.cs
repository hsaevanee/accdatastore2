using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ACCDataStore.Entity.RenderObject.Charts.SplineCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "plotOptionsSplineCharts")]
    public class plotOptions
    { 
        public spline spline { get; set; }
    }
}