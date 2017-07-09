(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('SeleccionController', SeleccionController);
    SeleccionController.$inject = ['$scope', '$http'];
    function SeleccionController($scope, $http) {
    }
}());