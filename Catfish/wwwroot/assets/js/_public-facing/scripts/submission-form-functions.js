function submitForm(suffix, successMessage) {
    var status;
    var buttonName;

    $("#submission-result-message_" + suffix).hide();

    $(document).on('click', "#Submit_" + suffix, function () {
        status = 'Submitted';
        buttonName = 'Submit';
        console.log('Status = ' + status)
    });
    $(document).on('click', "#Save_" + suffix, function () {
        status = 'Saved';
        buttonName = 'Submit';
        console.log('Status = ' + status)
    });

    $("#submissionForm_" + suffix).submit(function (event) {

        /* stop form from submitting normally */
        event.preventDefault();

        //Reguar expression for matching the variable name prefix up to the item's properties.
        var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\./;
        var name;
        var values = {};
        var form = $('#submissionForm_' + suffix);

        //Handling hidden fields
        $.each($('input[type=hidden]', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "");
            values[name] = field.value;
        });

       //Handling text fields
        $.each($('input[type=text]', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "");
            values[name] = field.value;
        });

        //Handling textarea fields
        $.each($('input[type=text], textarea', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "");
            values[name] = field.value;
        });

        //Handling radio-button fields
        $.each($('input[type=radio]:checked', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "") + ".Selected";
            values[name] = true;
        });

        //Handling drop-down fields
        $.each($('select', form).serializeArray(), function (i, field) {
            name = field.name.replace(prefix, "") + ".Selected";
            values[name] = true;
        });

        values["actionButton"] = buttonName;
        values["status"] = status;

        /* get the action attribute from the <form action=""> element */
        var $form = $(this),
            url = $form.attr('action');
        /* Send the data using post with element id name and name2*/
        var posting = $.post(url, values);
        var message = "";

        posting.done(function (data) {
            $("#submission-result-message_" + suffix).empty();
            $('.modal-backdrop').remove();
            if (data.success) {
                //  $(".submission-result-message").addClass("alert alert-success");
                message = successMessage !== "" ? successMessage : data.message;
                $("#submission-result-message_" + suffix).append("<div class='alert alert-success' ></div>");

                $("#submissionForm_" + suffix).hide();//[0].reset();
            }
            else {

                $("#submission-result-message_" + suffix).append("<div class='alert alert-danger' ></div>");
                message = data.message;
            }



            $("#submission-result-message_" + suffix + " div").text(message);
            $("#submission-result-message_" + suffix).show();
        });

    });
}