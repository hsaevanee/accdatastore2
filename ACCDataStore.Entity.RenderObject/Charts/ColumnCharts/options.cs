﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "optionsColumnCharts")]
    public class options
    {
        public chart chart { get; set; }
        public plotOptions plotOptions { get; set; }
        public tooltip tooltip { get; set; }
        public exporting exporting { get; set; }
    }
}
