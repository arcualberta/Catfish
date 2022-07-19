import { Field, OptionFieldType } from "../form-models";


export function isOptionField(field: Field) {
    return Object.keys(OptionFieldType).includes(field.type as unknown as string);
}