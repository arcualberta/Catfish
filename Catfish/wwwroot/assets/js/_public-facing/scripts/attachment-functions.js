
function uploadFiles(fieldId, ele) {
    var parent = $(ele).parent();
    var thumbnailPanel = $(parent).find('.thumbnail-panel');
    var messageBlock = $(parent).find('.message-block');
    var Files = new FormData();

    $.each(ele.files, function (i, file) {
        Files.append("files", file);
    });


    let uploadApiUrl = "/api/files/" + fieldId;
    if (Files.entries().next().value) {
        $.ajax({
            type: "POST",
            url: uploadApiUrl,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: Files,
            contentType: false,
            processData: false,
            success: function (response) {
                $(thumbnailPanel).html(response);
                let thumbnailHtml = "";
                response.forEach(fileRef, function (i, fileRef) {
                    //thumbnailHtml = "<div class='col-md-2"

                });
                //if (response.includes("|")) {
                //    //contain more than 1 field attachment that has file
                //    let elms = response.split("|");

                //    $.each(AttachmentHidden, function (i, el) {
                //        let names = elm.split(":");
                //        savedFiles = "";
                //        $.each(elms, function (i, elm) {//$.each(AttachmentHidden, function (i, el) {
                //            if (el.id.includes(names[0])) {

                //                savedFiles += names[1] + "|";
                //            }
                //        });
                //        //update the hidden value of the field
                //        $("#" + el.id).val(savedFiles);
                //        name = el.name.replace(prefix, "");
                //        values[name] = savedFiles;
                //    });
                //}
                //else {
                //    //only single attachment field
                //    let names = response.split(":"); //[0] + Field_4 ==> Field index and [1]: the fileName
                //    //update the correspondense hidden field
                //    $.each(AttachmentHidden, function (i, el) {
                //        if (el.id.includes(names[0])) {
                //            //update the hidden value of the field
                //            $("#" + el.id).val(names[1]);
                //            savedFiles = names[1];
                //            name = el.name.replace(prefix, "");
                //            values[name] = names[1];
                //        }
                //    });

                //}

            },
            error: function (error) {
                $(messageBlock).html("File uploading failed");
                $(messageBlock).show();
                return;
            },
            async: false
        });
    };
    
}