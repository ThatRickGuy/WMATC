$(document).ready(function () {
    function getDatemmddyyyy(value) {
        if (value == null)
            return null;
        return $.datepicker.parseDate("mm/dd/yy", value);
    }
    $('.date').each(function () {
        var minDate = getDatemmddyyyy($(this).data("val-rangedate-min"));
        var maxDate = getDatemmddyyyy($(this).data("val-rangedate-max"));
        $(this).datepicker({
            dateFormat: "mm/dd/yy",  // hard-coding US date format, but could embed this as an attribute server-side (based on the current culture)
            minDate: minDate,
            maxDate: maxDate
        });
    });
});