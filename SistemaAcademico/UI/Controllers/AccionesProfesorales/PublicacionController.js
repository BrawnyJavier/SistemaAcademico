(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('PublicacionController', PublicacionController);
    PublicacionController.$inject = ['$scope', '$http'];
    function PublicacionController($scope, $http) {
        var currser = localStorage.getItem("userid");
        $http.get('/api/SeccionesDePeriodos/getTeachersSections').then(function (data) {
            $scope.TeacherSections = data.data;
            console.log($scope.TeacherSections);
        });
        $scope.publicar = function (obj) {
            console.log(obj);
            if (obj.Nota) {
                $http.put('/api/SeccionesDePeriodos/publish', {
                    Nota: obj.Nota,
                    studentID: obj.idEst,
                    seccion: $('#TandaSelect').val()

                }).then(function (data, status, headers, config) {
                    swal('¡Listo!', 'Publicación registrada', 'success');
                    //  $scope.fetchRooms();
                });
            } else {
                swal('Porfavor ingrese la nota.')
            }
        }
        $scope.set = function () {
            //  alert($('#TandaSelect').val());
            $http.get('/api/SeccionesDePeriodos/getStudents?id=' + $('#TandaSelect').val()).then(function (data) {
                $scope.sec = data.data;
                console.log($scope.sec);
            });
        }
    }
}());