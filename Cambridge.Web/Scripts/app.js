var app = angular.module('app', ['app', 'ngCookies']);

app.run(function ($rootScope) {
    $rootScope.LoadingMessage = "Loading...";
    $rootScope.IsLoading = false;
    $rootScope.ErrorMessage = "";
    $rootScope.IsError = false;
    $rootScope.IsLoggedIn = false;
});