(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AdminPeriodosController', AdminPeriodosController);
    AdminPeriodosController.$inject = ['$scope', '$http'];
    function AdminPeriodosController($scope, $http) {
        $scope.query = "";
        $scope.queryResults = {};
        $scope.fetch = () => {
            $http.get('/api/Periodos/getPeriods?query=' + $scope.query).then(function (data) {
                console.log(data.data);
                $scope.queryResults = data.data;

            });
        };
    }
}());
