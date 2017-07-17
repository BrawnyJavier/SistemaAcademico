(function () {
    // Getting the Context Module
    angular.module("LoginContext")
    // Addig the  controller function
    // to the context module
    .controller('LoginController', LoginController);
    LoginController.$inject = ['$scope', '$http'];
    function LoginController($scope, $http) {
        $scope.credentials = {
            Identifier: null,
            Password: null
        };
        $scope.Login = function () {

         $http.post('/api/Usuarios/Login', $scope.credentials)
          .then(function (data, status, headers, config) {
              console.log(data);
              if (data.status === 200) {
                  localStorage.setItem('cred', $scope.credentials.Identifier + ',' + $('#pass').val())
                  window.location.replace('/');
              } else {
                  swal("ha ocurrido un error.", "", "warning");
              }
          }, function () {
              swal("Credenciales incorrectas.", "Por favor, confirma los datos ingresados.", "warning");
          });
        }
    }
}());