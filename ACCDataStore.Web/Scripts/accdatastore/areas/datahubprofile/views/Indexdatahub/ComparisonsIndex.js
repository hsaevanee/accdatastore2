var selected_school;
var selected_neighbourhood;
var datag2;
var selected = [];
var selectedN = [];
var chart;


$(document).ready(function () {
    MakeDemCharts();
    
    $('#selectedschoolcode').attr("multiple", "multiple");
    $('#selectedschoolcode').val("");
    $('#selectedschoolcode').multiselect({
        enableFiltering: true,
        numberDisplayed: 2,
        maxHeight: 200,
        onChange: function (element, checked) {
            var selectedOptions = $('#selectedschoolcode option:selected');
            if (selectedOptions.length >= 4) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $('#selectedschoolcode option').filter(function () {
                    return !$(this).is(':selected');
                });

                nonSelectedOptions.each(function () {
 
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                $('#selectedschoolcode option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
                
            }

            selected = [];

            $(selectedOptions).each(function (index, option) {
                selected.push([$(this).val(), $(this).text()]);
            });

            //console.log(selectedOptions);
    }
    });

    //$('#neighbourhoodssubmitButton').click(function () {
    //    alert("schoolsubmitButton1");      

    //})


    $('#selectedneighbourhoods').attr("multiple", "multiple");
    $('#selectedneighbourhoods').val("");
    $('#selectedneighbourhoods').multiselect({
        enableFiltering: true,
        numberDisplayed: 2,
        maxHeight: 200,
        onChange: function (element, checked) {
            //if (selectedN.length < 4) {
            //    var brands = $('#selectedneighbourhoods option:selected');
            //    selectedN = [];
            //    $(brands).each(function (index, brand) {
            //        selectedN.push([$(this).val(), $(this).text()]);
            //    });
            //}
            var selectedOptions = $('#selectedneighbourhoods option:selected');

            if (selectedOptions.length >= 4) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $('#selectedneighbourhoods option').filter(function () {
                    return !$(this).is(':selected');
                });

                nonSelectedOptions.each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                $('#selectedneighbourhoods option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
            }

            selectedN = [];
            $(selectedOptions).each(function (index, option) {
                selectedN.push([$(this).val(), $(this).text()]);
            });

            console.log(selectedN);
        }
    });

    //$('#neighbourhoodssubmitButton').click(function () {
        
    //    if (selectedN.length == 0) {
    //        alert("neighbourhoodButton1");
    //    }

    //})
    
    
    //selected_school = $('#selectedschoolcode :selected').val();
    //selected_neighbourhood = $('#selectedneighbourhoods :selected').val();
    
    //if (selected_school != null && selected_school != "") { FunctiongetDetail(selected_school, "school"); };
    //if (selected_neighbourhood != null && selected_neighbourhood != "") { FunctiongetDetail(selected_neighbourhood, "neighbourhood"); };
    //FunctiongetDetail("asd", "school");

    $('#selectedschoolcode').multiselect('select', selectionParams.school);
    $('#selectedschoolcode').multiselect('refresh');

    $('#selectedneighbourhoods').multiselect('select', selectionParams.neighbourhood);
    $('#selectedneighbourhoods').multiselect('refresh');

});

function goToCreateURL(object) {
    var code = $('#selectedschoolcode :selected').text();
    return object.href += code;
}

function SubmitForm(buttonID) {
   
    if (buttonID == "neighbourhoodssubmitButton") {
        if (selectedN.length == 0) {
            alert('Please select Neighbourhood');
            return false;
        } else {
            return true;
        }

    } else if (buttonID == "schoolsubmitButton") {

        if (selected.length == 0) {
            alert('Please select School');
            return false;
        } else {
            return true;
        }


    }
}

function Test(selected)
{
    var list = []

    selected.forEach(function (item) {
        list.push(item[0]);
    })

    var parms = {
        list: list
    }
    $.ajax({
        type: "POST",
        traditional: true,
        url: sContextPath + 'DatahubProfile/IndexDatahub/Test',
        data: parms,
        dataType: "html",
        success: function (data) {

            $('#myPartialContainer').html(data);


            var names = [];
            $("#opAllCa tbody th").each(function () {
                names.push({ name: $(this).text() });
            });

            chart = $('#container1').highcharts({
                data: {
                    table: 'opAllCa',
                    switchRowsAndColumns: true,
                    startColumn: 2,
                    firstRowAsNames: false
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Data extracted from a HTML table in the page'
                },
                yAxis: {

                    allowDecimals: false,
                    title: {
                        text: 'Units'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.name + '</b><br/>' +
                            this.point.y + ' ' + this.point.name.toLowerCase();
                    }
                },
                credits: {
                    enabled: false
                },
                series: names
            });
            

            
            //data.forEach(function (item) {
            //    $('#opAllC > tbody:last-child').append(
            //        '<tr id="' + item.datacode + '"><td class="text-center">' + item.datacode + '</td>' +
            //        '<td class="text-center">' + item.allpupilsexcludemovedoutscotland + '</td>' +
            //        '<td class="text-center">' + item.allMalepupils + '</td>' +
            //        '<td class="text-center">' + item.allFemalepupils + '</td>' +
            //        '<td class="text-center">' + item.all16pupils + '</td>' +
            //        '<td class="text-center">' + item.all17pupils + '</td>' +
            //        '<td class="text-center">' + item.all18pupils + '</td>' +
            //        '<td class="text-center">' + item.all19pupils + '</td>' +
            //        '</tr>'
            //        );
            //})
        }
    });
}

function MakeDemCharts() {
    //Opportunities for all clients
    createDataTable('OpportunitiesForAllClientsTable');
    var names = [];
    $("#OpportunitiesForAllClientsTable tbody th").each(function () {
        names.push({ name: $(this).text() });
    });

    createChartFromDataTable('OpportunitiesForAllClientsChart', 'OpportunitiesForAllClientsTable', 'Opportunities For All Clients', names, 2)
    //Current Positive Destinations
    createDataTable('CurrentPositiveDestintionsTable1');
    names = [];
    $("#CurrentPositiveDestintionsTable1 tbody th").each(function () {
        names.push({ name: $(this).text() });
    });

    createChartFromDataTable('CurrentPositiveDestintionsChart1', 'CurrentPositiveDestintionsTable1', 'Current Positive Destintions 1', names, 1)

    createDataTable('CurrentPositiveDestintionsTable2');
    names = [];
    $("#CurrentPositiveDestintionsTable2 tbody th").each(function () {
        names.push({ name: $(this).text() });
    });

    createChartFromDataTable('CurrentPositiveDestintionsChart2', 'CurrentPositiveDestintionsTable2', 'Current Positive Destintions 2', names, 1)


    //Non Positive Destinations
    createDataTable('NonPositiveDestinationsTable');
    names = [];
    $("#NonPositiveDestinationsTable tbody th").each(function () {
        names.push({ name: $(this).text() });
    });

    createChartFromDataTable('NonPositiveDestinationsChart', 'NonPositiveDestinationsTable', 'Non Positive Destinations', names, 2)
}

function createDataTable(id) {
    var table = $('#'+ id).DataTable({
        dom: 'Bfrtip',
        info: false,
        paging: false,
        searching: false,
        order: [],
        select: {
            style: 'os',
            items: 'cell'
        },
        buttons: [
            'copy',
            'excel',
            'pdf',
            {
                extend: 'selectedSingle',
                text: 'Log selected data',
                action: function (e, dt, button, config) {
                    console.log(dt.cell({ selected: true }).data());
                }
            }
        ]
    });

    //table.buttons().container().appendTo($('.col-sm-6:eq(0)', table.table().container()));
}

function createChartFromDataTable(idChart, idTable, title, names, startColumn) {
    $('#'+ idChart).highcharts({
        data: {
            table: idTable,
            switchRowsAndColumns: true,
            startColumn: startColumn,
            firstRowAsNames: false
        },
        chart: {
            type: 'column'
        },
        title: {
            text: title
        },
        yAxis: {

            allowDecimals: false,
            title: {
                text: 'People percentage (%)'
            }
        },
        tooltip: {
            formatter: function () {
                return '<b>' + this.series.name + '</b><br/>' +
                    this.point.y + '% of people in ' + this.point.name.toLowerCase();
            }
        },
        credits: {
            enabled: false
        },
        series: names
    });
}

function Test1(selected, type) {
    var list = []

    selected.forEach(function (item) {
        list.push(item[0]);
    })

    var parms = {
        list: list,
        type: type
    }
    $.ajax({
        type: "POST",
        traditional: true,
        url: sContextPath + 'DatahubProfile/IndexDatahub/Data',
        data: parms,
        dataType: "html",
        success: function (data) {
            alert(data);
            $('#table-test1').html(data);
            MakeDemCharts();
            //var table = $('#OpportunitiesForAllClientsTable').DataTable({
            //    dom: 'Bfrtip',
            //    info: false,
            //    paging: false,
            //    searching: false,
            //    order: [],
            //    select: {
            //        style: 'os',
            //        items: 'cell'
            //    },
            //    buttons:[
            //    {
            //        extend: 'selectedSingle',
            //        text: 'Log selected data',
            //        action: function ( e, dt, button, config ) {
            //            console.log( dt.cell( { selected: true } ).data() );
            //        }
            //    }   
            //    ]
            //});

            ////table.buttons().container().appendTo($('.col-sm-6:eq(0)', table.table().container()));

            //Opportunities for all clients
            //createDataTable('OpportunitiesForAllClientsTable');
            //var names = [];
            //$("#OpportunitiesForAllClientsTable tbody th").each(function () {
            //    names.push({ name: $(this).text() });
            //});

            //createChartFromDataTable('OpportunitiesForAllClientsChart', 'OpportunitiesForAllClientsTable', 'Opportunities For All Clients', names, 2)
            ////Current Positive Destinations
            //createDataTable('CurrentPositiveDestintionsTable1');
            //names = [];
            //$("#CurrentPositiveDestintionsTable1 tbody th").each(function () {
            //    names.push({ name: $(this).text() });
            //});

            //createChartFromDataTable('CurrentPositiveDestintionsChart1', 'CurrentPositiveDestintionsTable1', 'Current Positive Destintions 1', names,1)

            //createDataTable('CurrentPositiveDestintionsTable2');
            //names = [];
            //$("#CurrentPositiveDestintionsTable2 tbody th").each(function () {
            //    names.push({ name: $(this).text() });
            //});

            //createChartFromDataTable('CurrentPositiveDestintionsChart2', 'CurrentPositiveDestintionsTable2', 'Current Positive Destintions 2' , names, 1)


            ////Non Positive Destinations
            //createDataTable('NonPositiveDestinationsTable');
            //names = [];
            //$("#NonPositiveDestinationsTable tbody th").each(function () {
            //    names.push({ name: $(this).text() });
            //});

            //createChartFromDataTable('NonPositiveDestinationsChart', 'NonPositiveDestinationsTable', 'Non Positive Destinations', names,2)

            //chart = $('#OpportunitiesForAllClientsChart').highcharts({
            //    data: {
            //        table: 'OpportunitiesForAllClientsTable',
            //        switchRowsAndColumns: true,
            //        startColumn: 2,
            //        firstRowAsNames: false
            //    },
            //    chart: {
            //        type: 'column'
            //    },
            //    title: {
            //        text: 'Title, please'
            //    },
            //    yAxis: {

            //        allowDecimals: false,
            //        title: {
            //            text: 'Percentage'
            //        }
            //    },
            //    tooltip: {
            //        formatter: function () {
            //            return '<b>' + this.series.name + '</b><br/>' +
            //                this.point.y + ' ' + this.point.name.toLowerCase();
            //        }
            //    },
            //    credits: {
            //        enabled: false
            //    },
            //    series: names
            //});
        }
    });
}



function FunctiongetDetail(schcode, dataname) {
    var JSONObject = {
        "keyvalue": schcode,
        "keyname": dataname,

    }
    myFunctionColumn(JSONObject);
        //$.ajax({
        //    type: 'POST',
        //    url: sContextPath + 'DatahubProfile/IndexDatahub/GetDatadetails',
        //    data: JSON.stringify(JSONObject),
        //    contentType: 'application/json; charset=utf-8',
        //    dataType: 'json',
        //    success: function (data) {
        //        alert("getdatadetails"+data.length);
        //    },
        //    error: function (xhr, err) {
        //        if (xhr.readyState != 0 && xhr.status != 0) {
        //            alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
        //            alert('responseText: ' + xhr.responseText);
        //        }
        //    }
        //});
    
}

function myFunctionColumn(JSONObject) {
    $.ajax({
        type: 'POST',
        url: sContextPath + 'DatahubProfile/IndexDatahub/SearchByName',
        data: JSON.stringify(JSONObject),
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

function drawChartColumn(data) {

    $('#divChartContainer')
            .highcharts(
                    {
                        chart: {
                            type: 'column'
                        },
                        title: {
                            text: data.dataTitle
                        },
                        subtitle: {
                            text: ''
                        },
                        xAxis: {
                            //categories: [ '0%', '5%', '10%', '15%','20%','25%','30%'],
                            categories: data.dataCategories,
                            title: {
                                text: 'Destination'
                            }
                        },
                        yAxis: {
                            min: 0,
                            title: {
                                text: 'Percentages'
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
                        series: [{ name: data.schoolname, data: data.Schdata }, { name: 'All Clients', data: data.Abdcitydata }],
                        credits: {
                            enabled: false
                        }
                    });
}