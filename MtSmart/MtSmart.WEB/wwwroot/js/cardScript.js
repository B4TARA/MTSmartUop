
function getCreateCard(employeeId) {
    var url = '/Card/CreateCard?employeeId=' + employeeId;

    $.get(url)
        .done(function (data) {
            $('.modal-body').html(data);
        })
        .fail(function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        });
}

function getCardDetailsHandle(cardId, employeeId, columnNumber) {
    if (columnNumber == 4 || columnNumber == 5) {
        getCardAssessment(cardId, employeeId);
    } else {
        getCardDetails(cardId, employeeId);
    }
}

function getCardDetails(cardId, employeeId) {
    var url = '/Card/GetCardDetails?employeeId=' + employeeId + '&cardId=' + cardId;

    $.get(url)
        .done(function (data) {
            $('.modal-body').html(data);
        })
        .fail(function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        });
}

function getCardAssessment(cardId, employeeId) {
    var url = '/Card/GetCardAssessment?employeeId=' + employeeId + '&cardId=' + cardId;

    $.get(url)
        .done(function (data) {
            $('.modal-body').html(data);
        })
        .fail(function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        });
}

function setSupervisorAssessment(event, cardId, employeeId) {
    event.preventDefault(); // Предотвращаем отправку формы по умолчанию

    var supervisorQuailityAssessment = document.getElementById('supervisorQualityAssessment').value;
    if (supervisorQuailityAssessment <= 0) {
        alert("Пожалуйста, выберите оценку качества.");
        return;
    }

    var supervisorTermAssessment = document.getElementById('supervisorTermAssessment').value;
    if (supervisorTermAssessment <= 0) {
        alert("Пожалуйста, выберите оценку срока.");
        return;
    }

    var supervisorCommentForAssessment = document.getElementById('supervisorCommentForAssessment').value;
    if (supervisorCommentForAssessment.trim() === '') {
        alert('Пожалуйста, введите комментарий к оценочному суждению.');
        return;
    }

    var factTerm = document.getElementById('factTerm').value;
    if (factTerm === '') {
        alert("Пожалуйста, выберите фактическую дату реализации задачи.");
        return;
    }

    var formData = $(event.target).closest('form').serialize(); // Получаем данные формы

    $.post({
        url: '/Card/SetSupervisorAssessment',
        data: formData,
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        }
    });
}

function setEmployeeAssessment(event, cardId, employeeId) {
    event.preventDefault(); // Предотвращаем отправку формы по умолчанию

    var hoursOfWork = document.getElementById('hoursOfWork').value;
    if (hoursOfWork <= 0) {
        alert("Пожалуйста, введите человеко-часы, затраченные на выполнение данной задачи.");
        return;
    }

    var employeeQuailityAssessment = document.getElementById('employeeQualityAsessessment').value;
    if (employeeQuailityAssessment <= 0) {
        alert("Пожалуйста, выберите оценку качества.");
        return;
    }

    var employeeTermAssessment = document.getElementById('employeeTermAssessment').value;
    if (employeeTermAssessment <= 0) {
        alert("Пожалуйста, выберите оценку срока.");
        return;
    }

    var employeeCommentForAssessment = document.getElementById('employeeCommentForAssessment').value;
    if (employeeCommentForAssessment.trim() === '') {
        alert('Пожалуйста, введите отчет о выполнении задачи.');
        return;
    }

    var formData = $(event.target).closest('form').serialize(); // Получаем данные формы

    $.post({
        url: '/Card/SetEmployeeAssessment',
        data: formData,
        success: function (response) {
            var url = '/Card/GetCardAssessment?employeeId=' + employeeId + '&cardId=' + cardId;

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

function getCardHistory(cardId, employeeId) {
    var url = '/Card/GetCardHistory?employeeId=' + employeeId + '&cardId=' + cardId;

    $.get(url)
        .done(function (data) {
            $('.modal-body').html(data);
        })
        .fail(function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        });
}

function updateCard(event) {
    event.preventDefault(); // Предотвращаем отправку формы по умолчанию

    var formData = $(event.target).closest('form').serialize(); // Получаем данные формы

    $.post({
        url: '/Card/UpdateCard',
        data: formData,
        success: function (response) {
            var employeeId = $(event.target).closest('form').find('[name="UserId"]').val();
            var cardId = $(event.target).closest('form').find('[name="CardId"]').val();

            var url = '/Card/GetCardDetails?employeeId=' + employeeId + '&cardId=' + cardId;

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

function moveCard(event) {
    event.preventDefault(); // Предотвращаем отправку формы по умолчанию

    var formData = $(event.target).closest('form').serialize(); // Получаем данные формы

    $.post({
        url: '/Card/MoveCard',
        data: formData,
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        }
    });
}

function rejectCard(event) {
    event.preventDefault(); // Предотвращаем отправку формы по умолчанию

    var formData = $(event.target).closest('form').serialize(); // Получаем данные формы

    $.post({
        url: '/Card/RejectCard',
        data: formData,
        success: function (response) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            var errorMessage = 'Error: ' + status + ' - ' + error;
            alert(errorMessage);
            console.error(errorMessage);
        }
    });
}

function closeCard(elem) {
    window.location.reload();
}

function closePopupBg(elem) {
    elem.remove()
}