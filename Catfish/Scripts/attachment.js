function getThumbnailDivId(fileGuidName) {
    return fileGuidName.substr(0, fileGuidName.length - 4);
}

function deleteLinkedFile(fileGuidName, fileGuidListFieldId, visibleFileName, containerId) {
    if (confirm("Delete the file " + visibleFileName + "?") == false)
        return;
    var eleId = getThumbnailDivId(fileGuidName);
    $(".thumbnailPanel ." + eleId).remove(); //removing all thumbnail occuraces of the file

    var guids = $("#" + fileGuidListFieldId).val().split("|");
    guids.splice($.inArray(fileGuidName, guids), 1);
    guids = guids.join("|");
    $("#" + fileGuidListFieldId).val(guids);
    $("#" + containerId + " .messageBox").text("");
    $("#" + containerId + " .messageBox").hide()
}

function deleteUnlinkedFile(fileGuid, deleteApiUrl, containerId, fileGuidListFieldId) {
    if (confirm("Delete file?") == false)
        return;

    var myFrm = new FormData();
    myFrm.append("guid", fileGuid);

    var oReg = new XMLHttpRequest();
    var stateChange = function (data) {
        if (oReg.readyState === 4) {
            //after successfull execute the function then it will execute what ever inside this if {}
            if (oReg.status === 200) {
                var guid = JSON.parse(data)[0];

                //removing the guid from the fileGuidList hidden field
                var fileGuidList = $("#" + fileGuidListFieldId).val();
                fileGuidList = fileGuidList.replace(guid, "")
                    .replace("||", "|")
                    .replace(/^\s*\|\s*/, '')
                    .replace(/\s*\|\s*$/, '');
                $("#" + fileGuidListFieldId).val(fileGuidList);

                var eleId = getThumbnailDivId(guid);
                $("#" + eleId).remove();
                $("#" + containerId + " .messageBox").hide()
            }
            else {
                //Error
                $("#" + containerId + " .messageBox").text("File deletion failed: " + oReg.statusText);
                $("#" + containerId + " .messageBox").show()
            }
        }
    };

    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
        oReg.onload = function (event) { stateChange(event.target.response); };
    } else {
        oReg.onreadystatechange = function (data) { stateChange(data.srcElement.responseText); };
    }

    deleteApiUrl = deleteApiUrl == "" ? Url.Action("DeleteCashedFile", "items", new { guid: fileGuid }) : deleteApiUrl;
    oReg.open('POST', deleteApiUrl);
    oReg.send(myFrm);
}

function updateFileListView(data, deleteApiUrl, containerId, fileGuidListFieldId) {
    data = JSON.parse(data);
    var thumbnailPanel = $("#" + containerId + " .thumbnailPanel")[0];
    var fileGuidList = $("#" + fileGuidListFieldId).val();
    for (var i = 0; i < data.length; ++i) {
        var d = data[i];
        var eleId = getThumbnailDivId(d.Guid);
        var ele = '<div class="fileThumbnail" id="' + eleId
            + '" > <div class="img" style="background-image:url('
            + d.ThumbnailUrl + ')" ></div>'
            + '<button class="glyphicon glyphicon-remove" onclick="deleteUnlinkedFile(\''
            + d.Guid + '\',\'' + deleteApiUrl + '\',\'' + containerId
            + '\',\'' + fileGuidListFieldId
            + '\'); return false;"></button>'
            + '<div class="label"><a href="' + d.Url + '" target="_blank">'
            + d.FileName + '</a></div></div>';

        if (fileGuidList.length > 0)
            fileGuidList = fileGuidList + "|" + d.Guid;
        else
            fileGuidList = d.Guid;

        $(thumbnailPanel).append(ele);
    }

    $("#" + fileGuidListFieldId).val(fileGuidList);
}

var showInProgress = function (containerId) {
    $("#" + containerId + " .progressBar").show();
    $("#" + containerId + " .uploadButton").attr('disabled', 'disabled');
    $("#" + containerId + " .uploadField").attr('disabled', 'disabled');
}

var hideInProgress = function (containerId) {
    $("#" + containerId + " .progressBar").hide();
    $("#" + containerId + " .uploadField").prop('disabled', false);
    $("#" + containerId + " .uploadButton").prop('disabled', false);
}


var stateChange = function (data, deleteApiUrl, containerId, fileGuidListFieldId, oReg, onSuccess, onError) {
    var resultFunction = null;

    //after successfull execute the function then it will execute what ever inside this if {}
    if (oReg.readyState === 4) {

        var messageBoxSelector = "#" + containerId + " .messageBox"

        if (oReg.status === 200) {
            // Updating the value of the hidden field which carries the ID of this 
            // FileUpload object in the page
            updateFileListView(data, deleteApiUrl, containerId, fileGuidListFieldId);
            $(messageBoxSelector).text("").hide();

            if (onSuccess) {
                resultFunction = onSuccess;
            }
        }
        else {
            // Error
            var errorMessage = "File upload failed: " + oReg.statusText;
            $(messageBoxSelector).text(errorMessage).show();

            if (onError) {
                resultFunction = onError;
            }
        }

        $("#" + containerId + " .uploadField").val("");

        hideInProgress(containerId);
    }

    if (resultFunction != null) {
        resultFunction();
    }
};


function uploadFile(containerId, uploadApiUrl, deleteApiUrl, fileGuidListFieldId) {

    var myFrm = new FormData();
    var uploadField = $("#" + containerId + " .uploadField")[0];

    for (var i = 0; i < uploadField.files.length; i++) {
        myFrm.append("inputFile" + i, uploadField.files[i]);
    }

    showInProgress(containerId);

    var oReg = new XMLHttpRequest();

    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
        oReg.onload = function (event) {
            stateChange(event.target.response,
                deleteApiUrl,
                containerId,
                fileGuidListFieldId,
                oReg);
        };
    } else {
        oReg.onreadystatechange = function (data) {
            stateChange(data.srcElement.responseText,
                deleteApiUrl,
                containerId,
                fileGuidListFieldId,
                oReg);
        };
    }


    oReg.open('POST', uploadApiUrl);
    oReg.send(myFrm);

}// END function uploadFile()

function uploadFileKo(containerId, uploadApiUrl, deleteApiUrl, fileGuidListFieldId, koModel) {
    var myFrm = new FormData();
    containerId = koModel.Guid()
    var uploadField = $("#" + containerId + " .uploadField")[0];

    console.log(uploadField)
    for (var i = 0; i < uploadField.files.length; i++) {
        myFrm.append("inputFile" + i, uploadField.files[i]);
    }

    showInProgress(containerId);

    var oReg = new XMLHttpRequest();

    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
        oReg.onload = function (event) {
            stateChangeKo(event.target.response,
                deleteApiUrl,
                containerId,
                fileGuidListFieldId,
                oReg,
                koModel);
        };
    } else {
        oReg.onreadystatechange = function (data) {
            stateChangeKo(data.srcElement.responseText,
                deleteApiUrl,
                containerId,
                fileGuidListFieldId,
                oReg,
                koModel);
        };
    }


    oReg.open('POST', uploadApiUrl);
    oReg.send(myFrm);

}// END function uploadFile

var stateChangeKo = function (data, deleteApiUrl, containerId, fileGuidListFieldId, oReg, koModel) {
    //after successfull execute the function then it will execute what ever inside this if {}
    console.log("changing state");
    if (oReg.readyState === 4) {

        var messageBoxSelector = "#" + containerId + " .messageBox"

        if (oReg.status === 200) {
            // Updating the value of the hidden field which carries the ID of this 
            // FileUpload object in the page
            console.log("updating");
            console.log(data);
            updateFileListViewKo(data, deleteApiUrl, containerId, fileGuidListFieldId, koModel);
            $(messageBoxSelector).text("").hide();
        }
        else {
            // Error
            var errorMessage = "File upload failed: " + oReg.statusText;
            $(messageBoxSelector).text(errorMessage).show();
        }

        $("#" + containerId + " .uploadField").val("");

        hideInProgress(containerId);
    }
};

function updateFileListViewKo(data, deleteApiUrl, containerId, fileGuidListFieldId, koModel) {
    data = JSON.parse(data);
    //var thumbnailPanel = $("#" + containerId + " .thumbnailPanel")[0];

    data.forEach(function (file) {
        console.log(file);
        koModel.Files.push(file);
    })

    //koModel.Files.push(data[0])

    //var fileGuidList = $("#" + fileGuidListFieldId).val();
    /*
    var fileGuidList = koModel.FieldFileGuids();

    for (var i = 0; i < data.length; ++i) {
        var d = data[i];
        var eleId = getThumbnailDivId(d.Guid);
        var ele = '<div class="fileThumbnail" id="' + eleId
            + '" > <div class="img" style="background-image:url('
            + d.Thumbnail + ')" ></div>'
            + '<button class="glyphicon glyphicon-remove" onclick="deleteUnlinkedFile(\''
            + d.Guid + '\',\'' + deleteApiUrl + '\',\'' + containerId
            + '\',\'' + fileGuidListFieldId
            + '\'); return false;"></button>'
            + '<div class="label"><a href="' + d.Url + '" target="_blank">'
            + d.FileName + '</a></div></div>';

        if (fileGuidList.length > 0)
            fileGuidList = fileGuidList + "|" + d.Guid;
        else
            fileGuidList = d.Guid;

        $(thumbnailPanel).append(ele);
    }

    console.log(koModel.FieldFileGuids());
    //$("#" + fileGuidListFieldId).val(fileGuidList);
    koModel.FieldFileGuids(fileGuidList)
    */
}