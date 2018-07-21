$(document).ready(function () {
    $('#target_color').change(function () {
        $('#target_color_hex').val($(this).val());
    });

    $('#target_color_hex').change(function () {
        $('#target_color').val($(this).val());
    });

    $('#fill_color').change(function () {
        $('#fill_color_hex').val($(this).val());
    });

    $('#fill_color_hex').change(function () {
        $('#fill_color').val($(this).val());
    });
});