(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AdminAsignaturasController', AdminAsignaturasController);
    AdminAsignaturasController.$inject = ['$scope', '$http'];
    function AdminAsignaturasController($scope, $http) {
        $scope.query = "";
        $scope.queryResults = {};
        $scope.GoDet = function (id) {

        }
        $scope.fetch = function () {
            $http.get('/api/Asignatures/fetchAsignaturas?query=' + $scope.query).then(function (data) {
                console.log(data.data);
                $scope.queryResults = data.data;

            });
        }
    }
}());