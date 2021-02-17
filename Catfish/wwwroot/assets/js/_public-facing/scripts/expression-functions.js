function updateFields() {

    //Handling visible-if conditions
    var visibleIfFields = $("input[data-visible-if], textarea[data-visible-if], select[data-visible-if], option[data-visible-if]");
    for (i = 0; i < visibleIfFields.length; ++i) {
        let field = visibleIfFields[i];
        console.log(field);
        let expression = $(field).attr("data-visible-if");
        if (expression) {
            let fieldId = $(field).attr("data-field-id");
            let result = eval(expression);
            if (result) {
                $("#" + fieldId).show()
            }
            else {
                $("#" + fieldId).hide()

                //If this is an option
                if ($(field).prop("tagName").toLowerCase() === "option") {
                    let parent = $(field).parent();
                    //If this is a drop-down menu
                    if ($(parent).prop("tagName").toLowerCase() === "select") {
                        //We need to check whether the currently selected
                        //value is equal to the current field that is made hidden and if so, reset 
                        //the selected field.

                        if ($(parent).val() === $(field).val()) {
                            $(parent).val("")
                        }
                    }
                }

                //If this is a radio button
                if ($(field).is(':radio')) {
                    $(field).prop('checked', false);
                }

            }
        }
    }


    //Handling required-if conditions
    var requiredIfFields = $("input[data-required-if], textarea[data-required-if], select[data-required-if]");
    for (i = 0; i < requiredIfFields.length; ++i) {
        let field = requiredIfFields[i];
        let expression = $(field).attr("data-required-if");
        if (expression) {
            //console.log(expression)

            let fieldId = $(field).attr("data-field-id");
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

    //Handling value-expression fields (computed field values)
    var computedFields = $("input[data-value-expression], textarea[data-value-expression]");
    for (i = 0; i < computedFields.length; ++i) {
        let field = computedFields[i];
        let expression = $(field).attr("data-value-expression");
        if (expression) {
            console.log(expression)

            let result = eval(expression);
            $(field).val(result);
        }
    }

}

function StrValue(fieldModelId) {
    return $("[data-model-id='" + fieldModelId + "']").val();
}

function IntValue(fieldModelId) {
    let val = StrValue(fieldModelId);
    return parseInt(val);
}

function DoubleValue(fieldModelId) {
    let val = StrValue(fieldModelId);
    return parseFloat(val);
}

function FloatValue(fieldModelId) {
    let val = StrValue(fieldModelId);
    return parseFloat(val);
}

function DecimalValue(fieldModelId) {
    let val = StrValue(fieldModelId);
    return parseFloat(val);
}

function DateValue(fieldModelId) {
    let val = StrValue(fieldModelId);
    return Date.parse(val);
}

function RadioValue(fieldModelId) {
    return $("[data-model-id='" + fieldModelId + "']:checked").val();
}

function CheckboxValue(fieldId, optionId) {
    return $("[data-option-id='" + optionId + "']:checked").val() === "true";
}

function SelectFieldReadableValue(fieldModelId) {
    var field = $("[data-model-id='" + fieldModelId + "']");
    return $(field).children("option[value='" + $(field).val() + "']").text();
}

function RadioFieldReadableValue(fieldModelId) {
    var fieldVal = RadioValue(fieldModelId);
    return $("span[data-option-id='" + fieldVal + "']").text();
}


function Extract(str, delimiter, selectItemIndex, trimEnds) {

    var value = str;
    if (delimiter && value) {
        value = value.split(delimiter);
        value = value[selectItemIndex];
    }

    if (trimEnds && value)
        value = value.trim();

    return value;
}