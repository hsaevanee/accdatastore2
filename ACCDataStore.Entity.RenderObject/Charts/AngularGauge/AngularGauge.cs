using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ACCDataStore.Entity.RenderObject.Charts.AngularGauge;
using ACCDataStore.Entity.RenderObject.Charts.Generic;

namespace ACCDataStore.Entity.RenderObject.Charts.AngularGauge
{
    public class AngularGauge : BaseRenderObject
    {
        public chart chart { get; set; }
        public title title { get; set; }
        public subtitle subtitle { get; set; }
        public pane pane { get; set; }
        public yAxis yAxis { get; set; }
        public List<series> series { get; set; }
        public plotOptions plotOptions { get; set; }
        public credits credits { get; set; }

        public void SetDefault(bool bIsCreateConfigFile)
        {
            //var chart = new chart();
            //chart.type = "gauge";
            //chart.plotBackgroundColor = null;
            //chart.plotBackgroundImage = null;
            //chart.plotBorderWidth = 0;
            //chart.plotShadow = false;
            //this.chart = chart;

            //var title = new title();
            //title.text = "kWh";
            //var style = new style();
            //style.fontSize = "14px";
            //style.color = "#000000";
            //title.style = style;
            //this.title = title;

            //var pane = new pane();
            //pane.startAngle = -150;
            //pane.endAngle = 150;
            //var listBackground = new List<background>();
            //var background = new background();
            //var backgroundColor = new backgroundColor();
            //var linearGradient = new linearGradient();
            //linearGradient.x1 = 0;
            //linearGradient.x2 = 0;
            //linearGradient.y1 = 0;
            //linearGradient.y2 = 1;
            //backgroundColor.linearGradient = linearGradient;
            //var stops = new List<List<object>>() { new List<object>() { 0, "#DDDFFF" }, new List<object>() { 1, "#FFFFFF" } };
            //backgroundColor.stops = stops;
            //background.borderWidth = 0;
            //background.outerRadius = "103%";
            //background.innerRadius = "0%";
            //background.backgroundColor = backgroundColor;
            //listBackground.Add(background);
            //pane.background = listBackground;
            //this.pane = pane;

            //var yAxis = new yAxis();
            //yAxis.min = 0;
            //yAxis.max = 200;
            //yAxis.minorTickInterval = "auto";
            //yAxis.minorTickWidth = 1;
            //yAxis.minorTickLength = 10;
            //yAxis.minorTickPosition = "inside";
            //yAxis.minorTickColor = "#666";
            //yAxis.tickPixelInterval = 30;
            //yAxis.tickWidth = 2;
            //yAxis.tickPosition = "inside";
            //yAxis.tickLength = 10;
            //yAxis.tickColor = "#666";
            //var labels = new labels();
            //labels.step = 2;
            //labels.rotation = "auto";
            //style = new style();
            //style.fontSize = "14px";
            //style.color = "#000000";
            //labels.style = style;
            //yAxis.labels = labels;
            //title = new title();
            //title.text = "kWh";
            //style = new style();
            //style.fontFamily = "Tahoma";
            //style.fontSize = "36px";
            //style.fontWeight = "bold";
            //style.color = "#000000";
            //title.style = style;
            //yAxis.title = title;
            //var listPlotBands = new List<plotBands>();
            //var plotBands = new plotBands();
            //plotBands.from = 0;
            //plotBands.to = 120;
            //plotBands.color = "#55BF3B";
            //plotBands.innerRadius = "80%";
            //plotBands.outerRadius = "100%";
            //listPlotBands.Add(plotBands);
            //plotBands = new plotBands();
            //plotBands.from = 120;
            //plotBands.to = 160;
            //plotBands.color = "#DDDF0D";
            //plotBands.innerRadius = "80%";
            //plotBands.outerRadius = "100%";
            //listPlotBands.Add(plotBands);
            //plotBands = new plotBands();
            //plotBands.from = 160;
            //plotBands.to = 200;
            //plotBands.color = "#DF5353";
            //plotBands.innerRadius = "80%";
            //plotBands.outerRadius = "100%";
            //listPlotBands.Add(plotBands);
            //yAxis.plotBands = listPlotBands;
            //this.yAxis = yAxis;

            //var listSeries = new List<series>();
            //var series = new series();
            //series.name = "";
            //series.data = new List<float?> { null };
            //var tooltip = new tooltip();
            //tooltip.valueSuffix = "kWh";
            //series.tooltip = tooltip;
            //var dataLabels = new dataLabels();
            //dataLabels.enabled = true;
            //style = new style();
            //style.fontSize = "36px";
            //style.fontWeight = "bold";
            //style.color = "#000000";
            //dataLabels.style = style;
            //series.dataLabels = dataLabels;
            //listSeries.Add(series);
            //this.series = listSeries;

            //var plotOptions = new plotOptions();
            //var gauge = new gauge();
            //gauge.animation = false;
            //var dial = new dial();
            //dial.radius = "100%";
            //dial.backgroundColor = "silver";
            //dial.borderColor = "black";
            //dial.borderWidth = 1;
            //dial.baseWidth = 10;
            //dial.topWidth = 1;
            //dial.baseLength = "90%";
            //dial.rearLength = "10%";
            //gauge.dial = dial;
            //plotOptions.gauge = gauge;
            //this.plotOptions = plotOptions;

            //var credits = new credits();
            //credits.enabled = false;
            //this.credits = credits;

            if (bIsCreateConfigFile)
            {
                CreateConfigFile("AngularGauge.xml");
            }
        }
    }
}