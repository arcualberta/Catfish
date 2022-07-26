import { Guid } from "guid-typescript"

import { TextCollection } from "./textCollection";
import { Text } from "./text";

/**
 * The model for field values
 *  */
export interface FieldData {
    /** 
     *  Unique ID.
     * */
    id: Guid

    /**
     * ID of the Field to which this FieldData object belongs.
     * */
    fieldId: Guid

    /**
     * Stores multilingual text values when this field-data object belongs to a 
     * multilingual text field (e.g. Short Answer, Paragraph, or Rich Text).
     * */
    multilingualTextValues: TextCollection[] | null

    /**
     * Stores monolingual text values when this field-data object belongs to a 
     * monolingual text field such as Email, Integer, Decimal, etc.
     * */
    monolingualTextValues: Text[] | null

    /** 
     * Stores the IDs of selected options when this field-data object belongs to an
     * option field such as Checkboxes, Radio Buttons, etc.
     * */
    selectedOptionIds: Guid[] | null

    /**
     * Stores custom option values when this field-data object belongs to an option
     * field that allows custom option values values.
     * */
    customOptionValues: string[] | null

    /**
     * Stores extended option values when this field-data object belongs to an option
     * field that has some options which takes extended values.
     * */
    extendedOptionValues: ExtendedOptionValue[]

}

export interface ExtendedOptionValue {
    /**
     * The ID of the option to which this Extended Option Value belongs.
     * */
    optionId: Guid

    /**
     * List of extended option values.
     * */
    values: string[]
}