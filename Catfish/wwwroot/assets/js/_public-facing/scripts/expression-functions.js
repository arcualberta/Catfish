function updateFields(element) {

    //let triggeredFieldValue = $(element).val();
    var visibleIfFields = $("input[data-visible-if], textarea[data-visible-if], select[data-visible-if]");
    for (i = 0; i < visibleIfFields.length; ++i) {
        let field = visibleIfFields[i];
        let expression = $(field).attr("data-visible-if");
        if (expression) {
            let fieldId = $(field).attr("data-field-id");
            let result = evaluateExprssion(expression, element);
            if (result) {
                $("#" + fieldId).show()
            }
            else {
                $("#" + fieldId).hide()
            }
        }
        console.log("Expression: " + expression);
    }


    var requiredIfFields = $("input[data-required-if], textarea[data-required-if], select[data-required-if]");
    for (i = 0; i < requiredIfFields.length; ++i) {
        let field = requiredIfFields[i];
        let expression = $(field).attr("data-required-if");
        if (expression) {
            let fieldId = $(field).attr("data-field-id");
            let result = evaluateExprssion(expression, element);
            if (result) {
                $("#" + fieldId + " span.required").show();
                $("#" + fieldId + " input[data-required-if]").prop('required', true);
            }
            else {
                $("#" + fieldId + " span.required").hide();
                $("#" + fieldId + " input[data-required-if]").prop('required', false);
            }
        }
        console.log("Expression: " + expression);
    }
}

function evaluateExprssion(exp, element) {
    let modelId = $(element).data("model-id");
    if (modelId + ":" + element.value === exp) {
        //This is a simple matching to a option selected and the match is successful
        return true;
    }

    return false;
}