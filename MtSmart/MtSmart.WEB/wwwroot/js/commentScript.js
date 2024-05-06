

function addCommentArea(elem) {
    elem.nextElementSibling.classList.toggle("active");
}

function closeTextEditor() {
    const saveBtn = document.getElementById('addCommentBtns')
    saveBtn.classList.toggle("active");
}

async function saveComment(elem, cardId, userId) {

    const saveBtn = document.getElementById('addCommentBtns')
    saveBtn.classList.toggle("active");

    const commentAreaElem = document.getElementById('commentAreaElem')

    if (commentAreaElem.value != "") {

        createElementComment(commentAreaElem)

        const formData = new FormData();
        formData.append('requestModel.CardId', cardId);
        formData.append('requestModel.UserId', userId);
        formData.append('requestModel.Comment', commentAreaElem.value);

        $.ajax({
            url: '/Card/AddComment',
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

    commentAreaElem.value = ""
}


function createElementComment(elem) {
    const commentAreaMainWrapper = document.getElementById('comment_popup_card_wrapper');
    const imgUserPath = document.querySelector('.profile-content-image');
    const userName = document.querySelector('.profile_name')
    let div = document.createElement('div')
    div.classList.add('comment_user_wrapper')

    div.innerHTML = `
                            <div class="first_row_grid_description margin_container_top_middle">


                                <div class="card_holders_wrapper">
                                    <div class="holder_image_wrapper" style="background-image:url('${imgUserPath.src}');">



                                    </div>
                                </div>


                                <div class="mid_title">
                                    ${userName.innerText}
                                </div>
                            </div>


                            <div class="second_row_grid_description" id="description">
                                <div></div>


                                <div class="commentarea_wrapper" id="commentarea_wrapper">
                                    <div class="comment_area">


                                        <div class="mid_description">${elem.value}</div>


                                    </div>
                                </div>
                            </div>
    `
        commentAreaMainWrapper.appendChild(div)
}