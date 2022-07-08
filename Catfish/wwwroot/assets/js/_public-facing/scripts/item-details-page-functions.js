function submitDetailsWorkflowForm(suffix, successMessage, entityId, currentStatus, nextStatus, buttonName) {
    $("#item-details-result-message_" + suffix).hide();

    $("#submissionDetailsForm_" + suffix).submit(function (event) {
        /* stop form from submitting normally */
        event.preventDefault();

        //Reguar expression for matching the variable name prefix up to the item's properties.
        var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\./;
        var values = {};
        var form = $('#submissionDetailsForm_' + suffix);

        //Handling text areas and input elements EXCLUDING checkboxes, radio buttons, and drop-down (select) menus
        
        values["entityId"] = entityId;
        values["buttonName"] = buttonName;
        values["currentStatus"] = currentStatus;
        values["status"] = nextStatus;

        /* get the action attribute from the <form action=""> element */
        var $form = $(this),
            url = $form.attr('action');
        /* Send the data using post with element id name and name2*/
        var posting = $.post(url, values);
        var message = "Test Message";
        posting.done(function (data) {
            $("#item-details-result-message_" + suffix).empty();
            $('.modal-backdrop').remove();
            console.log(data.success);
            if (data.success) {
                //  $(".submission-result-message").addClass("alert alert-success");
                message = successMessage !== "" ? successMessage : data.message;
                $("#item-details-result-message_" + suffix).append("<div class='alert alert-success' ></div>");

                $("#submissionDetailsForm_" + suffix).hide();//[0].reset();
            }
            else {

                $("#item-details-result-message_" + suffix).append("<div class='alert alert-danger' ></div>");
                message = data.message;
            }



            $("#item-details-result-message_" + suffix + " div").text(message);
            $("#item-details-result-message_" + suffix).show();
        });

    });
}