import draggable from 'vuedraggable';
//import 'quill/dist/quill.core.css'
//import 'quill/dist/quill.snow.css'
//import 'quill/dist/quill.bubble.css'

import { quillEditor } from 'vue-quill-editor'
import { v1 as uuidv1 } from 'uuid';
/**
 * Javascript Vue code for creating the editable form from existing data in FieldContainerEdit.cshtml.
 * It is modelled after the file piranha.pagelist.js in Piranha's source code.
 */


/**
 * This check makes sure the file is only run on the page with
 * the element. Not a huge deal, can be removed if it's causing issues.
 */
if (document.getElementById("edit-field-form-page")) {
    piranha.editFieldForm = new Vue({
        el: '#edit-field-form-page',
        components: {
            draggable,
            quillEditor
		},
        data() {
            return {
                itemId: null,
                finishedGET: false,

                //api strings
                getString: "manager/api/forms/",
                //postString: "manager/items/save",

                names: null,
                descriptions: null,
                fields: null,
                id: null,
                modelType: null,

                dropdowns: null,
                //temp, need to call an api for these
                fieldTypes: [
                    {
                        id: 0,
                        name: 'Short Answer',
                        modelType: 'TextField'
                    },
                    {
                        id: 1,
                        name: 'Long Answer',
                        modelType: 'TextArea'
                    },
                    {
                        id: 2,
                        name: 'Multiple Choice',
                        modelType: 'Radio'
                    },
                    {
                        id: 3,
                        name: 'Check Box',
                        modelType: 'Checkbox'
                    },
                    {
                        id: 4,
                        name: 'Dropdown List',
                        modelType: 'Dropdown'
                    },
                    {
                        id: 5,
                        name: 'File Upload',
                        modelType: 'FileAttachment'
                    },
                    {
                        id: 6,
                        name: 'Display Text',
                        modelType: 'DisplayField'
                    }
                ],
                //temp until templates sent
                tmpTextfieldTemplate: null,
                tmpTextAreaTemplate: null,
                tmpRadioTemplate: null,
                tmpCheckboxTemplate: null,
                tmpDropdownTemplate: null,
                tmpFileAttachmentTemplate: null,
                tmpDisplayFieldTemplate: null
            }
        },
        methods: {

            /**
             * Returns a custom clone
             * @param event
             */
            cloneItem(event) {
                console.log(event);
                let newItem = {};
                //this is temporary
                console.log(event.modelType);
                switch (event.modelType) {
                    case 'TextField':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate)); //event.Template
                        break;
                    case 'TextArea':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpTextAreaTemplate)); //event.Template
                        break;
                    case 'Radio':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpRadioTemplate)); //event.Template
                        break;
                    case 'Checkbox':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpCheckboxTemplate)); //event.Template
                        break;
                    case 'Dropdown':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpDropdownTemplate)); //event.Template
                        break;
                    case 'FileAttachment':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpFileAttachmentTemplate)); //event.Template
                        break;
                    case 'DisplayField':
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpDisplayFieldTemplate)); //event.Template
                        break;
                    default:
                        //hardcoded until templates are provided
                        newItem = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate)); //event.Template
                        break;
				}
                
                newItem.id = uuidv1();
                this.dropdowns[newItem.id] = { isCollapsed: false };
                //newItem.Guid = uuidv1();
                return newItem;
            },

            /**
             * Toggles the field to either open or closed.
             * Icon for showing open/closed relies on open/closed state,
             * hence the necessity for this function. TODO still not working well on fast clicks,
             * if it can't be fixed then delete this function and just handle in template
             * 
             * @param {any} fieldId the field's index to open/close
             */
            toggleDropdown(fieldId) {
                this.dropdowns[fieldId].isCollapsed === true ? this.dropdowns[fieldId].isCollapsed = false : this.dropdowns[fieldId].isCollapsed = true;
                //this.lastDropdownAction = this.dropdowns[fieldId].isCollapsed;
                //this.assessExpandOrCollapseAll();
            },

            /**
             * Adds new option to either multiple choice or checkbox
             * @param {any} field the field to push multiple choice or checkbox objects onto
             */
            addNewOption(field) {
                field.values.push({
                    text: '',
                    isDisabled: false,
                    id: -1,
                });

                /*switch (field.$type) {
                    case 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core':
                        //hardcoded for now, use template provided item instead
                        this.tmpRadioTemplate.values.push( {
                            text: '',
                            id: -1,
                        } );
                        break;
                    case 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core':
                        //hardcoded for now, use template provided item instead
                        this.tmpCheckboxTemplate.values.push({
                            text: '',
                            id: -1,
                        });
                        break;
				}*/
            },

            /**
             * Adds 'Other' option to set for user to fill
             * @param {any} field
             */
            addOtherOption(field) {
                field.values.push({
                    text: 'Other...',
                    isDisabled: true,
                    id: -1,
                });
            },

            /**
             * Removes an option item
             * @param {any} fieldIndex
             * @param {any} optionIndex
             */
            removeOption(fieldIndex, optionIndex) {
                this.fields[fieldIndex].values.splice(optionIndex, 1);
            },

            /**
              * Fetches and loads the data from an API call
              * */
            load() {
                var self = this;
                return new Promise((resolve, reject) => {
                    piranha.permissions.load(function () {
                        fetch(piranha.baseUrl + self.getString + self.itemId)
                            .then(function (response) { return response.json(); })
                            .then(function (result) {
                                self.names = result.name;
                                self.descriptions = result.description;
                                self.fields = result.fields;
                                self.id = result.id;
                                self.modelType = result.modelType;

                                self.finishedGET = true;
                                //self.collections = result.collections;
                                //self.updateBindings = true;
                                console.log(result);

                                self.dropdowns = new function () {
                                    for (let field of self.fields) {
                                        this[field.id] = { isCollapsed: false }
                                    }
                                }

                                //temporary until templates sent, remove afterwards
                                for (let field of result.fields) {
                                    if (field.$type == 'Catfish.Core.Models.Contents.Fields.TextField, Catfish.Core') {
                                        let defaultTextfieldTemplate = JSON.parse(JSON.stringify(field));
                                        defaultTextfieldTemplate.name.values[0].value = '';

                                        self.tmpTextfieldTemplate = defaultTextfieldTemplate;
                                        //breaking for now bc my sample data only has 1 type
                                        break;
									}

                                }

                                //temp set other values that i dont have sample data for
                                //guessing for what will be needed, adjust when dummy data given
                                self.tmpTextAreaTemplate = JSON.parse(JSON.stringify(self.tmpTextfieldTemplate));
                                self.tmpTextAreaTemplate.$type = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core';

                                self.tmpRadioTemplate = JSON.parse(JSON.stringify(self.tmpTextfieldTemplate));
                                self.tmpRadioTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                self.tmpRadioTemplate.values = [];

                                self.tmpCheckboxTemplate = JSON.parse(JSON.stringify(self.tmpTextfieldTemplate));
                                self.tmpCheckboxTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                self.tmpCheckboxTemplate.values = [];

                                self.tmpDropdownTemplate = JSON.parse(JSON.stringify(self.tmpTextfieldTemplate));
                                self.tmpDropdownTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                self.tmpDropdownTemplate.values = [];

                                self.tmpFileAttachmentTemplate = JSON.parse(JSON.stringify(self.tmpTextfieldTemplate));
                                self.tmpFileAttachmentTemplate.$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';

                                self.tmpDisplayFieldTemplate = JSON.parse(JSON.stringify(self.tmpTextfieldTemplate));
                                self.tmpDisplayFieldTemplate.$type = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core';

                                resolve();

                            })
                            .catch(function (error) { console.log("error:", error); });
                    });

                });
                
            },
        },
        created() {
            this.itemId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
            this.load()
                .then(() => {
                    //for popovers
                    $(document).ready(function () {
                        $('[data-toggle="popover"]').popover();
                    });
                });
        }
    });
}