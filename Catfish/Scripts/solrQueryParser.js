"use strict"

var SolrParser = function (langCode) {
    this.langCode = langCode;
}
{
    var tokenTypes = ["\\(", "\\)", "\"", "(", ")", ":", "*", "[", "]", "TO", "AND", "and", "OR", "or"];

    function tokenizePiece(piece, outputBuffer) {
        if (piece.length == 0) {
            return;
        }

        var index;
        var checkType;

        for (var i = 0; i < tokenTypes.length; ++i) {
            checkType = tokenTypes[i];
            index = piece.indexOf(checkType);

            if (index == 0) {
                outputBuffer.push(checkType);
                tokenizePiece(piece.substring(checkType.length), outputBuffer);

                return;
            }
            if (index >= 0) {
                tokenizePiece(piece.substring(0, index), outputBuffer);
                outputBuffer.push(checkType);
                tokenizePiece(piece.substring(index + checkType.length), outputBuffer);

                return;
            }
        }

        outputBuffer.push(piece);
    }

    function tokenize(text, outputBuffer) {
        var pieces = text.split(/\s+/g);

        pieces.forEach(function (item, index) {
            tokenizePiece(item, outputBuffer);
        });

        return outputBuffer;
    }

    function solrEscapeString(input) {
        if (input == null) {
            return null;
        }

        var resultParts = input.replace(/\\/g, "\\").replace(/\(/g, "\(").replace(/\)/g, "\)").trim().split("\"");
        var result = "";
        var inQuotes = false; // Use a boolean instead of looking for even indexes.

        for (var i = 0; i < resultParts.length; ++i) {
            var part = resultParts[i].trim();

            if (part.length > 0) {
                if (inQuotes) {
                    if (result.length > 0) {
                        result += " || ";
                    }

                    result += "\"" + part + "\"";
                } else {
                    part.split(/\s+/).forEach(function (piece) {
                        if (piece.length > 0) {
                            if (result.length > 0) {
                                result += " || ";
                            }

                            result += piece;
                        }
                    });
                }
            }

            inQuotes = !inQuotes;
        }

        return result;
    }

    var encoders = {
        "string": function (data) {
            return solrEscapeString(data);
        },
        "range": function (data) {
            return '[' + data.min + " TO " + data.max + ']';
        },
        "multiselect": function (data) {
            return data.map(function (input) {
                return input; // Should we be performing an escape on these strings?
            }).join(" || ");
        }
    }

    function encodeField(fieldName, fieldData) {
        var result = null;
        var encodedString = encoders[fieldData.type].call(this, fieldData.data);

        if (fieldName == '*') {
            return "(" + encodedString + ")";
        }

        switch (fieldData.type) {
            case "string":
                result = fieldName + "_txts_" + this.langCode + ":(" + encodedString + ")";
                break;

            case "range":
                result = fieldName + "_is:" + encodedString;
                break;

            case "multiselect":
                result = "(" + fieldName + "_" + this.langCode + "_ss:(" + encodedString + "))";
                break;
        }

        return result;
    }

    SolrParser.prototype.encode = function (fields) {
        var pieces = [];
        for (var property in fields) {
            if (fields.hasOwnProperty(property)) {
                pieces.push(encodeField.call(this, property, fields[property]));
            }
        }

        return pieces.join(" && ");
    }

    SolrParser.prototype.decode = function (queryString) {
        var tokens = tokenize(outputBuffer);
    }

    SolrParser.prototype.createStringField = function (fieldString) {
        return {
            data: fieldString,
            type: "string"
        }
    }

    SolrParser.prototype.createRangeField = function (min, max) {
        return {
            data: {
                min: min,
                max: max
            },
            type: "range"
        }
    }

    SolrParser.prototype.createMultiSelectField = function (values) {
        return {
            data: values,
            type: "multiselect"
        }
    }
}