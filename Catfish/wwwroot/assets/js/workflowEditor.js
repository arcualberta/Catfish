$(function () {
    $(".fa-save").hide();
    $(".fa-window-close").hide();
   
});
function setTableColEditable(textFieldId) {  //this method enabled row to be editable
   
    $("#" + textFieldId).find("span").each(function (index, c) {
       
      $(this).prop("contenteditable", true);
        $(this).addClass("editableText");
        $("#btnEdit_" + textFieldId).hide();
        $("#btnSave_" + textFieldId).show();
        $("#btnCancel_" + textFieldId).show();
     
    });
}

function saveEditedText(templateId, dataItemId, fieldId, textFieldId) {

    let editedText = $("#" + textFieldId + " span").text();
   // alert(editedText);
    let url = "/manager/api/Workflow/SaveText";
    var data = {};
    data.TemplateId = templateId;
    data.DataItemId = dataItemId;
    data.FieldId = fieldId;
    data.TextFieldId = textFieldId;
    data.textValue = editedText;
   
    $.ajax({
            type: 'POST',
            url: url,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(data),
       // data: { 'templateId':templateId, 'dataItemId':dataItemId, 'fieldId':fieldId, 'textId':textFieldId, 'value':editedText },
            success: function (data) {
                location.reload();
            },
            error: function ( error) {
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
function cancelEditedText(textFieldId) {

    let editedText = $("#" + textFieldId + " span").text();
    alert(editedText);

    $("#btnEdit_" + textFieldId).show();
    $("#btnSave_" + textFieldId).hide();
    $("#btnCancel_" + textFieldId).hide();
}