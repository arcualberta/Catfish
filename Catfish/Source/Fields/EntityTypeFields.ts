import * as $ from "jquery"
import { FormFields } from "./FormFields.js"

/*
interface MetadataSet {
    label: string
    value: string
    chosen: boolean
}
*/

interface Field {
    Options: string;
    Id: number;
    Name: string;
    Description: string;
    IsRequired: boolean;
    ToolTip?: any;
    MetadataSetId: number;
}

interface Set {
    EntityTypes: any[];
    Fields: Field[];
    Id: number;
    Name: string;
    Description: string;
}

interface MetadataSet {
    sets: Set[];
    template: string;
}

declare var metadataSets: any

export class EntityTypeFields extends FormFields {

    private metadataSets: MetadataSet

    constructor() {
        super()
        // fetch definitions
        this.fetchMetdataSets()
    }

    private fetchMetdataSets() {
        this.metadataSets = metadataSets
        console.log(this.metadataSets)
    }

    protected addField() {
        let template: JQuery = this.getTemplate('')
        this.fieldsContainer.append(template)
        this.listenForRemoveFieldButton()
        this.populateSelect()
        this.listenMetadataSetChange()
    }

    private populateSelect() {
        let selectors: JQuery = $(".metadataset-selector")

        selectors.each((index, selector) => {
            if ($(selector).children("option").length == 0) {
                for (let set of this.metadataSets.sets) {
                    let option: string = "<option value='" + set.Id + "'>" + set.Name + "</option>"
                    $(selector).append(option)
                }
            }
        })
    }

    protected getTemplate(modelType: string): JQuery {
        let guid: string = this.getGUID()
        let hiddenGUID = '<input type="hidden" name="MetadataSets.Index" value="' + guid + '">'
        let template: string = hiddenGUID + this.metadataSets.template.replace(/CATFISH_GUID/g, guid)
        return $(template)
    }

    private listenMetadataSetChange() {
        let selectors: JQuery = $(".metadataset-selector")
        selectors.change((e: JQueryEventObject) => {
            //console.log($(e.target).val())
            let value: string = $(e.target).val()
            console.log(e.target)
            console.log($(e.target).closest(".metadataset-id"))
            $(e.target).closest(".metadataset-id").attr("value", value)
        })
    }


}