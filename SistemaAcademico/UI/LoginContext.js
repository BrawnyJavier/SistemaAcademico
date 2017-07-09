(function () {
    var context = angular.module("LoginContext",
        //Dependencies
        ['ngRoute']);
    context.config(function ($routeProvider, $locationProvider) {
        $locationProvider.hashPrefix('');
        $routeProvider
        .when("/Inicio",
        {
            templateUrl: '/UI/Templates/Inicio.html',
        })
        .when("/",
        {
            templateUrl: '/UI/Templates/Inicio.html',
        })
        .when("/AddMenuOption",
        {
            templateUrl: '/UI/Templates/Sys/AddMenuOption.html',
        })
            /// UI / Templates / Users / AddUser.html
        .when("/AddUSer",
        {
            templateUrl: '/UI/Templates/Users/AddUser.html',
        })
        .otherwise({ redirectTo: '/404' });
        
    });
}());