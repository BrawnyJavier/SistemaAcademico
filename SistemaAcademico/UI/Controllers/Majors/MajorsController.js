(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('MajorsController', MajorsController);
    MajorsController.$inject = ['$scope', '$http'];
    function MajorsController($scope, $http) {
        $scope.title = 'Registrar Carrera';
        $scope.regist = true;
        $scope.newMajor = {};
        var selectedMajor;
        $http.get('/Api/Areas/getAreas').then(function (data) {
            $scope.areas = data.data;
        });
        var fetchMajors = new function () {
            $http.get('/Api/Majors').then(function (data) {
                $scope.majors = data.data;
            });
        }
        $scope.Registrar = function () {
            $http.post('/api/Majors', $scope.newMajor)
            .then(function (data, status, headers, config) {
                swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                fetchMajors();
            },
            function () {
                swal('¡Oops!', 'Ha ocurrido un error.', 'error');
            });
        }
        $scope.Update = function () {
            $scope.newMajor.MajorID = selectedMajor;
            $http.put('/api/Majors', $scope.newMajor)
            .then(function (data, status, headers, config) {
                swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                fetchMajors();
            },
            function () {
                swal('¡Oops!', 'Ha ocurrido un error.', 'error');
            });
        }
        $scope.set = function (obj) {
            $scope.regist = false;
            $scope.title = 'Modificar ' + obj.majorTitle;
            selectedMajor = obj.id;
            for (var key in obj) {
                $scope.newMajor[key] = obj[key];
                if (key = 'area') $('#areasSelect').val(obj[key]);
            }
        }
        $scope.setModal = function (major) {
            $scope.MajorModal = major;
            $scope.modalPensum;
            getPensum();
        }
        var getPensum = function () {
            $http.get('/Api/Majors/getPensum?MajorID=' + $scope.MajorModal.id).then(function (data) {
                console.log("Pensum");
                console.log(data.data);
                $scope.modalPensum = data.data;
            });
        }
        $scope.toggleModalAdd = function () {
            $('#addPensumLine').toggle('blind');
        }
        // Busqueda Asignatura

        $scope.addAsig = function (asigID) {
            $http.post('/api/Majors/addPensumLine', {
                AsignaturaID: asigID,
                MajorID: $scope.MajorModal.id
            }).then(function () {
                getPensum();
            });
        }
        $scope.deletePensumLine = function (lineID) {
            $http.delete('/api/Majors/deletePensumLine?id='+lineID)
                .then(function () {
                    swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                    getPensum();
                });
        }
        $scope.fetchAsignatures = function () {
            $http.get('/Api/Asignatures/fetchAsignatures?query=' + $scope.queryParams).then(function (data) {
                $scope.modalAsigs = data.data;
            });
        }
    }
}());