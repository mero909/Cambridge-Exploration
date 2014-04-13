'use strict';

// Declare app level module which depends on filters, and services
angular.module('app', [
  'ngRoute',
  'app.filters',
  'app.services',
  'app.directives'
]);

angular.run(function ($rootScope) {
    $rootScope.LoadingMessage = "Loading...";
    $rootScope.IsLoading = false;
    $rootScope.ErrorMessage = "";
    $rootScope.IsError = false;
    $rootScope.IsLoggedIn = false;
});

angular.config(['$routeProvider', function($routeProvider) {
    $routeProvider.when(
        '/Register',
        {
            templateUrl: 'partials/register.html',
            controller: 'RegisterController'
        });
    $routeProvider.when(
        '/Auth',
        {
            templateUrl: 'partials/auth.html',
            controller: 'AuthController'
        });
    $routeProvider.when(
        '/Contact',
        {
            templateUrl: 'partials/contact.html',
            controller: 'ContactController'
        });
    $routeProvider.when(
        '/Dashboard',
        {
            templateUrl: 'partials/dashboard.html',
            controller: 'DashboardController'
        });
    $routeProvider.otherwise(
        {
            redirectTo: '/'
        });
}]);



