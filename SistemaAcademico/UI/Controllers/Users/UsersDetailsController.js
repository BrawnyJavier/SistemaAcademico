(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
    // Addig the  controller function
    // to the context module
    .controller('UsersDetailsController', UsersDetailsController);
    UsersDetailsController.$inject = ['$scope', '$http', '$routeParams'];
    function UsersDetailsController($scope, $http, $routeParams) {
        $scope.userId = $routeParams.userId;
        $scope.isStudent = false;
        var majors = [];
        var fetchMajors = new function () {
            $http.get('/Api/Majors').then(function (data) {
                $scope.majors = data.data;
                $.each($scope.majors, function (index, val) {
                    console.log(index); console.log(val);
                    var option = '<option value="' + val.id + '">' + val.majorTitle + '</option>';
                    majors.push(option);
                });
                majors = majors.join('');
                console.log(majors);
                $('#majorsSelect').html(majors);


            });
        }
        $scope.fetch = new function () {
            $http.get('/api/Usuarios/fetchUser?userId=' + $scope.userId).then(function (data) {
                console.log('curser');
                console.log(data.data);
                $scope.currser = data.data;

                $scope.currser.UserId = $scope.userId;
                $('#userTypeSelect').val($scope.currser.userType);
                if ($scope.currser.userType === 0) $scope.isStudent = true;
            });
        }
        $scope.update = function () {
            $scope.currser.userType = $('#userTypeSelect').val();
            $http.put('/api/Usuarios/UpdateUser', $scope.currser)
            .then(function () {
                swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
            },
            function () {
                swal('¡Oops!', 'Ha ocurrido un error.', 'error');
            });
        }
        $scope.PicChange = function () {
            if ($scope.profilePic.data) {
                var formdata = new FormData();
                for (var i in $scope.profilePic) {
                    formdata.append(i, $scope.profilePic[i]);
                }
                $http.post('/api/Usuarios/ChangeProfilePicture?userId=' + $scope.userId, formdata, {
                    transformRequest: angular.identity,
                    headers: {
                        'Content-Type': undefined
                    }
                }).then(function (data, headers, config) {
                    if (data.status == 200) {
                        swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                        $scope.currser.profilePicturePath = data.data;
                    }
                });
            } else swal("Por favor selecciona la imagen.");
        }
        $scope.Inscribir = function () {
            $http.post('/api/StudentsMajors/',
                        {
                            StudentID: $scope.userId,
                            MajorID: $('#majorsSelect').val()
                        }
            ).then(function (data, status, headers, config) {
                if (data.status === 200) {

                } else {
                    swal("ha ocurrido un error.", "", "warning");
                }
            });
        }
    }
}());