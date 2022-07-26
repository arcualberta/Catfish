/**
 * Model for form fields.
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
     * Field title. Displayed in the rendered form. Each Text entry in 
     * the "title" TextCollection represents the title in a given language.
     * */
    title: TextCollection;

    /**
     * Field description. Displayed in the rendered form. Each Text entry in 
     * the "description" TextCollection represents the description in a given language.
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
    ShortAnswer = "Short Answer",
    Paragraph = "Paragraph",
    RichText = "RichText"
}

/**
 * Mono-lingual text field types.
 * */
export enum MonolingualFieldType {
    Date = "Date",
    DateTime = "Date Time",
    Decimal = "Decimal",
    Integer = "Integer",
    Email = "Email"
}

/**
 * Option field types.
 * */
export enum OptionFieldType {
    Checkboxes = "Checkboxes",
    DataList = "Data List",
    RadioButtons = "Radio Buttons",
    DropDown = "Drop Down"
}

/**
 * Field types that does not take any user input.
 * */
export enum InfoSectionType{
    InfoSection = "Info Section"
}

export const FieldTypes = { ...TextType, ...MonolingualFieldType, ...OptionFieldType, ...InfoSectionType };
export type FieldType = typeof FieldTypes;

