angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables'])

.controller('rootCtrl', function ($scope, $rootScope) {
})

.controller('indexCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout,indexService) {
    $scope.mIndex = {};

    indexService.getCondition().then(function (response) {
        $scope.mIndex.bShowContent = false;
        $scope.mIndex = response.data;
    }, function (response) {
    });
    
    $scope.doGetReport = function (listSeedCode, sYear) { // ex: '#my-table'
        var listSeedCode = [];
        var sYear = $scope.mIndex.YearSelected.Year;

        angular.forEach($scope.mIndex.ListSchoolSelected, function (item, key) { // create list of selected school's id to send via http get
            listSeedCode.push(item.SeedCode);
        });

        indexService.getReport(listSeedCode, sYear).then(function (response) {
            $rootScope.bShowLoading = false;
            $scope.exportInfo = response.data;
            var anchor = angular.element('<a/>');
            anchor.css({ display: 'none' });
            angular.element(document.body).append(anchor);

            anchor.attr({
                href: sContextPath + $scope.exportInfo.DownloadUrl,
                target: '_blank',
                download: $scope.exportInfo.FileName
            })[0].click();

            anchor.remove();
        }, function (response) {
            $rootScope.bShowLoading = false;
        });
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

    $scope.doGetData = function () {
        var listSeedCode = [];
        var sYear = $scope.mIndex.YearSelected.Year;

        angular.forEach($scope.mIndex.ListSchoolSelected, function (item, key) { // create list of selected school's id to send via http get
            listSeedCode.push(item.SeedCode);
        });

        indexService.getData(listSeedCode, sYear).then(function (response) {
            $rootScope.bShowLoading = false;
            $timeout(function () {
                $scope.$broadcast('highchartsng.reflow');

            }, 10);

            $scope.mIndex = response.data;
            $scope.mIndex.bShowContent = true;
        }, function (response) {
            $rootScope.bShowLoading = false;
        });

    }
});

    