//DataHub
angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getCondition: function () {
            return $http.get(sContextPath + "CSSF/IndexCSSF/GetCondition");
        },
        getData: function (sClientID, sCategoryID, sServiceType) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "CSSF/IndexCSSF/GetData", { params: { "sClientID": sClientID, "sCategoryID": sCategoryID, "sServiceType": sServiceType } });
        }
  
    };
});
