var submitFormContainer = function () {
    function storeElement(name, jsonObject, value) {
        var currentObject = jsonObject;
        var lastName = "";
        var inArray = false;

        // Parses the name string to find the appropriate element path.
        for (var i = 0; i < name.length; ++i) {
            var char = name[i];

            if (inArray) {
                if (char == "]") {
                    var loc = parseInt(lastName);

                    if (currentObject[loc]) {
                        currentObject = currentObject[loc];
                    } else {
                        currentObject[loc] = {};
                        currentObject = currentObject[loc];
                    }

                    lastName = "";
                    ++i;
                    inArray = false;
                } else {
                    lastName += char;
                }
            } else {
                if (char == ".") {
                    if (currentObject[lastName]) {
                        currentObject = currentObject[lastName];
                    } else {
                        currentObject[lastName] = {};
                        currentObject = currentObject[lastName];
                    }
                    lastName = "";
                } else if (char == "[") {
                    if (currentObject[lastName]) {
                        currentObject = currentObject[lastName];
                    } else {
                        currentObject[lastName] = [];
                        currentObject = currentObject[lastName];
                    }
                    lastName = "";
                    inArray = true;
                } else {
                    lastName += char;
                }
            }
        }

        // Stores the value
        if (lastName.length > 0) {
            if (!currentObject.hasOwnProperty(lastName)) {
                currentObject[lastName] = value;
            }
        } else {
            currentObject = value;
        }
    }

    function generateFormContainerJson(baseName, formContainerElement) {
        var jsonObject = {};

        $(formContainerElement).serializeArray().forEach(function (item) {
            var name = item.name.startsWith(baseName) ? item.name.substring(baseName.length + 1) : item.name;

            storeElement(name, jsonObject, item.value);
        });

        return jsonObject;
    }

    function fillValidationErrors(baseName, serverName, formContainerElement, errors) {
        $(formContainerElement).find(".form-error").hide();

        for (var key in errors) {
            if (errors.hasOwnProperty(key)) {
                var message = "<span>" + errors[key][0] + "<span>";

                for (var i = 1; i < errors.length; ++i) {
                    message += "<br/><span>" + errors[i] + "</span>";
                }

                var name = baseName + "." + key.substring(serverName.length);
                $(formContainerElement).find(".form-error[name='" + name + "']").html(message).show();
            }
        }
    }

    function submitFormContainer(baseName, formContainerElement, formContainerJson, submitUrl, successFunction, errorFunction) {
        var data = {
            "vm": generateFormContainerJson(baseName, formContainerElement),
            "formContainer": formContainerJson
        }

        $.ajax({
            method: "POST",
            url: submitUrl,
            data: data,
            success: function (result) {
                if (result.Errors) {
                    fillValidationErrors(baseName, "vm.", formContainerElement, result.Errors);
                    if (errorFunction) {
                        errorFunction(result.Errors);
                    }
                } else {
                    if (successFunction) {
                        successFunction(formContainerElement, result);
                    }
                }
            },
            error: function (error) {
                if (errorFunction) {
                    errorFunction(error);
                } else {
                    console.error(error);
                }
            }
        })
    }

    return submitFormContainer;
}();

function addTextFieldRow(location, pathId, pathName, indexPath, langCode, isRequired) {
    var index = location.find("input[type='text']").length;
    var name = pathName.replace("{0}", index);
    var id = pathId.replace("{0}", index);

    var div = $("<div></div>");

    var inputIndex = $("<input></input>");
    inputIndex.attr("type", "hidden");
    inputIndex.attr(indexPath);
    inputIndex.val(id);
    div.append(inputIndex);

    var lang = $("<input></input>");
    lang.attr("type", "hidden");
    lang.attr("id", id + "__LanguageCode");
    lang.attr("name", name + ".LanguageCode");
    lang.val(langCode);
    div.append(lang);

    var lang = $("<input></input>");
    lang.attr("type", "hidden");
    lang.attr("id", id + "__LanguageCode");
    lang.attr("name", name + ".LanguageCode");
    lang.val(langCode);
    div.append(lang);

    var input = $("<input></input>");
    input.attr("type", "text");
    input.attr("id", id + "__Value");
    input.attr("name", name + ".Value");
    if (isRequired) {
        input.attr("required", "required");
    }
    div.append(input);

    var removeButton = $("<button></button>");
    removeButton.addClass("btn btn-secondary");
    removeButton.text("Remove");
    removeButton.click(function (ev) {
        ev.preventDefault();

        $(this).parent().remove();
    })
    div.append(removeButton);

    var span = $("<span></span>");
    span.addClass("field-validation-valid");
    span.attr("data-valmsg-for", name + ".Value");
    span.attr("data-valmsg-replace", "true");
    div.append(span);

    location.append(div);
}