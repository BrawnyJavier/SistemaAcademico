
(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('RevisionController', RevisionController);
    RevisionController.$inject = ['$scope', '$http'];
    function RevisionController($scope, $http) {

    }
}());