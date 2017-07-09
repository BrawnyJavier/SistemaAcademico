(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('OfertaAcademicaController', OfertaAcademicaController);
    OfertaAcademicaController.$inject = ['$scope', '$http', '$routeParams'];
    function OfertaAcademicaController($scope, $http, $routeParams) {

    }
}());