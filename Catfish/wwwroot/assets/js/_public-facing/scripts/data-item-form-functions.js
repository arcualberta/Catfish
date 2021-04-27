//Setting up a gloab variable to track whether data form(s) were modified
var dataFormdModified = false;

$(document).ready(function () {

    //Encoding inner-HTML of composite field templates into base 64 strings
    $.each($("form .composite-field-template"), function (idx, template) {

        if ($(template).attr("data-encoded") !== "true") {
            let html = $(template).html();
            let encodedHtml = btoa(html);
            $(template).html(encodedHtml);
            $(template).attr("data-encoded", "true");
        }
    });

    //Encoding inner-HTML of table field templates into base 64 strings
    $.each($("form .tf-template"), function (idx, template) {
        if ($(template).attr("data-encoded") !== "true") {
            let html = $(template).html();
            let encodedHtml = btoa(html);
            $(template).html(encodedHtml);
            $(template).attr("data-encoded", "true");
        }
    });

    setInterval(function () {
        if (dataFormdModified) {
            autoSaveDataItemForms();
            dataFormdModified = false;
        }
    }, 5 * 60000); //execute every 5 minutes.

    /*
    $('.data-item-form').validate(
        {
            rules:
            {
            //    Color: { required: true }
            },
            messages:
            {
            //    Color:
            //    {
            //        required: "Please select a Color<br/>"
            //    }
            },
            errorPlacement: function (error, element) {
                if (element.is(":radio") || element.is(":checkbox")) {
                    error.appendTo(element.parents('.field-value'));
                }
                else { // This is the default behavior 
                    error.insertAfter(element);
                }
            }
        });
    */

});

function removeRequiredAttribute(form) {
    $(form).find("input,select,textarea").removeAttr("required");
}

function autoSaveDataItemForms() {
        let forms = $(".data-item-form");
        $.each(forms, function (index, form) {
            autoSaveForm(form);
        });
}

function autoSaveForm(form) {

    //Reguar expression for matching the variable name prefix up to the item's properties.
    var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\./;
    var name;
    var values = {};

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


    let url = "/api/items/AutoSave";

    /* Send the data using post with element id name and name2*/
    var posting = $.post(url, values);
    var message = "";

    posting.done(function (data) {
        if (data.success) {
            //message = data.message ? data.message : "Auto-save succesful";
        }
        else {
            //message = data.message ? data.message : "Auto-save failed";
        }
    });
}

function createGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
} 
