import * as $ from "jquery"
import * as ko from "knockout"
import "knockout-file-bindings"

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
    Definition: any;
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

class MultipleFileData {
    dataURLArray: KnockoutObservableArray<string>
}

interface FileDescription {
    name: KnockoutObservable<string>;
    source: KnockoutObservable<string>;
}

class ItemForm {

    files: KnockoutObservableArray<FileDescription>
    //reader: FileReader

    constructor() {

        $('ul.custom-tabs li').click(function () {
            console.log("test")
            var tab_id = $(this).attr('data-tab');

            $('ul.custom-tabs li').removeClass('active');
            $('.tab-content').removeClass('active');

            $(this).addClass('active');
            $("#" + tab_id).addClass('active');
        })

        // Multifile upload
        
        this.files = ko.observableArray([])
        //this.reader = new FileReader()

        let dropZone: HTMLElement = document.getElementById("drop-zone")

        dropZone.addEventListener("dragover", this.handleDragOver)
        dropZone.addEventListener("drop", this.dropListener)

    }

    private dropListener = (event: DragEvent) => {
        event.stopPropagation()
        event.preventDefault()
        let fileList: FileList = event.dataTransfer.files
        for (let i: number = 0; i < fileList.length; ++i) {
            let reader: FileReader = new FileReader()
            reader.onload = ((file) => {
                return (event: any) => {
                    this.addFileToList(event, file)
                }
            })(fileList[i])
     
            reader.readAsDataURL(fileList[i])
        }
    }

    private addFileToList(event: any, file: File) {
        let fileDescription: FileDescription = {
            name: ko.observable(file.name),
            source: ko.observable(event.target.result)
        };

        this.files.push(fileDescription)
    }

    private removeFile = (file: FileDescription) => {
        this.files.remove(file)
    }

    private handleDragOver(event: DragEvent) {
        event.stopPropagation()
        event.preventDefault()
        event.dataTransfer.dropEffect = "copy"
    }
}

$(() => {
    ko.applyBindings(new ItemForm())
})
