import { eFieldConstraint } from "../../shared/constants";

export class FieldConstraint {
    field: string ;
    constraint: eFieldConstraint;
    value: [];

    constructor(field: string, constraint: eFieldConstraint, value: []) {
        this.field = field;
        this.constraint = constraint;
        this.value = [];
    }
}

