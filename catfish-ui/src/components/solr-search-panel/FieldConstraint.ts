import { eFieldConstraint } from "../shared/constants";

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

export type ConstraintType = FieldConstraint | FieldExpression;

export class FieldExpression {
    expressionComponents: ConstraintType[];
    operators: eFieldConstraint[];

    constructor(expressionComponents: ConstraintType[], operators: eFieldConstraint[]) {
        this.expressionComponents = expressionComponents;
        this.operators = operators;
    }
}