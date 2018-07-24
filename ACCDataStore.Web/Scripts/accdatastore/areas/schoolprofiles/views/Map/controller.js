angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables', 'ngMap', 'rzModule'])

.controller('rootCtrl', function ($scope, $rootScope) {

})

.controller('MapCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, mapService) {
    var map;
    var mapBounds;

    var dataMin = Number.MAX_VALUE, dataMax = -Number.MAX_VALUE;

    var infowindow = new google.maps.InfoWindow();

    $scope.mMap = {
    };

    $scope.mMap.bHeatMapShowContent = true;

    mapService.getCondition().then(function (response) {
        $scope.mMap = response.data;
        $scope.mMap.bShowContent = false;
        $scope.mMap.bHeatMapShowContent = true;
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

    $scope.updateDataset = function () {
        //console.log($scope.dataset_item.code, $scope.dataset_item.name)
        $scope.mMap.selectedLayer = $scope.mMap.selectedLayer;
        $scope.mMap.selectedDataset = $scope.mMap.selectedDataset;
        $scope.mMap.bShowContent = false;
        google.maps.event.addDomListener(window, 'load', initMap);
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
        minValue: 5,
        maxValue: 10,
        options: {
            floor: 0,
            ceil: 65,
            vertical: false,
            showOuterSelectionBars: false,
            onEnd: function() {
                console.info( $scope.slider.minValue + ' ' + $scope.slider.maxValue)
                map.data.setStyle(styleFeature);
            }
        }
    };

    var styleFeature = function (feature) {

        var data = $scope.mMap.heatmapdata;

        var datacatagory = $scope.mMap.selecteddatacatagory;

        var obj;

        for (y=0; y< data.length; ++y) {
            if( data[y].seedcode == feature.getProperty('DATAZONE') ){
                obj = data[y];
                break;
            }
        }

        //data.forEach( function( item ){


        //    if( item.seedcode.localeCompare(feature.getProperty('description') )){
        //        obj = item;
        //    }
        //} );



        //let obj = $filter('filter')(data, {seedcode: feature.getProperty('REFNO') })[0];
            
            
        //    data.filter(function(item) {
        //    return item.seedcode === feature.getProperty('REFNO');
        //})[0];
        var delta = 0;

        if(!obj){
            delta = 0;
        }else{
            var temp = obj.listdata.find(x => x.Code === datacatagory.Code);
            delta = temp.count;
        }

        if ( delta< $scope.slider.minValue) {

            color = '#1fc600' ;//green
        } else if (delta > $scope.slider.maxValue) {

            color = '#ff0000';//red
        } else {
            color = '#fcf823';
        }

        var outlineWeight = 0.5, zIndex = 1, opacity = 0.5;

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

        map.data.addGeoJson(Datazonejsondata);

        map.data.setStyle(styleFeature);

        // When the user clicks, set 'isColorful', changing the color of the letters.
        map.data.addListener('click', function (event) {
            event.feature.setProperty('isColorful', true);

            mapService.getData($scope.mMap.selectedLayer.Code, $scope.mMap.selecteddatacatagory.Code, event.feature.getProperty('DATAZONE'), $scope.mMap.selectedDataset.Code).then(function (response) {
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
            infowindow.setContent(event.feature.getProperty('NAME'));
            infowindow.setPosition(event.latLng);
            //infowindow.setOptions({ pixelOffset: new google.maps.Size(0, -34) });
            infowindow.open(map);
        });

        map.data.addListener('mouseout', function (event) {
            map.data.revertStyle();
        });

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


});


 