angular.module('root.controllers', ['ngSanitize', 'ui.select', 'highcharts-ng', 'datatables'])

.controller('rootCtrl', function ($scope, $rootScope) {
})

.controller('indexCtrl', function (Excel, $scope, $rootScope, $state, $stateParams, $timeout, indexService) {
    $scope.mIndex = {};

    indexService.getCondition().then(function (response) {
        $scope.mIndex.bShowContent = false;
        $scope.mIndex = response.data;
    }, function (response) {
    });

    //});


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

    }; //end doGetData function

    $scope.doExportXLSX = function (tablename) {
        var listSeedCode = [];
        var sYear = $scope.mIndex.YearSelected.Year;

        angular.forEach($scope.mIndex.ListSchoolSelected, function (item, key) { // create list of selected school's id to send via http get
            listSeedCode.push(item.SeedCode);
        });

        indexService.exportData(listSeedCode, sYear, tablename).then(function (response) {
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
    };

    $scope.exportToExcel = function (tableId) { // ex: '#my-table'
        $scope.exportHref = Excel.tableToExcel(tableId, 'sheet name');
        $timeout(function () { location.href = $scope.exportHref; }, 100); // trigger download
    }

    $scope.doGetReport = function () { // ex: '#my-table'
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

});

    
