import { Field, OptionFieldType, TextCollection } from "../form-models";
import { getTextValue } from './textHelper'

/**
 * Is the given field an option field
 * @param field
 */
export const isOptionField = (field: Field): boolean => Object.values(OptionFieldType).map(x => x as unknown as string).includes(field.type as unknown as string);

/**
 * Returns the title of a field as a string. If multiple values are specified, only returns the first value.
 * @param field: input field
 * @param lang: optional language. If not specified, returns the value in the first language.
 */
export const getFieldTitle = (field: Field, lang: string | null): string | null => getTextValue(field.title, lang)[0]

/**
 * Returns the description of a field as a string. If multiple values are specified, only returns the first value.
 * @param field: input field
 * @param lang: optional language. If not specified, returns the value in the first language.
 */
export const getFieldDescription = (field: Field, lang: string | null): string | null => getTextValue(field.description, lang)[0]

