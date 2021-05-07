﻿/*
 * Function for the free-text panel in views/shared/partial/_search.cshtml
 */
function searchText() {
    var searchText = $("input[name='searchTerm']").val();
    window.location.href = '/search?searchTerm=' + searchText;
    return false;
}

//conatinerId is MetadatasetId or DataItem id -- need to confirmed
function advanceSearch(containerId, scopeId, textInputClass, start) {
    let name;
   
    let selectedScope = $("#" + scopeId + " option:selected").val();
    let itemPerPage = $("#advanceSearchItemPerPage").val();
    let simpleSearchTerm = $("#simpleSearchTerm").val();
    let searchMode = $("[name='searchMode']:checked").val();

    let params = {};
    let constraints = [];

    //params["scope"] = selectedScope;
    //params["containerId"] = containerId;
   

    $("." + textInputClass).each(function (i, f) {
        let inputVal = $(f).val();
        let fieldId = $(f).attr("id");
        if (inputVal !== "") {

            //let _constraint = {
            //    Scope: selectedScope,
            //    ContainerId: containerId,
            //    FieldId: fieldId,
            //    SearchText: inputVal   
            //};

            let values = {};
            values["Scope"] = selectedScope;
            values["ContainerId"] = containerId;
            
            values["FieldId"] = fieldId;
           
            values["SearchText"]= inputVal;
            constraints.push( values );
           
        }

    });

    //params["fields"] = values;
    //$.ajax({

    //    url: '../../Search/AdvanceSearch/',
    //    type: 'GET',
    //    data: {
    //        'constraints': JSON.stringify(constraints),
    //        'itemPerPage':10
    //    },
    //    dataType: 'json',
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //        alert('success');
    //    },
    //    error: function (request, error) {
    //        alert("Failed");
    //    }
    //});

    let url = "../../Search/AdvanceSearch";

/* Send the data using post with element id name and name2*/

    params["constraints"] = JSON.stringify(constraints);
    params["itemPerPage"] = itemPerPage;
    params["start"] = start;
    params["simpleSearchTerm"] = simpleSearchTerm;
    params["searchMode"] = searchMode;
    var result = $.get(url, params);
    var message = "";

    result.done(function (data) {
        
       // alert("ok")
        $(".searchResults").show();
        var tbody = $(".searchResults table tbody");
        $(tbody).empty();

        let currentPageInfo = `${data.offset + 1} to ${data.offset + data.resultEntries.length} of ${data.totalMatches}`;
        $("#currentPageInfo").html(currentPageInfo);

        if (data.offset > 0) {
            //Enable navigating to the previous page
            let offset = data.offset < data.itemsPerPage ? 0 : data.offset - data.itemsPerPage;
            $("#previousPageOffset").val(offset)
            $("#prevPageBtn").removeAttr("disabled");
        }
        else {
            $("#prevPageBtn").attr("disabled", true);
        }

        if (data.offset + data.resultEntries.length < data.totalMatches) {
            //Enable navigating to the next page
            let offset = data.offset + data.resultEntries.length;
            $("#nextPageOffset").val(offset)
            $("#nextPageBtn").removeAttr("disabled");
        }
        else {
            $("#nextPageBtn").attr("disabled", true);
        }

        var colIds = $.map($("table thead tr th"), (x) => { return $(x).attr("id") });

        $(data.resultEntries).each((i, e) => {
            let itemId = e.id;

            //inserting a new row and adding td elements for each column in that row
            let tr = $("<tr/>")
            $(tbody).append(tr);
            let maxRowHeight = $('#max-row-height').val()
            if (!maxRowHeight) {
                $('#max-row-height').val(10)
                maxRowHeight
            }
            $(colIds).each((k, id) => {
                $(tr).append($(`<td class='${id}'><div  style="width:100%; max-height:${maxRowHeight}em; overflow-y: auto" /></td>`))
            });

            //Populating field data into appropriate columns in the new row
            $(e.fields).each((k, f) => {
                let td = $(tr).children(`.${f.fieldId}`);
                $(td).children('div').html(f.fieldContent);
            });

            ////let entry = `<div class="result-entry">`;
            ////$(e.fields).each(function (j, s) {
            ////    if (s.fieldName) {
            ////        entry += "<div class='field-name'>" + s.FieldName + "</div>";
            ////    }
            ////    entry += "<div class='field-highlight'><b>Matched Term:</b> "
            ////    $(s.highlights).each(function (k, h) {
            ////        entry += h + "<br />";
            ////    });
            ////    entry += "</div>";
            ////    entry += "<div class='field-value'><b>Matched Field Content:</b> "
            ////    $(s.fieldContent).each(function (k, h) {
            ////        entry += h + "<br />";
            ////    });
            ////    entry += "</div>";
            ////});

            ////entry += "</div>";
            ////$(".searchResults").append(entry);

        });

         
    });

}

function resizeRowHeight(height) {
    $('tbody tr td div').css('max-height', height + 'em');
}
