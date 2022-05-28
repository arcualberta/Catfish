import { Option } from './entityModels'

export class OptionsFieldMethods {

    public static getSelectedFieldLabels(options: Option[]) {
        return options?.filter(opt => opt.selected)
            .map(opt => opt.optionText?.values.$values
                .map(txt => txt.value)
                .join(" / ")
            )
            .join(", ")
    }
}
