angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getCondition: function () {
            return $http.get(sContextPath + "SchoolProfiles/SecondarySchoolProfile/GetCondition");
        },
        getData: function (listSeedCode, sYear) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "SchoolProfiles/SecondarySchoolProfile/GetData", { params: { "listSeedCode": listSeedCode, "sYear": sYear } });
        },
        getReport: function (listSeedCode, sYear, tablename) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "SchoolProfiles/SecondarySchoolProfile/GetReport/", { params: { "listSeedCode": listSeedCode, "sYear": sYear } });
        }
    };
});
