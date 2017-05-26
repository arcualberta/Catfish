import * as $ from "jquery"

interface Templates {
    single: string;
    multiple: string;
}

interface Field {
    type: string;
    label: string;
    template: string;
}

interface FieldTypes {
    templates: Templates;
    fields: Field[];
}
export class MetadataSetFields {

    private fieldTypes: FieldTypes
    private fieldTypesUrl: string
    private addButton: JQuery
    private removeFieldButtons: JQuery
    private fieldsContainer: JQuery

    private fieldEntryTemplate: string = `
        <div class="field-container">
            <select></select>
            <button>X</button>
            <div class="field-area">
            <div>
        </div>

    `

    private data: string = `
    {
        "templates": {
            "single": "single template text",
            "multiple": "multiple template text"
        },
        "fields": [
            {
                "type": "Catfish.Core.Models.Metadata.CheckBoxSet",
                "label": "Check box",
                "template": "multiple"
            },
            {
                "type": "Catfish.Core.Models.Metadata.DateField",
                "label": "Check box",
                "template": "single"
            },
            {
                "type": "Catfish.Core.Models.Metadata.DropDownMenu",
                "label": "Check box",
                "template": "multiple"
            },
            {
                "type": "Catfish.Core.Models.Metadata.RadioButtonSet",
                "label": "Check box",
                "template": "multiple"
            },
            {
                "type": "Catfish.Core.Models.Metadata.TextArea",
                "label": "Check box",
                "template": "single"
            },
            {
                "type": "Catfish.Core.Models.Metadata.TextField",
                "label": "Check box",
                "template": "single"
            }
        ]
    }`

    constructor() {
        this.fieldTypesUrl = "/manager/metadata/fieldTypes"
        this.fetchFieldTypes()
        this.addButton = $("#add-field")
        this.removeFieldButtons = $(".remove-field")
        this.fieldsContainer = $("#fields-container")
        this.listenForAddButton()
        console.log(this.fieldTypes)
    }

    private fetchFieldTypes() {

        //$.getJSON(this.fieldTypesUrl, "",
        //    (data) => {
        //        this.fieldTypes = data as FieldTypes
        //    }
        //)

        this.fieldTypes = JSON.parse(this.data) as FieldTypes

    }

    private listenForAddButton() {
        this.addButton.click((e) => {
            console.log(e)
            this.fieldsContainer.append("<div>added</div>")
        })
    }

    private listenForRemoveFieldButton() {
        this.removeFieldButtons = $(".removeField")
        this.removeFieldButtons.click((e) => {
            console.log("removing " + e)
        })
    }
}

//private fetchFieldTypes() {
//    $.getJSON("/manager/metadata/fieldTypes", "",
//        (data) => {
//            // Should DisplayType for input be fixed on back end ?
//            //for (let type: MetadataFieldType in data) {
//            let fieldTypes: Array<MetadataFieldType> = data as Array<MetadataFieldType>;
//            for (let type of fieldTypes) {
//                for (let property of type.Properties) {
//                    if (property.DisplayType == "") {
//                        property.DisplayType = "Line"
//                    }
//                }
//            }
//            this.fieldTypes = ko.observableArray(fieldTypes);
//        }
//    );
//}