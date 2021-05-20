/*
 * Function for the free-text panel in views/shared/partial/_search.cshtml
 */
function searchText() {
    var searchText = $("input[name='searchTerm']").val();
    window.location.href = '/search?searchTerm=' + searchText;
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


function showResultSlip(resultEntries) {
    $(resultEntries).each((i, e) => {
        let itemId = e.id;
        //Resounding culture 4TH COL = TITLE,17th=Keywords, 18TH=Description,

        //Populating field data into appropriate columns in the new row
        let entry = '';
        let keywords = '';
        let description = '';
        let title = '';
        $(e.fields).each((i, f) => {


            if (i == 3) //4th col is Title
                title = f.fieldContent;
            else if (i == 15)
                keywords = f.fieldContent;
            else if (i == 16) {
                description = f.fieldContent;
                let detailLink = '<br/><a href="#"><b>More</b></a>';

                let content = `<div class="col-md-10 entryContent"><div class="entryDescription">` + description + detailLink + `</div> <div class="entryTags">` + keywords + `</div></div>`;
                let imgHolder = `<div class=" col-md-2 entryImg"></div>`

                entry = "<div class='slipEntry'><h3>" + title + "</h3>";
                entry += "<div class='row'>" + content + imgHolder + "</div>";
                entry += "</div>";
                $("#slipRendering").append(entry);
                entry = '';
                //break; 
            }

        });
    });
}