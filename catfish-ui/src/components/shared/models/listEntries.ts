import { Guid } from "guid-typescript";
import { eState } from "../constants";

export interface ListEntry{
    /**
     * A unique entry ID used for the UI purposes
     * */
     id: Guid;

     /**
      * Entry name
      */
    name: string;

    /**
      * Entry name
      */
    state: eState;

 
}

export interface FormEntry extends ListEntry {
    //formId: Guid;
    isRequired?: boolean;
}

export interface EntityEntry extends ListEntry
{
    title: string
    description: string
    created: Date
    updated: Date
}
