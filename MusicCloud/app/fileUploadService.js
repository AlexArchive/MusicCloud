app.service('fileUpload', function () {
    this.uploadFileToUrl = function(file, address, completeCallback, progressCallback) {
        var postData = new FormData();
        postData.append('file', file);

        var request = new XMLHttpRequest();
        request.upload.onprogress = function (e) {
            var progressCompleted = Math.round(e.loaded / e.total * 100);
            progressCallback(progressCompleted);
        };
        request.onreadystatechange = function () {
            if (request.readyState == 4) {
                completeCallback(request.status);
            }
        };
        request.open('POST', address);
        request.send(postData);
    };
});