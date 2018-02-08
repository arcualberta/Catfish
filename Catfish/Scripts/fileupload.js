//function getThumbnailDivId(fileGuidName) {
//    return fileGuidName.substr(0, fileGuidName.length - 4);
//}


//function updateFileListView(data, deleteApiUrl, thumbnailPanelCssId, messageBoxCssId) {
//    data = JSON.parse(data, thumbnailPanelCssId);
//    for (var i = 0; i < data.length; ++i) {
//        var d = data[i];
//        var eleId = getThumbnailDivId(d.Guid);
//        //var eleId = d.Guid.substr(0, d.Guid.length - 4);
//        var ele = '<div class="fileThumbnail" id="' + eleId + '" > <img src="' + d.Thumbnail + '" alt="' + d.FileName + '" />' +
//            '<button class="glyphicon glyphicon-remove" onclick="deleteFile(\'' + d.Guid + '\',\'' + deleteApiUrl + '\',\'' + messageBoxCssId + '\'); return false;"></button>' +
//            '<div class="label"><a href="' + d.Url + '" target="_blank">' + d.FileName + '</a></div>' +
//            '</div>';

//        $(thumbnailPanelCssId).append(ele);
//    }
//}

//function uploadFile(itemId, uploadApiUrl, deleteApiUrl, uploadFieldCssId, uploadButtonCssId, progressBarCssId, messageBoxCssId, thumbnailPanelCssId, customFunction) {
//    var myFrm = new FormData();     //create a new form
//    myFrm.append("itemId", itemId);

//    var uploadField = $(uploadFieldCssId)[0];     //grab the FileUpload object

//    for (var i = 0; i < uploadField.files.length; i++) {
//        myFrm.append("inputFile" + i, uploadField.files[i]);
//    }

//    $(progressBarCssId).show();
//    $(uploadButtonCssId).attr('disabled', 'disabled');
//    $(uploadFieldCssId).attr('disabled', 'disabled');

//    var oReg = new XMLHttpRequest();

//    var stateChange = function (data) {
//        if (oReg.readyState === 4) {  //after successfull execute the function then it will execute what ever inside this if {}
//            if (oReg.status === 200) {
//                //Updating the value of the hidden field which carries the ID of this FileUpload object in the page
//                updateFileListView(data, deleteApiUrl, thumbnailPanelCssId, messageBoxCssId);
//                $(messageBoxCssId).text("");
//                if (customFunction != null)
//                    customFunction(data);
//                $(messageBoxCssId).hide()
//            }
//            else {
//                //Error
//                $(messageBoxCssId).text("File upload failed: " + oReg.statusText);
//                $(messageBoxCssId).show()
//            }
//            $(uploadFieldCssId).val("");
//            $(progressBarCssId).hide();
//            $(uploadButtonCssId).prop('disabled', false);
//            $(uploadFieldCssId).prop('disabled', false);
//        }
//    };

//    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
//        oReg.onload = function (event) { stateChange(event.target.response); };
//    } else {
//        oReg.onreadystatechange = function (data) { stateChange(data.srcElement.responseText); };
//    }
//    oReg.open('POST', uploadApiUrl);
//    oReg.send(myFrm);

//}// END function uploadFile()

//function deleteFile(guid, deleteApiUrl, messageBoxCssId) {
//    if (confirm("Delete file?") == false)
//        return;

//    var myFrm = new FormData();     //create a new form
//    myFrm.append("guid", guid);

//    var oReg = new XMLHttpRequest();
//    var stateChange = function (data) {
//        if (oReg.readyState === 4) {  //after successfull execute the function then it will execute what ever inside this if {}
//            if (oReg.status === 200) {
//                var guid = JSON.parse(data)[0];
//                var eleId = guid.substr(0, guid.length - 4)
//                $("#" + eleId).remove();
//                $(messageBoxCssId).hide()
//            }
//            else {
//                //Error
//                $(messageBoxCssId).text("File deletion failed: " + oReg.statusText);
//                $(messageBoxCssId).show()
//            }
//        }
//    };

//    if (navigator.userAgent.toLowerCase().indexOf('firefox') > -1) {
//        oReg.onload = function (event) { stateChange(event.target.response); };
//    } else {
//        oReg.onreadystatechange = function (data) { stateChange(data.srcElement.responseText); };
//    }

//    oReg.open('POST', deleteApiUrl);
//    oReg.send(myFrm);
//}