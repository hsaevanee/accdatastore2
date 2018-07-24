using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.ColumnCharts;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.StackedColumnCharts
{
    [XmlType(Namespace = "http://tempuri.org/", TypeName = "labelsStackedColumnCharts")]
    public class labels
    {
        public style style { get; set; }

        public static implicit operator labels(ColumnCharts.labels v)
        {
            throw new NotImplementedException();
        }
    }
}