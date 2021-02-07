function uploadFiles(ele, fieldId, fieldName, fileModelType) {
    var parent = $(ele).parent();
    var thumbnailPanel = $(parent).find('.thumbnail-panel');
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
                let thumbnailHtml = $(thumbnailPanel).html();
                let index = $('#' + fieldId + ' .data-panel').length;
                for (i = 0; i < response.length; ++i) {
                    d = response[i]; 
                    dataPanel = createSubmitDataFileds(d, fieldId, fieldName, index, fileModelType);
                    $('#' + fieldId).append(dataPanel);
                    ++index;

                    thumbnailHtml += "\
                    <div class='thumbnail ' style=''>\
                        <div style='background-image: url(\"" + d.thumbnail + "\");'></div>\
                        <div>" + d.originalFileName + "</div>\
                    </div>";
                }
                $(thumbnailPanel).html(thumbnailHtml);
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

    var dataPanel = $('<div class="data-panel">');

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

    dataPanel.append($('<input type="hidden", class="file-ref-data">', {
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

function createHiddenFileDataField(id, name, value) {
    value
}