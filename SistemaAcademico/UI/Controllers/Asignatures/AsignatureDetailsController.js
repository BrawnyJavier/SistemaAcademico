(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AsignatureDetailsController', AsignatureDetailsController);
    AsignatureDetailsController.$inject = ['$scope', '$http', '$routeParams'];
    function AsignatureDetailsController($scope, $http, $routeParams) {
        $scope.currentAsignature = $routeParams.AsignatureID;
        var fetch = function () {
            $http.get('/api/Asignatures/getAsignatura/' + $routeParams.AsignatureID).then(function (data) {
                console.log(data.data);
                $scope.currentAsignature = data.data;
                $('#tipoAsignaturaSelect').val($scope.currentAsignature.tipoAsignatura);
            });
        }
        fetch();

        $scope.Inscribir = function () {
            var data = { TeacherID: $('#majorsSelect').val(), AsignatureID: $routeParams.AsignatureID };
            console.log(data);
            $http.post('/api/TeachersAsignatures/',
                 data
                ).then(function (data) {
                    swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                    fetch();
                });
        }

        ////
        var teachers = [];
        var fetchTeachers = new function () {
            $http.get('/Api/Usuarios/getTeachers').then(function (data) {
                $scope.teachers = data.data;
                teachers.push('<option disabled selected hidden"> - Seleccionar - </option>')
                $.each($scope.teachers, function (index, val) {
                    console.log(index); console.log(val);
                    var option = '<option value="' + val.userId + '">' + val.name + ' ' + val.name2 + ' ' + val.lastName + '</option>';
                    teachers.push(option);
                });
                teachers = teachers.join('');
                console.log(teachers);
                $('#majorsSelect').html(teachers);
            });
        }
        ////
        $scope.DeleteTeacher = function (asignID) {
            swal({
                title: 'Eliminar Maestro de la Materia',
                text: "¿Estás seguro?, Esta acción no podrá ser revertida.",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Si, Eliminalo'
            }).then(function () {
                $http.delete('/api/Asignatures/DeleteTeacher?asignid=' + asignID).then(function (data) {
                    swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                    fetch();
                });
            });







        }
    }
}());