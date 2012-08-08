
$('span.field-validation-valid, span.field-validation-error').each(function () {
    $(this).addClass('help-inline');
});

$('form').submit(function () {
    if ($(this).valid()) {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length == 0) {
                $(this).removeClass('error');
            }
        });
    }
    else {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('error');
            }
        });
    }
});

$('form').each(function () {
    $(this).find('div.control-group').each(function () {
        if ($(this).find('span.field-validation-error').length > 0) {
            $(this).addClass('error');
        }
    });
});

// setup defaults for $.validator outside domReady handler
$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest(".clearfix").addClass("error");
    },
    unhighlight: function (element) {
        $(element).closest(".clearfix").removeClass("error");
    }
});

// code to fix ValidationSummary so that it appears styled
$('.validation-summary-valid, .validation-summary-errors')
                .addClass('alert alert-error')
                .prepend('<p><strong>Validation Exceptions:</strong></p>');