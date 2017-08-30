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
    // backend variables
    id: KnockoutObservable<string>
    guid: KnockoutObservable<string>
    fileName: KnockoutObservable<string>
    thumbnail: KnockoutObservable<string>
    url: KnockoutObservable<string>
    // computed variables
    status: KnockoutObservable<string>
    progress: KnockoutObservable<string>
    preloaded: KnockoutObservable<boolean>
}

//[{ "Id": 0, "Guid": null, "FileName": "example.jpg", "Thumbnail": "/manager/Items/Thumbnail/0", "Url": "/manager/Items/File/0" }]

interface FileBackend {
    Id: string,
    Guid: string,
    FileName: string,
    Thumbnail: string,
    Url: string
}


declare var fileList: any;

class ItemForm {

    files: KnockoutObservableArray<FileDescription>
    fileList: Array<FileBackend>
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
        this.fileList = fileList
        this.populateFiles()
    }

    private populateFiles() {
        for (let fileBackend of this.fileList) {
            console.log(fileBackend)

            let fileDescription: FileDescription = this.getFileDescription(fileBackend.FileName);
            fileDescription.id(fileBackend.Id)
            fileDescription.guid(fileBackend.Guid)
            fileDescription.thumbnail(fileBackend.Thumbnail)
            fileDescription.url(fileBackend.Url)
            fileDescription.preloaded(true)
            console.log(fileDescription.id())
            console.log(fileDescription.guid())
            console.log(fileDescription.thumbnail())
            console.log(fileDescription.url())
            this.files.push(fileDescription)
        }
    }

    private dropListener = (event: DragEvent) => {
        event.stopPropagation()
        event.preventDefault()
        let fileList: FileList = event.dataTransfer.files
        for (let i: number = 0; i < fileList.length; ++i) {
            let request: XMLHttpRequest = new XMLHttpRequest()
            let formData: FormData  = new FormData()
            let file: File = fileList[i]

            request.open('POST', '/manager/items/upload');
            formData.append("Files", file, file.name)
            
            // add file to list

            let fileDescription: FileDescription = this.getFileDescription(file.name)
            this.files.push(fileDescription)

            request.onprogress = (event: ProgressEvent) => {
                if (event.lengthComputable) {
                    let progress: string = Math.floor((event.loaded / event.total) * 100) + "%"
                    fileDescription.progress(progress)
                }
            }

            request.onreadystatechange = () => {
                if (request.readyState == 4 && request.status == 200) {
                    // response for single file
                    let responseJson: any = JSON.parse(request.responseText)[0]
                    this.updateFileDescription(fileDescription, responseJson, "OK")
                    console.log(responseJson)
                } else {
                    // Render error 
                    fileDescription.status("ERROR")
                    fileDescription.progress("0%")
                }
            }
            request.send(formData)
        }
    }

    private updateFileDescription(fileDescription: FileDescription, description: any, status: string) {
        fileDescription.id(description.Id)
        fileDescription.guid(description.Guid)
        console.log(description)
        fileDescription.fileName(description.FileName)
        fileDescription.thumbnail(description.Thumbnail)
        fileDescription.url(description.Url)
        fileDescription.status(status)
    }

    private getFileDescription(fileName: string): FileDescription {

        let fileDescription: FileDescription = {
            id: ko.observable(""),
            guid: ko.observable(""),
            fileName: ko.observable(fileName),
            thumbnail: ko.observable("/content/thumbnails/other.png"),
            url: ko.observable(""),
            status: ko.observable("LOADING"),
            progress: ko.observable("0%"),
            preloaded: ko.observable(false)
        }
        
        return fileDescription
    }

    private addFileToList(fileName: string) {
        let fileDescription: FileDescription = <FileDescription>{}
               

        this.files.push(fileDescription)
    }

    //private addFileToList(event: any, file: File) {
    //    let fileDescription: FileDescription = {
    //        name: ko.observable(file.name),
    //        source: ko.observable(event.target.result)
    //    };

    //    this.files.push(fileDescription)
    //}

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
