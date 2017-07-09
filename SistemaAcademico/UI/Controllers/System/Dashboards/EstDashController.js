(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('EstDashController', EstDashController);
    EstDashController.$inject = ['$scope', '$http'];
    function EstDashController($scope, $http) {

        $http.get('/Api/Usuarios/getContext').then(function (data) {
            $scope.sysConf = data.data.identity;
            $scope.studentConf = data.data.studentDash
            console.log("Context");
            console.log(data.data);

            //IndiceChart//
            var ctx = document.getElementById("IndiceChart").getContext('2d');
            var gpa = $scope.studentConf.programa.gpa;
            var data = {
                datasets: [{
                    data: [gpa, 4.0-gpa],
                    backgroundColor: [
                    'rgba(0, 129, 193, 0.8)',
                     'rgba(0, 129, 193, 0.2)'
                    ]
                }],
                labels: [
                    'Indice',
                    'Restante'
                ]
            };
            var myDoughnutChart = new Chart(ctx, {
                type: 'doughnut',
                data: data,
            });

        });

        var RenderCharts = new function () {
            //TrimestresCursadosChart//
            var ctx = document.getElementById("TrimestresCursadosChart").getContext('2d');
            var data = {
                datasets: [{
                    data: [1, 21],
                    backgroundColor: [
                    'rgba(226, 9, 52, 0.8)',
                     'rgba(226, 9, 52, 0.2)'
                    ]
                }],
                labels: [
                    'Cursados',
                    'Disponibles',
                ]

            };
            var myDoughnutChart = new Chart(ctx, {
                type: 'doughnut',
                data: data,

            });
          
            //AsignaturasAprobadasChart//
            var ctx = document.getElementById("AsignaturasAprobadasChart").getContext('2d');
            var data = {
                datasets: [{
                    data: [1.3, 4.0],
                    backgroundColor: [
                    'rgba(87, 229, 137, 0.8)',
                     'rgba(87, 229, 137, 0.2)'
                    ]
                }],
                labels: []
            };
            var myDoughnutChart = new Chart(ctx, {
                type: 'doughnut',
                data: data,
            });

            //CreditosAprobadosChart//
            var ctx = document.getElementById("CreditosAprobadosChart").getContext('2d');
            var data = {
                datasets: [{
                    data: [1.3, 4.0],
                    backgroundColor: [
                    'rgba(255, 182, 0, 0.8)',
                     'rgba(255, 182, 0, 0.2)'
                    ]
                }],
                labels: []
            };
            var myDoughnutChart = new Chart(ctx, {
                type: 'doughnut',
                data: data,
            });
        }
    }
}());