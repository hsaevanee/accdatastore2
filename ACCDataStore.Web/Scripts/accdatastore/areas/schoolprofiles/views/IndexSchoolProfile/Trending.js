

$(document).ready(function () {
    var data = {
        table: 'datatable'
        //switchRowsAndColumns: true
    };
    var chart = {
        type: 'line'
    };
    var title = {
        text: datatitle
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: 'Units'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ' + this.point.name.toLowerCase();
        }
    };
    var credits = {
        enabled: false
    };
    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;

    var chart = $('#Chartcontainer').highcharts(json, function (chart) {
        //remove English as a \"first-language\" data from graphs
        console.log(chart)
        var len = this.series.length
        var newCategories = [];
        //remove point in dataseries
        for (i = 0; i < len; i++) {
            var datalen = this.series[i].data.length
            for (j = 0; j < datalen; j++) {
                if (this.series[i].data[j].name == "English as a \"first-language\"") {
                    removeindex = j;
                    this.series[i].data[j].remove();
                    break
                }
            }
        }
        // create new catagories for label in X-line
        for (j = 0; j < this.series[0].data.length; j++) {
            newCategories.push(this.series[0].data[j].name);
        }
        this.xAxis[0].setCategories(newCategories); // set net x label

        //chart.xAxis[0].setCategories(categories);
        //chart.redraw();
    });
});