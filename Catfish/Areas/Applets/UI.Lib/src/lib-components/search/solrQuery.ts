﻿
export namespace SolrQuery {


    export enum AggregationOperator {
        AND = "AND",
        OR = "OR"
    }

    export interface ValueConstraint {
        value: string;
        selected: boolean;
    }

    export class FieldConstraint {
        internalId: string | null;
        solrFieldName: string;
        valueConstraints: ValueConstraint[];
        aggregationOperator: AggregationOperator

        constructor(solrFieldName: string, aggregationOperator: AggregationOperator, internalId?: string) {
            this.solrFieldName = solrFieldName;
            this.valueConstraints = [];
            this.aggregationOperator = aggregationOperator;
            this.internalId = internalId ? internalId : null;
        }

        buildQueryString(): string {
            const segments = [] as string[];
            this.valueConstraints.forEach(valConst => {
                if (valConst.selected)
                    segments.push(`${this.solrFieldName}:${valConst.value}`);
            });
            const joinResult = segments.join(` ${this.aggregationOperator} `);
            return this.aggregationOperator === AggregationOperator.AND ? joinResult : `(${joinResult})`;
        }
    }

    export type QueryConstraintType = FieldConstraint | QueryModel;

    export class QueryModel {
        internalId: string | null;
        queryConstraints: QueryConstraintType[];
        aggregationOperator: AggregationOperator;

        constructor(aggregationOperator: AggregationOperator, internalId?: string) {
            this.internalId = internalId ? internalId : null;
            this.queryConstraints = [];
            this.aggregationOperator = aggregationOperator;
        }

        appendNewFieldConstraint(solrFieldName: string, filedValueOptions: string[], aggregationOperator: AggregationOperator, internalId: string): FieldConstraint {
            const fieldConstraint = new FieldConstraint(solrFieldName, aggregationOperator, internalId);

            filedValueOptions.forEach(val =>
                fieldConstraint.valueConstraints.push({ value: val, selected: false } as ValueConstraint)
            );

            this.queryConstraints.push(fieldConstraint);
            return fieldConstraint;
        }

        appendNewChildQueryModel(aggregationOperator: AggregationOperator, internalId: string): QueryModel {
            const childQuery = new QueryModel(aggregationOperator, internalId);
            this.queryConstraints.push(childQuery);
            return childQuery;
        }

        buildQueryString(): string {
            const segments = [] as string[];

            this.queryConstraints.forEach(constraint =>
                segments.push(constraint.buildQueryString())
            );

            const joinResult = segments.join(` ${this.aggregationOperator} `);
            return this.aggregationOperator === AggregationOperator.AND ? joinResult : `(${joinResult})`;
        }
    }

}