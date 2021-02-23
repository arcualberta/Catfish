function addRow(tableFieldId) {
    let container = $("#" + tableFieldId);
    let dataTable = $(container).find("table.tf-data");
    let n = $(dataTable).find("tbody tr").length;

    if (n >= $(dataTable).data("max-rows")) {
        alert("Sorry, you can't add more rows to this table.");
        return false;
    }

    let row = atob($(container).find("table.tf-template").html());
    row = $(row).find("tr");


    //Updating ids and names of elements in the new row
    let namePrefix = $(dataTable).data("name-prefix")
    let oldNamePrefix = namePrefix + ".TableHead";
    let newNamePrefix = namePrefix + ".TableData[" + n + "]";

    //Example:
    //Blocks[1].Item.Fields[0]  <== example namePrefix
    //Blocks[1].Item.Fields[0].TableHead.Fields[1].Values[0].Values[0].Value <== example name before updating
    //Blocks[1].Item.Fields[0].TableData[4].Fields[1].Values[0].Values[0].Value <== example name after updating for a table which already has 4 rows

    let elements = $(row).find("input, select, textarea");
    $.each(elements, function (idx, ele) {
        let name = newNamePrefix + $(ele).attr("name").substr(oldNamePrefix.length);
        $(ele).attr("name", name);

        let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
        $(ele).attr("id", id);
    });


    //Updating the delete button onclick action
    $(row).find(".delete-btn").attr("onclick", `deleteRow('${tableFieldId}', ${n}); return false;`);

    //Inserting the row to the tbody section of the data table
    let tbody = $(dataTable).find("tbody");
    $(tbody).append(row);
}

function deleteRow(tableFieldId, index) {
    let container = $("#" + tableFieldId);
    let dataTable = $(container).find("table.tf-data");
    let rows = $(dataTable).find("tbody tr");

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
        let elements = $(rows[i]).find("input, select, textarea");

        $.each(elements, function (idx, ele) {
            let name = $(ele).attr("name").substr(namePrefix.length);
            name = name.substr(name.indexOf("]"));
            name = namePrefix + "[" + i + name;
            $(ele).attr("name", name);

            let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
            $(ele).attr("id", id);
        });

        //Updating the delete button onclick action
        $(rows[i]).find(".delete-btn").attr("onclick", `deleteRow('${tableFieldId}', ${i}); return false;`);
    }

}

