import { Guid } from 'guid-typescript';
/**
 * Creates a new TextColleciton object
 * @param lang
 */
export const createTextCollection = (lang) => {
    const textCollection = {
        id: Guid.create().toString(),
        values: []
    };
    const languages = typeof (lang) == 'string' ? [lang] : lang;
    languages.forEach(lang => {
        textCollection.values.push({
            id: Guid.create().toString(),
            lang: lang,
            value: ""
        });
    });
    return textCollection;
};
/**
 * Creates a new Text object
 * @param lang
 */
export const createText = (lang) => {
    const text = {
        id: Guid.create().toString(),
        value: "",
        lang: lang ? lang : "en"
    };
    //if (lang)
    //	text.lang = lang;
    return text;
};
export const createGuid = () => {
    return Guid.create().toString();
};
export const getTextValue = (container, lang, separator) => {
    let vals;
    if (lang)
        vals = container?.values?.filter(txt => txt.lang === lang).map(val => val.value);
    else
        vals = container?.values?.map(val => val.value);
    return separator ? vals.join(separator) : vals;
};
export const cloneTextCollection = (textCollection) => {
    const clone = JSON.parse(JSON.stringify(textCollection));
    clone.id = createGuid();
    clone.values.forEach(txt => { txt.id = createGuid(); });
    return clone;
};
export const getConcatenatedValues = (container, separator) => {
    var vals = "";
    var texts = [];
    if (container.multilingualTextValues && container.multilingualTextValues?.length > 0) {
        container.multilingualTextValues.forEach((multiTextVal) => {
            vals += getTextValue(multiTextVal, null, separator);
        });
    }
    else if (container.monolingualTextValues && container.monolingualTextValues?.length > 0) {
        container.monolingualTextValues.forEach((text) => {
            texts.push(text.value);
        });
        vals = texts?.join(separator);
    }
    return vals;
};
//# sourceMappingURL=textHelper.js.map