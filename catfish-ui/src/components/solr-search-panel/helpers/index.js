import { eConstraintType, eFieldConstraint, eFieldType } from "@/components/shared/constants";
export function buildQueryString(model) {
    if (model.type === eConstraintType.FieldExpression) {
        console.log('eConstraintType.FieldExpression');
        //This is a field expression
        const expression = model;
        if (expression.expressionComponents.length === 0) {
            return null;
        }
        else {
            let queryStr = buildQueryString(expression.expressionComponents[0]);
            for (let i = 1; i < expression.expressionComponents.length; ++i) {
                let nextQueryComponent = buildQueryString(expression.expressionComponents[i]);
                if (nextQueryComponent) {
                    if (queryStr)
                        queryStr = `(${queryStr}) ${expression.operators[i - 1]} (${nextQueryComponent})`;
                    else
                        queryStr = nextQueryComponent;
                }
            }
            return queryStr;
        }
    }
    else {
        //This is a field constraint
        const fieldConstraint = model;
        if (!fieldConstraint.field?.name) {
            //No field has been selected, so ignore this field constraint.
            return null;
        }
        else if ((fieldConstraint.constraint === eFieldConstraint.Equals || fieldConstraint.constraint === eFieldConstraint.Contains)
            && (!fieldConstraint.value || (fieldConstraint.value.toString())?.length == 0)) {
            //Limit the result to entries with value is not specified for the given field.
            //return `-${fieldConstraint.field.name}:*`;
            return `*:* NOT ${fieldConstraint.field.name}:*`;
        }
        else if (fieldConstraint.value) {
            //Enforce specified constraint on the field value
            switch (fieldConstraint.constraint) {
                case eFieldConstraint.Contains:
                    return `${fieldConstraint.field.name}:${getFieldConstraintValue(fieldConstraint)}`;
                case eFieldConstraint.Equals:
                    return `${fieldConstraint.field.name}:"${getFieldConstraintValue(fieldConstraint)}"`;
                case eFieldConstraint.NotEquals:
                    return `-${fieldConstraint.field.name}:"${getFieldConstraintValue(fieldConstraint)}"`;
                case eFieldConstraint.GreaterThan:
                    return `${fieldConstraint.field.name}:{${getFieldConstraintValue(fieldConstraint)} TO *}`;
                case eFieldConstraint.GreaterThanOrEqual:
                    return `${fieldConstraint.field.name}:[${getFieldConstraintValue(fieldConstraint)} TO *]`;
                case eFieldConstraint.LessThan:
                    return `${fieldConstraint.field.name}:{* TO ${getFieldConstraintValue(fieldConstraint)}}`;
                case eFieldConstraint.LessThanOrEqual:
                    return `${fieldConstraint.field.name}:[* TO ${getFieldConstraintValue(fieldConstraint)}]`;
                default:
                    return null; //Unsupported field constraint
            }
        }
        else {
            return null; //Field is selecyted byt no value is specified, so ignore
        }
    }
}
export function getFieldConstraintValue(fieldConstraint) {
    if (fieldConstraint.field?.type == eFieldType.Date) {
        return fieldConstraint.value ? `${fieldConstraint.value}T00:00:00Z` : null;
    }
    return fieldConstraint.value;
}
export function toTableData(rows, fieldDefs, requestedResultFieldNames, enableEditing, editPage) {
    const items = [];
    const tableHeadingDefs = requestedResultFieldNames?.length > 0
        ? requestedResultFieldNames.map(name => fieldDefs.filter(fd => fd.name === name)[0]) //fieldDefs.filter(fd => requestedResultFieldNames.indexOf(fd.name) >= 0)
        : fieldDefs;
    //    const tableHeadingDefs = requestedResultFieldNames?.length > 0
    //        ? requestedResultFieldNames.map(label => fieldDefs.find(fd => fd.label === label)); // fieldDefs.filter(fd => requestedResultFieldNames.indexOf(fd.name) >= 0)
    //        : fieldDefs;
    rows?.forEach((row) => {
        const item = {};
        tableHeadingDefs?.forEach((def) => {
            item[def.label.replaceAll(' ', '_')] = row.data.find(d => d.key === def.name)?.value;
        });
        if (enableEditing) {
            item['id'] = row.id;
        }
        items.push(item);
    });
    return items;
}
export function downloadCSV(rows, fieldDefs) {
    let csv = '';
    //header labels
    fieldDefs.forEach(fldef => {
        csv += fldef.label + ',';
    });
    //Replacing the last comma and adding a new line at the end
    csv = csv.replace(/,\s*$/, '') + '\n';
    //data
    rows.forEach((row) => {
        let csv_line = '';
        fieldDefs.forEach(def => {
            let val = row.data.find(d => d.key === def.name)?.value;
            if (!val) {
                //No value is there to add.
            }
            else if (typeof val == 'string') {
                val = val.replace(/"/g, '""'); //replace any double quotes with two double quotes
                csv_line += `"${val}"`;
            }
            else if (Array.isArray(val)) {
                csv_line += `"${val.join()}"`; //Join arrays by commas and append the result as a string value
            }
            else {
                csv_line += val; //append the value as is
            }
            csv_line += ','; //append a comma after appending the value to the csv line
        });
        csv += csv_line.replace(/,\s*$/, '') + '\n';
    });
    const anchor = document.createElement('a');
    anchor.href = 'data:text/csv;charset=utf-8,' + encodeURIComponent(csv);
    anchor.target = '_blank';
    anchor.download = 'search_results.csv';
    anchor.click();
}
export function copyToClipboard(text) {
    var dummy = document.createElement("textarea");
    // to avoid breaking orgain page when copying more words
    // cant copy when adding below this code
    // dummy.style.display = 'none'
    document.body.appendChild(dummy);
    //Be careful if you use texarea. setAttribute('value', value), which works with "input" does not work with "textarea". – Eduard
    dummy.value = text;
    dummy.select();
    document.execCommand("copy");
    document.body.removeChild(dummy);
}
//# sourceMappingURL=index.js.map