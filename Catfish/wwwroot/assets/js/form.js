function addValue(fieldId) {
    //example string
    //Item_Fields_3__Values_0__Values_0__Value

    let itemToDupe = $('#' + fieldId).children().last();
    let baseId = fieldId + "_Values_";
    //get digits from original item
    let split = itemToDupe.children().last()[0].id.replace(baseId, "").split("__", 2);
    baseId += (+split[0] + 1);

    let secondNumber = split[1].split("_");
    baseId += "__Values_" + secondNumber[1] + "__Value";

    let dupeItem = itemToDupe.clone();
    dupeItem.children().last()[0].id = baseId;

    $('#' + fieldId).append(dupeItem);
}