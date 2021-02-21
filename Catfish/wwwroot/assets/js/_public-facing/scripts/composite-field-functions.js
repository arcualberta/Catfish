function addDataItem(childListContainerId, templateId, min, max) {

    let childListContainer = $("#" + childListContainerId);
    let numItems = $(childListContainer).children(".composite-field-child").length; // number of item in the list
    if (numItems >= max) {
        alert("Sorry, you can't add more item into the list.");
        return false;
    }

    let templateClone = $("#" + templateId).clone();
    $(childListContainer).append(templateClone);

    let newChildGuid = createGuid();
    let wrappperId = newChildGuid + "_wrapper";
    $(templateClone).attr("id", wrappperId);

    //Updating ids and names of elements
    let elements = $(templateClone).find("input, select, textarea");
    var templateNamePrefix = $(templateClone).data("name-prefix");
    var elementNameSufixIndex = templateNamePrefix.length + ".ChildTemplate".length;
    var elementNamePrefix = templateNamePrefix + ".Children[" + numItems + "]";
    $.each(elements, function (idx, ele) {
        let name = elementNamePrefix + $(ele).attr("name").substr(elementNameSufixIndex);
        $(ele).attr("name", name);

        let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
        $(ele).attr("id", id);
    });

    //Specifying the onclick event of the delete button
    let delBtn = $(templateClone).find(".deleteButton");
    let childrenNamePrefix = templateNamePrefix + ".Children";
    $(delBtn).attr("onclick", `removeDataItem('${childListContainerId}', '${wrappperId}', ${min}, '${childrenNamePrefix}'); return false;`)

    $(templateClone).show();

}

function removeDataItem(childListContainerId, wrappperId, min, childrenNamePrefix) {
    let childListContainer = $("#" + childListContainerId);
    let numItems = $(childListContainer).children(".composite-field-child").length; // number of item in the list
    if (numItems <= min) {
        alert("Sorry, minimum item limit reached.");
        return false;
    }

    $("#" + wrappperId).remove();

    //Re-indexing children
    let children = $(childListContainer).children(".composite-field-child");
    var elementNameSufixIndex = childrenNamePrefix.length + ".ChildTemplate".length;
    $.each(children, function (childIdx, child) {
        let elements = $(child).find("input, select, textarea");
        $.each(elements, function (idx, ele) {
            let name = $(ele).attr("name");
            let suffixIdx = name.indexOf("]", childrenNamePrefix.length);
            name = `${childrenNamePrefix}[${childIdx}${name.substring(suffixIdx)}`;
            $(ele).attr("name", name);

            let id = name.split(/\[/).join('_').split(/\]/).join('_').split(/\./).join('_');
            $(ele).attr("id", id);
        });
    });
}

function createGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
} 
