/**
 * Javascript Vue code for creating a single item edit layout in ItemEdit.cshtml.
 */


/**
 * This check makes sure the file is only run on the page with
 * the element. Not a huge deal, can be removed if it's causing issues.
 */
if (document.getElementById("item-edit-page")) {
    piranha.itemlist = new Vue({
        el: '#item-edit-page',
        data() {
            return {
                item: null,
                itemId: null,
                nameAttribute: null,
                descriptionAttribute: null,
                metadataSets: null,

            }
        },
        methods: {
            /**
             * Fetches the data associated with the item's ID
             **/
            fetchData() {
                var self = this;
                console.log(piranha.baseUrl + "manager/api/items/" + this.itemId);
                piranha.permissions.load(function () {
                    fetch(piranha.baseUrl + "manager/api/items/" +self.itemId)
                        .then(function (response) { return response.json(); })
                        .then(function (result) {
                            self.item = result;
                            self.nameAttribute = result.name;
                            self.descriptionAttribute = result.description;
                            self.metadataSets = result.metadataSets;
                            self.updateBindings = true;

                        })
                        .catch(function (error) { console.log("error:", error); });
                });
			}
        },
        created() {
            this.itemId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
            console.log("youre editing item: ", this.itemId);
            //call api
            this.fetchData();
            
        }
    })
}