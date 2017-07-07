import * as $ from "jquery"
import * as ko from "knockout"

interface EntityType {
    MetadataSets: Metadataset[];
    Id: number;
    Name: string;
    Description: string;
}

interface Metadataset {
    Fields: Field[];
    Id: number;
    Name: string;
    Description: string;
}

interface Field {
    Options: string;
    // OptionsArray is processed for rendering 
    OptionsArray: string[];
    Id: number;
    Name: string;
    Description: string;
    IsRequired: boolean;
    ToolTip?: any;
    MetadataSetId: number;
    Value: string;
}

class Item {
    private entityTypeId: number
    private metadataSets: string

}

declare var metadata: any;

class ItemForm {
    private entityTypes: EntityType[]
    private selectedEntityType: KnockoutObservable<EntityType> 

    constructor() {
        this.entityTypes = metadata['entityTypes']
        this.selectedEntityType = ko.observable<EntityType>(this.entityTypes[0])
        this.addValuesToEntityTypes()
    }

    private addValuesToEntityTypes() {
        for (let entityType of this.entityTypes) {          
            for (let metadataSet of entityType.MetadataSets) {
                for (let field of metadataSet.Fields) {
                    field.Value = ""
                    field.OptionsArray = field.Options.split("\n")
                }                
            }            
        }
    }
}

$(() => {
    ko.applyBindings(new ItemForm())
})
