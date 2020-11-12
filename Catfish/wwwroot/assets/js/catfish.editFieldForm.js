import draggable from 'vuedraggable';
//import 'quill/dist/quill.core.css'
//import 'quill/dist/quill.snow.css'
//import 'quill/dist/quill.bubble.css'
import StaticItems from '../static/string-values.json';

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
            quillEditor,
        },
        data() {
            return {
                itemId: null,
                finishedGET: false,
                attemptedSave: false,

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

                //missing file attachment?
                TEXTFIELD_TYPE: "Catfish.Core.Models.Contents.Fields.TextField, Catfish.Core",
                TEXTAREA_TYPE: "Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core",
                CHECKBOX_TYPE: "Catfish.Core.Models.Contents.Fields.CheckboxField, Catfish.Core",
                RADIO_TYPE: "Catfish.Core.Models.Contents.Fields.RadioField, Catfish.Core",
                DROPDOWN_TYPE: "Catfish.Core.Models.Contents.Fields.SelectField, Catfish.Core",
                INFOSECTION_TYPE: "Catfish.Core.Models.Contents.Fields.InfoSection, Catfish.Core",

                DATE_TYPE: "Catfish.Core.Models.Contents.Fields.DateField, Catfish.Core",
                DECIMAL_TYPE: "Catfish.Core.Models.Contents.Fields.DecimalField, Catfish.Core",
                INTEGER_TYPE: "Catfish.Core.Models.Contents.Fields.IntegerField, Catfish.Core",
                MONOLINGUAL_TEXTFIELD_TYPE: "Catfish.Core.Models.Contents.Fields.MonolingualTextField, Catfish.Core",

                //templates
                textfieldTemplate: null,
                textAreaTemplate: null,
                radioTemplate: null,
                checkboxTemplate: null,
                dropdownTemplate: null,
                fileAttachmentTemplate: null,
                displayFieldTemplate: null,

                datePickerTemplate: null,
                numberPickerTemplate: null,
                monolingualTextFieldTemplate: null,

                optionItemTemplate: null,


                dropdowns: {},
                //temp, need to call an api for these
                fieldTypes: [
                    { DisplayLabel: 'Select One', $type: null },
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


                saveStatus: 0,

                //Constants TODO change above items into constants from static file
                saveSuccessfulLabel: null,
                saveFailedLabel: null,
                saveFieldFormButtonLabel: null,

                formTitleLabel: null,
                formTitlePlaceholder: null,
                formDescriptionLabel: null,
                formDescriptionPlaceholder: null,
                formFieldLabel: null,
                defaultFieldTitle: null,
                fieldTitlePlaceholder: null,
                fieldDescriptionLabel: null,
                fieldDescriptionPlaceholder: null,
                settingsLabel: null,

                longAnswerFormatTextLabel: null,
                choiceOptionLabel: null,
                choiceDefaultOptionLabel: null,
                choiceAdditionalInputLabel: null,
                anyLabel: null,
                allowMultipleFilesLabel: null,
                wholeNumbersOnlyLabel: null,
                requiredLabel: null,
                addDescriptionLabel: null,
                removeDescriptionLabel: null,
                loadingLabel: null
            }
        },
        validations() {

            let validationJson = {
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
                        },
                        Options: {
                            $values: {
                                $each: {
                                    OptionText: {
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
                        //for info section - $values is an array of characters
                        Content: {
                            Values: {
                                $values: {
                                    required
								}
							}
						}
                    }
                }
            };

            this.fields.forEach((field) => {
                if (field.$type == this.RADIO_TYPE || field.$type == this.CHECKBOX_TYPE ||
                    field.$type == this.DROPDOWN_TYPE ||
                    field.$type == 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
                ) {
                    console.log("ran this");
                    validationJson.fields.$each['Values'] = { required };
                    validationJson.fields.$each.Values['$values'] = {};
                    validationJson.fields.$each.Values['$values']['$each'] = {};
                    validationJson.fields.$each.Values['$values']['$each']['Value'] = { required };
                    return validationJson;
                }
            });

            validationJson.fields.$each['Values'] = {};
            return validationJson;
        },
        methods: {

            /**
			 * Checks all the inputs to make sure the data is valid
			 * @returns true is valid, false is invalid.
			 **/
            checkValidity(event) {
                event.preventDefault();

                this.attemptedSave = true;

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
             * 
             * @param {any} fieldIndex
             */
            onNumberCheckboxChange(event, fieldIndex) {
                console.log("e", event);
                //this.fields[fieldIndex].$type

            },


            /**
             * Saves the field form
             * @param {any} event
             */
            saveFieldForm(event) {
                //console.log("saving goes here", event);

                //handle integer/decimal field if any - integer and decimal are separate classes in backend
                let fieldTypesToCheck = this.fields.map((field) => field.$type);
                fieldTypesToCheck.forEach((fieldType, index) => {
                    if (fieldType == this.DECIMAL_TYPE && this.fields[index].isIntegerOnly) {
                        this.fields[index].$type = this.INTEGER_TYPE;
                    } else if (fieldType == this.INTEGER_TYPE && !this.fields[index].isIntegerOnly) {
                        this.fields[index].$type = this.DECIMAL_TYPE;
					}
				})

                console.log("the name, description, and fields saved TBA", this.names, this.descriptions, this.fields);
                this.attemptedSave = false;
            },

            /**
             * Changes the type of field via choice from the dropdown
             * @param {any} fieldIndex the fieldIndex being changed
             * @param {any} chosenFieldType the chosen field type of the dropdown
             */
            onDropdownChange(fieldIndex, chosenFieldType) {
                //dont want to lose any values that are not originally a part of the template tho...
                let tmpId = this.fields[fieldIndex].Id;
                switch (chosenFieldType) {
                    case this.TEXTFIELD_TYPE:
                        //textfield
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.textfieldTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.TEXTAREA_TYPE:
                        //textarea
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.textAreaTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.RADIO_TYPE:
                        //radio/mc
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.radioTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.CHECKBOX_TYPE:
                        //checkbox
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.checkboxTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.DROPDOWN_TYPE:
                        //dropdown
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.dropdownTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case "Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core":
                        //fileattachment
                        this.fields[fieldIndex].$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';
                        break;

                    case this.INFOSECTION_TYPE:
                        //displayfield
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.displayFieldTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.DATE_TYPE:
                        //displayfield
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.datePickerTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.DECIMAL_TYPE:
                    case this.INTEGER_TYPE:
                        //displayfield
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.numberPickerTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;

                    case this.MONOLINGUAL_TEXTFIELD_TYPE:
                        //displayfield
                        this.$set(this.fields, fieldIndex, JSON.parse(JSON.stringify(this.monolingualTextFieldTemplate)) );
                        this.fields[fieldIndex].Id = tmpId;
                        break;
				}
            },


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
                newItem = JSON.parse(JSON.stringify(this.textfieldTemplate)); //event.Template
                
                newItem.Id = uuidv1();
                this.$set(this.dropdowns, newItem.Id, {
                    isCollapsed: false,
                    showDescription: false,
                    hasOtherOption: false
                });
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
                let newOptionItemTemplate = JSON.parse(JSON.stringify(this.optionItemTemplate));
                newOptionItemTemplate.Id = uuidv1();
                newOptionItemTemplate.OptionText.Id = uuidv1();
                for (let languageOptionItem of newOptionItemTemplate.OptionText.Values.$values) {
                    languageOptionItem.Id = uuidv1();
				}

                field.Options.$values.push(newOptionItemTemplate);
                console.log("field options", field.Options.$values);
            },

            /**
             * 
             * @param {any} fieldIndex
             * @param {any} optionIndex
             */
            selectOptionAsDefault(fieldIndex, optionIndex) {
                //if selected already, deselect it

                if (this.fields[fieldIndex].Options.$values[optionIndex].Selected === null
                    || !this.fields[fieldIndex].Options.$values[optionIndex].Selected) {
                    this.fields[fieldIndex].Options.$values[optionIndex].Selected = true;
                } else {
                    this.fields[fieldIndex].Options.$values[optionIndex].Selected = false;
				}

                //desselect any others in the group
                for (let optionItem of this.fields[fieldIndex].Options.$values) {
                    if (optionItem.Id == this.fields[fieldIndex].Options.$values[optionIndex].Id) {
                        continue;
					}
                    optionItem.Selected = false;
				}

                console.log(this.fields[fieldIndex].Options.$values);
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
            removeOption(fieldIndex, optionIndex) {
                this.fields[fieldIndex].Options.$values.splice(optionIndex, 1);
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
             * Sets the static strings on the page 
             **/
            setStaticValues() {
                this.saveSuccessfulLabel = StaticItems.managerSideValues.editFieldFormLabels.SAVE_SUCCESS_LABEL;
                this.saveFailedLabel = StaticItems.managerSideValues.editFieldFormLabels.SAVE_FAILED_LABEL;
                this.saveFieldFormButtonLabel = StaticItems.managerSideValues.editFieldFormLabels.SAVE_FIELD_FORM_BUTTON_LABEL;

                this.formTitleLabel = StaticItems.managerSideValues.editFieldFormLabels.FORM_TITLE_LABEL;
                this.formTitlePlaceholder = StaticItems.managerSideValues.editFieldFormLabels.FORM_TITLE_PLACEHOLDER;
                this.formDescriptionLabel = StaticItems.managerSideValues.editFieldFormLabels.FORM_DESCRIPTION_LABEL;
                this.formDescriptionPlaceholder = StaticItems.managerSideValues.editFieldFormLabels.FORM_DESCRIPTION_PLACEHOLDER;
                this.formFieldLabel = StaticItems.managerSideValues.editFieldFormLabels.FORM_FIELD_LABEL;
                this.defaultFieldTitle = StaticItems.managerSideValues.editFieldFormLabels.DEFAULT_FIELD_TITLE;
                this.fieldTitlePlaceholder = StaticItems.managerSideValues.editFieldFormLabels.FIELD_TITLE_PLACEHOLDER;
                this.fieldDescriptionLabel = StaticItems.managerSideValues.editFieldFormLabels.FIELD_DESCRIPTION_LABEL;
                this.fieldDescriptionPlaceholder = StaticItems.managerSideValues.editFieldFormLabels.FIELD_DESCRIPTION_PLACEHOLDER;
                this.settingsLabel = StaticItems.managerSideValues.editFieldFormLabels.SETTINGS_LABEL;
                this.longAnswerFormatTextLabel = StaticItems.managerSideValues.editFieldFormLabels.LONG_ANSWER_FORMAT_TEXT_LABEL;
                this.choiceOptionLabel = StaticItems.managerSideValues.editFieldFormLabels.CHOICE_OPTION_LABEL;
                this.choiceDefaultOptionLabel = StaticItems.managerSideValues.editFieldFormLabels.CHOICE_DEFAULT_OPTION_LABEL;
                this.choiceAdditionalInputLabel = StaticItems.managerSideValues.editFieldFormLabels.CHOICE_ADDITIONAL_INPUT_LABEL;
                this.anyLabel = StaticItems.managerSideValues.editFieldFormLabels.ANY_LABEL;
                this.allowMultipleFilesLabel = StaticItems.managerSideValues.editFieldFormLabels.ALLOW_MULTIPLE_FILES_LABEL;
                this.wholeNumbersOnlyLabel = StaticItems.managerSideValues.editFieldFormLabels.WHOLE_NUMBERS_ONLY_LABEL;
                this.requiredLabel = StaticItems.managerSideValues.editFieldFormLabels.REQUIRED_LABEL;
                this.addDescriptionLabel = StaticItems.managerSideValues.editFieldFormLabels.ADD_DESCRIPTION_LABEL;
                this.removeDescriptionLabel = StaticItems.managerSideValues.editFieldFormLabels.REMOVE_DESCRIPTION_LABEL;
                this.loadingLabel = StaticItems.managerSideValues.editFieldFormLabels.LOADING_LABEL;
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
                                console.log("second res", fieldDefsResult)
                                
                                for (let defaultField of fieldDefsResult.$values) {

                                    //store fieldType for dropdown
                                    if (defaultField.$type != this.INTEGER_TYPE) {
                                        this.fieldTypes.push({
                                            $type: defaultField.$type,
                                            DisplayLabel: defaultField.DisplayLabel
                                        });
									}
                                    

                                    //templates handled here, remove any default data and store the structure
                                    switch (defaultField.$type) {
                                        case this.TEXTFIELD_TYPE:
                                            this.textfieldTemplate = defaultField;

                                            for (let languageIndex in this.textfieldTemplate.Name.Values.$values) {
                                                this.$set(this.textfieldTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.textfieldTemplate.Description.Values.$values[languageIndex], 'Value', '');
											}
                                            break;
                                        case this.TEXTAREA_TYPE:
                                            this.textAreaTemplate = defaultField;

                                            for (let languageIndex in this.textAreaTemplate.Name.Values.$values) {
                                                this.$set(this.textAreaTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.textAreaTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;
                                        case this.RADIO_TYPE:
                                            this.radioTemplate = defaultField;
                                            //stores an option item to be used by all option-item fields (radio/checkbox/dropdown)
                                            this.optionItemTemplate = JSON.parse(JSON.stringify(defaultField.Options.$values[0]));
                                            //if more than one option, remove the other options
                                            if (defaultField.Options.$values.length > 1){
                                                //delete all other options except for first one
                                                this.radioTemplate.Options.$values.splice(1, defaultField.Options.$values.length - 1);
                                            }

                                            for (let languageIndex in this.radioTemplate.Name.Values.$values) {
                                                this.$set(this.radioTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.radioTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.radioTemplate.Options.$values[0].OptionText.Values.$values[languageIndex], 'Value', '');

                                                this.$set(this.optionItemTemplate.OptionText.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;

                                        case this.CHECKBOX_TYPE:
                                            this.checkboxTemplate = defaultField;

                                            //if more than one option, remove the other options
                                            if (defaultField.Options.$values.length > 1) {
                                                //delete all other options except for first one
                                                this.checkboxTemplate.Options.$values.splice(1, defaultField.Options.$values.length - 1);
                                            }

                                            for (let languageIndex in this.checkboxTemplate.Name.Values.$values) {
                                                this.$set(this.checkboxTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.checkboxTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.checkboxTemplate.Options.$values[0].OptionText.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;

                                        case this.DROPDOWN_TYPE:
                                            this.dropdownTemplate = defaultField;

                                            //if more than one option, remove the other options
                                            if (defaultField.Options.$values.length > 1) {
                                                //delete all other options except for first one
                                                this.dropdownTemplate.Options.$values.splice(1, defaultField.Options.$values.length - 1);
                                            }

                                            for (let languageIndex in this.dropdownTemplate.Name.Values.$values) {
                                                this.$set(this.dropdownTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.dropdownTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.dropdownTemplate.Options.$values[0].OptionText.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;

                                        case this.INFOSECTION_TYPE:
                                            this.displayFieldTemplate = defaultField;
                                            //temporary line to prevent an error. 
                                            //QuillEditor expects $values to be type string, but it comes in as an array
                                            this.displayFieldTemplate.Content.Values.$values = '';

                                            for (let languageIndex in this.displayFieldTemplate.Name.Values.$values) {
                                                this.$set(this.displayFieldTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.displayFieldTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;

                                        case this.DATE_TYPE:
                                            this.datePickerTemplate = defaultField;

                                            for (let languageIndex in this.datePickerTemplate.Name.Values.$values) {
                                                this.$set(this.datePickerTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.datePickerTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;

                                        case this.DECIMAL_TYPE:
                                            this.numberPickerTemplate = defaultField;
                                            this.numberPickerTemplate.isIntegerOnly = false;

                                            for (let languageIndex in this.numberPickerTemplate.Name.Values.$values) {
                                                this.$set(this.numberPickerTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.numberPickerTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;

                                        case this.MONOLINGUAL_TEXTFIELD_TYPE:
                                            this.monolingualTextFieldTemplate = defaultField;

                                            for (let languageIndex in this.monolingualTextFieldTemplate.Name.Values.$values) {
                                                this.$set(this.monolingualTextFieldTemplate.Name.Values.$values[languageIndex], 'Value', '');
                                                this.$set(this.monolingualTextFieldTemplate.Description.Values.$values[languageIndex], 'Value', '');
                                            }
                                            break;
                                        //fileattachment need to be added from the backend
                                    }
                                    
                                }
                                
                                //TODO handle this area now that all data is being sent with api
                                //temp set other values that i dont have sample data for
                                //guessing for what will be needed, adjust when dummy data given
                                //this.textAreaTemplate = JSON.parse(JSON.stringify(this.textfieldTemplate));
                                //this.textAreaTemplate.$type = 'Catfish.Core.Models.Contents.Fields.TextArea, Catfish.Core';

                                //this.radioTemplate = JSON.parse(JSON.stringify(this.textfieldTemplate));
                                //this.radioTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Radio, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                //this.radioTemplate.Values.$values = [];

                                //this.checkboxTemplate = JSON.parse(JSON.stringify(this.textfieldTemplate));
                                //this.checkboxTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Checkbox, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                //this.checkboxTemplate.Values.$values = [];

                                //this.dropdownTemplate = JSON.parse(JSON.stringify(this.textfieldTemplate));
                                //this.dropdownTemplate.$type = 'Catfish.Core.Models.Contents.Fields.Dropdown, Catfish.Core';
                                //not sure if this would be right, will likely need to adjust this
                                //this.dropdownTemplate.Values.$values = [];

                                this.fileAttachmentTemplate = JSON.parse(JSON.stringify(this.textfieldTemplate));
                                this.fileAttachmentTemplate.$type = 'Catfish.Core.Models.Contents.Fields.FileAttachment, Catfish.Core';
                                this.fileAttachmentTemplate.Values.$values = [];

                                //this.displayFieldTemplate = JSON.parse(JSON.stringify(this.textfieldTemplate));
                                //this.displayFieldTemplate.$type = 'Catfish.Core.Models.Contents.Fields.DisplayField, Catfish.Core';
                                //this.displayFieldTemplate.Values.$values = "";

                            })
                            .then(() => {
                                //this.finishedGET = true; test for empty return, remove later (or dont)
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

                                resolve();

                            })
                            .catch(function (error) { console.log("error:", error); });
                    });

                });
                
            },
        },
        created() {
            this.itemId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
            this.setStaticValues();
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