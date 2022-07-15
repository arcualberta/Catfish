/**
 * Basic model that stores a string in a given language
 * */

import { Guid } from "guid-typescript"

export interface Text {

    /**
     * Unique ID
     * */
    id: Guid;

    /**
     * The text string enclosed in this object
     * */
    value: string | null;

    /**
     * Language
     * */
    lang: string | null;
}