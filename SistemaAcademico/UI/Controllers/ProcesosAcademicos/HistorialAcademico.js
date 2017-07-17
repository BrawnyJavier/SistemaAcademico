
(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
        // Addig the  controller function
        // to the context module
        .controller('HistorialAcademicoController', HistorialAcademicoController);
    HistorialAcademicoController.$inject = ['$scope', '$http'];
    function HistorialAcademicoController($scope, $http) {
        $http.get('/Api/Usuarios/getContext').then(function (data) {
            console.log(data.data);
            $scope.currser = data.data.identity;
            $scope.currentProgram = data.data.studentDash.programa.id;
      
            init();
        });
        function init() {
            var StudentsMajors = [];
            var FetchStudentsMajors = function () {
                $http.get('/Api/StudentsMajors/getStudentsMajors?id=' + $scope.currser.idusuario).then(function (data) {
                    $scope.StudentsMajors = data.data;
                    StudentsMajors.push('<option disabled selected hidden"> - Seleccionar - </option>')
                    $.each($scope.StudentsMajors, function (index, val) {
                        console.log(index); console.log(val);
                        var option = '<option value="' + val.id + '">' + val.name + '</option>';
                        StudentsMajors.push(option);
                    });
                    StudentsMajors = StudentsMajors.join('');
                    console.log(StudentsMajors);
                    $('#CarreraSelect').html(StudentsMajors);
                });
            };
            FetchStudentsMajors();
        }
        $scope.fetchHistory = function () {
            $http.get('/Api/StudentsMajors/getHistorialAcademico?userMajor=' + $scope.currentProgram).then(function (data) {
                $scope.history = data.data;
                $('#pensumTable').toggle('blind');
            });
        }
    }
}());