import * as $ from "jquery"
import { FormFields } from "./FormFields.js"

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

interface Field {
    Description: string    
    FieldType: string
    IsRequired: boolean   
    Name: string    
    Options: string
    ParentType: string
}

declare var fieldTypes: any;
declare var metadataSetId: any;
declare var fieldList: any

export class MetadataSetFields extends FormFields {

    private fieldTypes: FieldTypes
    private fieldList: Field[]
    private metadataSetId: string
    private fieldEntryTemplate: string
    private previousName: string
    private previousDescription: string
    private previousOptions: string
    private selectedFieldType: string

    constructor() {
        super()
        this.initializeFieldTypes()
        this.populateSelect()
        this.setSelectOptionFromHidden()
        this.listenTemplateSelector()
        this.listenFieldTypeSelector()

        if (this.fieldsContainer.children().length == 0) {
            //this.addField()
        }
        
        this.populateFieldTypeSelector()
        this.populateExistingFields()
    }

    private initializeFieldTypes() {
        // window.fieldTypes and window.metadataSetId are provided by back end
        this.fieldTypes = fieldTypes
        this.metadataSetId = metadataSetId
        this.selectedFieldType = this.fieldTypes.fields[0].ModelType
    }

    private populateFieldTypeSelector() {
        let fieldTypeSelector: JQuery = $("#field-type-selector")
        for (let field of this.fieldTypes.fields) {
            $(fieldTypeSelector).append($('<option>', {
                value: field.ModelType,
                text: field.Label
            }))

        }
    }

    private populateExistingFields() {
        console.log(fieldList)
        this.fieldList = fieldList
        for (let field of this.fieldList) {
            this.selectedFieldType = field.FieldType
            this.addField()
            this.SetFieldValues(field)
        }
    }

    private SetFieldValues(field: Field) {
        // set value to last defined field
        console.log(field)
        $(".field-entry:last .template-selector").val(field.FieldType)
        $(".field-entry:last .field-name").val(field.Name)
        $(".field-entry:last .field-description").text(field.Description)
        $(".field-entry:last .field-options").text(field.Options)
        if (field.IsRequired) {
            $(".field-entry:last .field-is-required").prop('checked', true)
        }     
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



    private setSelectOptionFromHidden() {
        let fields: JQuery = $(".field-entry")

        fields.each((index, element) => {
            let value: string = $(element).children(".model-type").val()
            $(element).find(".template-selector").val(value)
        })
    }

    protected getTemplate(modelType: string): JQuery {
        let guid: string = this.getGUID()
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

    private savePreviousValues(source: JQuery) {
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

        this.savePreviousValues(closest)
        closest.replaceWith(template)
        template.find(".metadataset-id").attr("value", this.metadataSetId)
        this.bindElements()
        this.restorePreviousValues(template)
        template.find(".template-selector").val(selectedType)
        template.find(".model-type").val(selectedType)
    }

    

    protected addField() {
        //let template: JQuery = this.getTemplate(this.fieldTypes.fields[0].Template)
        //console.log(this.fieldTypes.fields[0].Template)
        console.log(this.selectedFieldType)
        //let template: JQuery = this.getTemplate(this.selectedFieldType)
        let template: JQuery = this.getTemplate(this.getTemplateType(this.selectedFieldType))

        this.fieldsContainer.append(template)
        template.find(".metadataset-id").attr("value", this.metadataSetId)
        this.bindElements()
        template.find(".template-selector").val(this.selectedFieldType)
        template.find(".model-type").val(this.selectedFieldType)
    }

    private listenTemplateSelector() {
        let templateSelectors: JQuery = $(".template-selector")

        templateSelectors.change((e) => {
            $(e.target).closest(".field-entry").prev().remove()
            this.setTemplate($(e.target))
        })
    }

    private listenFieldTypeSelector() {
        let fieldTypeSelector: JQuery = $("#field-type-selector")
        fieldTypeSelector.change((e) => {
            this.selectedFieldType = $(e.target).val()
            //this.selectedFieldType = "Catfish.Core.Models.Metadata.OptionsField"
        })
    }
}
