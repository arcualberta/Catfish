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
     * Is this option selected
     * */
    selected: boolean;

    /**
     * The text label of the option, can be presented in multiple languages.
     * */
    optionText: TextCollection;

    /**
     * Is this an option that would allow the user to type-in an addional text input?
     * */
    isExtendedInput: boolean;

    /**
     * If the user selected an extended option, should we force the user to input a value 
     * for the extended input field. 
     * */
    isExtendedInputRequired: boolean

}

