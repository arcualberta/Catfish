/**
 * Base model for form fields.
 */

import { Guid } from "guid-typescript"

import { TextCollection } from "./textCollection";
import { Option } from './option'

export interface Field {
    /**
     * Type of the field.
     * */
    type: typeof FieldTypes;

    /**
     * Unique field ID.
     * */
    id: Guid;

    /**
     * Field name. Displayed in the rendered form. Each Text entry in 
     * the "name" TextCollection represents the name in a given language.
     * */
    name: TextCollection;

    /**
     * Field description. Displayed in the rendered form. Each Text entry in 
     * the "name" TextCollection represents the name in a given language.
     * */
    description: TextCollection | null

    /**
     * Is this a required field?
     * */
    isRequired: boolean | null;

    /**
     * Does this field take multiple values?. Only applicable for multilingual
     * and Monolingual text fields.
     */
    isMultiValued: boolean | null;

    /**
     * The number of decimal points to be used, when type is Decimal.
     * */
    decimalPoints: number | null;

    /**
      * List of options. Only applicable for OptionFieldType fields.
      */
    options: Option[] | null;

    /**
     * Should the user be allowed to input custom option values?
     * . Only applicable for OptionFieldType fields.
     * */
    allowCustomOptionValues: boolean | null;

}

/**
 * Text input field types, including types applicable for multi-lingual text fields.
 * */
export enum TextType {
    SingleLine = "SingleLine",
    Paragraph = "Paragraph",
    RichText = "RichText"
}

/**
 * Mono-lingual text field types.
 * */
export enum MonolingualFieldType {
    Date = "Date",
    DateTime = "DateTime",
    Decimal = "Decimal",
    Integer = "Integer",
    Email = "Email"
}

/**
 * Option field types.
 * */
export enum OptionFieldType {
    CheckBoxes = "CheckBoxes",
    DataList = "DataList",
    RadioButtons = "RadioButtons",
    DropDown = "DropDown"
}

/**
 * Field types that does not take any user input.
 * */
export enum InfoSectionType{
    InfoSection = "InfoSection"
}

export const FieldTypes = { ...TextType, ...MonolingualFieldType, ...OptionFieldType, ...InfoSectionType };
export type FieldType = typeof FieldTypes;

