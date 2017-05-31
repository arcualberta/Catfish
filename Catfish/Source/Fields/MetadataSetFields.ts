import * as $ from "jquery"

interface Templates {
    single: string
    multiple: string
}

interface FieldDefinition {
    Label: string
    ModelType: string
    Template: string
}

interface FieldTypes {
    templates: any // Templates interfaces do not allow dots in key names
    fields: FieldDefinition[]
}

declare var fieldTypes: any;
declare var metadataSetId: any;

export class MetadataSetFields {

    private fieldTypes: FieldTypes
    private metadataSetId: string
    private addButton: JQuery
    private removeFieldButtons: JQuery
    private fieldsContainer: JQuery
    //private templateSelectors: JQuery
    private fieldEntryTemplate: string

    constructor() {
        this.initializeFieldTypes()
        this.populateSelect()
        this.addButton = $("#add-field")
        this.removeFieldButtons = $(".remove-field")
        this.fieldsContainer = $("#fields-container")
        this.listenForAddButton()
        this.setSelectOptionFromHidden()
        this.listenTemplateSelector()
        this.listenForRemoveFieldButton()
    }

    private initializeFieldTypes() {
        // window.fieldTypes is provided by back end
        this.fieldTypes = fieldTypes
        this.metadataSetId = metadataSetId
    }

    private populateSelect() {
        let templateSelectors: JQuery = $(".template-selector")

        templateSelectors.each((index, templateSelector) => {
            if ($(templateSelector).children("option").length == 0) {
                for (let field of this.fieldTypes.fields) {
                    let option: string = "<option value='"+field.ModelType+"'>" + field.Label + "</option>"
                    $(templateSelector).append(option)
                }
            } 
        })
    }

    private getGUID() {
        function s4(): string {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '_' + s4() + '_' + s4() + '_' +
            s4() + '_' + s4() + s4() + s4();
    }

    private setSelectOptionFromHidden() {
        let fields: JQuery = $(".field-entry")
        fields.each((index, element) => {
            let value: string = $(element).children(".model-type").val()
            $(element).find(".template-selector").val(value)
        })
    }

    private getTemplate(modelType: string): JQuery {
        let guid: string = this.getGUID()
        //let template: string = this.fieldTypes.templates[modelType].replace(/CATFISH_GUID/g, guid)
        let hiddenGUID = '<input type="hidden" name="Fields.Index" value="' + guid + '">'
        let template: string = hiddenGUID + this.fieldTypes.templates[modelType].replace(/CATFISH_GUID/g, guid)
        return $(template)
    }

    private bindElements() {
        this.populateSelect()
        this.listenForRemoveFieldButton()
        this.listenTemplateSelector()
    }
    
    private getTemplateType(modelType: string): string {
        for (let field of this.fieldTypes.fields) {
            if (field.ModelType == modelType) {
                return field.Template
            } 
        }
    }
    
    private setTemplate(target: JQuery) {
        let selectedType: string = target.val()
        let template: JQuery = this.getTemplate(this.getTemplateType(selectedType))
        target.closest(".field-entry").replaceWith(template)
        template.find(".metadataset-id").attr("value", this.metadataSetId)
        this.bindElements()
        template.find(".template-selector").val(selectedType)
        template.find(".model-type").val(selectedType)
    }

    private listenForAddButton() {
        this.addButton.click((e) => {
            // add new guid
            let template: JQuery = this.getTemplate(this.fieldTypes.fields[0].Template)
            this.fieldsContainer.append(template)
            template.find(".metadataset-id").attr("value", this.metadataSetId)
            this.bindElements()
            template.find(".model-type").val(this.fieldTypes.fields[0].ModelType)
        })
    }

    private listenTemplateSelector() {

        let templateSelectors: JQuery = $(".template-selector")
        templateSelectors.change((e) => {
            console.log($(e.target).closest(".field-entry").prev())
            $(e.target).closest(".field-entry").prev().remove()
            // save values
            this.setTemplate($(e.target))
            // restore values
        })
    }

    private listenForRemoveFieldButton() {
        this.removeFieldButtons = $(".remove-field")
        this.removeFieldButtons.click((e) => {
            e.target.closest(".field-entry").remove()
        })
    }


}
