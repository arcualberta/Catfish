import { eFieldConstraint } from "@/components/shared/constants";
import { FieldConstraint } from "./FieldConstraint";

export type ConstraintType = FieldConstraint | FieldExpression;

export class FieldExpression {
    expressionComponents: ConstraintType[];
    operators: eFieldConstraint[];

    constructor() {
        this.expressionComponents = [];
        this.operators = [];
    }

    addConstraint(constraint: ConstraintType, operator: eFieldConstraint | undefined){
        if(this.expressionComponents.length > 0 && !operator)
            throw "You must specify an operator";

        this.expressionComponents.push(constraint);
        if(operator)
            this.operators.push(operator);
    }

    buildQueryString(): string | null {
     
        if(this.expressionComponents.length === 0)
            return null;

        let queryStr = this.expressionComponents[0].buildQueryString();
        if(queryStr)
            queryStr = `(${queryStr})`;
 
        for(let i=1; i<this.expressionComponents.length; ++i){
            let nextQueryComponent = this.expressionComponents[i].buildQueryString();
            if(nextQueryComponent){
                if(queryStr)
                    queryStr = `${queryStr} ${this.operators[i-1]} (${nextQueryComponent})`
                else
                    queryStr = nextQueryComponent;
            }
        }

        return queryStr;
    }
}