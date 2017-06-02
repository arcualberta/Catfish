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
    private previousName: string
    private previousDescription: string
    private previousOptions: string

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

    // from https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
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

    private savePrevioustValues(source: JQuery) {

        let options: JQuery = source.find(".field-options")
        this.previousName = source.find(".field-name").first().val()
        this.previousDescription = source.find(".field-description").first().val()
        if (options.length > 0) {
            this.previousOptions = options.first().val()
        }
            
    }

    private restorePreviousValues(template: JQuery) {
        let options: JQuery = template.find(".field-options")

        template.find(".field-name").first().val(this.previousName)
        template.find(".field-description").first().val(this.previousDescription)
        if (options.length > 0) {
            template.find(".field-options").first().val(this.previousOptions)
        }
        
    }

    private setTemplate(target: JQuery) {
        let selectedType: string = target.val()
        let template: JQuery = this.getTemplate(this.getTemplateType(selectedType))
        let closest: JQuery = target.closest(".field-entry")
        this.savePrevioustValues(closest)
        closest.replaceWith(template)
        template.find(".metadataset-id").attr("value", this.metadataSetId)
        this.bindElements()
        this.restorePreviousValues(template)
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
            $(e.target).closest(".field-entry").prev().remove()
            this.setTemplate($(e.target))
        })
    }

    private listenForRemoveFieldButton() {
        this.removeFieldButtons = $(".remove-field")
        this.removeFieldButtons.click((e) => {
            e.target.closest(".field-entry").remove()
        })
    }


}
