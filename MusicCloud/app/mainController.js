app.controller('mainController', function ($scope, fileUpload) {

    $scope.upload = function () {

        if ($scope.file == false) {
            $scope.uploadStatus = "Please choose a file to upload.";
            return;
        }

        var fileSizeLimit = 20000000;
        var fileSize = Math.round(parseInt($scope.file.size));
        if (fileSize > fileSizeLimit) {
            $scope.uploadStatus = "File is too big. Maximim file size: " + fileSizeLimit;
            return;
        }

        $scope.uploading = true;
        $scope.uploadStatus = "Uploading..";
        fileUpload.uploadFileToUrl(
            $scope.file,
            'api/upload',
            onUploadCompleted,
            onProgressChanged
        );
    };

    function onProgressChanged(progress) {
        $scope.progress = progress;
        $scope.$apply();
    };

    function onUploadCompleted(statusCode) {
        $scope.uploading = false;
        if (statusCode === 200) {
            $scope.uploadStatus = "Upload complete.";
        } else {
            $scope.uploadStatus = "Upload failed.";
        }
        $scope.$apply();
    }
});