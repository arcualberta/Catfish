/**
 * Basic model that stores a string in a given language
 * */

import { Guid } from "guid-typescript"
import { TextType } from "./field";

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
     * The type of text input taken by this Text object. */
    textType: TextType | null;

    /**
     * Language
     * */
    lang: string | null;
}