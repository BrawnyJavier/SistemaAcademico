
(function () {
    // Getting the Context Module
    angular.module("SisAcademicoPA")
      // Addig the  controller function
      // to the context module
      .controller('SectionDetailController', SectionDetailController);
    SectionDetailController.$inject = ['$scope', '$http', '$routeParams'];

    function SectionDetailController($scope, $http, $routeParams) {

        $scope.horarioBtn = 'Añadir Horario';
        $scope.SectionID = $routeParams.SectionID;
        var fetchSeccion = function () {
            $http.get('/api/SeccionesDePeriodos/getSection?id=' + $scope.SectionID).then(function (data) {
                $scope.seccion = data.data.querySet[0];
                $scope.estudiantesInscritos = data.data.estudiantesInscritos;
                fetchTeachers();
            });
        }
        fetchSeccion();
        $scope.AddHorario = function () {
            $scope.horarioBtn === 'Ocultar' ? $scope.horarioBtn = 'Añadir Horario' : 'Ocultar';
            $scope.horarioBtn === 'Añadir Horario' ? $scope.horarioBtn = 'Ocultar' : 'Añadir Horario';;
            $scope.horarioBtn = 'Ocultar';
            //

            var initFParams = $('#FechaInit').val().split(':');
            var fechaInit = new Date(1990, 1, 1, initFParams[0], initFParams[1], 0);

            var FinFParams = $('#FechaFin').val().split(':');
            var fechaFin = new Date(1990, 1, 1, FinFParams[0], FinFParams[1], 0);

            var DTO = {
                roomID: $('#AulaSelect').val(),
                day: $('#DaySelect').val(),
                FechaInit: fechaInit.toUTCString(),
                FechaFin: fechaFin.toUTCString(),
                PeriodAsigID: $routeParams.SectionID
            };
            $http.post('/api/Horarios/', DTO).then(
              function () {
                  swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                  fetchHorarios();
              },
              function () {
                  swal("Datos incorrectos.", "Por favor, revisa los datos suministrados e intenta una vez más.", "error");
              });
        }
        $scope.deleteHorario = function (HorarioID) {
            $http.delete('/api/Horarios/DeleteHorario?id=' + HorarioID).then(
              // if everything goes well, excecute this
              function (data) {
                  swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                  // find the just deleted horario in the scope array holding all horarios
                  var toDelete = _.findWhere($scope.Horarios, {
                      id: HorarioID
                  });
                  // Find and remove item from an array
                  var i = $scope.Horarios.indexOf(toDelete);
                  if (i != -1) $scope.Horarios.splice(i, 1);

              },
              // if the server cannot delete the item, excecute this function
              function () {
                  swal("Ha ocurrido un error.", "Por favor, intenta una vez más.", "error");

              });
        }
        $scope.showHorariosBar = function () {
            $('#addHorario').toggle('blind');

        }
        var teachers = [];
        var fetchTeachers = function () {
            $http.get('/Api/Usuarios/getTeachers').then(function (data) {
                $scope.teachers = data.data;
                teachers.push('<option disabled selected hidden"> - Seleccionar - </option>')
                $.each($scope.teachers, function (index, val) {
                    var option = '<option value="' + val.userId + '">' + val.name + ' ' + val.name2 + ' ' + val.lastName + '</option>';
                    teachers.push(option);
                });
                teachers = teachers.join('');
                $('#teachersSelect').html(teachers);
                $('#teachersSelect').val($scope.seccion.profesorID);
            });
        }
        var fetchHorarios = function () {
            $http.get('/api/Horarios/GetSeccionHorarios/' + $routeParams.SectionID).then(function (data) {
                console.log('Horarios');
                console.log(data);
                $scope.Horarios = data.data;
            });
        }
        fetchHorarios();
        $http.get('/api/Rooms/').then(function (data) {
            $scope.Rooms = data.data;
            //
            var Rooms = [];
            Rooms.push('<option disabled selected hidden"> - Seleccionar - </option>')
            $.each($scope.Rooms, function (index, val) {
                var option = '<option value="' + val.roomID + '">' + val.building + '-' + val.roomNumber + '</option>';
                Rooms.push(option);
            });
            Rooms = Rooms.join('');
            $('#AulaSelect').html(Rooms);
            //
        });
        $scope.inscribirAsig = function () {
            $http.post('/api/SeccionesDePeriodos/', {
                PeriodID: $scope.periodID,
                TeacherID: $('#teachersSelect').val(),
                AsignatureID: $scope.currAsignature.id
            })
              .then(function (data, status, headers, config) {
                  swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                  $scope.fetch();
              });
        }
        // Busqueda de Estudiantes
        $scope.query = "";
        $scope.queryResults = {};

        $scope.fetch = function () {
            $http.get('/api/Usuarios/fetchStudents?query=' + $scope.query + '&seccionID=' + $routeParams.SectionID).then(function (data) {
                console.log(data.data);
                $scope.queryResults = data.data;
            });
        }
        //
        $scope.setAsignature = function (asignature) {
            $scope.currAsignature = asignature;
            $http.get('/api/TeachersAsignatures/getAsignatureTeachers/' + $scope.currAsignature.id).then(function (data) {
                $scope.currAsignature.Teachers = data.data;
                //
                var teachers = [];
                teachers.push('<option disabled selected hidden"> - Seleccionar - </option>')
                $.each($scope.currAsignature.Teachers, function (index, val) {
                    var option = '<option value="' + val.userId + '">' + val.name + ' ' + val.name2 + ' ' + val.lastName + '</option>';
                    teachers.push(option);
                });
                teachers = teachers.join('');
                $('#teachersSelect').html(teachers);
                //
            });
        }
        // Añadir estudiantes
        $scope.inscribirEst = function (estudentID) {
            $http.post('/api/SeccionesDePeriodos/inscribirEst', {
                periodSeccionID: $routeParams.SectionID,
                estudentID: estudentID

            }).then(function (data) {
                swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                fetchSeccion();
                $scope.queryResults = null;
            });
        }
        $scope.BorrarEstudiante = function (estudentID) {

            swal({
                title: 'Eliminar Estudiante de la Sección',
                text: "¿Estás seguro?, Esta acción no podrá ser revertida.",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Si, Eliminalo'
            }).then(function () {
                $http.put('/api/SeccionesDePeriodos/BorrarEstudiante?id=' + estudentID).then(function (data) {
                    swal('¡Listo!', 'Todos los cambios han sido guardados.', 'success');
                    fetchSeccion();
                });
            });
        }
    }
}());
