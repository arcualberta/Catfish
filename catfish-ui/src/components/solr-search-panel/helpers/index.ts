import { eConstraintType, eFieldConstraint } from "@/components/shared/constants";
import { SearchFieldDefinition, SolrFieldData, SolrResultEntry } from "../models";
import { FieldConstraint } from "../models/FieldConstraint";
import { FieldExpression } from "../models/FieldExpression";

export function buildQueryString(model: FieldExpression | FieldConstraint): string | null {
     
    if(model.type === eConstraintType.FieldExpression){
        console.log('eConstraintType.FieldExpression')

        //This is a field expression
        const expression = (model as FieldExpression);

        if(expression.expressionComponents.length === 0){
            return null;
        }
        else {
            let queryStr = buildQueryString(expression.expressionComponents[0]);
        
            for(let i=1; i<expression.expressionComponents.length; ++i){
                let nextQueryComponent = buildQueryString(expression.expressionComponents[i]);
                if(nextQueryComponent){
                    if(queryStr)
                        queryStr = `(${queryStr}) ${expression.operators[i-1]} (${nextQueryComponent})`
                    else
                        queryStr = nextQueryComponent;
                }
            }

            return queryStr;
        }
    }
    else{
        //This is a field constraint
        const fieldConstraint = (model as FieldConstraint);

        if(!fieldConstraint.field?.name){
            //No field has been selected, so ignore this field constraint.
            return null;
        }
        else if(fieldConstraint.constraint === eFieldConstraint.Equals && (!fieldConstraint.value || (fieldConstraint.value.toString())?.length == 0)){
            //Entries with the field value is not specified.
            return `-${fieldConstraint.field.name}:*`;
        }
        else{
            //Enforce specified constraint on the field value
            switch(fieldConstraint.constraint){
                case eFieldConstraint.Contains:
                    return `${fieldConstraint.field.name}:${fieldConstraint.value}`;
                case eFieldConstraint.Equals:
                    return `${fieldConstraint.field.name}:"${fieldConstraint.value}"`;
                case eFieldConstraint.NotEquals:
                    return `-${fieldConstraint.field.name}:"${fieldConstraint.value}"`;
                case eFieldConstraint.GreaterThan:
                    return `${fieldConstraint.field.name}:"{${fieldConstraint.value} TO *}"`;
                case eFieldConstraint.GreaterThanOrEqual:
                    return `${fieldConstraint.field.name}:"[${fieldConstraint.value} TO *]"`;
                case eFieldConstraint.LessThan:
                    return `${fieldConstraint.field.name}:"{* TO ${fieldConstraint.value}}"`;
                case eFieldConstraint.LessThanOrEqual:
                    return `${fieldConstraint.field.name}:"[* TO ${fieldConstraint.value}]"`;
                default:
                    return null; //Unsupported field constraint
            } 
        }
    }
}

export function toTableData(rows: SolrResultEntry[], fieldDefs: SearchFieldDefinition[]){
    const items: Record<string, any>[] = [];

    rows?.forEach((row) => {
        const item: Record<string, any> = {}
        fieldDefs?.forEach((def) => {
            item[def.label.replaceAll(' ', '_')] = row.data.find(d => d.key === def.name)?.value
        })
        items.push(item);
    })

    return items;
}

export function downloadCSV(rows: SolrResultEntry[], fieldDefs: SearchFieldDefinition[]) {
    let csv = '';
    //header labels
    fieldDefs.forEach(fldef => {
        csv += fldef.label + ','
    });

    //Replacing the last comma and adding a new line at the end
    csv = csv.replace(/,\s*$/, '') + '\n'

     //data
    rows.forEach((row: SolrResultEntry) => {
        let csv_line = '';
        fieldDefs.forEach(def => {
            csv_line += row.data.find(d => d.key === def.name)?.value + ',';
        });
        csv += csv_line.replace(/,\s*$/, '') + '\n';
    });
 
    const anchor = document.createElement('a');
    anchor.href = 'data:text/csv;charset=utf-8,' + encodeURIComponent(csv);
    anchor.target = '_blank';
    anchor.download = 'search_results.csv';
    anchor.click();
}
