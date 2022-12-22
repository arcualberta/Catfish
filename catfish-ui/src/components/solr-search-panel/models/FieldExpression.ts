import { eConstraintType, eFieldConstraint } from "@/components/shared/constants";
import { eSolrBooleanOperators } from ".";
import { FieldConstraint } from "./FieldConstraint";

export type ConstraintType = FieldConstraint | FieldExpression;

export interface FieldExpression {
    expressionComponents: ConstraintType[],
    operators: eSolrBooleanOperators[],
    type: eConstraintType.FieldExpression
}

export const createFieldExpression = (): FieldExpression =>{ 
    return {
        expressionComponents: [],
        operators: [],
        type: eConstraintType.FieldExpression
    } as FieldExpression
}