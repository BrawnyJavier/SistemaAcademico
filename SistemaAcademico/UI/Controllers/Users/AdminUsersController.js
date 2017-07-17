(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AdminUsersController', AdminUsersController);
    AdminUsersController.$inject = ['$scope', '$http'];
    function AdminUsersController($scope, $http) {
        $scope.query = "";
        $scope.queryResults = {};
      
        $scope.fetch = function () {
            if ($scope.query.length >= 3)
                $http.get('/api/Usuarios/fetchUsers?query=' + $scope.query).then(function (data) {
                    console.log(data.data);
                    $scope.queryResults = data.data;
                });
        }
    }
}());