function addChildForm(stateMapping, suffix, successMessage) {
    $("#add-child-result-message_" + suffix).hide();


    $("#childForm_"+ suffix).submit(function (event) {
        /* stop form from submitting normally */
        event.preventDefault();

        //Reguar expression for matching the variable name prefix up to the item's properties.
        var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\.|^Child\./;
        var name;
        var values = {};
        var form = $('#childForm_' + suffix);

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

        values["stateMapping"] = stateMapping;


        /* get the action attribute from the <form action=""> element */
        var $form = $(this),
            url = $form.attr('action');
        /* Send the data using post with element id name and name2*/
        var posting = $.post(url, values);
        var message = "";

        posting.done(function (data) {
            $("#add-child-result-message_" + suffix).empty();
            $('.modal-backdrop').remove();
            if (data.success) {
                //  $(".submission-result-message").addClass("alert alert-success");
                message = successMessage !== "" ? successMessage : data.message;
                $("#add-child-result-message_" + suffix).append("<div class='alert alert-success' ></div>");

                $("#childForm_" + suffix).hide();//[0].reset();
            }
            else {

                $("#add-child-result-message_" + suffix).append("<div class='alert alert-danger' ></div>");
                message = data.message;
            }

            $("#add-child-result-message_" + suffix + " div").html(message);
            $("#add-child-result-message_" + suffix).show();
        });

    });
}
function validateEmail(element) {
    let val = $(element).val();
    let p = $(element).parent();
    let messageElement = $(p).find("span.validation-message");

    if (val && /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val) === false)
        $(messageElement).show();
    else
        $(messageElement).hide();
}

function countWords(fieldModelId) {

    let thisField = $("textarea[data-model-id='" + fieldModelId + "']");
    let maxwords = $(thisField).data('max-words');

    if (maxwords > 0) {
        let count = $(thisField).val().trim().split(' ');

        if (count.length > maxwords) {
            let thisVal = (count.slice(0, maxwords)).join(" ");
            $(thisField).val(thisVal);
            let message = "<span class='exceedWords' style='color: red;'>Maximum " + maxwords + " words have been reached! </span>";
            if ($("span.exceedWords").length == 0)
                $(thisField).after(message);
            else
                $("span.exceedWords").show();
            return;
        } else {
            //remove exceed message
            $("span.exceedWords").hide();
        }
    }
}