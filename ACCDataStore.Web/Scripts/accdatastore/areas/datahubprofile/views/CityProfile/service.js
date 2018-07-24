//Map Controller
angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getGeoJSON: function (layertype) {
            return $http.get(sContextPath + "DatahubProfile/Map/GetGeoJson", { params: { "layertype": layertype } });
        },
        getData: function (sYear, centretype) {
            return $http.get(sContextPath + "DatahubProfile/CityProfile/GetData", { params: { "sYear": sYear, "centretype":centretype } });
        },
        getCondition: function () {
            return $http.get(sContextPath + "DatahubProfile/CityProfile/GetCondition");
        },
        getDataDetails: function (seedcode, centretype, dataname, sYear) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "DatahubProfile/CityProfile/GetDataDetails", {
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                params: { "seedcode": seedcode, "centretype": centretype, "dataname": dataname, "sYear": sYear }
            });
        },
        getListPupils: function (seedcode, centretype, dataname, sYear) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "DatahubProfile/Datahub/GetListPupils", {
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                params: { "seedcode": seedcode, "centretype": centretype, "dataname": dataname, "sYear": sYear }
            });
        }
    };
});
