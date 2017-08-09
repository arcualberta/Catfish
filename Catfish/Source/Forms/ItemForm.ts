import * as $ from "jquery"
import * as ko from "knockout"
import "bootstrap"

interface EntityType {
    MetadataSets: Metadataset[];
    Id: number;
    Name: string;
    Description: string;
}

class Metadataset {
    Fields: Field[];
    Id: number;
    Name: string;
    Description: string;
    getUrlId: Function;

    public constructor() {
        this.getUrlId = () => {
            return "#" + this.Id
        }


        /*
        $('.tab').click(function (e) {
            console.log("here")
            e.preventDefault()
            $(this).tab('show')
        })

        $('#metadataset-tabs a:first').tab('show')
        */
    }
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

        this.selectedEntityType.subscribe((latest) => {
            $('#metadataset-tabs a').click(function (e) {
                console.log("here")
                e.preventDefault()
                $(this).tab('show')
            })

            $('#metadataset-tabs a:first').tab('show')
        })
    }

    private addValuesToEntityTypes() {
        for (let entityType of this.entityTypes) {          
            for (let metadataSet of entityType.MetadataSets) {
                for (let field of metadataSet.Fields) {
                    field.Value = ""
                    if (field.Options) {
                        field.OptionsArray = field.Options.split("\n")
                    }
                    
                }                
            }            
        }
    }
}

$(() => {
    ko.applyBindings(new ItemForm())
})
