﻿
module.factory('authentication', ['$http', '$q', function ($http, $q) {

    var _servicebase = 'http://localhost:5658/';

    _validateUser = function (credentials) {

        var data = "grant_type=password&username=" + credentials.username + "&password=" + credentials.password;

        var deferred = $q.defer();

        $http.post(_servicebase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
        .then(function (result) {
            //success
            deferred.resolve(result.data);
        },
        function (error) {
            //Error
            deferred.reject(error.data.error_description);
        });

        return deferred.promise;
    };

    return {
        login : _validateUser
    };
}]);