function uploadFiles(ele, fieldId, fieldName, fileModelType) {
    var parent = $(ele).parent();
    var fileContainer = $(parent).find('.file-container');
    var messageBlock = $(parent).find('.message-block');
    var Files = new FormData();

    $.each(ele.files, function (i, file) {
        Files.append("files", file);
    });

    let uploadApiUrl = "/api/files/";
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
                let index = $(fileContainer).find('.file-entry').length;
                for (i = 0; i < response.length; ++i) {
                    let d = response[i];
                    let entryId = 'file-entry-' + d.id;
                    let fileEntry = $('<div>', {
                        id: entryId,
                        class: 'file-entry'
                    });
                    $(fileContainer).append(fileEntry);

                    let thumbnail = $("\
                    <div class='thumbnail ' style='position: relative;'>\
                        <button type='button' class='btn btn-danger btn-circle btn-xm delete-btn' onclick='deleteFile(\"" + entryId + "\", \"" + fieldId + "\", \"" + fieldName + "\");'>X</button> \
                        <div class='icon' style='background-image: url(\"" + d.thumbnail + "\");' class='text-right'> \
                        </div>\
                        <div style='' class='label text-center'>" + d.originalFileName + "</div>\
                        <div style='' class='label-active text-center'>" + d.originalFileName + "</div>\
                    </div>");
                    $(fileEntry).append(thumbnail);

                    let dataPanel = createSubmitDataFileds(d, fieldId, fieldName, index, fileModelType);
                    $(fileEntry).append(dataPanel);


                    ++index;
                }
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

function createSubmitDataFileds(data, fieldId, fieldName, index, fileModelType) {
    var idPrefix = fieldId + "_Files_" + index + "__";
    var namePrefix = fieldName + ".Files[" + index + "].";
    
    var dataPanel = $('<div class="data-panel panel-' + index + '">');

    dataPanel.append($('<input>', {
        id: idPrefix + 'Id',
        name: namePrefix + 'Id',
        value: data.id,
        type: "hidden",
        class: "file-ref-data"
    }));

    dataPanel.append($('<input>', {
        id: idPrefix + 'ModelType',
        name: namePrefix + 'ModelType',
        value: fileModelType,
        type: "hidden",
        class: "file-ref-data"
    }));

    dataPanel.append($('<input>', {
        id: idPrefix + 'FileName',
        name: namePrefix + 'FileName',
        value: data.fileName,
        type: "hidden",
        class: "file-ref-data"
    }));

    dataPanel.append($('<input>', {
        id: idPrefix + 'OriginalFileName',
        name: namePrefix + 'OriginalFileName',
        value: data.originalFileName,
        type: "hidden",
        class: "file-ref-data"
   }));

    dataPanel.append($('<input>', {
        id: idPrefix + 'Thumbnail',
        name: namePrefix + 'Thumbnail',
        value: data.thumbnail,
        type: "hidden",
        class: "file-ref-data"
   }));

    dataPanel.append($('<input>', {
        id: idPrefix + 'ContentType',
        name: namePrefix + 'ContentType',
        value: data.contentType,
        type: "hidden",
        class: "file-ref-data"
    }));

    dataPanel.append($('<input>', {
        id: idPrefix + 'Size',
        name: namePrefix + 'Size',
        value: data.size,
        type: "hidden",
        class: "file-ref-data"
   }));


    return dataPanel;    
}

function deleteFile(entryId, fieldId, fieldName) {

    //Deleting the entry, including its interface elements and the data values
    $("#" + entryId).remove();

    //Reindexing the entry list
    let entries = $("#" + fieldId).find('.file-entry');
    let idPrefix = fieldId + "_Files_";
    let namePrefix = fieldName + ".Files[";
    for (n = 0; n < entries.length; ++n) {
        let entry = entries[n];
        let inputs = $(entry).find('input');
        for (k = 0; k < inputs.length; ++k) {
            ele = inputs[k];
            if (ele.id) {
                if (ele.id.startsWith(idPrefix)) {
                    let idSuffix = ele.id.substr(idPrefix.length);
                    idSuffix = idSuffix.substr(idSuffix.indexOf("__"));
                    ele.id = idPrefix + n + idSuffix;
                }
            }

            if (ele.name) {
                if (ele.name.startsWith(namePrefix)) {
                    let nameSuffix = ele.name.substr(namePrefix.length);
                    nameSuffix = nameSuffix.substr(nameSuffix.indexOf("]"));
                    ele.name = namePrefix + n + nameSuffix;
                }
            }
        }
    }
}