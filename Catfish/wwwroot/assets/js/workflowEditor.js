$(function () {
    $(".fa-save").hide();
    $(".fa-window-close").hide();
});
function setTableColEditable(fieldId) {  //this method enabled row to be editable
   
    $("#" + fieldId).find("span").each(function (index, c) {
       
      $(this).attr("contenteditable", true);
        $(this).addClass("editableText");
        $("#btnEdit_" + fieldId).hide();
        $("#btnSave_" + fieldId).show();
        $("#btnCancel_" + fieldId).show();
     
    });
}

function saveEditedText(fieldId) {

    let editedText = $("#" + fieldId + " span").text();
    alert(editedText);


    $("#" + fieldId + " span").prop("contenteditable", false);
    $("#" + fieldId + " span").removeClass("editableText");
    $("#btnEdit_" + fieldId).show();
    $("#btnSave_" + fieldId).hide();
    $("#btnCancel_" + fieldId).hide();
}
function cancelEditedText(fieldId) {

    let editedText = $("#" + fieldId + " span").text();
    alert(editedText);

    $("#btnEdit_" + fieldId).show();
    $("#btnSave_" + fieldId).hide();
    $("#btnCancel_" + fieldId).hide();
}