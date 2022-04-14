
function submitWorkflowForm(stateId, button, postActionId, suffix, successMessage) {
    $("#submission-result-message_" + suffix).hide();

    
    $("#submissionForm_" + suffix).submit(function (event) {
        /* stop form from submitting normally */
        event.preventDefault();
        var groupId = null;
        var e = document.getElementById("groupId");
        if (e !== null) {
            groupId = (e.nodeName?.toLowerCase() === 'input')
                ? e.value
                : e.options[e.selectedIndex].value;
        }
        
        //Reguar expression for matching the variable name prefix up to the item's properties.
        var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\./;
        var name;
        var values = {};
        var form = $('#submissionForm_' + suffix);

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

        values["groupId"] = groupId;
        values["actionButton"] = button;
        values["stateId"] = stateId;
        values["postActionId"] = postActionId;

       
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
                //message = successMessage !== "" ? successMessage : data.message;
                message = data.message;
                $("#submission-result-message_" + suffix).append("<div class='alert alert-success' ></div>");
            
                $("#submissionForm_" + suffix).hide();//[0].reset();
            }
            else {

                $("#submission-result-message_" + suffix).append("<div class='alert alert-danger' ></div>");
                message = data.message;
            }

            $("#submission-result-message_" + suffix + " div").html(message);
            $("#submission-result-message_" + suffix).show();
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
function createGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
} 


$(document).ready(function () {

    // Safari 3.0+ "[object HTMLElementConstructor]" 
    var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && window['safari'].pushNotification));

    // Internet Explorer 6-11
    var isIE = /*@cc_on!@*/false || !!document.documentMode;

    // Edge 20+
    var isEdge = !isIE && !!window.StyleMedia;

    if (isSafari || isIE || isEdge) //IF IE or SAFARI
    {
        $("input[type='date']").removeClass('hasDatepicker').datepicker({ onSelect: function () { $(".ui-datepicker a").removeAttr("href"); } });
        //alert("safari, IE or Edge");
        $("input[type='date']").datepicker(
            {
                dateFormat: 'yy-mm-dd',
                onSelect: function () {
                    selectedDate = $(this).datepicker.formatDate("yy-mm-dd", $(this).datepicker('getDate'));
                }
            }
        );

    }
});

