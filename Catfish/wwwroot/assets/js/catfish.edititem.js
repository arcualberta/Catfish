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
                testField: {
                    Values: [],
                    Type: ''
                },
                threeLayerTest: {
                    Values: [],
                    Type: ''
                },

                //the better idea stuff
                newFieldData: {
                    Name: [],
                    Description: [],
                    ValueIds: []
                },
                userInputs: {},
                //temp so ids are unique per field, they will be with real data
                uniqueIdForField: '',

                //the better idea stuff - 100% flattened
                newFieldDataFlattened: {
                    Name: [],
                    Description: [],
                    ValueIds: []
                },
                userInputsFlattened: {}

            }
        },
        methods: {
            /**
             * Adds another entry set to the field
             */
            addNewValue() {

                //regular object method. this does not work because the object is too embedded to be reactive.
                //Vue cannot detect the new value added and so a new input is not added
                let newEntry = JSON.parse( JSON.stringify(this.fieldData.Values.$values[0]) );
                newEntry.Id = uuidv1();

                //TODO need to check this for Date, Integer, etc, the structure is different
                for (let item of newEntry.Values.$values) {
                    item.Value = "";
                }

                this.fieldData.Values.$values.splice(this.fieldData.Values.$values.length, 0, newEntry);
                console.log(this.fieldData.Values.$values);
                /////////////////////////////////////////////////////////////

                //testField is simplified to two layers, so the reactivity works
                //Vue can make a new input from this change
                let newEntry2 = JSON.parse(JSON.stringify(this.testField.Values[0]));
                newEntry2.Id = uuidv1();

                //TODO need to check this for Date, Integer, etc, the sfor (let item of newEntry.Values.$values) {
                newEntry2.Value = "";

                this.testField.Values.splice(this.testField.Values.length, 0, newEntry2);

                //////////////////////////////////////////////////////////////

                //three layer object
                let newEntry3 = JSON.parse(JSON.stringify(this.threeLayerTest.Values[0].$values[0]));
                newEntry3.Id = uuidv1();

                //TODO need to check this for Date, Integer, etc, the sfor (let item of newEntry.Values.$values) {
                newEntry3.Value = "";

                this.threeLayerTest.Values[0].$values.splice(this.threeLayerTest.Values[0].$values.length, 0, newEntry3);

                //////////////////////////////////////////////////////////////

                //semiflattened object
                let newEntry4 = JSON.parse(JSON.stringify(this.userInputs[this.newFieldData.ValueIds[0]]));
                for (let val of newEntry4) {
                    val.Value = "";
                    val.Id = uuidv1();
                }
                //needs an overall id - just assign?
                let newEntry4Id = uuidv1();
                this.$set(this.userInputs, newEntry4Id, newEntry4);
                this.newFieldData.ValueIds.splice(this.newFieldData.ValueIds.length, 0, newEntry4Id);
                

            },

            /**
             * Deletes the field from the item
             */
            deleteField() {
                //this.metadataSets[metadataSetId].Fields.$values.splice(fieldId, 1);
                //this.setOriginalFields();
            },
        },

        mounted() {
            //first index request is for language, second is values per language
            //so therefore, this is the values for a single language

            //therefore, to handle multiple languages - each one flattened?
            //some variable need to hold the keys for each language - maybe is just indices though?
            //testField only one language supported as-is, but its a good test to see if simplified works
            if (this.testField.Values.length <= 0 && this.fieldData.Values.$values[0].hasOwnProperty('Values')) {
                //simplified object
                this.testField.Values = JSON.parse(JSON.stringify(this.fieldData.Values.$values[0].Values.$values));
                this.testField.Type = JSON.parse(JSON.stringify(this.fieldData.$type));

                //three layer test
                this.threeLayerTest.Values[0] = JSON.parse(JSON.stringify(this.fieldData.Values.$values[0].Values));
                this.threeLayerTest.Type = JSON.parse(JSON.stringify(this.fieldData.$type));



                //slightly not 100% flattened object - easier to display
                //main object holds value-group ids
                this.newFieldData.Name = [
                    {
                        id: '0',
                        Value: 'Name',
                        Language: 'English',
                    },
                    {
                        id: '1',
                        Value: 'Nom',
                        Language: 'French'
                    }
                ];

                this.newFieldData.Description = [
                    {
                        id: '00',
                        Value: "I am a description for this item",
                    },
                    {
                        id: '01',
                        Value: "French description for the item",
                    },
                ];

                this.newFieldData.ValueIds = ['123', '456', '789'];

                this.userInputs = {
                    123:
                        [
                            {
                                id: "abc",
                                Value: "English words",
                                $type: "Catfish.Core.Models.Contents.Fields.TextField"
                            },
                            {
                                id: "def",
                                Value: "French words",
                                $type: "Catfish.Core.Models.Contents.Fields.TextField"
                            }
                        ],
                    456:
                        [
                            {
                                id: "ghi",
                                Value: "English words value 2",
                                $type: "Catfish.Core.Models.Contents.Fields.TextField"
                            },
                            {
                                id: "jkl",
                                Value: "French words value 2",
                                $type: "Catfish.Core.Models.Contents.Fields.TextField"
                            }
                        ],
                    789:
                        [
                            {
                                id: "mno",
                                Value: "English words value 3",
                                $type: "Catfish.Core.Models.Contents.Fields.TextField"
                            },
                            {
                                id: "pqr",
                                Value: "French words value 3",
                                $type: "Catfish.Core.Models.Contents.Fields.TextField"
                            }
                        ],
                };

                this.uniqueIdForField = uuidv1();

                //final one - completely flattened
                //similar to the above but will require more loops, might not be as easy to use
                //but it is 'correct' irt flattening
                //main object holds grouped value ids
                this.newFieldDataFlattened.ValueIds = { '1': ['123', '456'], '2': ['789', '101112'] };
                this.userInputsFlattened = {
                    123: {
                        id: "abc",
                        Value: "English words",
                        $type: "Catfish.Core.Models.Contents.Fields.TextField"
                    },
                    456: {
                        id: "cdf",
                        Value: "French words",
                        $type: "Catfish.Core.Models.Contents.Fields.TextField"
                    },
                    789: {
                        id: "ghi",
                        Value: "English words value 2",
                        $type: "Catfish.Core.Models.Contents.Fields.TextField"
                    },
                    101112: {
                        id: "jkl",
                        Value: "French words value 2",
                        $type: "Catfish.Core.Models.Contents.Fields.TextField"
                    }
                }

            }

        },

        created() {
            this.fieldRequiredLabel = StaticItems.managerSideValues.editItemLabels.FIELD_REQUIRED_LABEL;
            this.valueLabel = StaticItems.managerSideValues.editItemLabels.VALUE_LABEL;
            this.deleteLabel = StaticItems.managerSideValues.editItemLabels.DELETE_LABEL;
        },
        template: `
        <div class="sitemap-item">
            <div class="link">{{fieldData.$type}}
                <div class="flex-row" v-for="(val, index) of newFieldData.ValueIds">
                    
                     <div v-if="!isInPreviewMode" class="col-md-4 mb-3 metadata-input">

<!-- fieldData.Values.$values[index].Values.$values[0].Value -->
<!--
                        <h3>Simplified, 2 layer object, reacts to added value</h3>
                        <div v-if="fieldData.$type.includes(inputTypes.text)" v-for="(fieldValue, fieldValueIndex) of testField.Values"> 
                            <input
                            required type="text" class="form-control"
                            v-model="testField.Values[fieldValueIndex].Value"
                            >
                            <div class="btn-group new-value-button" role="group">
                                <button :disabled="testField.Values.length <= 1"
                                    type="button" v-on:click="deleteField()"
                                    class="btn btn-sm btn-danger btn-labeled trash-button">
                                    <i class="fas fa-trash"></i>
                                    {{deleteLabel}}
                                </button>
                            </div>
                        </div>
<div>{{testField}}</div>

<hr>

<h3>Object with three layers, reacts to added value(?)</h3>
                        <div v-if="fieldData.$type.includes(inputTypes.text)" v-for="(fieldValue, fieldValueIndex) of threeLayerTest.Values[0].$values"> 
                            <input
                            required type="text" class="form-control"
                            v-model="threeLayerTest.Values[0].$values[fieldValueIndex].Value"
                            >
                            <div class="btn-group new-value-button" role="group">
                                <button :disabled="threeLayerTest.Values[0].$values.length <= 1"
                                    type="button" v-on:click="deleteField()"
                                    class="btn btn-sm btn-danger btn-labeled trash-button">
                                    <i class="fas fa-trash"></i>
                                    {{deleteLabel}}
                                </button>
                            </div>
                        </div>
<div>{{threeLayerTest}}</div>

<hr>

<h3>Regular object, will not react to added value</h3> <b>{{fieldData.Values.$values[index]}}</b>
        <div v-if="fieldData.$type.includes(inputTypes.text)" v-for="(fieldValue, fieldValueIndex) of fieldData.Values.$values[index].Values.$values">
                            <!--<input
                            required type="text" class="form-control"
                            v-model="fieldData.Values.$values[index].Values.$values[fieldValueIndex].Value"
                            >
                            <div class="btn-group new-value-button" role="group">
                                <button :disabled="fieldData.Values.$values[index].Values.$values.length <= 1"
                                    type="button" v-on:click="deleteField()"
                                    class="btn btn-sm btn-danger btn-labeled trash-button">
                                    <i class="fas fa-trash"></i>
                                    {{deleteLabel}}
                                </button>
                            </div>
                        </div>

                        <div>{{fieldData.Values.$values[index]}}</div>

<hr>-->

<!-- <h3>Semi-flattened object, reacts to added value</h3> -->
<div v-if="fieldData.$type.includes(inputTypes.text)" v-for="(fieldValue, fieldValueIndex) of userInputs[val]"> <!-- fieldData.Values.$values[index].Values.$values[0].Value -->
                                <div>
                                    <label :for="fieldValue.id + '-index-' + fieldValueIndex + '-' + index + uniqueIdForField"><h4>{{newFieldData.Name[fieldValueIndex].Value}} ({{newFieldData.Name[fieldValueIndex].Language}}): </h4></label>
                                    <input :id="fieldValue.id + '-index-' + fieldValueIndex + '-' + index + uniqueIdForField"
                                    required type="text" class="form-control"
                                    v-model="fieldValue.Value"
                                    >
                                </div>
                                <div class="btn-group new-value-button" role="group">
                                    <button :disabled="newFieldData.ValueIds.length <= 1"
                                        type="button" v-on:click="deleteField()"
                                        class="btn btn-sm btn-danger btn-labeled trash-button">
                                        <i class="fas fa-trash"></i>
                                        {{deleteLabel}}
                                    </button>
                                </div>
                            </div>
                        

<!--
<h3>Flattened object, should react to added value... but it is too complex to easily display</h3>
<div v-if="fieldData.$type.includes(inputTypes.text)" v-for="(fieldValue, fieldValueIndex) of Object.values(newFieldDataFlattened.ValueIds)">
                        <div v-for="val of fieldValue">|{{val}}|
                            <div v-for="userInput of userInputsFlattened[val]">
                                <input
                                required type="text" class="form-control"
                                v-model="userInput.Value"
                                >{{userInputsFlattened[val]}}
                                <div class="btn-group new-value-button" role="group">
                                    <button :disabled="fieldValue.length <= 1"
                                        type="button" v-on:click="deleteField()"
                                        class="btn btn-sm btn-danger btn-labeled trash-button">
                                        <i class="fas fa-trash"></i>
                                        {{deleteLabel}}
                                    </button>
                                </div>
                            </div>
                        </div>
                        </div>
                        <div>{{newFieldDataFlattened}}</div>
                        <div>{{userInputsFlattened}}</div>
-->

<!--
                        <textarea v-else-if="fieldData.$type.includes(inputTypes.textarea)"
                        required class="form-control" rows="3"
                        v-model="fieldData.Values.$values[index].Values.$values[0].Value"></textarea>
                        <input type="date" v-else-if="fieldData.$type.includes(inputTypes.date)"
                        v-model="fieldData.Values.$values[index].Value"
                        required class="form-control">
                        <input type="number" v-else-if="fieldData.$type.includes(inputTypes.integer)"
                        v-model="fieldData.Values.$values[index].Value"
                        required class="form-control">-->
                        <!--TODO need to come back and adjust this for better decimal functionality -->
                        <!--<input type="number" step=".01" v-else-if="fieldData.$type.includes(inputTypes.decimal)"
                        required class="form-control" v-model="fieldData.Values.$values[index].Value">-->
                        <!--<vue-editor v-else-if="fieldData.$type.includes(inputTypes.textarea)
                                v-model="fieldData.Values.$values[index].Values.$values[0].Value">
                        </vue-editor>-->
                            <div class="invalid-feedback">
                                {{fieldRequiredLabel}}
                            </div>
                        </div>
                        <div v-else>
                            <div v-if="fieldData.Values.$values[index].hasOwnProperty('Values')">
                                {{fieldData.Values.$values[index].Values.$values[0].Value}}
                            </div>
                            <div v-else>
                                <span>
                                    {{fieldData.Values.$values[index].Value}} <!--.Values.$values[0].Value-->
                                </span>
                            </div>

                        </div>
                    </div>
                </div>
                <div>{{newFieldData}}</div>
                <div>{{userInputs}}</div>

                <div v-if="piranha.permissions.pages.add" class="add-value-container">
                    <div class="btn-group new-value-button" role="group">
                        <button type="button" v-on:click="addNewValue()"
                                class="btn btn-sm btn-primary btn-labeled">
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
                postString: "manager/items/save",

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
                saveStatus: 0,
            }
        },
        computed: {
            itemName: {
                get: function () {
                    return this.nameAttribute.Values.$values[0].Value || "";
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
                            self.metadataSets = result.MetadataSets.$values;
                            self.metadataSets_type = result.MetadataSets.$type;
                            self.updateBindings = true;

                            self.sections[0].values = self.nameAttribute.Values.$values;
                            self.sections[1].values = self.descriptionAttribute.Values.$values;

                            //prepare language labels
                            self.setLanguageLabels(self.sections);

                            //track original field indices
                            self.setOriginalFields();

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
             * Saves the form, calls the API to send the data to.
             * @param {any} event
             */
            saveForm(event) {
                event.preventDefault();
                let validForm = true;

                //do form validation here and dont submit if problems
                var forms = document.getElementsByClassName('edit-form');
                // Loop over them and prevent submission
                Array.prototype.filter.call(forms, function (form) {
                        if (form.checkValidity() === false) {
                            event.preventDefault();
                            event.stopPropagation();
                            validForm = false;
                        }
                        console.log("form validated");
                        form.classList.add('was-validated');
                });

                if (validForm) {
                    this.item.Name = this.nameAttribute;
                    this.item.Description = this.descriptionAttribute;
                    this.item.MetadataSets = {
                        $type: this.metadataSets_type,
                        $values: this.metadataSets,

                    };

                    console.log("item being posted is here:", this.item);

                    fetch(piranha.baseUrl + this.postString,
                        {
                            method: "POST",
                            headers: {
                                'Content-Type': 'application/json',
                            },
                            body: JSON.stringify(this.item)
                        })
                        .then((res) =>  {
                            if (res.ok) {
                                this.saveStatus = 1;
                                console.log("????");
                                setTimeout(() => { this.saveStatus = 0; }, 3000);
                            } else {
                                this.saveStatus = -1;
                                console.log("!!!!!");
                            }
                            console.log("res",res);
                            return res;
                        })
                        .then(function (data) { /*alert(JSON.stringify(data))*/ })
                        .catch((error) => {
                            console.error('Error:', error);
                        });
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
                for (let item of sections[0].values) {
                    let tmp = this.languages[item.language];
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

            //initializes all tooltips
            $(document).ready(function () {
                $("body").tooltip({ selector: '[data-toggle=tooltip]' });
            });
		}
    })
}