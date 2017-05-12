import * as ko from "knockout"
import { MetadataField } from "./MetadataField.js"

export class MetadataSet {
    id: number
    name: string
    description: string
    metadataFields: KnockoutObservableArray<MetadataField>

    constructor() {
        this.name = ""
        this.description = ""
        this.metadataFields = ko.observableArray([])
    }

    addField() {
        this.metadataFields.push(new MetadataField())
    }

    removeField() {

    }

    show() {
        console.log(this.metadataFields())
    }
}