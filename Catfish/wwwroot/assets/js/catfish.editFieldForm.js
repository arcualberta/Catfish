import draggable from 'vuedraggable'

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
            draggable
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
                        name: 'User Upload',
                        modelType: 'FileAttachment'
                    },
                    {
                        id: 6,
                        name: 'Rich Text',
                        modelType: 'DisplayField'
                    }
                ]
            }
        },
        methods: {

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
              * Fetches and loads the data from an API call
              * */
            load() {
                var self = this;
                console.log(piranha.baseUrl + this.getString + this.itemId);
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

                                resolve();

                            })
                            .catch(function (error) { console.log("error:", error); });
                    });

                });
                
            },
        },
        created() {
            this.itemId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
            console.log(this.itemId);
            this.load()
                .then(() => {
                    //for popovers
                    $(document).ready(function () {
                        $('[data-toggle="popover"]').popover();
                        console.log("ran popover");
                    });
                });
        }
    });
}