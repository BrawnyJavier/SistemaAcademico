
(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('RetiroController', RetiroController);
    RetiroController.$inject = ['$scope', '$http'];
    function RetiroController($scope, $http) {

    }
}());