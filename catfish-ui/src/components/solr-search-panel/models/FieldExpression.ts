import { eFieldConstraint } from "@/components/shared/constants";
import { FieldConstraint } from "./FieldConstraint";

export type ConstraintType = FieldConstraint | FieldExpression;

export class FieldExpression {
    expressionComponents: ConstraintType[];
    operators: eFieldConstraint[];

    constructor(expressionComponents: ConstraintType[], operators: eFieldConstraint[]) {
        this.expressionComponents = expressionComponents;
        this.operators = operators;
    }
}