(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AddAsignatureController', AddAsignatureController);
    AddAsignatureController.$inject = ['$scope', '$http'];
    function AddAsignatureController($scope, $http) {
        $scope.newAsignature = {
            NombreAsignatura: null,
            Creditos: null,
            Codigo: null,
            TipoAsignatura : null
        };
        $scope.Registrar = function () {
            $http.post('/api/Asignatures/createAsignatura', $scope.newAsignature)
             .then(function (data, status, headers, config) { swal('Listo') });
        }
    }
}());