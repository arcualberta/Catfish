function enableInlineEditing(textFieldId) {
    //This method enables the read-only view of Catfish.Core.Models.Contents.Text
    //field editable

    let span = $("#" + textFieldId).find("span.text-view");
    $(span).attr("data-original-val", $(span).html());
    $(span).prop("contenteditable", true);
    $(span).addClass("editableText");
    $("#btnEdit_" + textFieldId).hide();
    $("#btnSave_" + textFieldId).show();
    $("#btnCancel_" + textFieldId).show();
}

function cancelInlineEditing(textFieldId) {

    //This methods cancels the editing and restores the original value
    let editedText = "Cancel your changes: " + $("#" + textFieldId + " span").text();
    if (confirm(editedText)) {
        let span = $("#" + textFieldId).find("span.text-view");
        $(span).html($(span).attr("data-original-val")); // restore the original value
        $(span).prop("contenteditable", false);
        $(span).removeClass("editableText");
        $("#btnEdit_" + textFieldId).show();
        $("#btnSave_" + textFieldId).hide();
        $("#btnCancel_" + textFieldId).hide();
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
    $("#" + textFieldId + " span").prop("contenteditable", false); //remove content editable
    $("#" + textFieldId + " span").removeClass("editableText");
    $("#btnEdit_" + textFieldId).show();
    $("#btnSave_" + textFieldId).hide();
    $("#btnCancel_" + textFieldId).hide();
}
