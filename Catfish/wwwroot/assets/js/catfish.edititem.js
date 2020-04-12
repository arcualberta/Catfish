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
                loading: true,
                item: null,
                itemId: null,
                nameAttribute: null,
                descriptionAttribute: null,
                metadataSets: null,
                //bring this in from somewhere else, will have ALL languages in it
                languages: {
                    en: "English",
                    fr: "Français",
                    sp: "Español"
                },
                //array for displaying languages listed in received JSON
                //im assuming here that both name and description will have the
                //same languages enabled, as languages are enabled sitewide
                languageLabels: [],

                sections: [
                    {
                        title: "Name"
                    },
                    {
                        title: "Description"
                    },
                ]

            }
        },
        computed: {
            itemName: {
                get: function () {
                    return this.nameAttribute.values[0].value || "";
				}
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

                            //for testing purposes, remove after
                            self.nameAttribute.values.push({
                                
                                    "format": "plain",
                                    "language": "fr",
                                    "rank": 0,
                                    "value": "Oui oui bonjour 166",
                                    "modelType": "Catfish.Core.Models.Contents.Text, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
                                
                            });

                            self.sections[0].values = self.nameAttribute.values;
                            self.sections[1].values = self.descriptionAttribute.values;

                            //prepare language labels
                            //for (let section of self.sections) {
                                for (let item of self.sections[0].values) {
                                    console.log(self.languages[item.language]);
                                    self.languageLabels.push(self.languages[item.language]);
                                }
                            //}

                        })
                        .catch(function (error) { console.log("error:", error); });
                });
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
             * Adds another entry set to the item
             * @param {any} entryType the type of entry, a string either 'name' or 'description'
             */
            addNewEntry(entryType) {
                /*let newEntry = {
                    format: null,
                    language: null,
                    rank: 0,
                    value: null,
                    modelType: null
                };*/

                //let newEntry = 

                if (entryType === 'name' || entryType === 'Name') {
                    newEntry.format = this.nameAttribute.values[0].format;
                    newEntry.language = this.nameAttribute.values[0].language;
                    newEntry.modelType = this.nameAttribute.values[0].modelType;
                    this.nameAttribute.values.push(newEntry);
                } else {
                    newEntry.format = this.descriptionAttribute.values[0].format;
                    newEntry.language = this.descriptionAttribute.values[0].language;
                    newEntry.modelType = this.descriptionAttribute.values[0].modelType;
                    this.descriptionAttribute.values.push(newEntry);
				}



            },

            /**
             * Deletes the set from the item
             * @param {any} setId
             */
            deleteSet(setId) {

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
            //call api
            this.fetchData();
            
        }
    })
}