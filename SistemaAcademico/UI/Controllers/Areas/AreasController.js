(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AreasController', AreasController);
    AreasController.$inject = ['$scope', '$http'];
    function AreasController($scope, $http) {
        $scope.newArea = {};
        $scope.editing = false;
        $scope.fetchAreas = function () {
            $http.get('/Api/Areas/getAreas').then(function (data) {
                $scope.Areas = data.data;
            });
        }
        $scope.fetchAreas();
        $scope.Registrar = function () {
            $http.post('/api/Areas/createArea', $scope.newArea)
             .then(function (data, status, headers, config) {
                 swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                 $scope.fetchAreas();
             });
        }
        $scope.set = function (area) {
            $scope.editing = true;
            for (var key in area) $scope.newArea[key] = area[key];
        }
        $scope.Actualizar = function () {
            $http.put('/Api/Areas/UpdateArea', $scope.newArea).then(function (data) {
                $scope.fetchAreas();
            });
        }

    }
}());