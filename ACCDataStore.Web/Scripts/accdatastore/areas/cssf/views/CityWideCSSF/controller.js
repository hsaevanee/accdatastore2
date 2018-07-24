angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables'])

.controller('rootCtrl', function ($scope, $rootScope) {
})

.controller('indexCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, indexService, DTOptionsBuilder) {

    // DataTables configurable options
    $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDOM('lrtip')
        .withOption('bLengthChange', true)
        .withOption('responsive', true)
        .withOption('bFilter', false)
        .withOption('order', [[0, 'asc']])
        .withOption('lengthMenu', [[25, 50, 100, -1], [25, 50, 100, "All"]]);
    $scope.mIndex = {};

    indexService.getCondition().then(function (response) {
        $scope.mIndex.bShowContent = false;
        $scope.mIndex = response.data;
    }, function (response) {
    });


    $scope.updateData = function () {
        $scope.mIndex.bShowContent = false;
    }

    $scope.doGetData = function () {
        var sReportID = $scope.mIndex.ListReportSelected.Code;

        indexService.getData(sReportID).then(function (response) {
            $rootScope.bShowLoading = false;
            $scope.mIndex = response.data;
            $scope.mIndex.bShowContent = true;
        }, function (response) {
            $rootScope.bShowLoading = false;
        });

    }; //end doGetData function

})

 
    