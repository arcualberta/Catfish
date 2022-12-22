import { eConstraintType, eFieldConstraint } from "@/components/shared/constants";
import { FieldConstraint } from "./FieldConstraint";

export type ConstraintType = FieldConstraint | FieldExpression;

export interface FieldExpression {
    expressionComponents: ConstraintType[],
    operators: eFieldConstraint[],
    type: eConstraintType.FieldExpression
}

export const createFieldExpression = (): FieldExpression =>{ 
    return {
        expressionComponents: [],
        operators: [],
        type: eConstraintType.FieldExpression
    } as FieldExpression
}