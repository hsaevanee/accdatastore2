//DataHub
angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getCondition: function () {
            return $http.get(sContextPath + "DatahubProfile/DataHub/GetCondition");
        },
        getData: function (listSeedCode, ListNeighbourhoodCode, sYear, sReport) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "DatahubProfile/DataHub/GetData", { params: { "listSeedCode": listSeedCode, "ListNeighbourhoodCode": ListNeighbourhoodCode, "sYear": sYear, "sReport": sReport } });
        },
        getListPupils: function (seedcode, centretype, dataname, sYear) {
            return $http.get(sContextPath + "DatahubProfile/Datahub/GetListPupils", {
                headers: { 'X-Requested-With': 'XMLHttpRequest'},
                params: { "seedcode": seedcode, "centretype":centretype, "dataname": dataname, "sYear": sYear } });
        }
    };
});
