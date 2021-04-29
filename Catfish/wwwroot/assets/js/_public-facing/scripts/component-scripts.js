/*
 * Function for the free-text panel in views/shared/partial/_search.cshtml
 */
function searchText() {
    var searchText = $("input[name='searchTerm']").val();
    window.location.href = '/search?searchTerm=' + searchText;
    return false;
}

//conatinerId is MetadatasetId or DataItem id -- need to confirmed
function advanceSearch(containerId, scopeId, textInputClass) {
    let name;
    let values = {};
    let selectedScope = $("#" + scopeId + " option:selected").val();
    let params = {};
    let constraints = [];

    //params["scope"] = selectedScope;
    //params["containerId"] = containerId;
   

    $("." + textInputClass).each(function (i, f) {
        let inputVal = $(f).val();
        let fieldId = $(f).attr("id");
        if (inputVal !== "") {

            let _constraint = {
                Scope: selectedScope,
                ContainerId: containerId,
                FieldId: fieldId,
                SearchText: inputVal   
            };

            
            values["Scope"] = selectedScope;
            values["ContainerId"] = containerId;
            
            values["FieldId"] = fieldId;
           
            values["SearchText"]= inputVal;
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
    params["itemPerPage"] = 10;
    var result = $.get(url, params);
    var message = "";

    result.done(function (data) {
        if (data.success) {
            //message = data.message ? data.message : "Auto-save succesful";
        }
        else {
            //message = data.message ? data.message : "Auto-save failed";
        }
    });

}
