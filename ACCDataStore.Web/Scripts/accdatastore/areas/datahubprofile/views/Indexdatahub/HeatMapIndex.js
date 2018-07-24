$(document).ready(function () {
    loadData("Participating");
    $(document.body).on('click', '.a-close-popup-information', function () {
        $("#popup-information").hide(250);
    });
    InitSpinner();
    $("#selecteddataset").change(function () {
        var datasetname = $('#selecteddataset :selected').text();
        loadData(datasetname);
    });
});

// initialize spinner on ajax loading
function InitSpinner() {
    $(document).ajaxSend(function () {
        $('#divSpinner').show();
    }).ajaxComplete(function () {
        $('#divSpinner').hide();
    }).ajaxError(function (e, xhr) {
        // do something on ajax error
    });
}

// initialize map object
function InitMap(data) {
    var mapCenter = new google.maps.LatLng(57.151810, -2.094451);
    var mapOptions = {
        zoom: 11,
        center: mapCenter
    }

    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    //map.data.loadGeoJson('https://dl.dropboxusercontent.com/u/870146/KML/V2/Datazone_with_Desc.json' + "?rand=" + (new Date()).valueOf());

    map.data.addGeoJson(datazonejsondata);

    map.data.setStyle(function (feature) {
        var low = [5, 69, 54];  // color of smallest datum
        var high = [151, 83, 34];   // color of largest datum

        var statisticdata = -1;

        var temp = feature.getProperty('ZONECODE');

        for (var i = 0; i < data.datacode.length; i++) { 
            if (data.datacode[i] == temp) {
                statisticdata = data.data[i];
                break;
            }

        }

        var delta = (statisticdata - data.minimum) / (data.maximum - data.minimum);

        var color = [];

        for (var i = 0; i < 3; i++) {
            // calculate an integer color based on the delta
            color[i] = (high[i] - low[i]) * delta + low[i];
        }


        // determine whether to show this shape or not
        var showRow = true;
        if (statisticdata == -1) {
            showRow = false;
        }

        var outlineWeight = 0.5, zIndex = 1;
        if (feature.getProperty('datazone') === 'hover') {
            outlineWeight = zIndex = 2;
        }

        return {
            strokeWeight: outlineWeight,
            strokeColor: '#fff',
            zIndex: zIndex,
            fillColor: 'hsl(' + color[0] + ',' + color[1] + '%,' + color[2] + '%)',
            fillOpacity: 0.75,
            visible: showRow
        };
    });

    // update and display the legend
    document.getElementById('census-min').textContent =
    data.minimum.toFixed(1) +"%";
    document.getElementById('census-max').textContent =
        data.maximum.toFixed(1) + "%";


    // set up the style rules and events for google.maps.Data
    map.data.addListener('mouseover', function (event) {
        // set the hover state so the setStyle function can change the border
        event.feature.setProperty('datazone', 'hover');
        var temp = event.feature.getProperty('ZONECODE');
        var statisticdata = -1;
        for (var i = 0; i < data.datacode.length; i++) {
            if (data.datacode[i] == temp) {
                statisticdata = data.data[i];
                break;
            }

        }

        var percent = (statisticdata - data.minimum) /
            (data.maximum - data.minimum) * 100;

        // update the label
        document.getElementById('data-label').textContent =
            event.feature.getProperty('ZONECODE');
        document.getElementById('data-value').textContent =
            statisticdata.toFixed(1) + '%';
        document.getElementById('data-box').style.display = 'block';
        document.getElementById('data-caret').style.display = 'block';
        document.getElementById('data-caret').style.paddingLeft = percent + '%';

    });

    map.data.addListener('mouseout', function (event) {
        event.feature.setProperty('datazone', 'normal');
    });

    map.data.addListener('click', function (kmlEvent) {
         SearchData(kmlEvent.feature.getProperty('ZONECODE'), "ZoneCode");
    });

}


function loadData(datasetname) {
    var JSONObject = {
        "datasetname": datasetname,
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/GetdataforHeatmap",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            InitMap(data)
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

// show search result when click on map
function ShowPopupInformation(sInformation) {
    //var popupInformation = document.getElementById('popup-information');
    //popupInformation.innerHTML = sInformation;
    //$("#popup-information").show(250);
    $("#divinformationContainer").html(sInformation);
}



// call server side method via ajax
function SearchData(sCondition, sKeyname) {
    var JSONObject = {
        "keyvalue": sCondition,
        "keyname": sKeyname
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/SearchByName",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            ShowPopupInfo(data);
            drawChartColumn(data);
            drawPieChart(data);
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

function myFunctionColumn(pdata) {
    $.ajax({
        type: 'POST',
        url: sContextPath + 'DatahubProfile/IndexDatahub/GetChartDataforMap',
        data: JSON.stringify(pdata.dataSeries),
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

function drawPieChart(data) {
    var name = Array();
    var y = Array();
    var dataArrayFinal = Array();
    for (i = 0; i < data.dataCategories.length; i++) {
        name[i] = data.dataCategories[i];
        y[i] = data.Schdata[i];
    }

    for (j = 0; j < name.length; j++) {
        var temp = new Array(name[j], y[j]);
        dataArrayFinal[j] = temp;
    }
    // Build the chart
    $('#divPieChartContainer').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: data.dataTitle
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Percentage',
            colorByPoint: true,
            data: dataArrayFinal
        }]
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

function ShowPopupInfo(data) {
    var sInformation = "<hr><div class='panel panel-primary text-center'> <div class='panel-heading'>";
    sInformation += "<h4 class='text-center'>" + data.dataTitle + "</h4>";
    sInformation += "</div><div class='panel-body'>";
    sInformation += "<table class='table table-bordered table-hover'>";
    sInformation += "<thead><tr><th> </th><th class='text-center'>" + data.schoolname + "</th><th class='text-center'> Aberdeen City </th></tr></thead>";
    sInformation += "<tbody>";
    if (data.dataCategories.length != 0) {
        for (var i = 0; i < data.dataCategories.length; i++) {
            //sInformation += "<tr><td>" + data.dataCategories[i] + "</td><td  align='center'>" + "<input type='button' style='width: 50px; height:25px' value='" + data.Schdata[i].toFixed(2) + "'id='" + data.dataCategories[i] + "'" + "onclick='goToCreateURL(id)'/></td><td  align='center'>" + data.Abdcitydata[i].toFixed(2) + "</td><tr>";
            sInformation += "<tr><td class='text-left'>" + data.dataCategories[i] + "</td><td  align='center'> <a href= " + sContextPath + "DatahubProfile/IndexDatahub/GetListpupils?searchby=" + data.searchby + "&code=" + data.searchcode + "&dataname=" + data.dataCategories[i] + "'><button enabled class='btn btn-info btn-xs enabled'>" + data.Schdata[i].toFixed(2) + "</button></a></td><td  align='center'> <a href=" + sContextPath + "DatahubProfile/IndexDatahub/GetListpupils?searchby=school&code=100&dataname=" + data.dataCategories[i] + "'><button enabled class='btn btn-info btn-xs enabled'>" + data.Abdcitydata[i].toFixed(2) + "</button></a></td><tr>";
        }

    } else {

        sInformation += "<tr><td colspan='3' align='center'> No data available</td><tr>"
    }


    sInformation += "</tbody></table></div></div>";
    ShowPopupInformation(sInformation);

}

//function goToCreateURL(object) {
//    alert("goToCreateURL");
//    return object.href = '/DatahubProfile/IndexDatahub/GetListpupils?searchby=school&code=100&dataname=' + datasetname;
//    //?searchby=school&code=100&dataname=Pupils16
//}

function SetErrorMessage(xhr) {
    if (xhr.responseText.length > 0) {
        var sErrorMessage = JSON.parse(xhr.responseText).Message;
        alert(sErrorMessage);
    }
}

