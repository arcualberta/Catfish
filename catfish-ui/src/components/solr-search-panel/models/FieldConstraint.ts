import { eConstraintType, eFieldConstraint } from "@/components/shared/constants";
import { constrainPoint } from "@fullcalendar/core";
import { SearchFieldDefinition } from ".";

export interface FieldConstraint {
    field: SearchFieldDefinition | null,
    constraint: eFieldConstraint | null,
    value: object | null,
    type: eConstraintType.FieldConstraint,
}

export const createFieldConstraint = (): FieldConstraint =>{ 
    return {
        field: null,
        constraint: eFieldConstraint.Equals,
        value: null,
        type: eConstraintType.FieldConstraint
    } as FieldConstraint
}