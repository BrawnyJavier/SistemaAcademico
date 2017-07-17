////****************************************//
////  AUTHOR : Brawny Javier Mateo Reyes    //
////  DATE: 26/6/2017                        //
////  DESCRIPTION: Directive usada para los //
////  inputs / forms con file input         //
////****************************************//
(
    function () {
        // call our Context
        angular.module("SisAcademicoPA")
        .directive('fileModel', ['$parse', function ($parse) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var model = $parse(attrs.fileModel);
                    var modelSetter = model.assign;
                    element.bind('change', function () {
                        scope.$apply(function () {
                            modelSetter(scope, element[0].files[0]);
                        })
                    })
                }
            }
        }]);
    }()
);