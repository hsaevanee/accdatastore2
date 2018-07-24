$(document).ready(function () {
    $('#listSelectedSchoolname').multiselect({
        buttonWidth: '80%',
        nonSelectedText: '--- Please Select School ---',
        maxHeight: 200,
        enableFiltering: true,
        numberDisplayed: 2,
        onDropdownShow: function (event) {
            var selectedOptions = $('#listSelectedSchoolname option:selected');
            if (selectedOptions.length == 0) {
                $('#buttonGetData').prop('disabled', true);
            } else if (selectedOptions.length >= 2) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $('#listSelectedSchoolname option').filter(function () {
                    return !$(this).is(':selected');
                });

                var dropdown = $('listSelectedSchoolname').siblings('.multiselect-container');
                nonSelectedOptions.each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                var dropdown = $('#listSelectedSchoolname').siblings('.multiselect-container');
                $('#listSelectedSchoolname option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
                $('#buttonGetData').prop('disabled', false);
            }
        },
        onChange: function (option, checked) {
            // Get selected options.
            var selectedOptions = $('#listSelectedSchoolname option:selected');
            if (selectedOptions.length == 0) {
                $('#buttonGetData').prop('disabled', true);
            } else if (selectedOptions.length >= 2) {
                // Disable all other checkboxes.
                var nonSelectedOptions = $('#listSelectedSchoolname option').filter(function () {
                    return !$(this).is(':selected');
                });

                var dropdown = $('listSelectedSchoolname').siblings('.multiselect-container');
                nonSelectedOptions.each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', true);
                    input.parent('li').addClass('disabled');
                });
            }
            else {
                // Enable all checkboxes.
                var dropdown = $('#listSelectedSchoolname').siblings('.multiselect-container');
                $('#listSelectedSchoolname option').each(function () {
                    var input = $('input[value="' + $(this).val() + '"]');
                    input.prop('disabled', false);
                    input.parent('li').addClass('disabled');
                });
                $('#buttonGetData').prop('disabled', false);
            }
        }
    });
    $('#listSelectedSchoolname').multiselect('select', selectid);

    $('#listSelectedSchoolname').multiselect('refresh');
   

});