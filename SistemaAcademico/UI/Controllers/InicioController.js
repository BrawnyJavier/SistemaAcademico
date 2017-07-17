(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('InicioController', InicioController);
    InicioController.$inject = ['$scope', '$http'];
    function InicioController($scope, $http) {
        $scope.query = "cxvxvsds";
        $scope.Dashboard = '/UI/Templates/Sys/Dashboards/StudentsDashboard.html';
        $scope.sysConf = {};
        console.log('asdas');
        console.log($http.defaults.headers.common.Authorization);
        $http.get('/Api/Usuarios/getContext').then(function (data) {
            console.log(data.data);
            $scope.sysConf = data.data;
            $scope.sysConf.TipoUsuarioText = 'Loading...';
            var t = $scope.sysConf.tipousuario;
            if (t === 2) {
                $scope.Dashboard = '/UI/Templates/Sys/Dashboards/AdminsDashboard.html';
                $scope.sysConf.TipoUsuarioText = 'Administrador';
            }
            else if (t === 1) {
                $scope.Dashboard = '/UI/Templates/Sys/Dashboards/TeachersDashboard.html';
                $scope.sysConf.TipoUsuarioText = 'Profesor';
            }
            else if (t === 0) {
                $scope.Dashboard = '/UI/Templates/Sys/Dashboards/StudentsDashboard.html';
                $scope.sysConf.TipoUsuarioText = 'Estudiante';
            }
        });
    }
}());