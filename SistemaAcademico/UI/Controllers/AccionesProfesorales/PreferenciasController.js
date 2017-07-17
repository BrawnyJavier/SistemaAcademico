(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
        // Addig the  controller function
        // to the context module
        .controller('PreferenciasController', PreferenciasController);
    PreferenciasController.$inject = ['$scope', '$http'];
    function PreferenciasController($scope, $http) {
        var fetchTeacherPreference = function () {
            $http.get('/api/Usuarios/getTeacherTanda').then(function (data) {
                console.log(data);
                if (data.data) $('#tandasSelect').val(data.data);
            });
        }
        fetchTeacherPreference();
        $scope.updateTanda = function () {
            $http.put('/api/Usuarios/UpdateTeacherTanda?newTanda=' + $('#tandasSelect').val()).then(function () {
                swal('¡Listo!', 'Se ha modificado la tanda.', 'success');
            });
        }
    }
}());