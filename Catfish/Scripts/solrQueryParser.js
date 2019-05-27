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

    var TokenEnumerator = function (inputString) {
        this.data = [];
        this.index = -1;

        tokenize(inputString, this.data);
    }
    {
        TokenEnumerator.prototype.push = function (value) {
            this.data.push(value);
        }

        TokenEnumerator.prototype.next = function () {
            var index = this.index;
            ++index;

            if (index < 0 || index >= this.data.length) {
                return null;
            }

            this.index = index;

            return this.data[index];
        }

        TokenEnumerator.prototype.current = function () {
            var index = this.index;

            if (index < 0 || index >= this.data.length) {
                return null;
            }

            return this.data[index];
        }

        TokenEnumerator.prototype.reset = function () {
            this.index = -1;
        }
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

        if (pieces.length == 0) {
            return "*:*";
        }

        return pieces.join(" && ");
    }

    function decodeTopLevelBracket(tokens, result) {
        var name = null;
        var token = null;

        while ((token = tokens.next()) != null) {
            switch (token) {
                case ":":
                    if (name == null) {
                        console.error("Error parsing search string. No name befor ':' on token " + tokens.index);
                    }

                    decodeMultiSelect.call(this, name, tokens, result);
                    name = null;
                    break;

                case "\"":
                    var value = decodeQuotation.call(this, tokens);
                    if (name == null) {
                        name = "\"" + token + "\"";
                    } else {
                        name += " " + "\"" + token + "\"";
                    }
                    break;

                case ")":
                    if (name != null) {
                        result['*'] = this.createStringField(name);
                    }

                    return;

                case "||":
                case "&&":
                    break;

                default:
                    if (name == null) {
                        name = token;
                    } else {
                        name += " " + token;
                    }
                    break;
            }
        }
    }

    function decodeMultiSelect(name, tokens, result) {
        var token = null;
        var values = [];
        var value = null;
        var resultName = name.substring(0, name.length - "_en_ss".length);

        token.next(); // Remove opening bracket.

        while ((token = tokens.next()) != null) {
            if (token == ")") {
                break;
            }

            switch (token) {
                case "(":
                    break;

                case "&&":
                case "||":
                    values.push(value);
                    value = null;
                    break;

                case "\"":
                    value = decodeQuotation.call(this, tokens);
                    break;

                default:
                    if (value == null) {
                        value = token;
                    } else {
                        value += " " + token;
                    }
            }
        }

        if (value != null) {
            values.push(value);
        }

        result[resultName] = this.createMultiSelectField(values);
    }

    function decodeRange(name, tokens, result) {
        var from = tokens.next();
        var resultName = name.substring(0, name.length - "_is".length);

        if (tokens.next().toLowerCase() != "to") {
            console.error("Invalid range statement at " + tokens.index);
        }

        var to = tokens.next();

        if (tokens.next() != "]") {
            console.error("Invalid range statement at " + tokens.index);
        }

        result[resultName] = this.createRangeField(from, to);
    }

    function decodeString(name, tokens, result) {
        var value = "";
        var token;
        var resultName = name.substring(0, name.length - "_txts_en".length);

        while ((token = tokens.next()) != null) {
            if (token == ")") {
                break;
            }

            switch (token) {
                case "||":
                case "&&":
                    value += " ";
                    break;

                case "\"":
                    value += "\"" + decodeQuotation.call(this, tokens) + "\"";
                    break;

                default:
                    value += token;
            }
        }

        result[resultName] = this.createStringField(value);
    }

    function decodeElement(name, tokens, result) {
        var token = null;
        var value = null;

        token = tokens.next();

        if (token == "[") {
            decodeRange.call(this, name, tokens, result);
        } else if (token == "(") {
            decodeString.call(this, name, tokens, result);
        } else {
            console.error("Invaid token found at index " + tokens.index);
        }
    }

    function decodeQuotation(tokens) {
        var result = "";
        var token;

        while ((token = tokens.next()) != null) {
            if (token == "\"") {
                break;
            } else {
                result += token + " ";
            }
        }

        return result.trim();
    }
    
    SolrParser.prototype.decode = function (queryString) {
        var result = {};

        if (queryString == null || queryString.trim() == "*:*") {
            return result;
        }

        var tokens = new TokenEnumerator(queryString);
        var name = null;
        var token = null;

        while ((token = tokens.next()) != null) {
            switch (token) {
                case "(":
                    decodeTopLevelBracket.call(this, tokens, result);
                    break;

                case ":":
                    if (name == null) {
                        console.error("Error parsing search string. No name befor ':' on token " + tokens.index);
                    }

                    decodeElement.call(this, name, tokens, result);
                    break;

                case "\"":
                    name = decodeQuotation.call(this, tokens);
                    break;
                    
                default:
                    name = token;
            }
        }

        return result;
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