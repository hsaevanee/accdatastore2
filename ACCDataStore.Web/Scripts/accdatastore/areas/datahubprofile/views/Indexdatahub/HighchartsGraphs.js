
var hGraphs = {
    cache: {},
    benchmark: {},
    getData: function (callback) {
        var urls = {
            'mainChart': '/DatahubProfile/IndexDatahub/MainPieChartData',
            'bigOlBarChart': '/DatahubProfile/IndexDatahub/getBarChartData',
            'monthsTrends': '/DatahubProfile/IndexDatahub/monthlyHistogram',
            'participatingBarChart': '/DatahubProfile/IndexDatahub/getBarChartData',
            'allSchoolComparison': '/DatahubProfile/IndexDatahub/getAllSchoolComparison'
        };
        $.get(urls[callback],
            function (data) {
                console.log(data);
                hGraphs.cache[callback] = data;
                hGraphs[callback]();
            });
    },
    mainChart: function () {
        var seriesTotal = [], seriesSpecific = [];
        for (var key in hGraphs.cache.mainChart.totals) {
            if (key != 'title') {
                seriesTotal.push({ name: key, y: hGraphs.cache.mainChart.totals[key] });
            }
        }
        if (hGraphs.cache.mainChart.selected != null) {
            for (var key in hGraphs.cache.mainChart.selected) {
                if (key != 'title') {
                    seriesSpecific.push({ name: key, y: hGraphs.cache.mainChart.selected[key] });
                }
            }
        }
        if (seriesTotal.length > 0) {
            hGraphs.drawPie('#datahub-index-mainpiechart', seriesTotal, hGraphs.cache.mainChart.totals.title);
        }
        if (seriesSpecific.length > 0) {
            hGraphs.drawPie('#datahub-index-specificpiechart', seriesSpecific, hGraphs.cache.mainChart.selected.title);
        }
        document.getElementById('beckmark-pies-ajax').innerHTML = Date.now() - hGraphs.benchmark.mainChart;
        document.getElementById('beckmark-pies-server').innerHTML = hGraphs.cache.mainChart.benchmarkResults;
    },
    allSchoolComparison: function() {
        var series = [];
        if (hGraphs.cache.allSchoolComparison.data != null) {
            for (var i = 0; i < hGraphs.cache.allSchoolComparison.data.length; i++) {
                series.push({ name: hGraphs.cache.allSchoolComparison.data[i].name, data: [] });
                for (var key in hGraphs.cache.allSchoolComparison.data[i]) {
                    if (key != 'name') {
                        series[i].data.push([key, hGraphs.cache.allSchoolComparison.data[i][key]]);
                    }
                }
            }
        }
        if (series.length > 0) {
            hGraphs.drawBar("#index-all-school-comparison-chart", series);
        }
        document.getElementById('beckmark-all-school-bar-ajax').innerHTML = Date.now() - hGraphs.benchmark.allSchoolComparison;
        document.getElementById('beckmark-all-school-bar-server').innerHTML = hGraphs.cache.allSchoolComparison.benchmarkResults;
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
        var series = [], counter = 0;
        for (var key in hGraphs.cache.participatingBarChart) {
            if (key != 'benchmarkResults' && hGraphs.cache.participatingBarChart[key] != null) {
                series.push({ name: hGraphs.cache.participatingBarChart[key].name, data: [] });
                for (var cat in hGraphs.cache.participatingBarChart[key]) {
                    if (cat != 'name') {
                        series[counter].data.push([cat, hGraphs.cache.participatingBarChart[key][cat]]);
                    }
                }
                counter++;
            }
        }
        if (series.length > 0) {
            hGraphs.drawBar('#datahub-index-bigolbarchart', series);
        }
        document.getElementById('beckmark-participation-bar-ajax').innerHTML = Date.now() - hGraphs.benchmark.participatingBarChart;
        document.getElementById('beckmark-participation-bar-server').innerHTML = hGraphs.cache.participatingBarChart.benchmarkResults;
    },
    monthsTrends: function () {
        var series = [];
        for (var i = 0; i < hGraphs.cache.monthsTrends.chart.length; i++) {
            for (var key in hGraphs.cache.monthsTrends.chart[i]) {
                if (key != 'months' && key != 'name') {
                    series.push({ name: hGraphs.cache.monthsTrends.chart[i].name + ' ' + key, data: hGraphs.cache.monthsTrends.chart[i][key].filter(function (x) { if (x > -1) { return x; } }) });
                }
            }
        }
        if (series.length > 0) {
            console.log(series);
            hGraphs.drawTrends("#month-trend-histogram", series);
        }
        document.getElementById('beckmark-lines-ajax').innerHTML = Date.now() - hGraphs.benchmark.monthsTrends;
        document.getElementById('beckmark-lines-server').innerHTML = hGraphs.cache.monthsTrends.benchmarkResults;
    },
    drawTrends: function (id, series) {
        $(id).highcharts({
            chart: {
                type: 'line'
            },
            title: {
                text: 'Monthly Trends',
                x: 0 //center
            },
            xAxis: {
                categories: hGraphs.cache.monthsTrends.chart[0].months.filter(function (x) { if (x != null && x != "") { return x; } })
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
                        enabled: true
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
                text: 'People Participation Overview'
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
                pointFormat: "Number of students: <b>{point.y:.1f}</b>"
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
    construct: function (type) {
        hGraphs.benchmark[type] = Date.now();
        if (hGraphs.cache[type] != null && hGraphs.cache[type] != undefined) {
            hGraphs[type]();
        } else {
            hGraphs.getData(type);
        }
    }
};

window.onload = function () {
    hGraphs.construct('allSchoolComparison');
    hGraphs.construct('mainChart');
    hGraphs.construct('participatingBarChart');
    hGraphs.construct('monthsTrends');
}