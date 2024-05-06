
function deleteFile(cardId, fileId, userId) {

    var url = '/Card/DeleteFile?cardId=' + cardId + '&fileId=' + fileId + '&userId=' + userId;

    $.get(url)
        .done(function (data) {
            var url = '/Card/GetCardDetails?employeeId=' + userId + '&cardId=' + cardId;

            $.get(url)
                .done(function (data) {
                    $('.modal-body').html(data);
                })
                .fail(function (xhr, status, error) {
                    var errorMessage = 'Error: ' + status + ' - ' + error;
                    alert(errorMessage);
                    console.error(errorMessage);
                });
        })
        .fail(function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        });
}

function uploadFile(target, cardId, userId) {
    const formData = new FormData();
    formData.append('requestModel.CardId', cardId);
    formData.append('requestModel.UserId', userId);
    for (let i = 0; i < target.files.length; i++) {
        formData.append('requestModel.FilesToUpload', target.files[i]);
    }

    $.ajax({
        url: '/Card/UploadFile',
        type: 'POST',
        data: formData,
        processData: false,  // отключаем автоматическую обработку данных
        contentType: false,  // отключаем автоматическое установление заголовка Content-Type
        success: function (response) {
            var url = '/Card/GetCardDetails?employeeId=' + userId + '&cardId=' + cardId;

            $.get(url)
                .done(function (data) {
                    $('.modal-body').html(data);
                })
                .fail(function (xhr, status, error) {
                    var errorMessage = 'Error: ' + status + ' - ' + error;
                    alert(errorMessage);
                    console.error(errorMessage);
                });
        },
        error: function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        }
    });
}