(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AdmDashController', AdmDashController);
    AdmDashController.$inject = ['$scope', '$http'];
    function AdmDashController($scope, $http) {
      
        $http.get('/Api/Usuarios/getContext').then(function (data) {
            $scope.sysConf = data.data;
            console.log(data.data);
        });
    }
}());