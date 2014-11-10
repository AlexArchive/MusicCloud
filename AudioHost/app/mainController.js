app.controller('mainController', function($scope, $http) {
    $scope.upload = function() {
        $scope.uploadStatus = "Uploading..";
        var data = new FormData();
        data.append('file', $scope.file);

        $http.post('api/upload', data,
        {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
        .success(function() {
            $scope.uploadStatus = "Upload complete.";
        })
        .error(function() {
            $scope.uploadStatus = "Failed to upload content.";
        });
    };
});