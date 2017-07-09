(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('AddUserController', AddUserController);
    AddUserController.$inject = ['$scope', '$http'];
    function AddUserController($scope, $http) {
        $scope.userID = null;
        $scope.shownPic = '../../../Content/img/default-d5f51047d8bd6327ec4a74361a7aae7f.jpg';
        $scope.registered = false;
        $scope.newUser = {
            Identifier: null,
            Password: null,
        };
        $scope.Registrar = function Registrar() {
            console.log($scope.newUser);
            var Bdate = new Date($scope.newUser.birthdate);
            $scope.birthdate = Bdate.toUTCString();


            $http.post('/api/Usuarios/Register', $scope.newUser)
          .then(function (data, status, headers, config) {
              if (data.status === 201) {
                  $scope.registered = true;
                  swal("Usuario Registrado", "Se ha creado el usuario correctamente.", "success");
                  $('#ms-account-modal').modal('toggle');
                  $('#registBtn').attr("disabled", false);
                  $('#registBtn').html('Registrarme');
                  $scope.userID = data.data;
                  
              } else {
                  swal("ha ocurrido un error.", "", "warning");
              }
          });
        }
        $scope.PicChange = function () {
            if ($scope.profilePic.data) {
                var formdata = new FormData();
                for (var i in $scope.profilePic) {
                    formdata.append(i, $scope.profilePic[i]);
                }
                $http.post('/api/Usuarios/ChangeProfilePicture?userId=' + $scope.userID, formdata, {
                    transformRequest: angular.identity,
                    headers: {
                        'Content-Type': undefined
                    }
                }).then(function (data, headers, config) {
                    if (data.status == 200) {
                        $scope.shownPic = data.data;
                    }
                });
            } else swal("Por favor selecciona la imagen.");
        }
    }
}());