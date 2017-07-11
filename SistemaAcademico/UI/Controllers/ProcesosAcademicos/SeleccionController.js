(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('SeleccionController', SeleccionController);
    SeleccionController.$inject = ['$scope', '$http'];
    function SeleccionController($scope, $http) {

        var currser = localStorage.getItem("userid");
        $scope.getOferta = function () {
            $http.get('/api/Periodos/getCurrent').then(function (r) {
                console.log(r);
                var currPeriod = r.data;
                $http.get('/api/Preseleccion/GetOferta?id=' + currPeriod.periodoID + '&query=' + $scope.query).then(function (d) {
                    $scope.Preseleccion = d.data;
                    console.log($scope.Preseleccion);
                });
            });
        }
        $scope.getMateriasSecciones = function (materiaID) {
            $http.get('/api/Preseleccion/getSeccionesByMateria?MateriaID=' + materiaID).then(function (d) {
                $scope.ModalSecciones = d.data;
                console.log('Result');
                console.log($scope.ModalSecciones);
            });
        }

        $scope.getStudentSeleccion = function () {
            $http.get('/api/Seleccion/getStudentSelection/').then(function (d) {
                $scope.seleccionEst = d.data;
                console.log("WWWWWWWWWWWWWWW");
                console.log(d);
              
            });
        }
        $scope.getStudentSeleccion();
        $scope.inPreseleccion = function (obj) {
            console.log(obj);
            var any = _.findWhere($scope.studentPre, { materiaId: obj.id });
            console.log("ANY!");
            console.log(any);
            if (any) return true;
            else return false;
        }

        //   getStudentCurrP();
        $scope.add = function (MateriaID) {
            $http.post('/api/Seleccion/PostHistorial?seccionID='+ MateriaID).then(function (d) {
                swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                console.log(d);
                $scope.getStudentSeleccion();
            });
        }
    }
}());