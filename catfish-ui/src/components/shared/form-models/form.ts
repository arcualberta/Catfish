import { Guid } from "guid-typescript";
import { Field } from "./field";

/**
 * Form model
 * */

export interface Form {
    /**
     * Unique field ID.
     * */
    id: Guid

    /**
     * Form name. Displayed to identify the form in form listings.
     * */
    name: string;

    /**
     * Form description.
     * */
    description: string | null;

    /**
     * List of fields in this form.
     * */
    fields: Field[];

}

