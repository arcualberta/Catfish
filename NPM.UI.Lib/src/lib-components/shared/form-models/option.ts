/**
 * A model that represents an option used in option fields
 * */

import { Guid } from "guid-typescript"
import { TextCollection } from "./textCollection";

export interface Option {
    /**
     * Unique ID
     * */
    id: Guid;

    /**
     * The text label of the option, can be presented in multiple languages.
     * */
    optionText: TextCollection;

    /**
     * Is this an option that would allow the user to type-in an addional text input?
     * */
    isExtendedInput: ExtensionType | null;
}

/**
 * Specifies whether the user should be presented with an option to input
 * a text value when this option is selected.
 * */
export enum ExtensionType {
    /**
     * No extended input is provided.
     * */
    None,

    /**
     * The User can optionally provide a text input.
     * */
    Optional,

    /**The user is required to provide a text input.
     */
    Required

}