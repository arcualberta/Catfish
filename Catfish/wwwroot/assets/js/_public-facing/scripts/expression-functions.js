function updateFields(triggerSrcFieldId, element) {

    console.log("triggered by: " + triggerSrcFieldId);

    let selectedOptionId = $(element).val();
    let selectedOptionValue = $("#" + selectedOptionId).val();
    console.log(selectedOptionId);
    console.log(selectedOptionValue);
}