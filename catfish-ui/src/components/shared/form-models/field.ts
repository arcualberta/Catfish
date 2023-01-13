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
    type: FieldType;

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
    isRequired: boolean;

    /**
     * Does this field take multiple values?. Only applicable for multilingual
     * and Monolingual text fields.
     */
    isMultiValued: boolean;

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
    allowCustomOptionValues: boolean;

    /* Files attachment*/
    files: FileReference[] | null;

    /* any child fields, example if this is a composite field */
    fields: Field[] | null;

}

/**
 * Field Types
 */
export enum FieldType{
    ShortAnswer,
    Paragraph,
    RichText,

    Date,
    DateTime,
    Decimal,
    Integer,
    Email,

    Checkboxes,
    DataList,
    RadioButtons,
    DropDown,

    InfoSection,

    AttachmentField,

    CompositeField
}

/**
 * Text input field types, including types applicable for multi-lingual text fields.
 * */
export enum TextType {
    ShortAnswer = FieldType.ShortAnswer,
    Paragraph = FieldType.Paragraph,
    RichText = FieldType.RichText,
    AttachmentField = FieldType.AttachmentField
}

/**
 * Mono-lingual text field types.
 * */
export enum MonolingualFieldType {
    Date = FieldType.Date,
    DateTime = FieldType.DateTime,
    Decimal = FieldType.Decimal,
    Integer = FieldType.Integer,
    Email = FieldType.Email
    
}

/**
 * Option field types.
 * */
export enum OptionFieldType {
    Checkboxes = FieldType.Checkboxes,
    DataList = FieldType.DataList,
    RadioButtons = FieldType.RadioButtons,
    DropDown = FieldType.DropDown
}

/**
 * Field types that does not take any user input.
 * */
export enum InfoSectionType{
    InfoSection = FieldType.InfoSection
}


export interface FileReference {
    id: Guid;
    fileName: string;
    originalFileName: string;
    thumbnail: string;
    contentType: string;
    size: number;
    created: Date;
    updated: Date;
    
    formDataId: Guid;
    fieldId: Guid;
}


export enum CompositeFieldType{
    CompositeField= FieldType.CompositeField
}
