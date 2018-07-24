angular.module('root.services', [])

.factory('indexService', function ($http, $rootScope) {
    return {
        login: function (eUser) {
            var param = {
                eUser: eUser
            };
            return $http.get(sContextPath + "Authorisation/IndexAuthorisation/ProcessLogin", {
                param: {
                    eUser: eUser
                }
            });
        }
    };
});
