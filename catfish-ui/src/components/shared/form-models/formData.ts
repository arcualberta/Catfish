
/**
 * The Form Data model which carries the submission data of a form.
 * */

import { Guid } from "guid-typescript"
import { eState } from "../constants";

import { FieldData } from './fieldData'

export interface FormData {
    /**
     * Unique form-data object ID.
     * */
    id: Guid;

    /**
     * The ID of the form
     * */
    formId: Guid

    /**
     * Created timestamp
     * */
    created: Date;

    /**
     * Last updated timestamp
     * */
    updated: Date;

    /**
     * List of field-data objects belongs to this form-data object. Each field-data
     * instance holds data submitted for a field in the parent form.
     * */
    fieldData: FieldData[];

    state: eState


}