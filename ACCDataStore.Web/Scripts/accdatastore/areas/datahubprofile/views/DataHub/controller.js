angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables'])

.controller('rootCtrl', function ($scope, $rootScope) {
})

.controller('indexCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, indexService) {
    $scope.mIndex = {};

    indexService.getCondition().then(function (response) {
        $scope.mIndex.bShowContent = false;
        $scope.mIndex = response.data;
    }, function (response) {
    });

    $scope.doGetPupilList = function (seedcode, centretype, dataname) {
        var sYear = $scope.mIndex.DatasetSelected.Code
        $state.go('list', { seedcode: seedcode, centretype: centretype, dataname: dataname, sYear: sYear });

    }

    $scope.doGetDatabySchool = function () {
        var listSeedCode = [];
        var ListNeighbourhoodCode = [];
        var sYear = $scope.mIndex.DatasetSelected.Code;
        var sReport = $scope.mIndex.ReportsetSelected.Code;

        angular.forEach($scope.mIndex.ListSchoolSelected, function (item, key) { // create list of selected school's id to send via http get
            listSeedCode.push(item.SeedCode);
        });

        indexService.getData(listSeedCode, ListNeighbourhoodCode, sYear, sReport).then(function (response) {
            $rootScope.bShowLoading = false;
            $scope.mIndex = response.data;
            $scope.mIndex.bShowContent = true;
        }, function (response) {
            $rootScope.bShowLoading = false;
        });
    }

    $scope.doGetDatabyNeighbourhood = function () {
        var listSeedCode = [];
        var ListNeighbourhoodCode = [];
        var sYear = $scope.mIndex.DatasetSelected.Code;
        var sReport = $scope.mIndex.ReportsetSelected.Code;

        angular.forEach($scope.mIndex.ListNeighbourSelected, function (item, key) { // create list of selected school's id to send via http get
            ListNeighbourhoodCode.push(item.SeedCode);
        });

        indexService.getData(listSeedCode, ListNeighbourhoodCode, sYear, sReport).then(function (response) {
            $rootScope.bShowLoading = false;
            $scope.mIndex = response.data;
            $scope.mIndex.bShowContent = true;
        }, function (response) {
            $rootScope.bShowLoading = false;
        });
    }


    $scope.updateData = function () {
        $scope.mIndex.bShowContent = false;
    }

    $scope.GetPositiveChart = function () {
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[0].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[1].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[2].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[3].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[4].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[5].visible = false;
    };
    $scope.GetNonPositiveChart = function () {
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[0].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[1].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[2].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[3].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[4].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[5].visible = false;
    };
    $scope.GetUnknownChart = function () {
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[0].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[1].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[2].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[3].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[4].visible = false;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[5].visible = true;
    };
    $scope.GetTimelineChart = function () {
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[0].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[1].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[2].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[3].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[4].visible = true;
        $scope.mIndex.ChartData.ChartTimelineDestinations.series[5].visible = true;
    };
})

.controller('listCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, indexService) {
    $scope.mIndex = {};

    indexService.getListPupils($stateParams.seedcode, $stateParams.centretype, $stateParams.dataname, $stateParams.sYear).then(function (response) {
        $scope.mIndex.bShowContent = true;
        $scope.mIndex = response.data;
    }, function (response) {
        window.location.href = sContextPath+"Authorisation/IndexAuthorisation";
    });

});
    