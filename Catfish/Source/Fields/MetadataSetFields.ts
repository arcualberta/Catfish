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
    templates: Templates
    fields: FieldDefinition[]
}

export class MetadataSetFields {

    private fieldTypes: FieldTypes
    private fieldTypesUrl: string
    private addButton: JQuery
    private removeFieldButtons: JQuery
    private fieldsContainer: JQuery
    private templateSelectors: JQuery
    private fieldEntryTemplate: string

    // XXX Move html templates to external file
    //Fields_0__
    //Fields[0]
    private singleTemplate: string = `
    <div class="field-entry">
    <ul class="form">
        <li>
            <label class="control-label col-md-2" for="Fields_0__Name"> Name </label>
            <div class="input">
                <input class="form-control text-box single-line" data-val="true" data-val-required="The Name field is required." id= "Fields_0__Name" name= "Fields[0].Name" type= "text" value= "">
                <span class="field-validation-valid text-danger" data-valmsg-for="Fields[0].Name" data-valmsg-replace="true" > </span>
            </div>
        </li>
        <li>
            <label class="control-label col-md-2" for="Fields_0__Description"> Description </label>
            <div class="input">
                <textarea class="form-control text-box multi-line" id="Fields_0__Description" name= "Fields[0].Description" ></textarea>
                <span class="field-validation-valid text-danger" data-valmsg-for="Fields[0].Description" data-valmsg - replace="true"> </span>
            </div>
        </li>
    </ul>
    </div>
    `

    private multipleTemplate: string = `
    <div class="field-entry">
    <ul class="form">
        <li>
            <label class="control-label col-md-2" for="Fields_0__Name"> Name </label>
            <div class="input">
                <input class="form-control text-box single-line" data-val="true" data-val-required="The Name field is required." id= "Fields_0__Name" name= "Fields[0].Name" type= "text" value= "" >
                <span class="field-validation-valid text-danger" data-valmsg-for="Fields[0].Name" data-valmsg-replace="true" > </span>
            </div>
        </li>
        <li>
            <label class="control-label col-md-2" for="Fields_0__Description"> Description </label>
            <div class="input">
                <textarea class="form-control text-box multi-line" id="Fields_0__Description" name= "Fields[0].Description" ></textarea>
                <span class="field-validation-valid text-danger" data-valmsg-for="Fields[0].Description" data-valmsg-replace="true"> </span>
            </div>
        </li>
        <li>
            <label class="control-label col-md-2" for="Fields_0__Options"> Options </label>
            <div class="input">
                <textarea class="form-control text-box multi-line" id= "Fields_0__Options" name= "Fields[0].Options"></textarea>
                <span class="field-validation-valid text-danger" data-valmsg-for="Fields[0].Options" data-valmsg-replace="true"> </span>
            </div>
        </li>
    </ul>
    </div>
    `

    constructor() {
        this.initializeFieldTypes()
        this.fieldTypesUrl = "/manager/metadata/fieldTypes"
        this.fetchFieldTypes()
        this.addButton = $("#add-field")
        this.removeFieldButtons = $(".remove-field")
        this.fieldsContainer = $("#fields-container")
        this.listenForAddButton()
    }

    private initializeFieldTypes() {
        this.fieldTypes = <FieldTypes>{}
        //let test: any = require("text/Templates/Input/single.html")
        //console.log(test)
        this.fieldTypes.templates = {
            single: this.singleTemplate,
            multiple: this.multipleTemplate
        }
    }

    private fetchFieldTypes() {
        $.getJSON(this.fieldTypesUrl, "",
            (data) => {
                this.fieldTypes.fields = data as FieldDefinition[]
                this.buildFieldEntryTemplate()
            }
        )
    }

    private getGUID() {
        function s4(): string {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    }

    /*
    function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
    }
    */

    private selectTemplate(templateType: string): string {
        if (templateType == "Catfish.Core.Models.Metadata.OptionsField") {
            return this.fieldTypes.templates.multiple
        }
        // "Catfish.Core.Models.Metadata.SimpleField"
        return this.fieldTypes.templates.single      
    }

    private buildFieldEntryTemplate() {
        // Remove template text from code
        this.fieldEntryTemplate = '<div class="field-entry-container"><select class="template-selector">'
      
        for (let field of this.fieldTypes.fields) {
            this.fieldEntryTemplate += "<option value ='" + field.Template + "'>"
            this.fieldEntryTemplate += field.Label + "</option>"
        }

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
            this.listenTemplateSelector()
            this.setNameIds()
        })
    }

    private setTemplate(target: JQuery) {
        let template: string = this.selectTemplate($(target).val())
        $(target).siblings(".field-area").html(template)
    }

    private listenTemplateSelector() {
        this.templateSelectors = $(".template-selector")
        this.setTemplate(this.templateSelectors.last())
        this.templateSelectors.change((e) => {
            this.setTemplate($(e.target))
        })
    }

    private listenForRemoveFieldButton() {
        this.removeFieldButtons = $(".remove-field")
        this.removeFieldButtons.click((e) => {
            e.target.closest(".field-entry-container").remove()
            this.setNameIds()
        })
    }

    private setNameIds() {
      
        console.log(this.fieldsContainer.children())
    }
}
