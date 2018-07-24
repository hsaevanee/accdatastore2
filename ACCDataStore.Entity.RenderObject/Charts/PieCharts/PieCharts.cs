using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.PieCharts
{
    public class PieCharts : BaseRenderObject
    {
        public options options { get; set; }
        public chart chart { get; set; }
        public title title { get; set; }
        public subtitle subtitle { get; set; }
        public tooltip tooltip { get; set; }
        public plotOptions plotOptions { get; set; }
        public List<series> series { get; set; }
        public credits credits { get; set; }
        public exporting exporting { get; set; }

        public void SetDefault(bool bIsCreateConfigFile)
        {
            this.chart = new chart()
            {
                type = "pie"
            };

            this.title = new title()
            {
                text = ""
            };

            this.subtitle = new subtitle()
            {
                text = ""
            };

            this.tooltip = new tooltip()
            {
                pointFormat = "{series.name}: <b>{point.percentage:.2f}%</b>"
            };

            this.plotOptions = new plotOptions()
            {
                pie = new pie() { animation = true, allowPointSelect = true, cursor = "pointer", dataLabels = new dataLabels() { enabled = true, format = "<b>{point.name}</b>: {point.percentage:.2f} %" } }
            };

            this.exporting = new exporting()
            {
                enabled = true
            };

            this.series = new List<series>()
            {
                //new Charts.PieCharts.series() { name = "Brands", colorByPoint = true, data = new List<dataItem>()
                //    {
                //        new dataItem() { name = "Microsoft Internet Explorer", y = 56.33f },
                //        new dataItem() { name = "Chrome", y = 24.03f },
                //        new dataItem() { name = "Firefox", y = 10.38f },
                //        new dataItem() { name = "Safari", y = 4.77f },
                //        new dataItem() { name = "Opera", y = 0.91f }
                //    }
                //}
            };

            this.credits = new credits()
            {
                enabled = false
            };

            // hightcharts-ng need this
            //this.options = new options()
            //{
            //    chart = this.chart,
            //    plotOptions = this.plotOptions,
            //    tooltip = this.tooltip
            //};

            //var chart = new chart();
            //chart.type = "pie";
            //chart.plotShadow = false;
            //var options3d = new options3d();
            //options3d.enabled = true;
            //options3d.alpha = 45;
            //options3d.beta = 0;
            //chart.options3d = options3d;
            //this.chart = chart;

            //var title = new title();
            //title.text = "";
            //var style = new style();
            //style.fontSize = "14px";
            //style.color = "#000000";
            //title.style = style;
            //this.title = title;

            //var tooltip = new tooltip();
            //tooltip.pointFormat = "{series.name}: <b>{point.percentage:.1f}%</b>";
            //this.tooltip = tooltip;

            //var plotOptions = new plotOptions();
            //var pie = new pie();
            //pie.animation = false;
            //pie.allowPointSelect = true;
            //pie.cursor = "pointer";
            //pie.depth = 35;
            //var dataLabels = new dataLabels();
            //dataLabels.enabled = true;
            //dataLabels.format = "<b>{point.name}</b>: {point.percentage:.1f} %";
            //style = new style();
            //style.color = "black";
            //dataLabels.style = style;
            //pie.dataLabels = dataLabels;
            //plotOptions.pie = pie;
            //this.plotOptions = plotOptions;

            //var listSeries = new List<series>();
            //var series = new series();
            //series.name = "Total Energy";
            //series.colorByPoint = true;
            //listSeries.Add(series);
            //this.series = listSeries;

            //var credits = new credits();
            //credits.enabled = false;
            //this.credits = credits;

            if (bIsCreateConfigFile)
            {
                CreateConfigFile("PieCharts.xml");
            }
        }
    }
}
