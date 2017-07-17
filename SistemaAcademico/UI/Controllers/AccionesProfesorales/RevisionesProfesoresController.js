(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
        // Addig the  controller function
        // to the context module
        .controller('RevisionesProfesoresController', RevisionesProfesoresController);
    RevisionesProfesoresController.$inject = ['$scope', '$http'];
    function RevisionesProfesoresController($scope, $http) {
        var currser = localStorage.getItem("userid");
        var fetch = () => {
            $http.get('/api/SeccionesDePeriodos/getTeachersRevisiones').then(function (data) {
                $scope.TeacherSections = data.data;
                console.log($scope.TeacherSections);
            });
        }
        // idSolicitud
        fetch();
        $scope.setModal = function (e) {
            console.log(e);
            $scope.modalData = e;
        }
        $scope.denegate = function () {
            swal({
                title: '(?)',
                input: 'text',
                showCancelButton: true,
                confirmButtonText: 'Denegar',
                showLoaderOnConfirm: true,
                allowOutsideClick: false
            }).then(function (val) {
                $http.put('/api/Revisiones/denegateSolicitud', {
                    solicitudID: $scope.modalData.idSolicitud,
                    TeacherComment: val,
                }).then(function () {
                    swal('¡Listo!', 'Solicitud denegada.', 'success');
                    fetch();
                });
            });
            $('#swal2-title').html('<h2>Denegar la solicitud de ' + $scope.modalData.estudiante + ' </h2><h3 style="color:red">OJO: Esta acción no puede ser revertida.</h3>');
        }
        $scope.agendar = function () {
            var Bdate = new Date($('#Fecha').val());
            $scope.cita.fechaReunion = Bdate.toUTCString();
            $scope.cita.solRevisionID = $scope.modalData.idSolicitud;
            $http.post('/api/Revisiones/generateReunion', $scope.cita).then(function () {
                swal('¡Listo!', 'Se ha agendado la cita.', 'success');
                fetch();
            }, function () {
                swal('¡Ooops, Ha ocurrido un error!', 'Por favor, intentalo otra vez.', 'success');
            });
        }
        $scope.changeCalif = function (obj) {
            swal({
                title: 'Introduzca la nueva calificacion',
                input: 'number',
                showCancelButton: true,
                confirmButtonText: 'Cambiar',
                showLoaderOnConfirm: true,
                allowOutsideClick: false
            }).then(function (val) {
                $http.put('/api/SeccionesDePeriodos/changeCalif', {
                    newCalif: val,
                    historyLineId: $scope.modalData.historyLine,
                    solicitudId: $scope.modalData.idSolicitud
                }).then(function () {
                    swal('¡Listo!', 'Cambio registrado.', 'success');
                });
            });
            $('#swal2-title').html('<h2>Cambiar calificación a ' + $scope.modalData.estudiante+' </h2><h3 style="color:red">OJO: Esta acción no puede ser revertida.</h3>');
        }
    }
}());