import draggable from 'vuedraggable';
//import 'quill/dist/quill.core.css'
//import 'quill/dist/quill.snow.css'
//import 'quill/dist/quill.bubble.css'

import { quillEditor } from 'vue-quill-editor'
import { v1 as uuidv1 } from 'uuid';

import { required, requiredIf } from 'vuelidate/lib/validators'
import Vuelidate from 'vuelidate'
Vue.use(Vuelidate)

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
                //this one is for the default templates
                getFieldDefs: "manager/api/forms/fielddefs",
                //postString: "manager/items/save",

                names: null,
                descriptions: null,
                fields: null,
                fields_type: null,
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
                saveFieldFormButtonLabel: "Save",
            }
        },
        validations: {
            names: {
                required,
                    Values: {
                        $values: {
                            $each: {
                                Value: {
                                    required,
                                }
                            }
                        }
				    }
            },
            descriptions: {
                Values: {
                    $values: {
                        $each: {
                            Value: {
                            }
                        }
                    }
                }
            },
            fields: {
                $each: {
                    Values: {
                        //currently the display text option can be submitted regardless of any text or not
                        //it errors on reading an array instead of an empty string on creation, need different place to store it

                        //all start with this value at Array(0)
                        required: requiredIf(function (fieldModel) {
                            return (fieldModel.ModelType ==
                                'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
                                || fieldModel.ModelType ==
                                'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
                                || fieldModel.ModelType ==
                                'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
                                || fieldModel.ModelType ==
                                'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
                            )
                        }), 
                        $values: {
                            //only need the object for radio/checkbox/dropdown's inner content
                            $each: {
                                text: {
                                    required: requiredIf(function (textModel) {
                                        //this might not work with api update, hoping to store mc/radio/dropdown in different section from file attachment
                                        return (typeof (textModel) == 'object');
                                    })
                                }
                            }
                        }
                    },
                    Name: {
                        Values: {
                            $values: {
                                $each: {
                                    Value: {
                                        required
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        methods: {

            /**
			 * Checks all the inputs to make sure the data is valid
			 * @returns true is valid, false is invalid.
			 **/
            checkValidity(event) {
                event.preventDefault();

                if (this.$v.$invalid) {
                    console.log("something is invalid", this.$v);
                } else {
                    console.log("all good!");
                    this.saveFieldForm(event);
				}

            },

            /**
			 * Checks that the value matches its requirements from Vuelidate
			  * (ie required, is a string, etc)
			 * @param name the name of the v-model binded to.
			 */
            validateState(name, indexOrGuid = null, attribute = null) {
                if (indexOrGuid != null) {
                    //this is a $each situation - array
                    const { $dirty, $invalid } = this.$v[name][attribute].$values.$each[indexOrGuid].Value;
                    return $dirty ? !$invalid : null;
                } else {
                    const { $dirty, $error } = this.$v[name];
                    return $dirty ? !$error : null;
                }
            },

            /**
             * TODO: work this one and above into a generic function
             * This one is for fields only, very hardcody bc it has so many embedded attributes
             * @param {any} fieldIndex
             * @param {any} name
             * @param {any} secondIndex
             */
            validateFieldState(fieldIndex, name, secondIndex = null) {
                if (secondIndex == null) {
                    const { $dirty, $invalid } = this.$v.fields.$each[fieldIndex][name];
                    return $dirty ? !$invalid : null;
                } else {
                    const { $dirty, $invalid } = this.$v.fields.$each[fieldIndex][name].Values.$values.$each[secondIndex].Value;
                    return $dirty ? !$invalid : null;
				}

			},


            /**
			 * Touches nested items from Vuex so validation works properly.
			 */
            touchNestedItem(name, indexOrGuid = null, attribute = null, event = null) {
                if (indexOrGuid != null) {
                    if (isNaN(indexOrGuid)) {
                        this.$v[name][indexOrGuid][attribute].$touch();
                    } else {
                        this.$v[name][attribute].$values.$each[indexOrGuid].Value.$touch();
                    }

                }
            },


            /**
             * Saves the field form
             * @param {any} event
             */
            saveFieldForm(event) {
                //console.log("saving goes here", event);

                console.log("the name, description, and fields saved TBA", this.names, this.descriptions, this.fields);
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
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.TextField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
                    case 1:
                        //textarea
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core';
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
                    case 2:
                        //radio/mc
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core';
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
                    case 3:
                        //checkbox
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core';
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
                    case 4:
                        //dropdown
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core';
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
                    case 5:
                        //fileattachment
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
                    case 6:
                        //displayfield
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core';
                        this.fields[fieldIndex].ModelType = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null';
                        break;
				}
            },


            /*test(fieldId) { //detects the 'show' after transition is complete so this will never work...also only runs once unless computed. computed doesnt allow for parameter except if a setter
                if (document.getElementById('collapse-' + fieldId) == null) {
                    return 'fas fa-chevron-right';
                } else if (document.getElementById('collapse-' + fieldId).classList.contains('show')) {
                    console.log("item has show:", document.getElementById('collapse-' + fieldId).classList);
                    return 'fas fa-chevron-right';
                } else {
                    console.log("item does not have show:", document.getElementById('collapse-' + fieldId).classList);
                    return 'fas fa-chevron-down';
                }
            },*/

            /**
             * Fire when any item sorted/moved (includes adding new item to list)
             * @param {any} event
             */
            sortItem(event) {
                let collapsingSections = document.getElementsByClassName('collapsing-items');
                console.log("event on sort:", event);
                let shownSectionIndex = null;
                let previousSection = null;
                let nextSection = null;

                //track sections above and below current open item
                for (let i = 0; i < collapsingSections.length; i++) {
                    if (collapsingSections[i].classList.contains('show')) {
                        shownSectionIndex = i;
                        previousSection = (i - 1 >= 0) ? collapsingSections[i - 1] : null;
                        nextSection = (i + 1 < collapsingSections.length) ? collapsingSections[i + 1] : null;
					}
                }

                //if all items closed and not adding something new, just return
                if (shownSectionIndex == null && previousSection == null && nextSection == null
                    && event.from.id == event.to.id) {
                    return;
				}

                //the field id of the sorted section
                let tmpId = collapsingSections[event.newIndex].id.split('collapse-')[1];

                //if item is new, open that one
                if (event.from.id != event.to.id) {
                    console.log("added new item", collapsingSections[event.newIndex].id);
                    $('#' + collapsingSections[event.newIndex].id).collapse('show');
                    this.dropdowns[tmpId].isCollapsed = false;
                    if (shownSectionIndex != null) {
                        this.dropdowns[tmpId].isCollapsed = true;
					}
                    return;
                }

                //if the user is dragging the showing item around
                if (shownSectionIndex == event.oldIndex) {
                    console.log("dragging showing item");
                    $('#' + collapsingSections[event.newIndex].id).collapse('show');
                    this.dropdowns[tmpId].isCollapsed = false;
                    if (shownSectionIndex != null) {
                        this.dropdowns[tmpId].isCollapsed = true;
					}
                    return;
				}

                //move show class to the index below open item
                if (event.oldIndex <= shownSectionIndex && shownSectionIndex <= event.newIndex) {

                    //test suppressing animation - not sure if it will work, cant 
                    //remove .collapsing bc it's not applied until the collapse call is made
                    //previousSection.addClass('suppress-collapsing-animation');
                    //$('#' + previousSection.id).css({ "transition": "none", "display": "none"}); doesnt work, must override

                    console.log("moved item DOWN over shown");
                    $('#' + previousSection.id).collapse('show');
                    let prevId = previousSection.id.split('collapse-')[1];
                    this.dropdowns[prevId].isCollapsed = false;

                    //move item above open item
                } else if (event.oldIndex >= shownSectionIndex && shownSectionIndex >= event.newIndex) {
                    console.log("moved item UP over shown");
                    $('#' + nextSection.id).collapse('show');
                    let nextId = nextSection.id.split('collapse-')[1];
                    this.dropdowns[nextId].isCollapsed = false;
                } else {
                    //just sorting, does not interfere with the open item
                    return;
				}

                this.dropdowns[tmpId].isCollapsed = true;
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
                this.$set(this.dropdowns, newItem.Id, {
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
                if (field.Values.$values.indexOf("any") > -1) {
                    let index = field.Values.$values.indexOf("any");
                    field.Values.$values.splice(index, 1);
				}

                if (field.Values.$values.length == this.fileTypes.length) {
                    //uncheck all
                    field.Values.$values = [];
                } else {
                    //check all
                    field.Values.$values = [];
                    field.Values.$values = this.fileTypes;
				}

            },

            /**
             * Checks if the checkboxes are all checked and will check 'any',
             * or if 'any' is checked and the user unchecks a checkbox, uncheck 'any'
             * @param {any} field
             */
            checkCheckboxState(field, fieldIndex) {
                if (field.Values.$values.length == this.fileTypes.length) {
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
             * hence the necessity for this function.
             * 
             * @param {any} fieldId the field's index to open/close
             */
            toggleDropdown(fieldId) {
                let lastDropdownIdOpened = '';
                for (let dropdownId of Object.keys(this.dropdowns)) {
                    if (this.dropdowns[dropdownId].isCollapsed == false) {
                        lastDropdownIdOpened = dropdownId;
					}
                }

                if (fieldId != lastDropdownIdOpened && lastDropdownIdOpened != '') {
                    //close dropdown that is not the same one previously opened
                    this.dropdowns[lastDropdownIdOpened].isCollapsed = true;
				}

                this.dropdowns[fieldId].isCollapsed === true ? this.dropdowns[fieldId].isCollapsed = false : this.dropdowns[fieldId].isCollapsed = true;
            },

            /**
             * Adds new option to either multiple choice or checkbox
             * @param {any} field the field to push multiple choice or checkbox objects onto
             */
            addNewOption(field) {
                //if theres a disabled other option, push into index before it
                //the disabled item will always be the last item
                if (field.Values.$values.length > 0) {
                    if (field.Values.$values[field.Values.$values.length - 1].isDisabled) {
                        field.Values.$values.splice(field.Values.$values.length - 1, 0, {
                            text: '',
                            isDisabled: false,
                            id: -1,
                        });
                        return;
                    }
                    
				}
                 
                field.Values.$values.push({
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
                field.Values.$values.push({
                    text: 'Other...',
                    isDisabled: true,
                    id: -1,
                });
                this.dropdowns[field.Id].hasOtherOption = true;
            },

            /**
             * Removes an option item
             * @param {any} fieldIndex
             * @param {any} optionIndex
             */
            removeOption(field, fieldIndex, itemValue, optionIndex) {
                if (itemValue.isDisabled) {
                    this.dropdowns[field.Id].hasOtherOption = false;
				}
                this.fields[fieldIndex].Values.$values.splice(optionIndex, 1);
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
                        fetch(piranha.baseUrl + this.getFieldDefs)
                            .then((fdResponse) => { return fdResponse.json(); })
                            .then((fieldDefsResult) => {
                                //templates handled here
                                console.log("second res", fieldDefsResult)
                                for (let defaultFieldIndex in fieldDefsResult.$values) {
                                    switch (defaultFieldIndex) {
                                        case '0':
                                            this.tmpTextfieldTemplate = fieldDefsResult.$values[defaultFieldIndex];
                                            this.tmpTextfieldTemplate.Selected = 0;
                                            break;
                                        case '1':
                                            this.tmpTextAreaTemplate = fieldDefsResult.$values[defaultFieldIndex];
                                            this.tmpTextAreaTemplate.Selected = 1;
                                            break;
                                        //the rest still need to be added from the backend
                                    }
                                    
                                }

                                //temp set other values that i dont have sample data for
                                //guessing for what will be needed, adjust when dummy data given
                                //this.tmpTextAreaTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                //this.tmpTextAreaTemplate.$type = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core';

                                this.tmpRadioTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpRadioTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                this.tmpRadioTemplate.Values.$values = [];

                                this.tmpCheckboxTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpCheckboxTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                this.tmpCheckboxTemplate.Values.$values = [];

                                this.tmpDropdownTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpDropdownTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                this.tmpDropdownTemplate.Values.$values = [];

                                this.tmpFileAttachmentTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpFileAttachmentTemplate.$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';
                                this.tmpFileAttachmentTemplate.Values.$values = [];

                                this.tmpDisplayFieldTemplate = JSON.parse(JSON.stringify(this.tmpTextfieldTemplate));
                                this.tmpDisplayFieldTemplate.$type = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core';
                                this.tmpDisplayFieldTemplate.Values.$values = "";

                            })
                            .then(() => {
                                return fetch(piranha.baseUrl + this.getString + this.itemId);
                            })
                            .then((response) => { return response.json(); })
                            .then((result) => {
                                //data for this form handled here

                                this.names = result.Name;
                                this.descriptions = result.Description;
                                this.fields = result.Fields.$values;
                                this.fields_type = result.Fields.$type;
                                this.id = result.Id;
                                this.modelType = result.ModelType;

                                this.finishedGET = true;
                                //this.collections = result.collections;
                                //this.updateBindings = true;
                                console.log(result);

                                for (let field of this.fields) {
                                    this.$set(this.dropdowns, field.Id, {
                                        isCollapsed: true,
                                        showDescription: false,
                                        hasOtherOption: false
                                    });
                                }

                                //temporary until templates sent, remove afterwards
                                //Selected needs to be sent still as an attribute
                                for (let field of result.Fields.$values) {
                                    switch (field.$type) {
                                        case 'Catfish.Core.Models.Contents.Fields.TextField, Catfish.Core':
                                            field.Selected = 0;
                                            break;
                                        case 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core':
                                            field.Selected = 1;
                                            break;
                                        case 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core':
                                            field.Selected = 2;
                                            break;
                                        case 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core':
                                            field.Selected = 3;
                                            break;
                                        case 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core':
                                            field.Selected = 4;
                                            break;
                                        case 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core':
                                            field.Selected = 5;
                                            break;
                                        case 'Catfish.Core.Models.Contents.Fields.DisplayText, Catfish.Core':
                                            field.Selected = 6;
                                            break;
                                    }
                                }

                                

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

                    //for the accordion, if one panel is triggered to open, close any others
                    $('#accordion').on('show.bs.collapse', function () {
                        console.log("called to hide");
                        let test = $('#accordion .show').length;
                        console.log(test);
                        $('#accordion .show').collapse('hide');
                    });
                });
        }
    });
}