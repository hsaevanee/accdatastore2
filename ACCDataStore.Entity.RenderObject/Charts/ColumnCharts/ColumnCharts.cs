using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.ColumnCharts
{
    public class ColumnCharts : BaseRenderObject
    {
        //public options options { get; set; }
        public chart chart { get; set; }
        public title title { get; set; }
        public subtitle subtitle { get; set; }
        public xAxis xAxis { get; set; }
        public yAxis yAxis { get; set; }
        public tooltip tooltip { get; set; }
        public plotOptions plotOptions { get; set; }
        public List<series> series { get; set; }
        public credits credits { get; set; }
        //public legend legend { get; set; }
        public exporting exporting { get; set; }

        public void SetDefault(bool bIsCreateConfigFile)
        {
            this.chart = new chart()
            {
                type = "column"
            };

            this.title = new title()
            {
                text = ""
            };

            this.subtitle = new subtitle()
            {
                text = ""
            };

            this.xAxis = new xAxis()
            {
                categories = new List<string>(),
                crosshair = true
            };

            this.yAxis = new yAxis()
            {
                min = 0,
                title = new title() { text = "" }
            };

            this.tooltip = new tooltip()
            {
                headerFormat = @"<span style='font - size:10px'>{point.key}</span><table>",
                pointFormat = "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y:,.2f}</b></td></tr>",
                footerFormat = "</table>",
                shared = true,
                useHTML = true
            };

            this.plotOptions = new plotOptions()
            {
                column = new column() { animation = true, pointPadding = 0.1f, borderWidth = 0 },
                line = new line() { animation = true, marker = new marker() { enabled = false, symbol = "circle", radius = 2 } },
                spline = new spline() { animation = true, marker = new marker() { enabled = false, symbol = "circle", radius = 2 } }

            };

            this.series = new List<series>()
            {
                //new series() { name = "Tokyo", data = new List<float?>() { 49.9f, 71.5f, 106.4f, 129.2f, 144.0f, 176.0f, 135.6f, 148.5f, 216.4f, 194.1f, 95.6f, 54.4f } },
                //new series() { name = "New York", data = new List<float?>() { 83.6f, 78.8f, 98.5f, 93.4f, 106.0f, 84.5f, 105.0f, 104.3f, 91.2f, 83.5f, 106.6f, 92.3f } },
                //new series() { name = "London", data = new List<float?>() { 48.9f, 38.8f, 39.3f, 41.4f, 47.0f, 48.3f, 59.0f, 59.6f, 52.4f, 65.2f, 59.3f, 51.2f } },
                //new series() { name = "Berlin", data = new List<float?>() { 42.4f, 33.2f, 34.5f, 39.7f, 52.6f, 75.5f, 57.4f, 60.4f, 47.6f, 39.1f, 46.8f, 51.1f } }
            };

            this.credits = new credits()
            {
                enabled = false
            };

            
            this.exporting = new exporting()
            {
                enabled = true
            };

            // hightcharts-ng need this
            //this.options = new options()
            //{
            //    chart = this.chart,
            //    plotOptions = this.plotOptions,
            //    tooltip = this.tooltip
            //};

            if (bIsCreateConfigFile)
            {
                CreateConfigFile("ColumnCharts.xml");
            }
        }
    }
}
