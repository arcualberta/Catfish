import { Guid } from "guid-typescript";

export interface ListEntry{
    /**
     * A unique entry ID used for the UI purposes
     * */
     id: Guid;

     /**
      * Entry name
      */
     name: string;
 
}

export interface FormEntry extends ListEntry {
    formId: Guid;
    isRequired?: boolean;
}

export interface EntityEntry extends ListEntry
{
    description: string
    created: Date
    updated: Date
}
