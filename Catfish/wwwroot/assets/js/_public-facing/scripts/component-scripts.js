/*
 * Function for the free-text panel in views/shared/partial/_search.cshtml
 */
$(function () {
    //advance search
    //the query param need to be "?q=searchText"  ==> ?q= 
    let searchMode = $('input[name="searchMode"]:checked').val();
    if (searchMode === "simple") {
        let queryStr = window.location.search;
        if (queryStr !== "") {
            $("#simpleSearchTerm").val(queryStr.substring(3));
          
            $("#advanceSearchSubmitBtn").click();
        }
    }

});

function searchText() {
    var searchText = $("input[name='searchTerm']").val();
    window.location.href = '/search?q=' + searchText;
    return false;
}

//conatinerId is MetadatasetId or DataItem id -- need to confirmed
function advanceSearch(containerId, scopeId, textInputClass, start, resultRenderingType) {
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

            values["SearchText"] = inputVal;
            constraints.push(values);

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

        ///BEGIN: Displaying Result Entries
        ///================================
        //'Tabular', 'Slip'
        if (resultRenderingType == "Tabular")
            showResultTable(data.resultEntries);
        else if (resultRenderingType == "Slip")
            showResultSlip(data.resultEntries);
        ///END: Displaying Result Entries
        ///================================
    });
}

function resizeRowHeight(height) {
    $('tbody tr td div').css('max-height', height + 'em');
}


function showResultTable(dataEntries) {
    var colIds = $.map($("table thead tr th"), (x) => { return $(x).attr("id") });
    //$(data.resultEntries).each((i, e) => {
    $(dataEntries).each((i, e) => {
        let itemId = e.id;

        //inserting a new row and adding td elements for each column in that row
        let tr = $("<tr/>")
        $('tbody').append(tr);
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
    });
}

function resizeRowHeight(height) {
    $('tbody tr td div').css('max-height', height + 'em');
}


function showResultSlip(resultEntries) {
    $("#slipRendering").empty();
    let slipTemplate = $("#slipTemplate");
    if (!slipTemplate) {
        alert("No template is found to render results.")
        return;
    }

    let maxWords = $("#maxWords").val();

    $(resultEntries).each((i, e) => {
        let itemId = e.id;

        let slip = slipTemplate.clone();
        $(slip).show();
        $(slip).attr("id", "slip-" + i);

        //iterating through all children with a data-field-id attribute
        $(slip).find("[data-field-id]").each((k, slipElement) => {
            let fieldId = $(slipElement).data("field-id");
            let field = e.fields.filter(f => f.fieldId === fieldId)[0];
            if (field) {
                if ($(slipElement).data("thumbnail")) {
                    //Set the thumbnail image
                    let thumbnailUrl = "/assets/images/thumbnails/" + field.fieldContent;
                    $(slipElement).css("background-image", "url(" + thumbnailUrl + ")");
                }
                else {
                    let separator = $(field).data("val-separator");
                    if (!separator)
                        separator = ",";

                    let fieldContent = field.fieldContent.join(separator);
                    fieldContent = wordLimit(fieldContent, maxWords, "..."); 
                    $(slipElement).html(fieldContent);
                }
            }
        });

        //data-field-link
        //setting any links to the item title
        $(slip).find("a[data-field-link]").each((k, anchor) => {
            //TODO: Set the the appropriate url
            //let url = anchor;
            let fieldId = $(anchor).data("field-link");
            let field = e.fields.filter(f => f.fieldId === fieldId)[0];
            let url = field.fieldContent;
            $(anchor).attr("href", url);
        });

        //setting any links to the detailed view of the item
        $(slip).find("a[data-details-view-link]").each((k, anchor) => {
            //TODO: Set the the appropriate url
            let url = window.location.origin + "/items/" + itemId;

            $(anchor).attr("href", url);
        });

        let thumbnailDiv = $(slip).find("div[data-details-view-link]")[0];
        if (thumbnailDiv) {
            let fieldId = $(slipElement).data("field-id");
            if (fieldId !== "") {
                let field = e.fields.filter(f => f.fieldId === fieldId)[0];
                if (field) {
                    let thumbFileName = field.fieldContent[0];
                    if (thumbFileName) {
                        let urlSpecs = "url(BASE-PATH-REPLACE-THIS/" + thumbFileName + ")";
                        $(thumbnailDiv).css("background-image", urlSpecs)
                    }
                }
            }
        }

        $("#slipRendering").append(slip);
    });
}
function wordLimit(str, limit, end) {

    //default limit to 100 
    limit = (limit) ? limit : 100;
    end = (end) ? end : '...';
    str = str.split(' ');

    if (str.length > limit) {
        var cutTolimit = str.slice(0, limit);
        return cutTolimit.join(' ') + ' ' + end;
    }

    return str.join(' ');
}
