var mSchCriteriaParams, mdataParams;
var dataNationality;

$(function () {
    InitSpinner();
});

$(document).ready(function () {

    $('#buttonGetData').click(function () {
        if (validateCheckBoxs() == true) {
            document.forms[0].submit();
        }
        
    });


    $("input[name='ethnicity']").click(function () {
        $('input[name="CheckEthnicityAll"]').prop("checked", false);
    });

    $("input[name='CheckEthnicityAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="ethnicity"]').prop("checked", true);
        } else {
            $('input[name="ethnicity"]').prop("checked", false);
        }
    });

    $("input[name='gender']").click(function () {
        $('input[name="CheckGenderAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitem']").click(function () {
        $('input[name="CheckDataitemAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitemAll']").change(function () {
        if (this.checked) {
            $('input[name="CheckDataitem"]').prop("checked", true);
        } else {
            $('input[name="CheckDataitem"]').prop("checked", false);
        }
    });

    $("input[name='CheckGenderAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="gender"]').prop("checked", true);
        } else {
            $('input[name="gender"]').prop("checked", false);
        }
    });

});

function validateCheckBoxs() {
    // get all checked checkbox
    var arrCheckboxCheckedEthnic = [];
    $('input[name="ethnicity"]:checked').each(function () {
        arrCheckboxCheckedEthnic.push($(this).val());
    });

    if (arrCheckboxCheckedEthnic.length == 0) {
        alert('Please select EthnicBackground');
        return false;    
    } else {
        return true;
    }

}


function myFunctionBar() {
    // clear divContainer
    //$("#divBarChartContainer").html("");
    //$("#divColumnChartContainer").html("");
    //$("#divLineChartContainer").html("");
    var arrCheckboxCheckedCheckDataitem = [];
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {
        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/EthnicBackground/GetChartDataEthnic',
            data: JSON.stringify(arrCheckboxCheckedCheckDataitem),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                drawChartBar(data);
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
    } 
}

function drawChartBar(data) {
    $('#divBarChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'bar'
                        },
                        title: {
                            text: 'Ethnic Background - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Ethnic Background'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Ethnic Background'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td nowrap style="color:{series.color};padding:0">{series.name}: </td>'
                                    + '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: data.ChartSeries,
                        credits: {
                            enabled: false
                        }
                    });
}

function myFunctionColumn() {
    //alert("myFunctionColumn");
    var arrCheckboxCheckedCheckDataitem = [];
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {

        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/EthnicBackground/GetChartDataEthnic',
            data: JSON.stringify(arrCheckboxCheckedCheckDataitem),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                drawChartColumn(data);
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
    }

}

function drawChartColumn(data) {
    $('#divColumnChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: 'Ethnic Background - Primary Schools (%pupils)'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Ethnic Background'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Ethnic Background'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td nowrap style="color:{series.color};padding:0">{series.name}: </td>'
                                    + '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                pointPadding: 0.2,
                                borderWidth: 0
                            }
                        },
                        series: data.ChartSeries,
                        credits: {
                            enabled: false
                        }
                    });
}

function myFunctionLine() {
    var arrCheckboxCheckedCheckDataitem = [];
    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {
        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/EthnicBackground/GetChartDataEthnic',
            data: JSON.stringify(arrCheckboxCheckedCheckDataitem),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                drawChartLine(data);
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
    }
}

function drawChartLine(data) {
    $('#divLineChartContainer').highcharts({
        title: {
            text: 'Ethnic Background'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            categories: data.ChartCategories,
            title: {
                text: 'Ethnic Background'
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: '% Pupils in Each Ethnic Background'
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }]
        },
        tooltip: {
            valueSuffix: '°C'
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            borderWidth: 0
        },
        series: data.ChartSeries,
    });
}



