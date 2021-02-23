

function submitEditWorkflowForm(status, button, suffix, successMessage) {
    $("#submission-edit-result-message_" + suffix).hide();


    $("#submissionEditForm_" + suffix).submit(function (event) {
        /* stop form from submitting normally */
        event.preventDefault();

        //Reguar expression for matching the variable name prefix up to the item's properties.
        var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\./;
        var name;
        var values = {};
        var form = $('#submissionEditForm_' + suffix);

        //$(form).valid();

        //Handling text areas and input elements EXCLUDING checkboxes, radio buttons, and drop-down (select) menus
        $.each($('input, textarea', form).not('input[type=checkbox], input[type=radio], select').serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "");
            values[name] = field.value;
        });

        //Handling checkbox sets
        $.each($('input[type=checkbox]:checked', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "");
            values[name] = true;
        });

        //Handling radio-button fields and drop-down menus
        $.each($('input[type=radio]:checked, select', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "") + ".SelectedOptionGuids";
            values[name] = field.value ? [field.value] : [];
        });

        //===================================end processed files ======================================

        values["actionButton"] = button;
        values["status"] = status;


        /* get the action attribute from the <form action=""> element */
        var $form = $(this),
            url = $form.attr('action');
        /* Send the data using post with element id name and name2*/
        var posting = $.post(url, values);
        var message = "";

        posting.done(function (data) {
            $("#submission-edit-result-message_" + suffix).empty();
            $('.modal-backdrop').remove();
            if (data.success) {
                //  $(".submission-result-message").addClass("alert alert-success");
                message = successMessage !== "" ? successMessage : data.message;
                $("#submission-edit-result-message_" + suffix).append("<div class='alert alert-success' ></div>");

                $("#submissionEditForm_" + suffix).hide();//[0].reset();
            }
            else {

                $("#submission-edit-result-message_" + suffix).append("<div class='alert alert-danger' ></div>");
                message = data.message;
            }

            $("#submission-edit-result-message_" + suffix + " div").text(message);
            $("#submission-edit-result-message_" + suffix).show();
        });

    });
}