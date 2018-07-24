
var map; // map object

// initialize all kml layers
var kml = {
    // type : 0 = direct kml file, 1 = kml from fusion table
    //a: {
    //    name: "Aberdeen PostCode",
    //    type: 1,
    //    query: {
    //        select: "col4",
    //        from: "1FDKdWdKSXGkpQmvvMxVuxkBUmXlIuAqbuGpKpugM",
    //        where: ""
    //    }
    //},
    a: {
        name: "Schools Locations",
        type: 2,
        url: 'https://storage.googleapis.com/maps-devrel/google.json' + "?rand=" + (new Date()).valueOf(),
        dataType : 1
    },
    b: {
        name: "Aberdeen DataZone Districts",
        type: 2,
        url: '~/Scripts/accdatastore/areas/datahubprofile/views/Indexdatahub/Datazone_with_Desc.json' + "?rand=" + (new Date()).valueOf(),
        dataType: 2
    },
    c: {
        name: "Neighbourhoods Districts",
        type: 2,
        url: 'https://dl.dropboxusercontent.com/u/55734762/Neighbourhoods.json' + "?rand=" + (new Date()).valueOf(),
        dataType: 3
    },
    //d: {
    //    name: "Glasgow Districts",
    //    type: 2,
    //    url: 'https://dl.dropboxusercontent.com/u/55734762/Neighbourhoods.json' + "?rand=" + (new Date()).valueOf(),
    //    dataType: 4
    //},
};

// on document ready
$(function () {
    // binding 'close' click event to popup windows
    $(document.body).on('click', '.a-close-popup-information', function () {
        $("#popup-information").hide(250);
    });
    InitSpinner();
    InitMap();
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
function InitMap() {
    var mapCenter = new google.maps.LatLng(57.151810, -2.094451);
    var mapOptions = {
        zoom: 11,
        center: mapCenter
    }

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    // create dummy overlay
    mapOverlay = new google.maps.OverlayView();
    mapOverlay.draw = function () { };
    mapOverlay.setMap(map);

    CreateLayerControl();
}

// show search result when click on map
function ShowPopupInformation(sInformation) {
    //var popupInformation = document.getElementById('popup-information');
    //popupInformation.innerHTML = sInformation;
    //$("#popup-information").show(250);
    $("#divinformationContainer").html(sInformation);
}



// call server side method via ajax
function SearchData(sCondition,sKeyname) {
    //var param = JSON.stringify({ 'sCondition': sCondition }); // just an example, need to adjust
    //sCondition = "5235324,MILLTIMBER PRIMARY SCHOOL";
    //var res = sCondition.split(",");
    //var kcode = res[0];
    //var kname = res[1];

    //var JSONObject = {
    //    "keyvalue": kcode,
    //    "keyname": sKeyname
    //}

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
            //alert(data.schoolname);
            //alert(data.data);
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
            //sInformation += "<tr><td>" + data.dataCategories[i] + "</td><td  align='center'>" + data.Schdata[i].toFixed(2) + "</td><td  align='center'>" + data.Abdcitydata[i].toFixed(2) + "</td><tr>";
            sInformation += "<tr><td class='text-left'>" + data.dataCategories[i] + "</td><td  align='center'> <a href= " + sContextPath + "DatahubProfile/IndexDatahub/GetListpupils?searchby=" + data.searchby + "&code=" + data.searchcode + "&dataname=" + data.dataCategories[i] + "'><button enabled class='btn btn-info btn-xs enabled'>" + data.Schdata[i].toFixed(2) + "</button></a></td><td  align='center'> <a href= " + sContextPath + "DatahubProfile/IndexDatahub/GetListpupils?searchby=school&code=100&dataname=" + data.dataCategories[i] + "'><button enabled class='btn btn-info btn-xs enabled'>" + data.Abdcitydata[i].toFixed(2) + "</button></a></td><tr>";

        }

    } else {

        sInformation += "<tr><td colspan='3' align='center'> No data available</td><tr>";
    }


    sInformation += "</tbody></table></div></div>";
    ShowPopupInformation(sInformation);

}

function SetErrorMessage(xhr) {
    if (xhr.responseText.length > 0) {
        var sErrorMessage = JSON.parse(xhr.responseText).Message;
        alert(sErrorMessage);
    }
}

// show/hide layer on click event
function ToggleKMLLayer(checked, id) {
    if (checked) {
        var layer;
        switch (kml[id].type) {
            case 0: // direct kml file
                layer = new google.maps.KmlLayer(kml[id].url, {
                    preserveViewport: true,
                    suppressInfoWindows: true
                });
                break;
            case 1: // kml from fusion table
                layer = new google.maps.FusionTablesLayer({
                    query: kml[id].query,
                    suppressInfoWindows: true
                });
                break;
            case 2: // geojson for geometry
                layer = new google.maps.Data();

                if (kml[id].dataType == 1) {
                    layer.addGeoJson(InsightSchoollocationsjsondata);
                } else if (kml[id].dataType == 2) {
                    layer.addGeoJson(datazonejsondata);
                } else if (kml[id].dataType == 3) {
                    layer.addGeoJson(Neighbourhoodsjsondata);
                }
                else if (kml[id].dataType == 4) {
                    layer.addGeoJson(datazoneglasgowdata);
                }

                layer.setStyle(function (feature) {
                    var fillop = 0.4;
                    if (feature.getProperty('isColorful')) {
                        fillop = feature.getProperty('fillOpacity');
                    }
                    return /** @type {google.maps.Data.StyleOptions} */({
                        fillColor: '#2262cc',
                        fillOpacity: fillop,
                        strokeColor: '#2262cc',
                        strokeWeight: 3
                    });
                });

                layer.addListener('click', function (event) {
                    event.feature.setProperty('isColorful', true);
                    event.feature.setProperty('fillOpacity', '0.75');
                });

                var infoWindows = new google.maps.InfoWindow();
                layer.addListener('mouseover', function (event) {
                    var sGeometryType = event.feature.getGeometry().getType();
                    switch (sGeometryType) {
                        case "Polygon":
                            var point = mapOverlay.getProjection().fromLatLngToContainerPixel(event.latLng);
                            layer.revertStyle();
                            layer.overrideStyle(event.feature, { strokeWeight: 5 });
                            var divContent = document.getElementById('content-windows-mouse-over');
                            divContent.style.display = "block";
                            divContent.style.left = (point.x+10) + "px";
                            divContent.style.top = (point.y+10) + "px";
                            divContent.textContent = event.feature.getProperty('description');
                            break;
                        case "Point":
                            infoWindows.setContent("<div style='width: 150px;'>" + event.feature.getProperty("Name") + "</div>");
                            infoWindows.setPosition(event.feature.getGeometry().get());
                            infoWindows.setOptions({ pixelOffset: new google.maps.Size(0, -30) });
                            infoWindows.open(map);
                            setTimeout(function () { infoWindows.close(); }, 4000);
                            break;
                    }
                });

                layer.addListener('mouseout', function (event) {
                    layer.revertStyle();
                    var divContent = document.getElementById('content-windows-mouse-over');
                    divContent.style.display = "none";
                });
                break;
        }

        kml[id].obj = layer;
        kml[id].obj.setMap(map);

        google.maps.event.addListener(layer, 'click',
        function (kmlEvent) {
            if (kml[id].dataType == 0) {
                SearchData(kmlEvent.featureData.description, "ZoneCode");
            } else if (kml[id].dataType == 1) {
                SearchData(kmlEvent.feature.getProperty('SCHOCODE'), "School");
            } else if (kml[id].dataType == 2) {
                SearchData(kmlEvent.feature.getProperty('ZONECODE'), "ZoneCode");
            } else if (kml[id].dataType == 3) {
                SearchData(kmlEvent.feature.getProperty('name'), "Neighbourhood");
            }
        });

    } else {
        kml[id].obj.setMap(null);
        delete kml[id].obj;
    }
};

// create layer control box on top right of screen
function CreateLayerControl() {
    var i = -1;
    var html = "<form action='' name='formLayer'><ul class='list-unstyled'>";
    for (var prop in kml) {
        i++;
        html += "<li class='text-left' id=\"selector" + i + "\"><input name='box' type='checkbox' id='" + prop + "'" +
        " onclick='ToggleKMLLayer(this.checked, this.id)' \/>&nbsp;" +
        kml[prop].name + "<\/li><hr>";
    }
    html += "<li class='control'><a href='#' onclick='RemoveAllLayers();return false;'>" +
    "Remove all layers<\/a><\/li>" +
    "<\/ul><\/form>";

    document.getElementById("mapcontrolbox").innerHTML = html;
}

// remove all layers
function RemoveAllLayers() {
    for (var prop in kml) {
        if (kml[prop].obj) {
            kml[prop].obj.setMap(null);
            delete kml[prop].obj;
        }
    }

    var boxes = document.getElementsByName("box");
    for (var i = 0, m; m = boxes[i]; i++) {
        m.checked = false;
    }
}