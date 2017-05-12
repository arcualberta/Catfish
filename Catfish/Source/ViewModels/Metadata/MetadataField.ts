import * as ko from "knockout";

export enum MetadataFieldType {
    text,
    paragraph,
    checkboxes,
    dropdown,
    date,
    time
}

export class MetadataField {
    id: number;
    name: string;
    description: string;
    type: KnockoutObservable<MetadataFieldType>;
    isRequired: boolean;

    constructor(id: number = 0, name: string = "", description: string = "", isRequired: boolean = false, type: MetadataFieldType = MetadataFieldType.text) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.isRequired = isRequired;
        this.type = ko.observable(type);
    }
}
