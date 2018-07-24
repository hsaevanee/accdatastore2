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

    //$scope.doTabClick = function () {
    //    $timeout(function () {
    //        $scope.$broadcast('highchartsng.reflow');
    //    }, 10);
    //}

    $scope.doGetData = function () {
        var listSeedCode = [];
        var sYear = $scope.mIndex.YearSelected.Year;

        angular.forEach($scope.mIndex.ListSchoolSelected, function (item, key) { // create list of selected school's id to send via http get
            listSeedCode.push(item.SeedCode);
        });

        indexService.getData(listSeedCode, sYear).then(function (response) {
            $rootScope.bShowLoading = false;
            $scope.mIndex = response.data;
            $scope.mIndex.bShowContent = true;
        }, function (response) {
            $rootScope.bShowLoading = false;
        });

    }
});

    