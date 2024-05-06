
let showReportBtn = {
    content: 'Показать',
    className: 'custom-button-classname',
    onClick: (dp) => {
        GetReportFromDatepicker()
    }
}

new AirDatepicker('#datepickerReportOnlyMonthPeriod', {
    range: true,
    view: 'months',
    minView: 'months',
    dateFormat: 'MMMM yyyy',
    multipleDatesSeparator: ' - ',
    buttons: [showReportBtn, 'clear'],
});

function GetReportFromDatepicker(param) {
    let viewDateInterval = document.querySelector('.input_datepicker').value

    $.post('GetReportViewComponent', { viewDateInterval: viewDateInterval },
        function (result) {
            $("#main_container_content").html(result);
        });

}

function SaveReportFromDatepicker(viewDateInterval, param) {
    $.post("SaveReport", { viewDateInterval: viewDateInterval }, function (result) {
            window.open(`/attachments/${result}`, "_blank");
        })
            .done(function () {
                popupAlert("Отчет успешно скачан!", false);
            })
            .fail(function () {
                popupAlert(result.responseJSON, false);
            })
}