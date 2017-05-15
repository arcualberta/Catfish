import * as ko from "knockout"
import * as $ from "jquery"
import { MetadataField } from "./MetadataField.js"
import { MetadataFieldType } from "./MetadataFieldTypeDefinition"

export class MetadataSet {
    id: number
    name: string
    description: string
    metadataFields: KnockoutObservableArray<MetadataField>
    fieldTypes: KnockoutObservableArray<MetadataFieldType>

    constructor() {
        this.name = "";
        this.description = "";
        this.metadataFields = ko.observableArray([]);

        // fetch FieldTypes
        // /manager/metadata/fieldTypes
       // $.getJSON("/manager/metadata/fieldTypes", "", (data) => { console.log(data) });
        this.fetchFieldTypes();
        // fetch MetadataSet data when editing
    }

    addField() {
        console.log("test")
        this.metadataFields.push(new MetadataField())
    }

    removeField = (fieldToRemove: MetadataField) => {
        this.metadataFields.remove(fieldToRemove);
    }
    
    show() {
        console.log(this.metadataFields())
        console.log(this.fieldTypes())
    }

    private fetchFieldTypes() {
        $.getJSON("/manager/metadata/fieldTypes", "",
            (data) => {
                this.fieldTypes = ko.observableArray(data as Array<MetadataFieldType>);
            }
        );
    }

}