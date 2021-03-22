function enableInlineEditing(textFieldId) {
    //This method enables the read-only view of Catfish.Core.Models.Contents.Text
    //field editable

    let span = $("#" + textFieldId).find("span.text-view");
   // $(span).attr("data-original-val", $(span).html());

    $("#hidden_original_text_" + textFieldId).val($(span).html());

    $(span).prop("contenteditable", true);
    $(span).addClass("editableText");
    $("#btnEdit_" + textFieldId).hide();
    $("#btnSave_" + textFieldId).show();
    $("#btnCancel_" + textFieldId).show();
}

function resetInlineEditingButtons(textFieldId) {
    // Reset all the buttons to initial state
    let span = $("#" + textFieldId).find("span.text-view");
    $(span).prop("contenteditable", false);
    $(span).removeClass("editableText");
    $("#btnEdit_" + textFieldId).show();
    $("#btnSave_" + textFieldId).hide();
    $("#btnCancel_" + textFieldId).hide();
}
function cancelInlineEditing(textFieldId) {

    //This methods cancels the editing and restores the original value
    let editedText = "Cancel your changes: " + $("#" + textFieldId + " span").text();
    if (confirm(editedText)) {
        let span = $("#" + textFieldId).find("span.text-view");
       // $(span).html($(span).attr("data-original-val")); // restore the original value
        let originalText = $("#hidden_original_text_" + textFieldId).val();
        $(span).html(originalText); // restore the original value

        resetInlineEditingButtons(textFieldId);
    }

}

function saveEditedText(templateId, dataItemId, fieldId, textFieldId, metadataSetId) {

    let editedText = $("#" + textFieldId + " span").html();
   // alert(editedText);
    let url = "/manager/api/Workflow/SaveText";
    var data = {};
    data.TemplateId = templateId;
    data.DataItemId = dataItemId;
    data.FieldId = fieldId;
    data.TextFieldId = textFieldId;
    data.MetadataSetId = metadataSetId;
    data.textValue = editedText;
   
    $.ajax({
            type: 'POST',
            url: url,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (data) {
                //location.reload();
                //TODO:some sort of indication to show the success of saving.
            },
            error: function (error) {
                alert("Encounter problems while saving data.")
            }
    });
    
    /*   hide/show button */
    resetInlineEditingButtons(textFieldId);
}


function addNewOptionValue(newOptId) {
    $("#newOptionVal_" + newOptId +"> div").show();

    $("#btnAddOption_" + newOptId).hide();
    $("#btnCancel_" + newOptId).show();
    $("#btnSave_" + newOptId).show();
}


function cancelAddOption(newOptId) {
    $("#newOptionVal_" + newOptId + "> div").hide();

    $("#btnAddOption_" + newOptId).show();
    $("#btnCancel_" + newOptId).hide();
    $("#btnSave_" + newOptId).hide();

}

function saveOptionVal(templateId,dataItemId,fieldId, newOptionFieldId) {

    let newValue = $("#input_" + newOptionFieldId).val();
    let lang = $("#select_" + newOptionFieldId + " option:selected").val();

    let url = "/manager/api/Workflow/AddOptionText";
    var data = {};
    data.TemplateId = templateId;
    data.DataItemId = dataItemId;
    data.FieldId = fieldId;
    data.TextFieldId = newOptionFieldId; //new GUID cr
    data.textValue = newValue;
    data.Language = lang;

    $.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(data),
        success: function (data) {
            location.reload();
        },
        error: function (error) {
            alert("Encounter problems while saving data.")
        }
    });
}

function removeOptionVal(templateId, dataItemId, fieldId, optionTextId, optionText) {

    //if (confirm("Are you sure you want to remove option: " + optionText + "?")) {
    //    let url = "/manager/api/Workflow/RemoveOptionText";
    //    var data = {};
    //    data.TemplateId = templateId;
    //    data.DataItemId = dataItemId;
    //    data.FieldId = fieldId;
    //    data.TextFieldId = optionTextId; //new GUID cr
    //    $.ajax({
    //        type: 'POST',
    //        url: url,
    //        contentType: 'application/json; charset=utf-8',
    //        dataType: 'json',
    //        data: JSON.stringify(data),
    //        success: function (data) {
    //            location.reload();
    //        },
    //        error: function (error) {
    //            alert("Encounter problems while saving data.")
    //        }
    //    });
    //}


    $.confirm({
        title: '<span class="fa fa-exclamation-triangle"></span> Delete Option',
        content: 'You are about to delete option "' + optionText + '". Are you sure?',
        buttons: {
            confirm: function () {
              
                let url = "/manager/api/Workflow/RemoveOptionText";
                var data = {};
                data.TemplateId = templateId;
                data.DataItemId = dataItemId;
                data.FieldId = fieldId;
                data.TextFieldId = optionTextId; //new GUID cr
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify(data),
                    success: function (result) {
                        if (result.message !== null) {
                            $.alert({
                                title: 'Info',
                                content: result.message,
                            });
                        } else {

                            location.reload();
                        }
                    },
                    error: function (error) {
                        alert("Encounter problems while saving data.")
                    }
                });
            },
            cancel: function () {
                //$.alert('Canceled!');
            }
        }
    });
   
}

//function displayNewOption(templateId, dataItemId, fieldId, newOptionId, newValue) {
//    let option = `<div id="` + fieldId + `" class="option-value">
                
//    <div id="` + newOptionId +`" class="inline-editor">

//        <i class="far fa-edit float-right" onclick="enableInlineEditing('` + newOptionId + `');" id="btnEdit_newOptionId"></i>
//        <i class="far fa-window-close float-right" style="display:none" onclick="cancelInlineEditing('` + newOptionId + `');" id="btnCancel_` +newOptionId + `"></i>
//        <i class="far fa-save float-right" style="display:none" onclick="saveEditedText('`+templateId+`','` +dataItemId + `','` + fieldId + `','` + newOptionId +` ', '00000000-0000-0000-0000-000000000000');" id="btnSave_` + newOptionId + `"></i>
//        <span class="text-view">Option 4</span>
//    </div></div>`;
//}


