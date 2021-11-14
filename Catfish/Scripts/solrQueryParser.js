"use strict"
/*eslint eqeqeq: ["error", "smart"]*/

var SolrToken = function (value) {
    this.value = value;
    SolrToken.tokenMap[value] = this;
}
SolrToken.tokenMap = {};

var SolrParser = function (langCode) {
    this.langCode = langCode;
}
{
    var tokenTypes = [
        new SolrToken("\\("),
        new SolrToken("\\)"),
        new SolrToken("\""),
        new SolrToken("&&"),
        new SolrToken("||"),
        new SolrToken("("),
        new SolrToken(")"),
        new SolrToken(":"),
        new SolrToken("*"),
        new SolrToken("["),
        new SolrToken("]"),
        new SolrToken("TO"),
        new SolrToken("AND"),
        new SolrToken("OR")];

    function tokenizePiece(piece, outputBuffer) {
        if (piece.length == 0) {
            return;
        }

        var index;
        var checkType;
        var wordLength;

        for (var i = 0; i < tokenTypes.length; ++i) {
            checkType = tokenTypes[i];
            wordLength = checkType.value.length;
            index = piece.indexOf(checkType.value);

            if (index == 0) {
                outputBuffer.push(checkType);
                tokenizePiece(piece.substring(wordLength), outputBuffer);

                return;
            }
            if (index >= 0) {
                tokenizePiece(piece.substring(0, index), outputBuffer);
                outputBuffer.push(checkType);
                tokenizePiece(piece.substring(index + wordLength), outputBuffer);

                return;
            }
        }

        outputBuffer.push(piece);
    }

    // Go through each piece and remove quote tokens around keywords
    function cleanTokenList(result) {
        var tokenSize = result.length;
        var token;

        var isInQuote = false;
        for (var i = tokenSize - 1; i >= 0; --i) {
            token = result[i];

            if (typeof (token) != "string") {
                if (token.value == "\"") {
                    if (isInQuote) {
                        isInQuote = false;
                    } else {
                        // Check the next two elements
                        var v1, v2;

                        --i;
                        v1 = i > 0 ? result[i] : "";

                        if (typeof (v1) != "string" || v1.value != "\"") {
                            --i;
                            v2 = i >= 0 ? result[i] : "";

                            if (typeof (v2) != "string" && v2.value == "\"") {
                                // We are in a single token which has been quoted.
                                result.splice(i, 3, v1);
                            }
                        }

                        isInQuote = true;
                    }
                }
            }
        }

        return result;
    }

    function tokenize(text) {
        var result = [];
        var pieces = text.split(/\s+/g);

        pieces.forEach(function (item, index) {
            tokenizePiece(item, result);
        });

        result = cleanTokenList(result);

        return result;
    }

    var TokenEnumerator = function (inputString) {
        this.index = -1;
        this.data = tokenize(inputString);
    }
    {
        TokenEnumerator.prototype.push = function (value) {
            this.data.push(value);
        }

        TokenEnumerator.prototype.next = function () {
            var value = this.nextRaw();

            if (value == null) {
                return null;
            }
            
            return typeof (value) == "string" ? value : value.value;
        }

        TokenEnumerator.prototype.nextRaw = function () {
            var index = this.index;
            ++index;

            if (index < 0 || index >= this.data.length) {
                return null;
            }

            this.index = index;

            return this.data[index];
        }

        TokenEnumerator.prototype.current = function () {
            var value = this.currentRaw();

            if (value == null) {
                return null;
            }

            return typeof(value) == "string" ? value : value.value;
        }

        TokenEnumerator.prototype.currentRaw = function () {
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
        var showOr = false;

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
                            var addPiece = true;

                            // Check if we need to AND or OR our data
                            if (result.length > 0) {
                                switch (piece.toUpperCase()) {
                                    case "AND":
                                        addPiece = false;
                                        result += ") AND (";
                                        showOr = false;
                                        break;

                                    case "OR":
                                        addPiece = false;
                                        result += ") OR ("
                                        showOr = false;
                                        break;
                                }
                            }

                            if (addPiece) {
                                if (showOr) {
                                    result += " || ";
                                }

                                showOr = true;
                                if (SolrToken.tokenMap.hasOwnProperty(piece.toUpperCase())) {
                                    // Avoid keywords being used directly in the search.
                                    result += '"' + piece + '"';
                                } else {
                                    result += piece;
                                }
                            }
                        }
                    });
                }
            }

            inQuotes = !inQuotes;
        }

        if (result != null && result.endsWith("(")) {
            // Prevent the query string from being invalid.
            result = result.substr(0, result.length - 1).trim();

            if (result.endsWith(" AND")) {
                result = result.substr(0, result.length - 3) + "|| (\"and\"";
            } else if (result.endsWith(" OR")) {
                result = result.substr(0, result.length - 2) + "|| (\"or\"";
            }
        }

        return "(" + result + ")";
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
        var bracketLevel = 0;


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
                        name = "\"" + value + "\"";
                    } else {
                        name += " " + "\"" + value + "\"";
                    }
                    break;

                case ")":
                    if (bracketLevel > 0) {
                        bracketLevel--;
                    } else {
                        if (name != null) {
                            result['*'] = this.createStringField(name);
                        }

                        return;
                    }
                    break;

                case "(":
                    bracketLevel++;
                    break;

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

        tokens.next(); // Remove opening bracket.

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
        var bracketCount = 0;

        while ((token = tokens.next()) != null) {
            if (token == ")") {
                if (bracketCount > 0) {
                    bracketCount--;
                    continue;
                } else {
                    break;
                }
            }

            switch (token) {
                case "||":
                case "&&":
                    value += " ";
                    break;

                case "AND":
                case "OR":
                    value += " " + token + " ";
                    break;

                case "(":
                    bracketCount++;
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
