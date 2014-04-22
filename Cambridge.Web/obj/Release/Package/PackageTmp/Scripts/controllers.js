app.controller("HomeController", function ($scope, $rootScope) {
    $scope.SubscribeEmail = "";

    $scope.Subscribe = function() {

    };

    $scope.GoToRegister = function() {
        location.href = '/Register';
    };

    $scope.modalShown = false;
    $scope.toggleModal = function () {
        $scope.modalShown = !$scope.modalShown;
    };
});

app.controller("ContactController", function ($scope, $rootScope, cambridgeServices) {
    $scope.Contact = {};
    $scope.IsSent = false;

    $scope.Send = function () {
        $rootScope.IsLoading = true;

        if ($scope.contactForm.$valid) {
            cambridgeServices.submitContactForm($scope.Contact)
                .then(function(data) {
                    if (data === '0') {
                        $rootScope.IsLoading = false;
                        $rootScope.ErrorMessage = "Contact Form Submission failed.";
                        $rootScope.IsError = true;
                    } else {
                        $rootScope.IsLoading = false;
                        $scope.IsSent = true;
                    }
                }, function(msg) {
                    $rootScope.IsLoading = false;
                    $rootScope.ErrorMessage = "Contact Form Submission failed.  Error: " + msg;
                    $rootScope.IsError = true;
                });
        }
    };
});

app.controller("AuthController", function ($scope, $rootScope, $cookieStore, cambridgeServices) {
    $scope.Auth = {};
    $scope.IsReset = false;
    $scope.IsPasswordReset = false;
    $scope.FullName = "";
    $scope.IsLoggedIn = false;

    $scope.modalShown = false;
    $scope.toggleModal = function () {
        $scope.modalShown = !$scope.modalShown;
    };

    $scope.getFullName = function() {
        if ($cookieStore.get("id") !== "0" || $cookieStore.get("id") !== undefined) {

            console.log($cookieStore.get("id"));

            cambridgeServices.getFullName($cookieStore.get("id"))
                .then(function(data) {
                    if (data !== '') {
                        $scope.FullName = "Welcome, " + data;
                        $scope.IsLoggedIn = true;
                    }
                }, function(msg) {
                    $scope.FullName = "Login";
                    $scope.IsLoggedIn = false;
                    console.log(msg);
            });
        }
    };

    $scope.Authenticate = function() {
        $rootScope.IsLoading = true;
        
        if ($scope.loginform.$valid) {
            cambridgeServices.authenticate($scope.Auth)
                .then(function(data)
                {
                    if (data === "0") {
                        $rootScope.IsLoading = false;
                        $rootScope.ErrorMessage = "Authentication failed.";
                        $rootScope.IsError = true;
                    } else {
                        $cookieStore.put("id", data);
                        $rootScope.IsLoading = true;
                        $rootScope.IsLoggedIn = true;
                        $rootScope.LoadingMessage = "Logging In...";

                        location.href = '/Dashboard/?contactId=' + data;
                    }
                }, function (msg) { // error
                    $rootScope.IsLoading = false;
                    $rootScope.ErrorMessage = "Authentication failed.";
                    console.log(msg);
                    $rootScope.IsError = true;
                });
        }
    };
});

app.controller("RegisterController", function ($scope, $rootScope, cambridgeServices) {
    $scope.Contact = {};
    $scope.IsProcessed = false;

    $rootScope.LoadingMessage = "Validating...";

    $scope.Register = function() {
        $rootScope.LoadingMessage = "Registering...";
        $rootScope.IsLoading = true;

        if ($scope.registerForm.$valid) {
            cambridgeServices.register($scope.Contact)
                .then(function (data) {
                    $rootScope.IsLoading = false;

                    if (data === "0") {
                        alert("Your email already exists.");
                    } else {
                        location.href = '/Dashboard/?contactId=' + data;
                    }
                }, function (msg) { // error
                    alert("There was an error during registration.  Please try again later.  Error: " + msg);
                    console.log(msg);
                });
        }
    };
});

app.controller("PasswordResetController", function($scope, $http, $rootScope) {
    $rootScope.LoadingMessage = "Resetting Password...";
    $rootScope.IsError = false;

    $scope.IsReset = false;
    $scope.NewPassword = "";
    $scope.ConfirmPassword = "";

    $scope.ResetPassword = function() {
        $rootScope.IsLoading = true;
    };

    $scope.ValidatePass = function(p1, p2) {
        if (p1.value != p2.value || p1.value == '' || p2.value == '') {
            $rootScope.ErrorMessage = "The Passwords do not match.";
            $rootScope.IsError = true;
        }
    };
});

app.controller("DashboardController", function($scope) {
    'use strict';

    
});