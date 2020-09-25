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

                dropdowns: {},
                //temp, need to call an api for these
                fieldTypes: [
                    { text: 'Select One', value: null },
                    {
                        value: 0,
                        text: 'Short Answer',
                        modelType: 'TextField'
                    },
                    {
                        value: 1,
                        text: 'Long Answer',
                        modelType: 'TextArea'
                    },
                    {
                        value: 2,
                        text: 'Multiple Choice',
                        modelType: 'Radio'
                    },
                    {
                        value: 3,
                        text: 'Check Box',
                        modelType: 'Checkbox'
                    },
                    {
                        value: 4,
                        text: 'Dropdown List',
                        modelType: 'Dropdown'
                    },
                    {
                        value: 5,
                        text: 'File Upload',
                        modelType: 'FileAttachment'
                    },
                    {
                        value: 6,
                        text: 'Display Text',
                        modelType: 'DisplayField'
                    }
                ],

                rightColumnOptions: [
                    {
                        value: 0,
                        text: "Add Question"
                    },
                    {
                        value: 1,
                        text: "Add Section (TBA)"
                    }
                ],

                //will be sent through API, temp
                fileTypes: [
                    "PDF", "DOC", "DOCX", "PS", "EPS", "JPG", "PNG"
                ],

                //temp until templates sent
                tmpTextfieldTemplate: null,
                tmpTextAreaTemplate: null,
                tmpRadioTemplate: null,
                tmpCheckboxTemplate: null,
                tmpDropdownTemplate: null,
                tmpFileAttachmentTemplate: null,
                tmpDisplayFieldTemplate: null,

                saveStatus: 0,
                //TODO: make a file of constant strings
                saveSuccessfulLabel: "Save Successful",
                saveFailedLabel: "Failed to Save",
                saveFieldFormButtonLabel: "Save"
            }
        },
        methods: {

            /**
             * Saves the field form
             * @param {any} event
             */
            saveFieldForm(event) {
                console.log("saving goes here", event);
                event.preventDefault();

                console.log(this.names, this.descriptions, this.fields);
            },

            /**
             * Changes the type of field via choice from the dropdown
             * @param {any} fieldIndex the fieldIndex being changed
             * @param {any} event the index value of the dropdown
             */
            onDropdownChange(fieldIndex, event) {
                console.log("fieldIndex", fieldIndex);
                //cant change $type directly... could work something with the templates?
                //dont want to lose any values that are not originally a part of the template tho...
                switch (event) {
                    case 0:
                        //textfield
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.TextField, Catfish.Core';
                        break;
                    case 1:
                        //textarea
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core';
                        break;
                    case 2:
                        //radio/mc
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core';
                        break;
                    case 3:
                        //checkbox
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core';
                        break;
                    case 4:
                        //dropdown
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core';
                        break;
                    case 5:
                        //fileattachment
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';
                        break;
                    case 6:
                        //displayfield
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core';
                        break;
				}
            },

            /**
             * Handles the cloned item (see cloneItem function)
             * Controls the 'show' class for collapse to open the new item on creation and
             * close any other items
             * @param {any} event
             */
            addNewItem(event) {
                console.log(event);

                let newIdArray = event.srcElement.children[event.newIndex].children[0].children[1].children[1].id.split('-');
                let newId = '';
                let iterations = newIdArray.length;
                console.log(newIdArray);
                for (let item of newIdArray) {
                    if (item == 'collapse') {
                        --iterations;
                        continue;
                    }
                    newId += item;
                    if (--iterations) {
                        newId += '-';
                    }
                }
                console.log(newId);
                //attempt at always having the newest item open first, remove the 'show' class
                //from other items and assign to the new item - already can see this will be an issue
                //because the newest item doesnt exist yet. Can remove the show class though to reduce confusion
                let collapsingSections = document.getElementsByClassName('collapsing-items');
                for (let section of collapsingSections) {
                    if (section.classList.contains('show')) {
                        section.classList.remove('show');

                        //need to hunt down the new item's id via the id number i believe
                        for (let item of Object.keys(this.dropdowns)) {
                            if (item != newId) {
                                this.dropdowns[item].isCollapsed = true;
                            }
						}

                        /*let sectionIdArray = section.id.split('-'); //PROBLEM! This id is of the previous position before new item shifted it...
                        let sectionId = '';
                        let iterations = sectionIdArray.length;
                        console.log(sectionIdArray);
                        for (let item of sectionIdArray) {
                            if (item == 'collapse') {
                                --iterations;
                                continue;
                            }
                            sectionId += item;
                            if (--iterations) {
                                sectionId += '-';
                            }
                            
                            
                        }*/
                        //console.log(sectionId, this.dropdowns);
                        //this.$set(this.dropdowns[sectionId], 'isCollapsed', true);
                    }
                }
                //TODO: reset the isCollapsed for the one with 'show' class

                //what u want is 3 children deep                
                event.srcElement.children[event.newIndex].children[0].children[1].children[1].classList.add('show');
			},

            /**
             * Returns a custom clone
             * @param event
             */
            cloneItem(event) {
                let newItem = {};

                //hardcoded until templates are provided
                newItem = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate)); //event.Template
                
                newItem.id = uuidv1();
                this.$set(this.dropdowns, newItem.id, {
                    isCollapsed: false,
                    showDescription: false,
                    hasOtherOption: false
                });
                //newItem.Guid = uuidv1();
                console.log(newItem);

                return newItem;
            },

            /**
             * Checks all options - ie user has checked 'Any' option in File Upload.
             * If all already checked, uncheck them all
             * @param {any} field
             */
            checkAllFileTypes(field) {
                if (field.values.indexOf("any") > -1) {
                    let index = field.values.indexOf("any");
                    field.values.splice(index, 1);
				}

                if (field.values.length == this.fileTypes.length) {
                    //uncheck all
                    field.values = [];
                } else {
                    //check all
                    field.values = [];
                    field.values = this.fileTypes;
				}

            },

            /**
             * Checks if the checkboxes are all checked and will check 'any',
             * or if 'any' is checked and the user unchecks a checkbox, uncheck 'any'
             * @param {any} field
             */
            checkCheckboxState(field, fieldIndex) {
                if (field.values.length == this.fileTypes.length) {
                    //check the 'any' box
                    document.getElementById("filetype-checkbox-" + fieldIndex + "-" + "any").checked = true;
                } else {
                    //uncheck the 'any' box
                    document.getElementById("filetype-checkbox-" + fieldIndex + "-" + "any").checked = false;
				}
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
                console.log(this.dropdowns[fieldId]);
                this.dropdowns[fieldId].isCollapsed === true ? this.dropdowns[fieldId].isCollapsed = false : this.dropdowns[fieldId].isCollapsed = true;
                //this.lastDropdownAction = this.dropdowns[fieldId].isCollapsed;
                //this.assessExpandOrCollapseAll();
            },

            /**
             * Adds new option to either multiple choice or checkbox
             * @param {any} field the field to push multiple choice or checkbox objects onto
             */
            addNewOption(field) {
                //if theres a disabled other option, push into index before it
                //the disabled item will always be the last item
                if (field.values.length > 0) {
                    if (field.values[field.values.length - 1].isDisabled) {
                        field.values.splice(field.values.length - 1, 0, {
                            text: '',
                            isDisabled: false,
                            id: -1,
                        });
                        return;
                    }
                    
				}
                 
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
                this.dropdowns[field.id].hasOtherOption = true;
            },

            /**
             * Removes an option item
             * @param {any} fieldIndex
             * @param {any} optionIndex
             */
            removeOption(field, fieldIndex, itemValue, optionIndex) {
                if (itemValue.isDisabled) {
                    this.dropdowns[field.id].hasOtherOption = false;
				}
                this.fields[fieldIndex].values.splice(optionIndex, 1);
            },

            /**
             * Deletes a given field
             * @param {any} fieldIndex
             */
            deleteField(fieldIndex) {
                this.fields.splice(fieldIndex, 1);
                delete this.dropdowns[fieldIndex];
            },

            /**
             * Adds the description field to the field.
             * @param {any} fieldId
             */
            addDescription(fieldId) {
                this.dropdowns[fieldId].showDescription = true;
            },

            /**
             * Removes the description field from the field.
             * Not sure if this should delete the info in it, if any.
             * CURRENTLY it does not.
             * @param {any} fieldId
             */
            removeDescription(fieldId) {
                this.dropdowns[fieldId].showDescription = false;
            },

            /**
              * Fetches and loads the data from an API call
              * */
            load() {
                //var self = this;
                return new Promise((resolve, reject) => {
                    piranha.permissions.load(() => {
                        fetch(piranha.baseUrl + this.getString + this.itemId)
                            .then((response) => { return response.json(); })
                            .then((result) => {
                                this.names = result.name;
                                this.descriptions = result.description;
                                this.fields = result.fields;
                                this.id = result.id;
                                this.modelType = result.modelType;

                                this.finishedGET = true;
                                //this.collections = result.collections;
                                //this.updateBindings = true;
                                console.log(result);

                                for (let field of this.fields) {
                                    this.$set(this.dropdowns, field.id, {
                                        isCollapsed: true,
                                        showDescription: false,
                                        hasOtherOption: false
                                    });
                                    }

                                //temporary until templates sent, remove afterwards
                                for (let field of result.fields) {
                                    if (field.$type == 'Catfish.Core.Models.Contents.Fields.TextField, Catfish.Core') {
                                        field.selected = 0;
                                        let defaultTextfieldTemplate = JSON.parse(JSON.stringify(field));
                                        defaultTextfieldTemplate.name.values[0].value = '';
                                        defaultTextfieldTemplate.selected = null;

                                        this.tmpTextfieldTemplate = defaultTextfieldTemplate;
									}

                                }

                                //temp set other values that i dont have sample data for
                                //guessing for what will be needed, adjust when dummy data given
                                this.tmpTextAreaTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpTextAreaTemplate.$type = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core';

                                this.tmpRadioTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpRadioTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                this.tmpRadioTemplate.values = [];

                                this.tmpCheckboxTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpCheckboxTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                this.tmpCheckboxTemplate.values = [];

                                this.tmpDropdownTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpDropdownTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                this.tmpDropdownTemplate.values = [];

                                this.tmpFileAttachmentTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpFileAttachmentTemplate.$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';

                                this.tmpDisplayFieldTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpDisplayFieldTemplate.$type = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core';

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
                    console.log("check on dropdowns:", this.dropdowns); //why are these only ids? how is this workign at all???
                });
        }
    });
}