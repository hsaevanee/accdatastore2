
var hGraphs = {
    cache: {},
    benchmark: {},
    getData: function (callback, param) {
        var urls = {
            mainChart: sContextPath + 'DatahubProfile/IndexDatahub/MainPieChartDataNew',
            bigOlBarChart: sContextPath + 'DatahubProfile/IndexDatahub/getBarChartData',
            monthsTrends: sContextPath + 'DatahubProfile/IndexDatahub/monthlyHistogramNew',
            participatingBarChart: sContextPath + 'DatahubProfile/IndexDatahub/getBarChartDataNew',
            allSchoolComparison: sContextPath + 'DatahubProfile/IndexDatahub/getAllSchoolComparison',
            allIMDatazoneComparison: sContextPath + 'DatahubProfile/IndexDatahub/getAllIMDatazoneComparison',
            scotlandIndexLine: sContextPath + 'DatahubProfile/IndexDatahub/GetScotlandLineGraph'
        };
        $.get(urls[callback],
            function (data) {
                hGraphs.cache[callback] = data;
                if (param != null && param != undefined) {
                    hGraphs[callback](param);
                } else {
                    hGraphs[callback]();
                }
            });
    },
    scotlandIndexLine: function () {
        var series = [];
        for (var key in hGraphs.cache.scotlandIndexLine) {
            if (key != 'months' && key != 'name') {
                series.push({ name: hGraphs.cache.scotlandIndexLine.name + ' ' + key, data: hGraphs.cache.scotlandIndexLine[key].map(function (x) { return Math.round(x * Math.random() * 100) / 100; }) });
            }
        }
        if (series.length > 0) {
            hGraphs.drawTrends('#month-trend-histogram', series, hGraphs.cache.scotlandIndexLine.months);
        }
    },
    mainChart: function () {
        var seriesTotal = [], seriesSpecific = [];
        for (var key in hGraphs.cache.mainChart.totals) {
            if (key != 'title') {
                seriesTotal.push({ name: key, y: hGraphs.cache.mainChart.totals[key] });
            }
        }
        if (hGraphs.cache.mainChart.selected != null && hGraphs.cache.mainChart.selected.length > 0) {
            for (var i = 0; i < hGraphs.cache.mainChart.selected.length; i++) {
                for (var key in hGraphs.cache.mainChart.selected) {
                    if (key != 'title') {
                        seriesSpecific.push({ name: key, y: hGraphs.cache.mainChart.selected[key] });
                    }
                }
            }
        }
        if (seriesTotal.length > 0) {
            hGraphs.drawPie('#datahub-index-mainpiechart', seriesTotal, hGraphs.cache.mainChart.totals.title);
        }
        if (seriesSpecific.length > 0) {
            hGraphs.drawPie('#datahub-index-specificpiechart', seriesSpecific, hGraphs.cache.mainChart.selected.title);
        }
        //document.getElementById('beckmark-pies-ajax').innerHTML = Date.now() - hGraphs.benchmark.mainChart;
        //document.getElementById('beckmark-pies-server').innerHTML = hGraphs.cache.mainChart.benchmarkResults;
    },
    allSchoolComparison: function (type) {
        if (type == null || type == undefined) {
            type = 'participating';
        }
        var series = [], lineLength = 4, line = [], axis = [];
        if (hGraphs.cache.allSchoolComparison.data != null) {
            for (var i = 0; i < hGraphs.cache.allSchoolComparison.data.length; i++) {
                series.push({ name: hGraphs.cache.allSchoolComparison.data[i].name, type: 'column', xAxis: 1, data: [] });
                for (var key in hGraphs.cache.allSchoolComparison.data[i]) {
                    if (key == type) {
                        series[i].data.push(Math.round(hGraphs.cache.allSchoolComparison.data[i][key]*100)/100);
                    }
                }
            }
        }
        
        for (var i = 0; i < lineLength; i++) {
            line.push(Math.round(hGraphs.cache.allSchoolComparison.councilAverage[type]*100)/100);
            axis.push(i.toString());
        }
        series.push({ name: hGraphs.cache.allSchoolComparison.councilAverage.name + ' Average', type: 'line', xAxis: 0, data: line });
        if (series.length > 0) {
            hGraphs.drawBarLine("#index-all-school-comparison-chart", series, [type], axis);
        }
        //document.getElementById('beckmark-all-school-bar-ajax').innerHTML = Date.now() - hGraphs.benchmark.allSchoolComparison;
        //document.getElementById('beckmark-all-school-bar-server').innerHTML = hGraphs.cache.allSchoolComparison.benchmarkResults;
    },
    allIMDatazoneComparison: function (type) {
        if (type == null || type == undefined) {
            type = 'participating';
        }
        var series = [];
        if (hGraphs.cache.allIMDatazoneComparison.data != null) {
            for (var i = 0; i < hGraphs.cache.allIMDatazoneComparison.data.length; i++) {
                series.push({ name: hGraphs.cache.allIMDatazoneComparison.data[i].name, data: [] });
                for (var key in hGraphs.cache.allIMDatazoneComparison.data[i]) {
                    if (key == type) {
                        series[i].data.push([key, hGraphs.cache.allIMDatazoneComparison.data[i][key]]);
                    }
                }
            }
        }
        if (series.length > 0) {
            hGraphs.drawBar("#index-all-IMDatazone-comparison-chart", series);
        }
        //document.getElementById('beckmark-all-IMDatazone-bar-ajax').innerHTML = Date.now() - hGraphs.benchmark.allIMDatazoneComparison;
        //document.getElementById('beckmark-all-IMDatazone-bar-server').innerHTML = hGraphs.cache.allIMDatazoneComparison.benchmarkResults;
    },
    bigOlBarChart: function () {
        var series = [];
        series.push({ name: 'Student data', data: [] });
        if (hGraphs.cache.bigOlBarChart.AberdeencityData != null) {
            for (var key in hGraphs.cache.bigOlBarChart.AberdeencityData) {
                if (key != "datacode" && key != "allpupils" && key != "allpupilsexcludemovedoutscotland") {
                    series[0].data.push([key, parseInt(hGraphs.cache.bigOlBarChart.AberdeencityData[key])]);
                }
            }
        }
        if (series.length > 0) {
            hGraphs.drawBar('#datahub-index-bigolbarchart', series);
        }
    },
    participatingBarChart: function () {
        var series = [];
        for (var i = 0; i < hGraphs.cache.participatingBarChart.selected.length; i++) {
            series.push({ name: hGraphs.cache.participatingBarChart.selected[i].name, data: [] });
            for (var cat in hGraphs.cache.participatingBarChart.selected[i]) {
                if (cat != 'name') {
                    series[series.length - 1].data.push([cat, hGraphs.cache.participatingBarChart.selected[i][cat]]);
                }
            }
        }
        series.push({ name: hGraphs.cache.participatingBarChart.totals.name, data: [] });
        for (var cat in hGraphs.cache.participatingBarChart.totals) {
            if (cat != 'name') {
                series[series.length - 1].data.push([cat, hGraphs.cache.participatingBarChart.totals[cat]]);
            }
        }
        if (series.length > 0) {
            hGraphs.drawBar('#datahub-index-bigolbarchart', series);
        }
        //document.getElementById('beckmark-participation-bar-ajax').innerHTML = Date.now() - hGraphs.benchmark.participatingBarChart;
        //document.getElementById('beckmark-participation-bar-server').innerHTML = hGraphs.cache.participatingBarChart.benchmarkResults;
    },
    monthsTrends: function (type) {
        if (type == null || type == undefined) {
            type = 'participating';
        }
        var series = [];
        for (var i = 0; i < hGraphs.cache.monthsTrends.chart.length; i++) {
            for (var key in hGraphs.cache.monthsTrends.chart[i]) {
                if (key == type) {
                    series.push({ name: hGraphs.cache.monthsTrends.chart[i].name + ' ' + key, data: hGraphs.cache.monthsTrends.chart[i][key].filter(function (x) { if (x > -1) { return x; } }) });
                }
            }
        }
        if (series.length > 0) {
            hGraphs.drawTrends("#month-trend-histogram", series, hGraphs.cache.monthsTrends.chart[0].months.filter(function (x) { if (x != null && x != "") { return x; } }));
        }
        //document.getElementById('beckmark-lines-ajax').innerHTML = Date.now() - hGraphs.benchmark.monthsTrends;
        //document.getElementById('beckmark-lines-server').innerHTML = hGraphs.cache.monthsTrends.benchmarkResults;
    },
    drawTrends: function (id, series, legend) {
        $(id).highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: 'Monthly Trends',
                x: 0 //center
            },
            xAxis: {
                categories: legend
            },
            yAxis: {
                title: {
                    text: 'People percentage (%)'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }],
                min: 0,
                max: 100
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true,
                        format: '{point.y:.1f}%'
                    },
                    enableMouseTracking: false
                }
            },
            tooltip: {
                valueSuffix: '%'
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: series
        });
    },
    drawPie: function (id, data, title) {
        $(id).highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                ploitShadow: false,
                type: 'pie'
            },
            title: {
                text: title + ' Students by Age and Gender'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f}%',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                name: 'Student groups',
                colorByPoint: true,
                data: data
            }]
        });
    },
    drawBar: function (id, data) {
        $(id).highcharts({
            chart: {
                type: 'column',
                height: 400
            },
            title: {
                text: 'People Destination Overview'
            },
            xAxis: {
                type: 'category',
                label: {
                    rotation: -45,
                    style: {
                        fontSize: '13px',
                        fontFamily: 'Verdana, sans-serif'
                    }
                }
            },
            yAxis: {
                min: 0,
                max: 100,
                title: {
                    text: 'People percentage (%)'
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            tooltip: {
                pointFormat: "People: <b>{point.y:.1f}%</b>"
            },
            series: data,
            dataLabels: {
                enabled: true,
                rotation: -90,
                color: '#FFFFFF',
                align: 'right',
                format: "{point.y:.1f}",
                y: 10,
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        });
    },
    drawBarLine: function (id, series, axis1, axis2) {
        $(id).highcharts({
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: 'People Destination Overview'
            },
            xAxis: [{
                categories: axis2, 
                lineWidth: 0,
                minorGridLineWidth: 0,
                lineColor: 'transparent',
                labels: {
                    enabled: false
                },
                minorTickLength: 0,
                tickLength: 0
            },{
                categories: axis1,
                crosshair: true
            }],
            yAxis: [{
                min: 0,
                max: 100,
                labels: {
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                title: {
                    text: 'People percentage (%)',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                }
            }],
            tooltip: {
                shared: false,
                pointFormat: "People: <b>{point.y:.1f}%</b>"
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                y: 100,
                floating: false,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
            },
            plotOptions: {
                line: {
                    marker: {
                        enabled: false
                    }
                }
            },
            series: series
        });
    },
    construct: function (type, param) {
        hGraphs.benchmark[type] = Date.now();
        if (hGraphs.cache[type] != null && hGraphs.cache[type] != undefined) {
            if (param != null && param != undefined) {
                hGraphs[type](param);
            } else {
                hGraphs[type]();
            }
        } else {
            hGraphs.getData(type, param);
        }
    },
    switchAllSchoolComparison: function (type) {
        var graph = document.getElementById('index-all-school-comparison-chart');
        graph.removeChild(graph.firstChild);
        hGraphs.construct('allSchoolComparison', type);
    },
    switchMonthsTrends: function (type) {
        var graph = document.getElementById('month-trend-histogram');
        graph.removeChild(graph.firstChild);
        hGraphs.construct('monthsTrends', type);
    },
    switchAllIMDatazoneComparison: function (type) {
        var graph = document.getElementById('index-all-IMDatazone-comparison-chart');
        graph.removeChild(graph.firstChild);
        hGraphs.construct('allIMDatazoneComparison', type);
    }
};

window.onload = function () {
    var page = location.pathname.split('/')[location.pathname.split('/').length - 1].split('?')[0].toLowerCase().trim();
    if (page == 'data') {
        hGraphs.construct('allSchoolComparison');
        hGraphs.construct('allIMDatazoneComparison');
        //hGraphs.construct('mainChart'); <-- Legacy
        hGraphs.construct('participatingBarChart');
        hGraphs.construct('monthsTrends');
    } else if (page == 'scotlandindex') {
        hGraphs.construct('scotlandIndexLine');
    }
}