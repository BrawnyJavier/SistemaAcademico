(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('PreseleccionController', PreseleccionController);
    PreseleccionController.$inject = ['$scope', '$http'];
    function PreseleccionController($scope, $http) {
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
        var getStudentCurrP = function () {
            $http.get('/api/Preseleccion/GetStudentCurrent?id=' + currser).then(function (d) {
                $scope.studentPre = d.data;
                console.log("EST")
                console.log(d);
            }, function (data) {
                if (data.status == 403)
                    swal('No disponible.', 'La Preselección no está disponible actualmente.', 'warning');
            });
        }
        $scope.inPreseleccion = function (obj) {
            console.log(obj);
            var any = _.findWhere($scope.studentPre, { materiaId: obj.id });
            console.log("ANY!");
            console.log(any);
            if (any) return true;
            else return false;
        }

        getStudentCurrP();
        $scope.add = function (asig) {
            var dto = {
                userID: currser,
                AsignatureID: asig.id,
                Tanda: $('#' + asig.id + 'TandaSelect').val()
            }
            $http.post('/api/Preseleccion/PostLine', dto).then(function (d) {
                swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                getStudentCurrP();
            });
        }
    }
}());