app.directive('fileModel', function($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attributes) {
            element.bind('change', function () {
                var model = $parse(attributes.fileModel);
                model.assign(scope, element[0].files[0]);
                scope.$apply();
            });
        }
    };
});