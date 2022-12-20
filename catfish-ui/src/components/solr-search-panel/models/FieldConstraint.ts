import { eConstraintType, eFieldConstraint } from "@/components/shared/constants";

export class FieldConstraint {
    field: string | null;
    constraint: eFieldConstraint | null;
    value: object | null;

    constructor() {
        this.field = "";
        this.constraint = eFieldConstraint.Equals;
        this.value = null;
    }

    getType = () => eConstraintType.FieldConstraing;

    buildQueryString(): string | null {

        //Reference: https://solr.apache.org/guide/solr/latest/query-guide/standard-query-parser.html

        if(!(this.field && this.constraint && this.value))
            return null;

        switch(this.constraint){
            case eFieldConstraint.Contains:
            case eFieldConstraint.Equals:
                return `${this.field}:"${this.value}"`;
            case eFieldConstraint.NotEquals:
                return `-${this.field}:"${this.value}"`;
            case eFieldConstraint.GreaterThan:
                return `${this.field}:"{${this.value} TO *}"`;
            case eFieldConstraint.GreaterThanOrEqual:
                return `${this.field}:"[${this.value} TO *]"`;
            case eFieldConstraint.LessThan:
                return `${this.field}:"{* TO ${this.value}}"`;
            case eFieldConstraint.LessThanOrEqual:
                return `${this.field}:"[* TO ${this.value}]"`;
            case eFieldConstraint.Between:
                const vals = this.value as object[]
                return `${this.field}:"[${vals[0]} TO ${vals[1]}]"`;
            default:
                return null;
        }
    }

}
