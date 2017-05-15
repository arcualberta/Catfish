import * as ko from "knockout";
import { MetadataFieldType } from "./MetadataFieldTypeDefinition"
//import * as fieldDefinitions from "./MetadataFieldTypeDefinition";

//export enum MetadataFieldType {
//    text,
//    paragraph,
//    checkboxes,
//    dropdown,
//    date,
//    time
//}

export class MetadataField {
    id: number;
    name: string;
    description: string;
    type: KnockoutObservable<MetadataFieldType>;
    isRequired: boolean;

    constructor(id: number = 0, name: string = "", description: string = "", isRequired: boolean = false, type: string = "") {
        this.id = id;
        this.name = name;
        this.description = description;
        this.isRequired = isRequired;
        this.type = ko.observable(<MetadataFieldType>{ ModelType: "" });
    }
}
