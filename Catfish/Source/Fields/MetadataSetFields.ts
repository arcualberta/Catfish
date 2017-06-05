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

    private fieldEntryTemplate: string

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
        this.buildFieldEntryTemplate()
        this.fieldTypesUrl = "/manager/metadata/fieldTypes"
        this.fetchFieldTypes()
        this.addButton = $("#add-field")
        this.removeFieldButtons = $(".remove-field")
        this.fieldsContainer = $("#fields-container")
        this.listenForAddButton()
    }

    private fetchFieldTypes() {

        //$.getJSON(this.fieldTypesUrl, "",
        //    (data) => {
        //        this.fieldTypes = data as FieldTypes
        //    }
        //)
        
        this.fieldTypes = JSON.parse(this.data) as FieldTypes

    }

    private buildFieldEntryTemplate() {
        this.fieldEntryTemplate = '<div class="field-container"><select>'
           

        this.fieldEntryTemplate += `
            </select>
            <button type="button" class='remove-field'>X</button>
            <div class="field-area">
            <div>
        </div>
    `}

    private listenForAddButton() {
        this.addButton.click((e) => {
            this.fieldsContainer.append(this.fieldEntryTemplate)
            this.listenForRemoveFieldButton()
        })
    }

    private listenForRemoveFieldButton() {
        this.removeFieldButtons = $(".remove-field")
        this.removeFieldButtons.click((e) => {
            e.target.parentElement.remove()
            console.log(e)
        })
    }
}
