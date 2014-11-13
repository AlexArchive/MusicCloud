app.directive('dropzoneModel', function ($parse) {
    return {
        restrict: 'A',
        link: function(scope, element, attributes) {
            element.bind('dragover', function (event) {
                event.stopPropagation();
                event.preventDefault();
                event.dataTransfer.dropEffect = 'copy';
            });
            element.bind('drop', function(event) {
                event.stopPropagation();
                event.preventDefault();
                var files = event.dataTransfer.files;
                var model = $parse(attributes.dropzoneModel);
                model.assign(scope, files[0]);
                scope.$apply();
                element.text(files[0].name);
            });
        }
    };
});