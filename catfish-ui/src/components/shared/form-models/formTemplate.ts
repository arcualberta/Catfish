import { Guid } from "guid-typescript";
import { Field } from "./field";
import { TextCollection } from "./textCollection";

/**
 * Form model
 * */

export interface FormTemplate {
    /**
     * Unique form ID.
     * */
    id: Guid

    /**
     * Form name. Displayed to identify the form in form listings.
     * */
    name: string

    /**
     * Form description.
     * */
    description: string

    /**
     * List of fields in this form.
     * */
    fields: Field[]

}

export interface FormEntry {
    /**
     * A unique form-entry ID used for the UI purposes
     * */
    id: Guid;
    name: string;
    formId: Guid;
    isRequired?: boolean;
}

export interface FieldEntry {
    formId: Guid;
    fieldId: Guid;
}