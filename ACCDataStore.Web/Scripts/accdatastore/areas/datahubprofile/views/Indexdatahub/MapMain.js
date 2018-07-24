//var map;
var data1;
var datag;

var schools = new google.maps.Data();
var intermediateZones = new google.maps.Data();
var dataZones = new google.maps.Data();
var currentSelection;

var map;

function asdf(map) {
    $(document.body).on('click', '.a-close-popup-information', function () {
        $("#popup-information").hide(250);
    })
    InitSpinner();
    $("#selecteddataset").change(function () {
        var datasetname = $('#selecteddataset :selected').text();
        loadData(datasetname, currentSelection, map);
    })

    $("#thresholdViewBtn").click(function () {
        var low = parseInt($('#thresholdViewLow').val())
        var high = parseInt($('#thresholdViewHigh').val())
        threshholdView(datag, low, high, map);
    })

    $("#thresholdViewAvg").click(function () {
        $('#thresholdViewCenter').val(getAverage(datag.data));
    })
}
$(document).ready(function () {
    InitMap(map)
    $("#selectedtype").change(function () { $(thresholdControl).show() })

});


function InitMap(map) {
    var url = "https:\/\/maps.googleapis.com/maps/api/geocode/json?&address=" + currentCouncil.name + ",Scotland,UK";
    var mapCenter;

    var jqxhr = $.getJSON(url, function (result) {
        
        mapCenter = result.results[0].geometry.location;
        var sw = result.results[0].geometry.bounds.southwest;
        ne = result.results[0].geometry.bounds.northeast;
        mapBounds = new google.maps.LatLngBounds(sw, ne);
         
        //console.log(result);
    })
    
    jqxhr.complete(function (data) {
        console.log("complete")
        console.log(mapCenter);
        map = new google.maps.Map(document.getElementById('map-canvas'), {
            center: mapCenter
        });
        map.fitBounds(mapBounds);
        //loadDataZones(map)
        $("#selectedtype").change(function () {
            
            var type = $('#selectedtype :selected').text();
            if (type == "Intermediate Zones") {
                currentSelection = "Intermediate Zone";
                loadData("Participating", currentSelection, map)
                

            } else if (type == "Data Zones") {
                currentSelection = "Data Zone";
                loadData("Participating", currentSelection, map)
            }
        })
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
        "type": sKeyname,
        "code": sCondition
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/MapDataForSelection",
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

function onSlide(ui) {
    var totalWidth = $(".slider").width();
    $(".left-color").width((ui.values[0]) / 100 * totalWidth);
}

//Threshold view slider
function createSlider(centerValue) {
    $(".slider").slider({
        min: 0,
        max: 100,
        //step: 1,

        range: true,
        values: [centerValue, centerValue],
        slide: function (event, ui) {
            
            if (ui.values[1] < centerValue || ui.values[0] > centerValue) { return false };
            onSlide(ui)
            $("input.sliderValue[data-index=" + 0 + "]").val(centerValue - ui.values[0]);
            $("input.sliderValue[data-index=" + 1 + "]").val(-(centerValue - ui.values[1]));
        },
        create: function (event, ui) {
            onSlide({
                values: [centerValue, centerValue]
            });
        }
    })

    $("input.sliderValue").change(function () {
        var $this = $(this);
        if ($this.attr('data-index') == 0) {
            if ($this.val() < (0)) { $this.val(0) };
            if ($this.val() > centerValue) { $this.val(centerValue) };
            $(".slider").slider("values", $this.data("index"), (centerValue - parseInt($this.val())));
        }
        else {
            if ($this.val() < (0)) { $this.val(0) };
            if ($this.val() > (100 - centerValue)) { $this.val(100 - centerValue) };
            $(".slider").slider("values", $this.data("index"), (parseInt($this.val()) + centerValue));
        };
    });

    //var averge = $('<label>|</label>').css('left', Math.round(getAverage(datag.data)) + '%');
    //$('.slider').append(averge);
};


function threshholdView(data, type, low, high, map) {
    console.log("now");
    console.log(map);
    map.data.setStyle(function (feature) {
        var statisticdata = -1;

        if (type == "Data Zone") var temp = feature.getProperty('DZ_CODE');
        else if (type == "Intermediate Zone") var temp = feature.getProperty('IZ_CODE');

        for (var i = 0; i < data.datacode.length; i++) {
            if (data.datacode[i] == temp) {
                statisticdata = data.data[i];
                break;
            }

        }

        //var center = getAverage(datag.data);
        var color = 'yellow';
        var center = getAverage(datag.data);
        if (statisticdata < (center - low)) { color = 'red'; }
        if (statisticdata > (center + high)) { color = 'green'; }

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
            fillColor: color,
            fillOpacity: 0.70,
            visible: showRow
        };
    });
}

function getAverage(data) {
    var sum = 0;
    for (var i = 0; i < data.length; i++) {
        sum += parseInt(data[i], 10); //don't forget to add the base
    }
    var avg = sum / data.length;
    return avg;
}

function loadData(datasetname, type, map) {
    map.data.forEach(function (feature) {
        map.data.remove(feature);
    })
    console.log("loading data");
    console.log(map);
    var JSONObject = {
        "datasetname": datasetname,
        "type": type,
        "name": currentCouncil.name
    }
    
    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/GetHeatmapData",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            map.data.setStyle(function (feature) {

                var outlineWeight = 0.5, zIndex = 1;
                if (feature.getProperty('datazone') === 'hover') {
                    outlineWeight = zIndex = 2;
                }

                return {
                    strokeWeight: outlineWeight,
                    strokeColor: '#fff',
                    zIndex: zIndex,
                    fillOpacity: 0.70
                };
            });
            if (type == "Intermediate Zone")
            {
                
                currentCouncil.intermediateZones.forEach(function (item) {
                    $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetGeoJSON?id=" + item.seedcode).done(function (data) {
                        //console.log(data);
                        console.log(item.seedcode);
                        map.data.addGeoJson(data);

                    })
                })
                datag = data;
                createSlider(Math.round(getAverage(datag.data)));
                $('#averageText').html(' ' + getAverage(datag.data).toFixed(2) + '%');
                // update and display the legend
                document.getElementById('census-min').textContent =
                data.minimum.toFixed(1) + "%";
                document.getElementById('census-max').textContent =
                    data.maximum.toFixed(1) + "%";

                google.maps.event.clearListeners(map.data, 'click');
                // set up the style rules and events for google.maps.Data
                map.data.addListener('mouseover', function (event) {
                    // set the hover state so the setStyle function can change the border
                    event.feature.setProperty('datazone', 'hover');
                    var temp = event.feature.getProperty('IZ_CODE');
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
                        event.feature.getProperty('IZ_CODE');
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
                    SearchData(kmlEvent.feature.getProperty('IZ_CODE'), "intermediate zone");
                });
                $(document.body).on('click', '.a-close-popup-information', function () {
                    $("#popup-information").hide(250);
                })
                InitSpinner();
                $("#selecteddataset").unbind()
                $("#selecteddataset").change(function () {
                    var datasetname = $('#selecteddataset :selected').text();
                    map.data.forEach(function (feature) {
                        map.data.remove(feature);
                    })
                    loadData(datasetname, type, map);
                })

                $("#thresholdViewBtn").click(function () {
                    var low = parseInt($('#thresholdViewLow').val())
                    var high = parseInt($('#thresholdViewHigh').val())
                    threshholdView(datag, type, low, high, map);
                })

                $("#thresholdViewAvg").click(function () {
                    $('#thresholdViewCenter').val(getAverage(datag.data));
                })
            }
            else if (type=="Data Zone")
            {
                
                currentCouncil.dataZones.forEach(function (item) {
                    $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetGeoJSON?id=" + item.seedcode).done(function (data) {
                        //console.log(data);
                        //console.log(data);
                        map.data.addGeoJson(data);

                    })
                })
                datag = data;
                createSlider(Math.round(getAverage(datag.data)));
                $('#averageText').html(getAverage(datag.data).toFixed(2));
                // update and display the legend
                document.getElementById('census-min').textContent =
                data.minimum.toFixed(1) + "%";
                document.getElementById('census-max').textContent =
                    data.maximum.toFixed(1) + "%";

                google.maps.event.clearListeners(map.data, 'click');
                // set up the style rules and events for google.maps.Data
                map.data.addListener('mouseover', function (event) {
                    // set the hover state so the setStyle function can change the border
                    event.feature.setProperty('datazone', 'hover');
                    var temp = event.feature.getProperty('DZ_CODE');
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
                        event.feature.getProperty('DZ_CODE');
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
                    SearchData(kmlEvent.feature.getProperty('DZ_CODE'), "data zone");
                });
                $(document.body).on('click', '.a-close-popup-information', function () {
                    $("#popup-information").hide(250);
                })
                InitSpinner();
                $("#selecteddataset").unbind()
                $("#selecteddataset").change(function () {
                    var datasetname = $('#selecteddataset :selected').text();
                    loadData(datasetname, type, map);
                })

                $("#thresholdViewBtn").click(function () {
                    var low = parseInt($('#thresholdViewLow').val())
                    var high = parseInt($('#thresholdViewHigh').val())
                    threshholdView(datag, type, low, high, map);
                })

                $("#thresholdViewAvg").click(function () {
                    $('#thresholdViewCenter').val(getAverage(datag.data));
                })
            }
            
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
    sInformation += "<thead><tr><th> </th><th class='text-center'>" + data.schoolname + "</th><th class='text-center'>" + currentCouncil.name +"</th></tr></thead>";
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

