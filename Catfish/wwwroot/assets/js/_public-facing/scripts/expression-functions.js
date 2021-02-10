function updateFields() {

    //let triggeredFieldValue = $(element).val();
    var visibleIfFields = $("input[data-visible-if], textarea[data-visible-if], select[data-visible-if]");
    for (i = 0; i < visibleIfFields.length; ++i) {
        let field = visibleIfFields[i];
        let expression = $(field).attr("data-visible-if");
        if (expression) {
            let fieldId = $(field).attr("data-field-id");
            //let result = evaluateExprssion(expression, element);
            let result = eval(expression);
            if (result) {
                $("#" + fieldId).show()
            }
            else {
                $("#" + fieldId).hide()
            }
        }
    }


    var requiredIfFields = $("input[data-required-if], textarea[data-required-if], select[data-required-if]");
    for (i = 0; i < requiredIfFields.length; ++i) {
        let field = requiredIfFields[i];
        let expression = $(field).attr("data-required-if");
        if (expression) {
            let fieldId = $(field).attr("data-field-id");

            console.log(expression)
            let result = eval(expression);
           if (result) {
                $("#" + fieldId + " span.required").show();
                $("#" + fieldId + " input[data-required-if]").prop('required', true);
            }
            else {
                $("#" + fieldId + " span.required").hide();
                $("#" + fieldId + " input[data-required-if]").prop('required', false);
            }
        }
    }
}

function StrValue(fieldModelId) {
    return $("[data-model-id='" + fieldModelId + "']").val();
}

function IntValue(fieldModelId) {
    let val = GetStrValue(fieldModelId);
    return parseInt(val);
}

function DoubleValue(fieldModelId) {
    let val = GetStrValue(fieldModelId);
    return parseFloat(val);
}

function FloatValue(fieldModelId) {
    let val = GetStrValue(fieldModelId);
    return parseFloat(val);
}

function DecimalValue(fieldModelId) {
    let val = GetStrValue(fieldModelId);
    return parseFloat(val);
}

function RadioValue(fieldModelId) {
    return $("[data-model-id='" + fieldModelId + "']:checked").val();
}

function CheckboxValue(fieldId, optionId) {
    return $("[data-option-id='" + optionId + "']:checked").val() === "true";
}