(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('PublicacionController', PublicacionController);
    PublicacionController.$inject = ['$scope', '$http'];
    function PublicacionController($scope, $http) {
        var currser = localStorage.getItem("userid");
        $http.get('/api/SeccionesDePeriodos/getTeachersSections').then(function (data) {
            $scope.TeacherSections = data.data;
            console.log($scope.TeacherSections);
        });
    }
}());