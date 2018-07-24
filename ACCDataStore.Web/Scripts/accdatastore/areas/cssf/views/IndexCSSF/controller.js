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


    $scope.updateData = function () {
        $scope.mIndex.bShowContent = false;
    }

    $scope.doGetData = function () {
        var sClientID = $scope.mIndex.ClientSelected.Client_Id;
        var sCategoryID = $scope.mIndex.ListCategorySelected.Code;
        var sServiceType = $scope.mIndex.ListServiceTypeSelected.Code;

        indexService.getData(sClientID, sCategoryID, sServiceType).then(function (response) {
            $rootScope.bShowLoading = false;
            $scope.mIndex = response.data;
            $scope.mIndex.bShowContent = true;
        }, function (response) {
            $rootScope.bShowLoading = false;
        });

    }; //end doGetData function

})

 
    