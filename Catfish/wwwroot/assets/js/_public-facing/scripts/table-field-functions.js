function addRow(tableFieldId) {
    let container = $("#" + tableFieldId);
    let dataTable = $(container).find("table.tf-data");
    let n = $(dataTable).find("tbody tr.data-row").length;

    if (n >= $(dataTable).data("max-rows")) {
        alert("Sorry, you can't add more rows to this table.");
        return false;
    }

    let row = atob($(container).find("table.tf-template").html());
    row = $(row).find("tr");


    //Updating ids, names, and data-r attribute of elements in the new row
    let namePrefix = $(dataTable).data("name-prefix")
    let oldNamePrefix = namePrefix + ".TableHead";
    let newNamePrefix = namePrefix + ".TableData[" + n + "]";

    $(row).attr("data-r", n);
    $(row).addClass("data-row");

    //Example:
    //Blocks[1].Item.Fields[0]  <== example namePrefix
    //Blocks[1].Item.Fields[0].TableHead.Fields[1].Values[0].Values[0].Value <== example name before updating
    //Blocks[1].Item.Fields[0].TableData[4].Fields[1].Values[0].Values[0].Value <== example name after updating for a table which already has 4 rows

    cols = $(row).find("td");
    $.each(cols, function (c, cell) {

        let elements = $(cell).find("input, select, textarea");
        $.each(elements, function (idx, ele) {
            let name = newNamePrefix + $(ele).attr("name").substr(oldNamePrefix.length);
            $(ele).attr("name", name);

            let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
            $(ele).attr("id", id);

            $(ele).attr("data-r", n);
            $(ele).attr("data-c", c);


            //If this element contains a data-model-id, then assign a new GUID to it
            let oldModelId = $(ele).attr("data-model-id");
            if (oldModelId) {
                let newGuid = createGuid();
                $(ele).attr("data-model-id", newGuid);

                //If the current row contains any elements with value expressions 
                //that refer to the oldModelGuid, then replace them with the new one
                let computedFields = $(row).find("[data-value-expression!=''][data-value-expression]");
                if (computedFields) {
                    $.each(computedFields, function (idx, compField) {
                        let val = $(compField).attr("data-value-expression");
                        val = val.replace('{data-model-id}', newGuid);
                        val = val.replace(oldModelId, newGuid);
                        $(compField).attr("data-value-expression", val);
                    });
                }
            }

            //////Creating a new Guid for the field represented in the cell and setting it as the ID
            ////let guid = createGuid();
            ////$("input[name$='.Id']").val(guid);

            //////Resolving all data model references in computation expressions
            ////let computedFields = $(cell).find("[data-value-expression!=''][data-value-expression]");
            ////if (computedFields) {
            ////    $.each(computedFields, function (idx, compField) {
            ////        //setting the new Guid as the data-model-id to the element
            ////        $(compField).attr("data-model-id", guid);

            ////        //updating references in the value expression
            ////        let val = $(compField).attr("data-value-expression");
            ////        val = val.replace('{data-model-id}', guid);
            ////        $(compField).attr("data-value-expression", val);
            ////    });
            ////}
        }); //END: $.each(elements, function (idx, ele)
    }); //END: $.each(cols, function (c, cell)

    //Updating the delete button onclick action
    $(row).find(".delete-btn").attr("onclick", `deleteRow('${tableFieldId}', ${n}); return false;`);

    //Inserting the row to the tbody section of the data table
    let tbody = $(dataTable).find("tbody");

    //If there is a footer, then insert the new row above the first footer row
    if ($(tbody).find("tr.first-footer-row").length > 0) {
        $(row).insertBefore(`#${tableFieldId} tbody tr.first-footer-row`)
    }
    else {
        $(tbody).append(row);
    }

    updateButtonVisibility(tableFieldId);

}

function deleteRow(tableFieldId, index) {
    let container = $("#" + tableFieldId);
    let dataTable = $(container).find("table.tf-data");
    let rows = $(dataTable).find("tbody tr[data-r]");

    if (rows.length <= $(dataTable).data("min-rows")) {
        alert("Sorry, you can't delete more rows from this table.");
        return false;
    }

    //remmove the row
    $(rows[index]).remove();
    rows.splice(index, 1)

    //Re-indexing remaining elements
    let namePrefix = $(dataTable).data("name-prefix") + ".TableData";
    for (i = index; i < rows.length; ++i) {

        $(rows[i]).attr("data-r", i);

        let elements = $(rows[i]).find("input, select, textarea");

        $.each(elements, function (idx, ele) {
            let name = $(ele).attr("name").substr(namePrefix.length);
            name = name.substr(name.indexOf("]"));
            name = namePrefix + "[" + i + name;
            $(ele).attr("name", name);

            let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
            $(ele).attr("id", id);

            if ($(ele).attr("data-r")) {
                $(ele).attr("data-r", i);
            }
        });

        //Updating the delete button onclick action
        $(rows[i]).find(".delete-btn").attr("onclick", `deleteRow('${tableFieldId}', ${i}); return false;`);

        updateButtonVisibility(tableFieldId);

        //Making sure all field calculations are updated
        updateFields();
    }
}

function updateButtonVisibility(tableFieldId) {
    let container = $("#" + tableFieldId);
    let dataTable = $(container).find("table.tf-data");
    let n = $(dataTable).find("tbody tr[data-r]").length;

    if (n >= $(dataTable).data("max-rows")) 
        $(dataTable).find(".tableAddRowBtn").hide();
    else
        $(dataTable).find(".tableAddRowBtn").show();

    if (n <= $(dataTable).data("min-rows"))
        $(dataTable).find(".tableDeleteRowBtn").hide();
    else
        $(dataTable).find(".tableDeleteRowBtn").show();

}
