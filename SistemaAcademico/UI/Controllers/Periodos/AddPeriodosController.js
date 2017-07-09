(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AddPeriodosController', AddPeriodosController);
    AddPeriodosController.$inject = ['$scope', '$http', '$location'];
    function AddPeriodosController($scope, $http, $location) {
        $scope.registered = false;
        $scope.Registrar = function () {

            var _FINIT = new Date($scope.fechaInicio);
            var fechaInicio = _FINIT.toUTCString();

            var _FFIN = new Date($scope.fechaFin);
            var fechaFin = _FFIN.toUTCString();

            var fechaInicioPreselecion = new Date($scope.fechaInicioPreselecion);
            var fechaInicioPreselecion = fechaInicioPreselecion.toUTCString();

            var fechafinPreseleccion = new Date($scope.fechafinPreseleccion);
            var fechafinPreseleccion = fechafinPreseleccion.toUTCString();

            var fechainicioSeleccion = new Date($scope.fechainicioSeleccion);
            var fechainicioSeleccion = fechainicioSeleccion.toUTCString();

            var fechaLimiteRetiro = new Date($scope.fechaLimiteRetiro);
            var fechaLimiteRetiro = fechaLimiteRetiro.toUTCString();

            var fechafinSeleccion = new Date($scope.fechafinSeleccion);
            var fechafinSeleccion = fechafinSeleccion.toUTCString();

            var newPeriod = {
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                fechaInicioPreselecion: fechaInicioPreselecion,
                fechafinPreseleccion: fechafinPreseleccion,
                fechainicioSeleccion: fechainicioSeleccion,
                fechafinSeleccion: fechafinSeleccion,
                fechaLimiteRetiro: fechaLimiteRetiro
            };

            $http.post('/api/Periodos/', newPeriod)
             .then(function (data, status, headers, config) {
                 if (data.status === 201) {
                     $location.path('/PeriodDetail/' + data.data);
                 }
             }, function () {
                 swal('¡Oops!', 'Ha ocurrido un error.', 'error');
             });
        }
    }
}());