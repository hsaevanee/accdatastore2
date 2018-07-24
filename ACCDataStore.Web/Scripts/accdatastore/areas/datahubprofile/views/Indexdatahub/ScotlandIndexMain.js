$(document).ready(function () {
    console.log('success!');
    initMap();
    loadDataTable();
})

function initMap()
{
    var url = "https:\/\/maps.googleapis.com/maps/api/geocode/json?&address=Scotland";
    var loc;
    var bounds;
    
    $.getJSON(url, function (result) {
        console.log(result);
        loc = result.results[0].geometry.location;
        var sw = result.results[0].geometry.bounds.southwest;
        var ne = result.results[0].geometry.bounds.northeast;
        bounds = new google.maps.LatLngBounds(sw, ne);
    })

    var mapCenter = new google.maps.LatLng(56.49067119999999, -4.2026458);
    var mapOptions = {
        zoom: 7,
        center: mapCenter
    }

    var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    //var cities = ["S12000033", "S12000046"]
    //cities.forEach( function(item) {
    //    $.getJSON(sContextPath + "DatahubProfile/IndexDatahub/GetGeoJSON?id=" + item, function (result) {
    //        console.log(result);
    //        map.data.addGeoJson(result);
    //    })
    //})

    var cities = ["Aberdeen City", "Glasgow City", "Aberdeenshire"];
    councilList.forEach( function(item) {
        $.getJSON("http://maps.googleapis.com/maps/api/geocode/json?&address=" + item.name + ",Scotland,UK")
            .done(function (result) {
                console.log(result);
                /*var markerProps = {
                    position: result.results[0].geometry.location,
                    map: map,
                    title: item
                };
                if (item.name != "Glasgow City") {
                    markerProps.icon = 'http://i.imgur.com/3a4ykxd.png';
                }*/
                var marker = new google.maps.Marker({
                    position: result.results[0].geometry.location,
                    map: map,
                    title: item,
                    icon: "http://maps.google.com/mapfiles/ms/icons/red-dot.png"
                });
                if (cities.indexOf(item.name) == -1) {
                    marker.setIcon('http://i.imgur.com/3a4ykxd.png');
                }
                if (cities.indexOf(item.name) > -1) {
                    marker.addListener('click', function () {
                        selectCouncil(item.name);
                    });
                }
            });
        });

    

    //map.fitBounds(bounds);
}

function loadDataTable() {
    var scotlandTable = $('#scotland-all-councils').DataTable({
        dom: 'Bfrtip',
        "scrollY": "400px",
        "scrollCollapse": true,
        paging: false,
        "order": [],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'All Council Destinations'
                }, 'print',
            ]
        }
    });

    scotlandTable.on('order.dt search.dt', function () {
        scotlandTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}