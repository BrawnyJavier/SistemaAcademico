(function () {
    var context = angular.module("SisAcademicoPA",
        //Dependencies
        ['ngRoute']);
    context.config(function ($routeProvider, $locationProvider) {
        $locationProvider.hashPrefix('');
        $routeProvider
            .when("/Inicio",
            {
                templateUrl: '/UI/Templates/Inicio.html',
            })
            .when("/",
            {
                templateUrl: '/UI/Templates/Inicio.html',
            })
            .when("/AddMenuOption",
            {
                templateUrl: '/UI/Templates/Sys/AddMenuOption.html',
            })
            .when("/AddUSer",
            {
                templateUrl: '/UI/Templates/Users/AddUser.html',
            })
            .when("/AdministrarUsuarios",
            {
                templateUrl: '/UI/Templates/Users/AdminUsers.html',
            })
            .when("/AddAsignature",
            {
                templateUrl: '/UI/Templates/Asignatures/AddAsignature.html',
            })
            .when("/adminAsignatures",
            {
                templateUrl: '/UI/Templates/Asignatures/AdminAsignatures.html',
            })
            .when("/addMajor",
            {
                templateUrl: '/UI/Templates/Majors/AddMajors.html',
            })
            .when("/UsersDetails/:userId",
            {
                templateUrl: '/UI/Templates/Users/UsersDetails.html',
            })
            .when("/addArea",
            {
                templateUrl: '/UI/Templates/Areas/AddArea.html',
            })
            .when("/AddPeriodo",
            {
                templateUrl: '/UI/Templates/Periodos/AddPeriodo.html',
            })
            .when("/AdminPeriodo",
            {
                templateUrl: '/UI/Templates/Periodos/AdminPeriodo.html',
            })
            .when("/PeriodDetail/:periodID",
            {
                templateUrl: '/UI/Templates/Periodos/PeriodDetail.html',
            })
            .when("/AsignatureDetail/:AsignatureID",
            {
                templateUrl: '/UI/Templates/Asignatures/AsignatureDetails.html',
            })
            .when("/SectionDetails/:SectionID",
            {
                templateUrl: '/UI/Templates/Secciones/SectionDetail.html',
            })
            .when("/Preseleccion",
            {
                templateUrl: '/UI/Templates/ProcesosAcademicos/Preseleccion.html',
            })
            .when("/Seleccion",
            {
                templateUrl: '/UI/Templates/ProcesosAcademicos/Seleccion.html',
            })
            .when("/Retiro",
            {
                templateUrl: '/UI/Templates/ProcesosAcademicos/Retiro.html',
            })
            .when("/Programas",
            {
                templateUrl: '/UI/Templates/Majors/Pensums.html',
            })
            .when("/OfertaAcademica",
            {
                templateUrl: '/UI/Templates/Periodos/OfertaAcademica.html',
            })
            .when("/Revision",
            {
                templateUrl: '/UI/Templates/ProcesosAcademicos/Revision.html',
            })
            .when("/AddAula",
            {
                templateUrl: '/UI/Templates/Rooms/AddAula.html',
            })
            .when("/Publicar",
            {
                templateUrl: '/UI/Templates/AccionesProfesorales/Publicacion.html',
            })
            .when("/Revisiones",
            {
                templateUrl: '/UI/Templates/AccionesProfesorales/Revision.html',
            })
            .when("/Preferencia",
            {
                templateUrl: '/UI/Templates/AccionesProfesorales/Preferencias.html',
            })
            .when("/Hisorial",
            {
                templateUrl: '/UI/Templates/ProcesosAcademicos/HistorialAcademico.html',
            })
            .otherwise({ redirectTo: '/404' });

    });
    context.run(['$http', function ($http) {
        var tkn = localStorage.cred;
        if (!tkn) {
            window.location.replace('/Login');
        }
        else {
            var coded = btoa(tkn);
            $http.defaults.headers.common.Authorization = coded;
        }
    }]);
}());