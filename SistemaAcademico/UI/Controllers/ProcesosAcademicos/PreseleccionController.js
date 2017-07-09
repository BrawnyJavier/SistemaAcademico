(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('PreseleccionController', PreseleccionController);
    PreseleccionController.$inject = ['$scope', '$http'];
    function PreseleccionController($scope, $http) {

    }
}());