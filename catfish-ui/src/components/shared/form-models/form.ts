import { Guid } from "guid-typescript";
import { Field } from "./field";
import { TextCollection } from "./textCollection";

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
    name: TextCollection;

    /**
     * Form description.
     * */
    description: TextCollection;

    /**
     * List of fields in this form.
     * */
    fields: Field[];

}

