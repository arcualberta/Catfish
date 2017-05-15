
import { MetadataField } from "./MetadataField.js"

export class MetadataOptionsField extends MetadataField {
    options: KnockoutObservableArray<string>

    constructor(id: number = 0, name: string = "", description: string = "", isRequired: boolean = false, type: string = "", options: string[]) {
        super(id, name, description, isRequired, type);
        this.options = ko.observableArray(options);
    }

    addOption(option: string) {
        this.options.push(option);
    }

    removeOption() {

    }

    //removeOption
}