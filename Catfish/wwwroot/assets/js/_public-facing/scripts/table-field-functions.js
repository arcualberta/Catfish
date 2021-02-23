function addRow(tableFieldId) {
    let container = $("#" + tableFieldId);
    let dataTable = $(container).find("table.tf-data");
    let numRows = $(dataTable).find("tbody tr").length;

    if (numRows >= $(dataTable).data("max-rows")) {
        alert("Sorry, you can't add more rows to this table.");
        return false;
    }

    let row = atob($(container).find("table.tf-template").html());
    row = $(row).find("tr");

    let namePrefix = $(dataTable).data("name-prefix")
    let oldNamePrefix = namePrefix + ".TableHead";
    let newNamePrefix = namePrefix + ".TableData[" + numRows + "]";

    //Blocks[1].Item.Fields[0]
    //Blocks[1].Item.Fields[0].TableHead.Fields[1].Values[0].Values[0].Value
    //Blocks[1].Item.Fields[0].TableData[3].Fields[1].Values[0].ModelType

    //Updating ids and names of elements in the new row
    let elements = $(row).find("input, select, textarea");
    $.each(elements, function (idx, ele) {
        let name = newNamePrefix + $(ele).attr("name").substr(oldNamePrefix.length);
        $(ele).attr("name", name);

        let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
        $(ele).attr("id", id);
    });

    //Inserting the row to the tbody section of the data table
    let tbody = $(dataTable).find("tbody");
    $(tbody).append(row);
}