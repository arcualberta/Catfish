export var SolrQuery;
(function (SolrQuery) {
    let AggregationOperator;
    (function (AggregationOperator) {
        AggregationOperator["AND"] = "AND";
        AggregationOperator["OR"] = "OR";
    })(AggregationOperator = SolrQuery.AggregationOperator || (SolrQuery.AggregationOperator = {}));
    class FieldConstraint {
        internalId;
        solrFieldName;
        valueConstraints;
        aggregationOperator;
        constructor(solrFieldName, aggregationOperator, internalId) {
            this.solrFieldName = solrFieldName;
            this.valueConstraints = [];
            this.aggregationOperator = aggregationOperator;
            this.internalId = internalId ? internalId : null;
        }
        buildQueryString() {
            const segments = [];
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
    SolrQuery.FieldConstraint = FieldConstraint;
    class QueryModel {
        internalId;
        queryConstraints;
        aggregationOperator;
        excludeIds;
        constructor(aggregationOperator, internalId) {
            this.internalId = internalId ? internalId : null;
            this.queryConstraints = [];
            this.aggregationOperator = aggregationOperator;
            this.excludeIds = [];
        }
        appendNewFieldConstraint(solrFieldName, filedValueOptions, aggregationOperator, internalId) {
            const fieldConstraint = new FieldConstraint(solrFieldName, aggregationOperator, internalId);
            filedValueOptions.forEach(val => fieldConstraint.valueConstraints.push({ value: val, selected: false }));
            this.queryConstraints.push(fieldConstraint);
            return fieldConstraint;
        }
        appendNewChildQueryModel(aggregationOperator, internalId) {
            const childQuery = new QueryModel(aggregationOperator, internalId);
            this.queryConstraints.push(childQuery);
            return childQuery;
        }
        buildQueryString() {
            const segments = [];
            this.queryConstraints.forEach(constraint => {
                const segment = constraint.buildQueryString();
                if (segment)
                    segments.push(segment);
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
    SolrQuery.QueryModel = QueryModel;
})(SolrQuery || (SolrQuery = {}));
//# sourceMappingURL=solrQuery.js.map