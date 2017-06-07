import * as $ from "jquery"
import { FormFields } from "./FormFields.js"

interface MetadataSet {
    label: string
    value: string
    chosen: boolean
}

export class EntityTypeFields extends FormFields {

    private metadatasets: MetadataSet[]
    //private templateString: string = "<div>template</div>"
    private templateString: string = `
    <div class="box field-entry">
        <div class="inner">
            <button type="button" class="remove-field">x</button>
            <input data-val="true" data-val-number="The field Id must be a number." data-val-required="The Id field is required." id="MetadataSets_0__Id" name="MetadataSets[0].Id" type="hidden" value="1">
            1: <label for="MetadataSets_0__New_metadata_set_ed">New metadata set ed</label>
        </div> 
    </div>
    `


    constructor() {
        super()
        // fetch definitions
        this.fetchMetdataSets()
    }

    private fetchMetdataSets() {
        this.metadatasets = []
        this.metadatasets.push(<MetadataSet>{ label: "1", value: "val", chosen: false })
        this.metadatasets.push(<MetadataSet>{ label: "2", value: "val 2", chosen: false })
    }


    protected addField() {
        let template: JQuery = $(this.templateString)
        this.fieldsContainer.append(template)
        this.listenForRemoveFieldButton()
    }


}