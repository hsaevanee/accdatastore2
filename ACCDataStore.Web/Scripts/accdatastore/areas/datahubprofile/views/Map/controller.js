angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables', 'ngMap', 'rzModule'])

.controller('rootCtrl', function ($scope, $rootScope) {

})

.controller('MapCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, mapService) {
    var map;
    var mapBounds;

    var dataMin = Number.MAX_VALUE, dataMax = -Number.MAX_VALUE;

    var infowindow = new google.maps.InfoWindow();

    var heatmapdata = [];

    $scope.mMap = {
    };

    $scope.mMap.bHeatMapShowContent = true;

    mapService.getCondition().then(function (response) {
        $scope.mMap = response.data;
        $scope.mMap.bShowContent = false;
        $scope.mMap.bHeatMapShowContent = true;
        heatmapdata = $scope.mMap.heatmapdata ;
    }, function (response) {
        $scope.mMap.bShowContent = false;
        $scope.mMap.bHeatMapShowContent = true;
    });

    google.maps.event.addDomListener(window, 'load', initMap);

    function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: new google.maps.LatLng(57.151810, -2.094451),
            mapTypeId: google.maps.MapTypeId.TERRAIN
        });

        createLayer($scope.mMap.selectedLayer);
    }

    $scope.updateDataset = function (selectedDataset) {
        //console.log($scope.dataset_item.code, $scope.dataset_item.name)
        //alert(selectedDataset.Code);
        $scope.mMap.selectedLayer = $scope.mMap.selectedLayer;
        $scope.mMap.selectedDataset = $scope.mMap.selectedDataset;
        $scope.mMap.bShowContent = false;

        var myDataPromise = mapService.LoadHeatMapdata($scope.mMap.selectedDataset.Code);
        myDataPromise.then(function(result) {  

            // this is only run after getData() resolves
            $scope.mMap.heatmapdata = result;
            //console.log("data.name"+ $scope.mMap.heatmapdata.length);
        });


        heatmapdata = $scope.mMap.heatmapdata ;

        createLayer($scope.mMap.selectedLayer);
    }

    $scope.updateCatagory = function () {
        //console.log($scope.dataset_item.code, $scope.dataset_item.name)
        $scope.mMap.selectedLayer = $scope.mMap.selectedLayer;
        $scope.mMap.selectedDataset = $scope.mMap.selectedDataset;
        $scope.mMap.bShowContent = false;
        //update data for highlight 
        map.data.setStyle(styleFeature);
    }

    $scope.updateLayer = function () {

        $scope.mMap.selectedLayer = $scope.mMap.selectedLayer;
        $scope.mMap.selectedDataset = $scope.mMap.selectedDataset;
        $scope.mMap.selectedDestination = $scope.mMap.selectedDestination;
        $scope.mMap.bShowContent = false;
        //console.log($scope.layer_item.code, $scope.layer_item.name)
        if ($scope.mMap.selectedLayer.Code == 'S01') {
            $scope.mMap.bHeatMapShowContent = true;
            createLayer($scope.mMap.selectedLayer);
        } else {
            $scope.mMap.bHeatMapShowContent = false;
            createMarker($scope.mMap.selectedLayer);
        } 

    }

    $scope.otherData = {
        start: 0,
        change: 0,
        end: 0
    }

    $scope.slider = {
        minValue: 80,
        maxValue: 95,
        options: {
            floor: 0,
            ceil: 100,
            vertical: false,
            showOuterSelectionBars: false,
            onEnd: function() {
                //console.info( $scope.slider.minValue + ' ' + $scope.slider.maxValue)
                map.data.setStyle(styleFeature);
            }
        }
    };

    var styleFeature = function (feature) {

        //var heatmapdata = $scope.mMap.heatmapdata;

        var datacatagory = $scope.mMap.selecteddatacatagory;

        var datavariable;

        for (var i = 0; i < heatmapdata.length; i++) {
            if (heatmapdata[i].seedcode === feature.getProperty('REFNO')){
                datavariable = heatmapdata[i].listdata.find(x => x.Code === datacatagory.Code).Percent;
                break;
            }
        }


        if(datavariable < $scope.slider.minValue){
            if(datacatagory.Code === 'Participating Destination'){
                color = '#ff0000'//red
            }else{
                color = '#1fc600'//green    
            }
        
        }else if(datavariable > $scope.slider.maxValue){
            if(datacatagory.Code === 'Participating Destination'){
                color = '#1fc600'//green
            }else{
                color = '#ff0000'//red    
            }
        }else{
            color = '#fcf823'
        }

        var outlineWeight = 0.5, zIndex = 1, opacity = 0.5;

            if (feature.getProperty('isColorful')) {
                color = shadeColor(color,-20);
            }

        return {
            zIndex: zIndex,
            fillColor: color,
            strokeColor: color,
            strokeWeight: 1,
            opacity: opacity

        };
    }

    var createLayer = function (layertype) {
        //get json data from database via controller 
        //render on google map

        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: new google.maps.LatLng(57.151810, -2.094451)
        });

        map.data.addGeoJson(Neighbourhoodsjsondata);

        map.data.setStyle(styleFeature);

        // When the user clicks, set 'isColorful', changing the color of the letters.
        map.data.addListener('click', function (event) {
            event.feature.setProperty('isColorful', true);

            mapService.getData($scope.mMap.selectedLayer.Code, $scope.mMap.selecteddatacatagory.Code, event.feature.getProperty('REFNO'), $scope.mMap.selectedDataset.Code).then(function (response) {
                $rootScope.bShowLoading = false;
                $scope.mMap = response.data;
                $scope.mMap.bShowContent = true;
                $scope.mMap.bHeatMapShowContent = true;
            }, function (response) {
                $rootScope.bShowLoading = false;
            });
        });

        // When the user hovers, tempt them to click by outlining the letters.
        // Call revertStyle() to remove all overrides. This will use the style rules
        // defined in the function passed to setStyle()
        map.data.addListener('mouseover', function (event) {
            map.data.revertStyle();
            map.data.overrideStyle(event.feature, { strokeWeight: 3.5, zIndex: 3});
            infowindow.setContent(event.feature.getProperty('name'));
            infowindow.setPosition(event.latLng);
            //infowindow.setOptions({ pixelOffset: new google.maps.Size(0, -34) });
            infowindow.open(map);
        });

        map.data.addListener('mouseout', function (event) {
            map.data.revertStyle();
        });

    }

    var createMarker = function (layertype) {
        //get json data from database via controller 
        //render on google map

        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 11,
            center: new google.maps.LatLng(57.151810, -2.094451)
        });

        for (var i = 0; i < InsightSchoollocationsjsondata.features.length; i++) {
            var coords = InsightSchoollocationsjsondata.features[i].geometry.coordinates;
            var title = InsightSchoollocationsjsondata.features[i].properties.name;
            var latLng = new google.maps.LatLng(coords[1], coords[0]);
            var marker = new google.maps.Marker({
                position: latLng,
                map: map,
                title: title
            });

            (function (i, marker) {
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.setContent('<h2>' + marker.title + '</h2>');
                    infowindow.open($scope.map, marker);

                    var pos = map.getZoom();
                    map.setZoom(17);
                    map.setCenter(marker.getPosition());
                    window.setTimeout(function () { map.setZoom(pos); }, 3000);

                    var seedcode = InsightSchoollocationsjsondata.features[i].properties.SCHOCODE;

                    mapService.getData($scope.mMap.selectedLayer.Code, $scope.mMap.selecteddatacatagory.Code, seedcode, $scope.mMap.selectedDataset.Code).then(function (response) {
                        $rootScope.bShowLoading = false;
                        $scope.mMap = response.data;
                        $scope.mMap.bShowContent = true;
                    }, function (response) {
                        $rootScope.bShowLoading = false;
                    });

                });

                google.maps.event.addListener(marker, 'mouseover', function () {

                    infowindow.setContent(marker.title);
                    infowindow.open($scope.map, marker);
                });
            })(i, marker);
        }

    }

    function shadeColor(color, percent) {

        var R = parseInt(color.substring(1,3),16);
        var G = parseInt(color.substring(3,5),16);
        var B = parseInt(color.substring(5,7),16);

        R = parseInt(R * (100 + percent) / 100);
        G = parseInt(G * (100 + percent) / 100);
        B = parseInt(B * (100 + percent) / 100);

        R = (R<255)?R:255;  
        G = (G<255)?G:255;  
        B = (B<255)?B:255;  

        var RR = ((R.toString(16).length==1)?"0"+R.toString(16):R.toString(16));
        var GG = ((G.toString(16).length==1)?"0"+G.toString(16):G.toString(16));
        var BB = ((B.toString(16).length==1)?"0"+B.toString(16):B.toString(16));

        return "#"+RR+GG+BB;
    }

    $scope.GetPositiveChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = false;
    };
    $scope.GetNonPositiveChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = false;
    };
    $scope.GetUnknownChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = false;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = true;
    };
    $scope.GetTimelineChart = function () {
        $scope.mMap.ChartData.ChartTimelineDestinations.series[0].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[1].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[2].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[3].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[4].visible = true;
        $scope.mMap.ChartData.ChartTimelineDestinations.series[5].visible = true;
    };
});


 