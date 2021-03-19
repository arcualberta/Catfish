//import { VueEditor } from "vue2-editor";
import { v1 as uuidv1 } from 'uuid';
import StaticItems from '../static/string-values.json';
/**
 * Javascript Vue code for creating a single item edit layout in ItemEdit.cshtml.
 */

/**
 * This check makes sure the file is only run on the page with
 * the element. Not a huge deal, can be removed if it's causing issues.
 */
if (document.getElementById("item-edit-page")) {

    Vue.component('item-field-component', {
        props: ['fieldData', 'isInPreviewMode', 'languageLabels'],
        data() {
            return {
                //key-value pairs of input types from the database and their associated
                //input type
                inputTypes: {
                    "text": "Catfish.Core.Models.Contents.Fields.TextField",
                    "textarea": "Catfish.Core.Models.Contents.Fields.TextArea",
                    "date": "Catfish.Core.Models.Contents.Fields.DateField",
                    "integer": "Catfish.Core.Models.Contents.Fields.IntegerField",
                    "decimal": "Catfish.Core.Models.Contents.Fields.DecimalField",
                },
                fieldRequiredLabel: '',
                valueLabel: '',
                deleteLabel: '',
                
                //temp so ids are unique per field, they will be with real data
                uniqueIdForField: '',

            }
        },
        methods: {
            /**
             * Adds another entry set to the field
             */
            addNewValue() {
                let valueObjToCopy = Object.values(this.fieldData.ValueGroups)[0];
                let newEntry = JSON.parse(JSON.stringify(valueObjToCopy));
                for (let val of newEntry) {
                    val.Value = "";
                    val.Id = uuidv1();
                }
                let newEntryId = uuidv1();
                this.$set(this.fieldData.ValueGroups, newEntryId, newEntry);
                this.fieldData.ValueIds.splice(this.fieldData.ValueIds.length, 0, newEntryId);
                

            },

            /**
             * Deletes the field from the item
             */
            deleteField(fieldValueId) {
                //this.metadataSets[metadataSetId].Fields.$values.splice(fieldId, 1);
                //this.setOriginalFields();
                let indexToRemove = this.fieldData.ValueIds.indexOf(fieldValueId);
                this.fieldData.ValueIds.splice(indexToRemove, 1);

                delete this.fieldData.ValueGroups[fieldValueId]
                   
                console.log(this.fieldData);
            }
        },

        created() {
            this.fieldRequiredLabel = StaticItems.managerSideValues.editItemLabels.FIELD_REQUIRED_LABEL;
            this.valueLabel = StaticItems.managerSideValues.editItemLabels.VALUE_LABEL;
            this.deleteLabel = StaticItems.managerSideValues.editItemLabels.DELETE_LABEL;
        },
        template: `
        <div class="sitemap-item additional-spacing">
            <div class="link">
                <div class="flex-row" v-for="(val, index) of fieldData.ValueIds">
                    
                    <!--<div v-if="!isInPreviewMode" class="col-md-4 mb-3 metadata-input">-->

                        <div v-if="fieldData.ModelType.includes(inputTypes.text)" v-for="(fieldValue, fieldValueIndex) of fieldData.ValueGroups[val]" class="metadata-input">
                            <div>
                                <label :for="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"><h4>{{fieldData.Name[fieldValueIndex].Value}} </h4></label>
                                <div v-if="!isInPreviewMode">
                                    <input :id="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"
                                    required type="text" class="form-control"
                                    v-model="fieldValue.Value"
                                    >
                                    <div class="invalid-feedback">
                                        This field is required.
                                    </div>
                                </div>
                                <div v-else>
                                    <div>{{fieldValue.Value}}</div>
                                </div>
                            </div>
                            <div v-if="!isInPreviewMode" class="btn-group float-right-button space-above" role="group">
                                <button :disabled="fieldData.ValueIds.length <= 1"
                                    type="button" v-on:click="deleteField(val)"
                                    class="btn btn-sm btn-danger btn-labeled trash-button">
                                    <i class="fas fa-trash"></i>
                                    {{deleteLabel}}
                                </button>
                            </div>
                            <hr style="width: 100%; display:inline-flex;">
                        </div>
                        
                        <div v-else-if="fieldData.ModelType.includes(inputTypes.textarea)" v-for="(fieldValue, fieldValueIndex) of fieldData.ValueGroups[val]" class="metadata-input">
                            <div>
                                <label :for="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"><h4>{{fieldData.Name[fieldValueIndex].Value}} </h4></label>
                            </div>
                            <div v-if="!isInPreviewMode">
                                <textarea :id="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"
                                required class="form-control" rows="3"
                                v-model="fieldValue.Value"></textarea>
                                <div class="invalid-feedback">
                                    This field is required.
                                </div>
                            </div>
                            <div v-else>
                                <div>{{fieldValue.Value}}</div>
                            </div>
                            <div v-if="!isInPreviewMode" class="btn-group float-right-button space-above" role="group">
                                <button :disabled="fieldData.ValueIds.length <= 1"
                                    type="button" v-on:click="deleteField(val)"
                                    class="btn btn-sm btn-danger btn-labeled trash-button">
                                    <i class="fas fa-trash"></i>
                                    {{deleteLabel}}
                                </button>
                            </div>
                        </div>

                        <div v-else-if="fieldData.ModelType.includes(inputTypes.date)" v-for="(fieldValue, fieldValueIndex) of fieldData.ValueGroups[val]" class="metadata-input">
                            <div>
                                <label :for="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"><h4>{{fieldData.Name[fieldValueIndex].Value}} </h4></label>
                            </div>
                            <div v-if="!isInPreviewMode">
                                <input type="date" 
                                v-model="fieldValue.Value" required class="form-control">
                                <div class="invalid-feedback">
                                    This field is required.
                                </div>
                            </div>
                            <div v-else>
                                <div>
                                    {{fieldValue.Value}}
                                </div>
                            </div>
                        </div>

                        <div v-else-if="fieldData.ModelType.includes(inputTypes.integer)" v-for="(fieldValue, fieldValueIndex) of fieldData.ValueGroups[val]" class="metadata-input">
                            <div>
                                <label :for="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"><h4>{{fieldData.Name[fieldValueIndex].Value}} </h4></label>
                            </div>
                            <div v-if="!isInPreviewMode">
                                <input type="number" v-if="!isInPreviewMode"
                                v-model="fieldValue.Value" required class="form-control">
                                <div class="invalid-feedback">
                                    This field is required.
                                </div>
                            </div>
                            <div v-else>
                                <div>
                                    {{fieldValue.Value}}
                                </div>
                            </div>
                        </div>

                        <!--TODO need to come back and adjust this for better decimal functionality -->
                        <div v-else-if="fieldData.ModelType.includes(inputTypes.decimal)" v-for="(fieldValue, fieldValueIndex) of fieldData.ValueGroups[val]" class="metadata-input">
                            <div>
                                <label :for="val + '-index-' + fieldValueIndex + '-' + index + fieldValue.Id"><h4>{{fieldData.Name[fieldValueIndex].Value}} </h4></label>
                            </div>
                            <div v-if="!isInPreviewMode">
                                <input type="number" step=".01"
                                required class="form-control" v-model="fieldValue.Value">
                                <div class="invalid-feedback">
                                    This field is required.
                                </div>
                            </div>
                            <div v-else>
                                <div>
                                    {{fieldValue.Value}}
                                </div>
                            </div>
                        </div>
                        <div class="invalid-feedback">
                            {{fieldRequiredLabel}}
                        </div>
                    </div>
                    <!--<div v-else>
                        <span>
                            {{fieldData.Value}} 
                        </span>
                    </div>-->
                <!--</div>-->
                </div>
                

                <div v-if="piranha.permissions.pages.add && !isInPreviewMode" class="add-value-container">
                    <div class="btn-group float-right-button" role="group">
                        <button v-if="!fieldData.ModelType.includes(inputTypes.integer) && !fieldData.ModelType.includes(inputTypes.date)" type="button" v-on:click="addNewValue()"
                                class="btn btn-lg btn-primary btn-labeled">
                            <i class="fas fa-plus"></i>
                            {{valueLabel}}
                        </button>
                    </div>
                    

                </div>

            </div>
    `
    });

    piranha.itemlist = new Vue({
        el: '#item-edit-page',
        /*components: {
            VueEditor
        },*/
        data() {
            return {
                //api strings
                getString: "manager/api/items/",
                postString: "manager/api/items",//"manager/items/save",

                content: "<h1>Some initial content</h1>",

                loading: true,
                item: null,
                itemId: null,
                nameAttribute: null,
                descriptionAttribute: null,
                buttonOptions: [],
                //label for multichoice dropdown button
                mcDropdownButtonLabel: "",
                activeOption: "",

                //bring this in from somewhere else, will have ALL language abbreviations in it
                languages: {
                    en: "English",
                    fr: "Français",
                    sp: "Español"
                },
                DEFAULT_LANGUAGE: 'en',
                //array for displaying language labels listed in received JSON
                //im assuming here that all fields will have the
                //same languages enabled, since languages are enabled sitewide
                languageLabels: [],

                //using this because the root Name and Description display is the same, so i load them into this to display in a loop
                sections: [
                    {
                        title: ''
                    },
                    {
                        title: ''
                    },
                ],

                metadataSets: [],
                metadataSets_type: null,
                metadataSetLabel: "Metadata Sets",

                //stores the first time a field appears in the fields of a metadata set
                //this would be better handled by using child components but 
                //project structure for Vue stuff is really weird...
                originalFieldIndexMaster: [],
                originalFields: [],
                isInPreviewMode: false,
                savePreviewEditButtonType: "submit",

                saveSuccessfulLabel: "Saved!",
                saveFailedLabel: "Failed to Save",
                saveFieldsRequiredLabel: "Some fields are required",
                saveStatus: 0,
                validForm: true
            }
        },
        computed: {
            itemName: {
                get: function () {
                    return this.nameAttribute.Value || "";
				}
			}
		},
        methods: {
            /**
             * Fetches the data associated with the item's ID
             **/
            fetchData() {
                var self = this;
                console.log(piranha.baseUrl + this.getString + this.itemId);
                piranha.permissions.load(function () {
                    fetch(piranha.baseUrl + self.getString + self.itemId)
                        .then(function (response) { return response.json(); })
                        .then(function (result) {
                            self.item = result;
                            console.log("json received:", self.item);
                            self.nameAttribute = result.Name;
                            self.descriptionAttribute = result.Description;
                            self.metadataSets = result.MetadataSets;
                            self.updateBindings = true;

                            self.sections[0].value = self.nameAttribute;
                            self.sections[1].value = self.descriptionAttribute;

                            //prepare language labels
                            self.setLanguageLabels(self.sections);

                            //track original field indices
                            //self.setOriginalFields();

                        })
                        .catch(function (error) { console.log("error:", error); });
                });
            },

            /**
             * Perform the action the multichoice button states.
             * @param {any} event
             */
            performMCButtonAction(event, option) {
                switch (option) {
                    case this.buttonOptions[0]:
                        this.saveForm(event);
                        break;
                    case this.buttonOptions[1]:
                        //edit view
                        this.isInPreviewMode = false;
                        break;
                    case this.buttonOptions[2]:
                        //preview view
                        this.isInPreviewMode = true;
                        break;
                }
                this.activeOption = option;
            },

            /**
             * Checks the form for validity and displays required styles if invalid
             * */
            validateForm() {
                let validity = this.$refs.myForm.checkValidity();
                this.$refs.myForm.classList.add('was-validated');
                return validity;
            },

            /**
             * Saves the form, calls the API to send the data to.
             * @param {any} event
             */
            saveForm(event) {
                event.preventDefault();
                this.validForm = this.validateForm();

                //OLD - this is the Bootstrap way to do this, doesn't utilize Vue
                //do form validation here and dont submit if problems
                //var forms = document.getElementsByClassName('edit-form');
                // Loop over them and prevent submission
                //Array.prototype.filter.call(forms, function (form) {
                //        if (form.checkValidity() === false) {
                //            event.preventDefault();
                //            event.stopPropagation();
                //            validForm = false;
                //        }
                //        console.log("form validated");
                //        form.classList.add('was-validated');
                //});

                if (this.validForm) {
                    console.log("item being posted is here:", this.item);

                    fetch(piranha.baseUrl + this.postString,
                        {
                            method: "POST",
                            headers: {
                                'Content-Type': 'application/json', //TODO add multipart for files possiblility
                            },
                            body: JSON.stringify(this.item)
                        })
                        .then((res) => {
                            if (res.ok) {
                                this.saveStatus = 1;
                                console.log("????");
                                setTimeout(() => { this.saveStatus = 0; }, 3000);
                            } else {
                                this.saveStatus = -1;
                                console.log("!!!!!");
                            }
                            console.log("res", res);
                            return res;
                        })
                        .then(function (data) { /*alert(JSON.stringify(data))*/ })
                        .catch((error) => {
                            console.error('Error:', error);
                        });
                } else {
                    console.log("form invalid");
                    this.saveStatus = -1;
                }
                
			},

            bind() {
                var self = this;

                $(".sitemap-container").each(function (i, e) {
                    $(e).nestable({
                        maxDepth: 100,
                        group: i,
                        callback: function (l, e) {
                            fetch(piranha.baseUrl + "manager/api/page/move", {
                                method: "post",
                                headers: {
                                    "Content-Type": "application/json"
                                },
                                body: JSON.stringify({
                                    id: $(e).attr("data-id"),
                                    items: $(l).nestable("serialize")
                                })
                            })
                                .then(function (response) { return response.json(); })
                                .then(function (result) {
                                    piranha.notifications.push(result.status);

                                    if (result.status.type === "success") {
                                        $('.sitemap-container').nestable('destroy');
                                        self.sites = [];
                                        Vue.nextTick(function () {
                                            self.sites = result.sites;
                                            Vue.nextTick(function () {
                                                self.bind();
                                            });
                                        });
                                    }
                                })
                                .catch(function (error) {
                                    console.log("error:", error);
                                });
                        }
                    })
                });
            },

            /**
             * Sets the initial language labels youll need for the item.
             * @param {any} sections
             */
            setLanguageLabels(sections) {
                for (let item of sections) {
                    let tmp = this.languages[item.Language];
                    if (typeof tmp === 'undefined') {
                        tmp = this.languages[this.DEFAULT_LANGUAGE];
					}
                    this.languageLabels.push(tmp);
                }
            },

            /**
             * Changes the multichoice button's title to the
             * pass parameter (user chose it from the dropdown)
             * @param {any} newLabel the new label for the button
             */
            changeButtonLabel(newLabel) {
                this.mcDropdownButtonLabel = newLabel;
                if (this.mcDropdownButtonLabel === this.buttonOptions[0]) {
                    this.savePreviewEditButtonType = "submit";
                } else {
                    this.savePreviewEditButtonType = "button";
				}
			},

            /**
             * Stores the indices of the first original version of a field.
             * This is useful for knowing which fields will not be able to be deleted
             * because they are the original version to be shown to the user.
             * If they were able to be deleted, there would be no way to show that field again!
             **/
            setOriginalFields() {
                this.originalFieldIndexMaster.splice(0);
                this.originalFields.splice(0);

                for (let [index, metadataSet] of this.metadataSets.entries()) {
                    this.originalFieldIndexMaster.splice(this.originalFieldIndexMaster.length, 1, {});
                    this.originalFields.splice(this.originalFields, 1, []); 

                    for (let [i, field] of metadataSet.Fields.$values.entries()) {
                        //if field differs from fields in originalFieldIndexMaster,
                        //track as a new field
                        var flattened = Object.keys(this.originalFieldIndexMaster[index]);

                        if (this.originalFieldIndexMaster[index].length === 0
                            || !flattened.some(item => item === field.Id) ) {

                            this.$set(this.originalFieldIndexMaster[index], field.Id, {
                                field: field.Id,
                                count: 1,
                                startingIndex: null
                            });
                            this.$set(this.originalFieldIndexMaster[index][field.Id], 'startingIndex', i);
                            this.originalFields[index].splice(this.originalFields[index].length, 1, i); 
                        }else {
                            //add to count of whichever is already in the object
                            //this needs to be checked to see if it works
                            var matched = flattened.filter((item, index) => {
                                if (item === field.Id) {
                                    return item;
								}
                            });

                            this.$set(this.originalFieldIndexMaster[index][matched[0]], 'count', 
                                this.originalFieldIndexMaster[index][matched[0]].count + 1);
	                    }
					}
                    console.log("originalFieldIndexMaster:", this.originalFieldIndexMaster);
                    console.log("indices: ", this.originalFields);
                }

            },

            setStaticItems() {
                this.buttonOptions = StaticItems.managerSideValues.editItemLabels.BUTTON_OPTION_LABELS;
                this.mcDropdownButtonLabel = StaticItems.managerSideValues.editItemLabels.DROPDOWN_BUTTON_LABEL;
                this.activeOption = StaticItems.managerSideValues.editItemLabels.ACTIVE_OPTION_LABEL;
                this.sections[0].title = StaticItems.managerSideValues.editItemLabels.SECTION_LABEL_1;
                this.sections[1].title = StaticItems.managerSideValues.editItemLabels.SECTION_LABEL_2;
                this.metadataSetLabel = StaticItems.managerSideValues.editItemLabels.METADATASET_LABEL;
                this.saveSuccessfulLabel = StaticItems.managerSideValues.editItemLabels.SAVE_SUCCESSFUL_LABEL;
                this.saveFailedLabel = StaticItems.managerSideValues.editItemLabels.SAVE_FAILED_LABEL;

			}
        },
        updated() {
            if (this.updateBindings) {
                this.bind();
                this.updateBindings = false;
            }

            this.loading = false;
        },
        created() {
            this.itemId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
            this.setStaticItems();
            //call api
            this.fetchData();
        },
        mounted() {

            //initializes all tooltips TODO put these back in, they were lost - or don't?
            $(document).ready(function () {
                $("body").tooltip({ selector: '[data-toggle=tooltip]' });
            });
		}
    })
}