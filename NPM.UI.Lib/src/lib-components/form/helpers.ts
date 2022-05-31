import * as models from './models'
import { eFieldType, eDataElementType } from './enumerations'

export const getFieldName = (obj: models.Field | models.FieldContainer): string => {
    return obj?.name?.values?.$values
        .map(txt => txt.value)
        .join(" | ") as string;
}

export const getSelectedFieldLabels = (options: models.Option[]): string => {
    return options?.filter(opt => opt.selected)
        .map(opt => opt.optionText?.values.$values
            .map(txt => txt.value)
            .join(" / ")
        )
        .join(", ")
}

export const getTypeString = (obj: models.Field | models.Text | models.Option | models.FileReference): string => {
    const typeName: string = obj?.$type.substring(0, obj.$type.indexOf(","));
    return typeName?.substring(typeName.lastIndexOf(".") + 1);
}

export const getFieldType = (field: models.Field): eFieldType => {
    const typeName: string = getTypeString(field);
    return (<any>eFieldType)[typeName];
}

export const testFieldType = (field: models.Field, type: eFieldType): boolean => {
    return getFieldType(field) === type;
}

export const getDataElementType = (obj: models.DataElementType): eDataElementType => {
    const typeName: string = getTypeString(obj);
    return (<any>eDataElementType)[typeName];
}