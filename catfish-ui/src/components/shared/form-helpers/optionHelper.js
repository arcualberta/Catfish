import { Guid } from 'guid-typescript';
import { createTextCollection } from '../../shared/form-helpers';
export function createOption(languages, optionText) {
    const opt = {
        id: Guid.create().toString(),
        isExtendedInput: false,
        isExtendedInputRequired: false,
        optionText: optionText ? optionText : createTextCollection(languages)
    };
    return opt;
}
export const getOptionText = (option, lang) => getOptionValues(option.optionText, lang);
export function getOptionValues(optionText, lang) {
    if (lang)
        return optionText?.values?.filter(txt => txt.lang === lang).map(val => val.value)?.at(0);
    else
        return optionText?.values?.map(val => val.value);
}
export const cloneOption = (option) => {
    const clone = JSON.parse(JSON.stringify(option));
    clone.id = Guid.create().toString(),
        clone.optionText.id = Guid.create().toString(),
        clone.optionText.values.forEach(txt => txt.id = Guid.create().toString());
    console.log;
    return clone;
};
//# sourceMappingURL=optionHelper.js.map