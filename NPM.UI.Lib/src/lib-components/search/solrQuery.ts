import { Guid } from "guid-typescript";

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

        buildQueryString(): string | null {
            const segments = [] as string[];
            this.valueConstraints.forEach(valConst => {
                if (valConst.selected) {
                    if (this.solrFieldName.endsWith("_s") || this.solrFieldName.endsWith("_ss") || this.solrFieldName.endsWith("_ts"))
                        segments.push(`${this.solrFieldName}:"${valConst.value}"`);
                    else
                        segments.push(`${this.solrFieldName}:${valConst.value}`);

                }
            });

            if (segments.length === 0)
                return null;
            else {
                const joinResult = segments.join(` ${this.aggregationOperator} `);
                return this.aggregationOperator === AggregationOperator.AND ? joinResult : `(${joinResult})`;
            }
        }
    }

    export type QueryConstraintType = FieldConstraint | QueryModel;

    export class QueryModel {
        internalId: string | null;
        queryConstraints: QueryConstraintType[];
        aggregationOperator: AggregationOperator;
        excludeIds: Guid[];

        constructor(aggregationOperator: AggregationOperator, internalId?: string) {
            this.internalId = internalId ? internalId : null;
            this.queryConstraints = [];
            this.aggregationOperator = aggregationOperator;
            this.excludeIds = [];
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

            this.queryConstraints.forEach(constraint => {
                const segment = constraint.buildQueryString();
                if (segment)
                    segments.push(segment)
            });

            let joinResult = segments.join(` ${this.aggregationOperator} `);
            joinResult = this.aggregationOperator === AggregationOperator.AND ? joinResult : `(${joinResult})`;

            if (this.excludeIds?.length > 0) {
                //console.log("Exclude IDs: ", JSON.stringify(this.excludeIds))
                const excludeIdConstraints = this.excludeIds.map(id => `!(id:${id})`).join(" AND ");

                joinResult = `${joinResult} AND ${excludeIdConstraints}`;
            }
            return joinResult;
        }
    }

}