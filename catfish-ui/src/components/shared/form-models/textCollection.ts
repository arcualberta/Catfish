/**
 * Stores a collection of text strings expressed in a given set of languages.
 * This model can be used to store the value of a given text string in multiple translations 
 * or a set of text strings in the same language, or even a combinaiton of the above
 * depending on the reuqirements of the end use.
 * */

import { Guid } from "guid-typescript"
import { Text } from './text';

export interface TextCollection {
    /**
     * Unique ID
     * */
    id: Guid;

    /**
     * List of Text values encapsulated in this Text Collecton object.
     * */
    values: Text[]
}