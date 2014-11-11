app.controller('mainController', function ($scope) {
    $scope.upload = function () {
        $scope.uploadStatus = "Uploading..";

        var postData = new FormData();
        postData.append('file', $scope.file);

        var request = new XMLHttpRequest();
        request.upload.onprogress = function (e) {
            var progressCompleted = Math.round(e.loaded / e.total * 100);
            $scope.progress = progressCompleted;
            $scope.$apply();
        };
        request.onreadystatechange = function () {
            if (request.readyState == 4) {
                if (request.status == 200) {
                    $scope.uploadStatus = "Upload complete.";
                } else {
                    $scope.uploadStatus = "Upload failed.";
                }
                $scope.$apply();
            }
        };
        request.open('POST', 'api/upload');
        request.send(postData);
    };
});