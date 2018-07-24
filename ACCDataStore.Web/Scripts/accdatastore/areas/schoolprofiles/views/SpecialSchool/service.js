﻿angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getCondition: function () {
            return $http.get(sContextPath + "SchoolProfiles/SpecialSchoolProfile/GetCondition");
        },
        getData: function (listSeedCode, sYear) {
            $rootScope.bShowLoading = true;
            return $http.get(sContextPath + "SchoolProfiles/SpecialSchoolProfile/GetData", { params: { "listSeedCode": listSeedCode, "sYear": sYear } });
        },
        //getAvailableListingReports:function(){
        //    $rootScope.bShowLoading = true;
        //    return $http.get(sContextPath + "SchoolProfiles/PrimarySchoolProfile/GetCondition");
        //}
    };
});
