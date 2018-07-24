$(function () {
    InitSpinner();
});

$(document).ready(function () {


    $('#buttonGetData').click(function () {
        if (validateCheckBoxs() == true && validateDropdownlist() == true) {
            document.forms[0].submit();
        }

    });

    $("input[name='SIMD']").click(function () {
        $('input[name="CheckSIMDAll"]').prop("checked", false);
    });

    $("input[name='years']").click(function () {
        $('input[name="CheckYearAll"]').prop("checked", false);
    });

    $("input[name='CheckYearAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="years"]').prop("checked", true);
            //$('input[name="years"]').attr( "disabled", "disabled" );
        } else {
            $('input[name="years"]').prop("checked", false);
        }
    });

    $("input[name='CheckSIMDAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="SIMD"]').prop("checked", true);
        } else {
            $('input[name="SIMD"]').prop("checked", false);
        }
    });

    $("input[name='CheckDataitem']").click(function () {
        $('input[name="CheckDataitemAll"]').prop("checked", false);
    });

    $("input[name='CheckDataitemAll']").change(function () {
        if (this.checked) {
            //alert('ChecknationalityAll check');
            $('input[name="CheckDataitem"]').prop("checked", true);
        } else {
            $('input[name="CheckDataitem"]').prop("checked", false);
        }
    });

    $('#datatable').DataTable({
        paging: false,
        dom: 'Bfrtip',
        buttons: [
             {
                 extend: 'copyHtml5',
             },
             {
                 extend: 'excelHtml5',
             },
             {
                 extend: 'csvHtml5',
             },
             {
                 extend: 'pdfHtml5',
                 orientation: 'landscape',

             },
             {
                 extend: 'print',
             }
        ]
    });

});

function validateCheckBoxs() {
    // get all checked checkbox
    var arrCheckboxCheckedYear = [];
    $('input[name="years"]:checked').each(function () {
        arrCheckboxCheckedYear.push($(this).val());
    });
    var arrCheckboxCheckedSIMD = [];
    $('input[name="SIMD"]:checked').each(function () {
        arrCheckboxCheckedSIMD.push($(this).val());
    });

    if (arrCheckboxCheckedSIMD.length == 0) {
        alert('Please select Deciles');
        return false;
    } else if(arrCheckboxCheckedYear.length ==0)
    {
        alert('Please select Years');
        return false;
    }else{
         return true;
    }

}

function validateDropdownlist() {
    var value1 = $('#selectedschoolname :selected').text();
    //var value2 = $('#selectedschoolname2 :selected').text();

    if (value1 == "---Please Select School---") {
        alert('Please select School');
        return false;
    } else {
        return true;
    }

}

function myFunctionBar() {
    var arrCheckboxCheckedCheckDataitem = [];
    var arrCheckboxCheckedCheckgender = [];

    $('input[name="CheckDataitem"]:checked').each(function () {
        arrCheckboxCheckedCheckDataitem.push($(this).val());
    });

    if (arrCheckboxCheckedCheckDataitem.length == 0) {
        alert("Please select data to create graph");
    } else {
        $.ajax({
            type: 'POST',
            url: sContextPath + 'SchoolProfile/SIMD/GetChartDataSIMD',
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
    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'bar'
                        },
                        title: {
                            text: 'Scottish Index of Multiple Deprivation'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Deciles'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Decile'
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
            url: sContextPath + 'SchoolProfile/SIMD/GetChartDataSIMD',
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
    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: 'Scottish Index of Multiple Deprivation'
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.ChartCategories,
                            title: {
                                text: 'Deciles'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: '% Pupils in Each Decile'
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