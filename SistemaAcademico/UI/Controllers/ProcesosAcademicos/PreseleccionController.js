(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('PreseleccionController', PreseleccionController);
    PreseleccionController.$inject = ['$scope', '$http'];
    function PreseleccionController($scope, $http) {
        var currser = sessionStorage.getItem("userid");
        $http.get('/api/Periodos/getCurrent').then(function (r) {
            console.log(r);
            var currPeriod = r.data;
            $http.get('/api/Preseleccion/GetOferta/' + currPeriod.periodoID).then(function (d) {
                $scope.Preseleccion = d.data;
                console.log($scope.Preseleccion);
            });
        });
        var getStudentCurrP = function () {
            $http.get('/api/Preseleccion/GetStudentCurrent?id=' + currser).then(function (d) {
                $scope.studentPre = d.data;
                console.log("EST")
                console.log(d);
            });
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