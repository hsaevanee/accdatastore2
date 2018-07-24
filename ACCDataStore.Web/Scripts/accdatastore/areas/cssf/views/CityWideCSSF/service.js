//DataHub
angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getCondition: function () {
            return $http.get(sContextPath + "CSSF/CityWideCSSF/GetCondition");
        },
        getData: function (sReportID) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "CSSF/CityWideCSSF/GetData", { params: { "sReportID": sReportID } });
        }
  
    };
});
