
$(document).ready(function () {
    $(".launch-modal").click(function () {
        $("#submissionModal").modal({
            backdrop: 'static'
        });
    });  
});

function submitWorkflowForm(suffix, successMessage) {
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
            values[name] = [field.value];
        });


        //========================= process attached files first =====================================
        //get the attachmentFiles -- MR Jan 23 2021 attemp to attach file upload
        ////////var Files = new FormData();
        //////////get all corresponde of the hiiden field for each of the AttachmentField on the form
        ////////var AttachmentHidden = $("input[name$='FileNames']");
        ////////var hiddenFiles = "";
        ////////$.each($("input[type=file]"), function (i, field) {

        ////////    if ($(field)[0].files.length > 0) {
        ////////        let file = $(field)[0].files[0];
        ////////        let fname = field.id + "_" + file.name;
        ////////        Files.append("files", file, fname);

        ////////         //set the hidden field value
        ////////        hiddenFiles = fname;
        ////////        $(AttachmentHidden[i]).val(fname);
        ////////    }
           
           
        ////////});

        ////////let savedFiles="";
        ////////let urlstr = "/api/items/SaveFiles";
        //////////should be called only if there's (are) attachment file(s).
        ////////if (!!Files.entries().next().value) {
        ////////    $.ajax({
        ////////        type: "POST",
        ////////        url: "/api/items/SaveFiles",
        ////////        beforeSend: function (xhr) {
        ////////            xhr.setRequestHeader("XSRF-TOKEN",
        ////////                $('input:hidden[name="__RequestVerificationToken"]').val());
        ////////        },
        ////////        data: Files,
        ////////        contentType: false,
        ////////        processData: false,
        ////////        success: function (response) {
        ////////            alert("succes " + response);
        ////////            if (response.includes("|")) {
        ////////                //contain more than 1 field attachment that has file
        ////////                let elms = response.split("|");
                       
        ////////                $.each(AttachmentHidden, function (i, el) {
        ////////                    let names = elm.split(":");
        ////////                    savedFiles = "";
        ////////                    $.each(elms, function (i, elm) {//$.each(AttachmentHidden, function (i, el) {
        ////////                        if (el.id.includes(names[0])) {
                                   
        ////////                            savedFiles += names[1] + "|";
        ////////                        }
        ////////                    });
        ////////                    //update the hidden value of the field
        ////////                    $("#" + el.id).val(savedFiles);
        ////////                    name = el.name.replace(prefix, "");
        ////////                    values[name] = savedFiles;
        ////////                });
        ////////            }
        ////////            else {
        ////////                //only single attachment field
        ////////                let names = response.split(":"); //[0] + Field_4 ==> Field index and [1]: the fileName
        ////////                //update the correspondense hidden field
        ////////                $.each(AttachmentHidden, function (i, el) {
        ////////                    if (el.id.includes(names[0])) {
        ////////                        //update the hidden value of the field
        ////////                        $("#" + el.id).val(names[1]);
        ////////                        savedFiles = names[1];
        ////////                        name = el.name.replace(prefix, "");
        ////////                        values[name] = names[1];
        ////////                    }
        ////////                });

        ////////            }


        ////////        },
        ////////        error: function (error) {
        ////////            $("#submission-result-message_" + suffix + " div").text("Error try saving file(s)");
        ////////            $("#submission-result-message_" + suffix).show();
        ////////            return;
        ////////        },
        ////////        async: false
        ////////    });
        ////////}

        ////////values["fileNames"] = savedFiles;
        //===================================end processed files ======================================

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

function addDataItem(templateId, min, max) {
   
    let numItems = $(".composite-field-child").length; // number of item in the list
    if (numItems == max) {
        alert("Sorry, you can't add more item into the list.");
        return false;
    }

    let chilItemId = "composite-field-child-" + numItems;
    let dataItm = $("#" + templateId).clone().addClass("row composite-field-child").removeAttr('style').attr("id", chilItemId);
    let newGuid = createGuid();

    //replace names/ids of input field
    $(dataItm).find("input").map(function () {
        let divId = this.id;
        //replace "ChilTemplate" => Children_@numItems
        divId = divId.replace('ChildTemplate', 'Children_' + numItems +"_");
        $(this).attr("id", divId);

        let divName = this.name;
        divName = divName.replace('ChildTemplate', 'Children[' + numItems + ']');
        $(this).attr("name", divName);
    });

     //replace id of child div
    $(dataItm).find("div").map(function () {
        let divId = this.id;
        //replace "ChilTemplate" => Children_@numItems
        divId = divId.replace('ChildTemplate', 'Children_' + numItems);
        $(this).attr("id", divId);
    });

    //insert removeDataItem()
    $(dataItm).find("span.fa-trash").map(function () {
        let deleteFunc = "removeDataItem('" + chilItemId + "','" + min + "'); return false;";
        $(this).attr("onclick", deleteFunc);
    });

    let newItm = dataItm[0];
    $("#addNewdataItem").before(newItm);

    
}

function removeDataItem(dataItemDivId, min) {
    let numItems = $(".composite-field-child").length;
    if (numItems == min) {
        alert("You can't remove this item.")
        return false;
    }

    if (numItems > parseInt(min, 10)) {
        $("#" + dataItemDivId).remove();
    }
}

function createGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
} 
