function updateFields(triggerFieldId, element) {

    //let triggeredFieldValue = $(element).val();
    var visibleIfFields = $("input[data_visible_if], textarea[data_visible_if], select[data_visible_if]");
    for (i = 0; i < visibleIfFields.length; ++i) {
        let field = visibleIfFields[i];
        let expression = $(field).attr("data_visible_if");
        if (expression) {
            let fieldId = $(field).attr("data_field_id");
            let result = evaluateExprssion(expression, triggerFieldId, element);
            if (result) {
                $("#" + fieldId).show()
            }
            else {
                $("#" + fieldId).hide()
            }
        }
        console.log("Expression: " + expression);
    }


    var requiredIfFields = $("input[data_required_if], textarea[data_required_if], select[data_required_if]");
    for (i = 0; i < requiredIfFields.length; ++i) {
        let field = requiredIfFields[i];
        let expression = $(field).attr("data_required_if");
        if (expression) {
            let fieldId = $(field).attr("data_field_id");
            let result = evaluateExprssion(expression, triggerFieldId, element);
            if (result) {
                $("#" + fieldId + " span.required").show();
                $("#" + fieldId + " input[data_required_if]").prop('required', true);
            }
            else {
                $("#" + fieldId + " span.required").hide();
                $("#" + fieldId + " input[data_required_if]").prop('required', false);
            }
        }
        console.log("Expression: " + expression);
    }
}

function evaluateExprssion(exp, triggerFieldId, element) {

    if (triggerFieldId + ":" + element.value === exp) {
        //This is a simple matching to a option selected and the match is successful
        return true;
    }

    return false;
}