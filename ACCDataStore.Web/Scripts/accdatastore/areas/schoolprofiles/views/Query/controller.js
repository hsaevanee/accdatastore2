angular.module('root.controllers', ['ngSanitize'])

.controller('rootCtrl', function ($scope, $rootScope) {
})

.controller('indexCtrl', function ($scope, $rootScope, $state, $stateParams, $timeout, indexService) {
    $scope.mIndex = {};

    $scope.doGetData = function () {
        indexService.getAll().then(function (response) {
            $scope.mIndex = response.data;
        }, function (response) {
        });
    }


});


