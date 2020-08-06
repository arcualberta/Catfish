//import draggable from 'vuedraggable'

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
        data() {
            return {
                itemId: null,

                //api strings
                getString: "manager/api/forms/",
                //postString: "manager/items/save",
            }
        },
        methods: {
            /**
              * Fetches and loads the data from an API call
              * */
            load() {
                var self = this;
                console.log(piranha.baseUrl + this.getString + this.itemId);
                piranha.permissions.load(function () {
                    fetch(piranha.baseUrl + self.getString + self.itemId)
                        .then(function (response) { return response.json(); })
                        .then(function (result) {
                            //self.collections = result.collections;
                            self.updateBindings = true;
                            console.log(result);

                        })
                        .catch(function (error) { console.log("error:", error); });
                });
            },
        },
        created() {
            this.itemId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
            console.log(this.itemId);
            this.load();
            console.log(piranha);
        },
    });
}