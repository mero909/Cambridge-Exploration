app.factory('cambridgeServices', function ($http, $q) {
    'use strict';

    var _authenticate = function (auth) {
        var deferred = $q.defer();

        $http({
            url: '/Auth/Login',
            method: 'POST',
            data: { email: auth.Email, password: auth.Password }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function () {
            deferred.reject();
        });

        return deferred.promise;
    };

    var _register = function(contact) {
        var deferred = $q.defer();

        $http({
            url: '/Register/Submit',
            method: 'POST',
            data: {
                Name: contact.Name,
                Email: contact.Email,
                Passcode: contact.Passcode,
                Phone: contact.Phone,
                Address: contact.Address,
                Address2: contact.Address2,
                City: contact.City,
                StateID: contact.StateID,
                PostalCode: contact.PostalCode,
                Solution: contact.Solution,
                InvestorType1: contact.InvestorType_1,
                InvestorType2: contact.InvestorType_2,
                InvestorType3: contact.InvestorType_3,
                InvestorType4: contact.InvestorType_4,
                Message: contact.Message
            }
        }).success(function (data) {
            deferred.resolve(data);
        }).error(function () {
            deferred.reject();
        });

        return deferred.promise;
    };

    var _getFullName = function(id) {
        var deferred = $q.defer();

        $http({
            url: '/Auth/GetFullName',
            method: 'GET',
            data: { id: id }
        }).success(function(data) {
            deferred.resolve(data);
        }).error(function() {
            deferred.reject();
        });

        return deferred.promise;
    };

    var _submitContactForm = function(contact) {
        var deferred = $q.defer();

        $http({
            url: '/Contact/Send',
            method: 'POST',
            data: {
                Name: contact.Name,
                Email: contact.Email,
                Phone: contact.Phone,
                TimeToCall: contact.TimeToCall,
                Message: contact.Message
            }
        }).success(function(data) {
            deferred.resolve(data);
        }).error(function(msg) {
            deferred.reject();
            console.log(msg);
        });

        return deferred.promise;
    };

    return {
        authenticate: _authenticate,
        register: _register,
        getFullName: _getFullName,
        submitContactForm: _submitContactForm
    };
});