
(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
        // Addig the  controller function
        // to the context module
        .controller('RevisionController', RevisionController);
    RevisionController.$inject = ['$scope', '$http'];
    function RevisionController($scope, $http) {
        var getAsig = function () {
            $http.get('/api/SeccionesDePeriodos/getAsignaturasPublicadas').then(function (data) {
                console.log(data);
                $scope.pubs = data.data;

            });
        }
        getAsig();
        $scope.Registrar = function () {
            $http.post('/api/Revisiones/solicitarRevision', {
                historialID: $scope.solData.historialId,
                motivo: $scope.solicitud.motivo

            }).then(function () {
                getAsig();
                swal('¡Listo!', 'Se ha generado la solicitud.', 'success');

            });
        }
        $scope.goBack = () => {
            $('#solForm').toggle('blind');
            $('#pubstable').toggle('blind');
            $('#solMotivo').prop('disabled', false);
            $('#solicitudBtn').prop('disabled', false);
            $scope.solicitud = null;
            $scope.comentariosProfesor = false;
            $scope.comentariosAdmin = false;
        }
        $scope.getRevisionDetails = (revision) => {
            $http.get('/api/Revisiones/getRevisionDetails/' + revision.solicitudRevisionId).then(function (data) {
                console.log('  console.log(data);');
                console.log(data.data);
                $scope.solicitud = {};
                $scope.solicitud.motivo = data.data.motivo;
                $('#solMotivo').prop('disabled', true);
                $('#solicitudBtn').prop('disabled', true);
                //motivo
                $('#solForm').toggle('blind');
                $('#pubstable').toggle('blind');
                $scope.solData = revision;
                $scope.solicitud.motivo = data.data.motivo;
                if (data.data.comentariosProfesor) {
                    $scope.comentariosProfesor = true;
                    $scope.solicitud.comentarioProfe = data.data.comentariosProfesor;
                }
                if (data.data.comentarioAdmin) {
                    $scope.comentariosAdmin = true;
                    $scope.solicitud.comentarioAdmin = data.data.comentarioAdmin;
                }
                if (data.data.fechaReunion) {
                    $scope.fechaReunion = true;
                    $scope.solicitud.fechaReunion = data.data.fechaReunion;
                }
            });
        }
        $scope.initRevision = function (obj) {
            $scope.solData = null;
            $('#solForm').toggle('blind');
            $('#pubstable').toggle('blind');

            $scope.solData = obj;
            var d = new Date();
            $scope.solData.date = d.toUTCString();

        }
    }
}());