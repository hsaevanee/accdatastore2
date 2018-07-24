angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        getAll: function () {
            return $http.get(sContextPath + "SchoolProfiles/Query/GetAll");
        }
    };
})
