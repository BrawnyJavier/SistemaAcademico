/*(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AulasController', AulasController);
    AulasController.$inject = ['$scope', '$http'];
    function AulasController($scope, $http) {
        $scope.newRoom = {};
        $scope.editing = false;
        $scope.fetchRoom = function () {
            $http.get('/Api/Rooms').then(function (data) {
                $scope.Aulas = data.data;
            });
        }
        $scope.fetchRoom();
        $scope.Registrar = function () {
            $http.post('/api/Rooms/createRoom', $scope.newRoom)
             .then(function (data, status, headers, config) {
                 swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                 $scope.fetchRoom();
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
}());*/
(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('RoomsController', RoomsController);
    RoomsController.$inject = ['$scope', '$http'];
    function RoomsController($scope, $http) {
        $scope.newRooms = {};
        $scope.editing = false;
        $scope.fetchRooms = function () {
            $http.get('/api/Rooms').then(function (data) {
                $scope.Rooms = data.data;
            });
        }
        $scope.fetchRooms();
        $scope.Registrar = function () {
            $http.post('/api/Rooms', $scope.newRooms)
             .then(function (data, status, headers, config) {
                 swal('‥isto!', 'Todos los cambios han sido guardados.', 'success');
                 $scope.fetchRooms();
             });
        }
        $scope.set = function (Rooms) {
            $scope.editing = true;
            for (var key in Rooms) $scope.newRooms[key] = Rooms[key];
        }
        $scope.Actualizar = function () {
            $http.put('/Api/Rooms', $scope.newRooms).then(function (data) {
                $scope.fetchRooms();
            });
        }

    }
}());