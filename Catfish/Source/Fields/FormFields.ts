import * as $ from "jquery"

export class FormFields {

    protected fieldsContainer: JQuery
    private removeFieldButtons: JQuery
    protected addButton: JQuery
    
    constructor() {
        this.addButton = $("#add-field")
        this.removeFieldButtons = $(".remove-field")
        this.fieldsContainer = $("#fields-container")
        this.listenForAddButton()
        this.listenForRemoveFieldButton()
    }

    // from https://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
    protected getGUID() {
        function s4(): string {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }

        return s4() + s4() + '_' + s4() + '_' + s4() + '_' +
            s4() + '_' + s4() + s4() + s4();
    }

    protected listenForRemoveFieldButton() {
        this.removeFieldButtons = $(".remove-field")
        this.removeFieldButtons.click((e) => {
            $(e.target).closest(".field-entry").prev().remove()
            e.target.closest(".field-entry").remove()
        })
    }

    protected listenForAddButton() {
        this.addButton.click((e) => {
            this.addField()
        })
    }

    protected addField() {
        console.log("aqui")
    }

    protected getTemplate(modelType: string): JQuery {
        return $('')
    }

    
}