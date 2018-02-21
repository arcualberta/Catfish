var submitFormContainer = function () {
    function storeElement(name, jsonObject, value) {
        var currentObject = jsonObject;
        var lastName = "";
        var inArray = false;

        for (var i = 0; i < name.length; ++i) {
            var char = name[i];

            if (inArray) {
                if (char == "]") {
                    currentObject[parseInt(lastName)] = {};
                    currentObject = currentObject[parseInt(lastName)];
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

        if (lastName.length > 0) {
            currentObject[lastName] = value;
        } else {
            currentObject = value;
        }
    }

    function fillChildElements(baseName, jsonObject, currentElement) {
        if (currentElement == null) {
            return;
        }

        var nodeName = currentElement.nodeName.toLowerCase();

        if (nodeName == "input") {
            var name = currentElement.name.startsWith(baseName) ? currentElement.name.substring(baseName.length + 1) : currentElement.name;

            storeElement(name, jsonObject, $(currentElement).val());
        } else if (nodeName == "select") {
            var name = currentElement.name.startsWith(baseName) ? currentElement.name.substring(baseName.length + 1) : currentElement.name;

            storeElement(name, jsonObject, $(currentElement).val());
        } else if (nodeName == "textarea") {
            var name = currentElement.name.startsWith(baseName) ? currentElement.name.substring(baseName.length + 1) : currentElement.name;

            storeElement(name, jsonObject, $(currentElement).text());
        } else {
            for (var i = 0; i < currentElement.children.length; ++i) {
                fillChildElements(baseName, jsonObject, currentElement.children[i]);
            }
        }
    }

    function generateFormContainerJson(baseName, formContainerElement) {
        var jsonObject = {};

        fillChildElements(baseName, jsonObject, formContainerElement);

        return jsonObject;
    }

    function submitFormContainer(baseName, formContainerElement, formContainerJson) {
        var data = {
            "vm" : generateFormContainerJson(baseName, formContainerElement),
            "formContainer" : formContainerJson
        }

        $.ajax({
            method: "POST",
            url: "http://localhost:49696/apix/forms/submit",
            data: data,
            success: function (result) {
                console.log(result);
            },
            error: function (error) {
                console.log(error);
            }
        })
    }

    return submitFormContainer;
}();