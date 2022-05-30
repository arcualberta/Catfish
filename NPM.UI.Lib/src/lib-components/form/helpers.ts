import * as models from './models'

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

