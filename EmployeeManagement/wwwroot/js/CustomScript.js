$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});

function showFilePath(customFileLabel) {
    var fileName = $(this).val().split("\\").pop();
    var elem = document.getElementById('customFileLabel');
    $(this).next(elem).html(fileName);
}