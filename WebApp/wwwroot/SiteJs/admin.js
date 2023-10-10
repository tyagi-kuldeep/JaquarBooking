var $body = $("body");
$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); },
    ajaxComplete: function () { $body.removeClass("loading"); },
    ajaxError: function () { $body.removeClass("loading"); },
    ajaxSuccess: function () { $body.removeClass("loading"); }
});
//toaster notification https://codeseven.github.io/toastr/demo.html
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": true,
    "preventOpenDuplicates": true,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
function Notification(type, title, msg) {
    if (title == '' && msg == '') return false;
    if (type == "warning")
        toastr.warning(msg, title, toastr.options);
    else if (type == "success")
        toastr.success(msg, title, toastr.options);
    else if (type == "error")
        toastr.error(msg, title, toastr.options);
    else if (type == "info")
        toastr.info(msg, title, toastr.options);
}
function Validate(btnname) {
    $("#CallerBtnName").val(btnname);
}