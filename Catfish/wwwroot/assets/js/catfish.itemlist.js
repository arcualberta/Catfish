/**
 * Javascript Vue code for creating the item layout in ItemList.cshtml.
 * It is modelled after the file piranha.pagelist.js in Piranha's source code.
 */


/**
 * This check makes sure the file is only run on the page with
 * the element. Not a huge deal, can be removed if it's causing issues.
 */
if (document.getElementById("itemlist-page")) {
    piranha.itemlist = new Vue({
        el: '#itemlist-page',
        data() {
            return {
                loading: true,
                updateBindings: false,
                noResultsFound: false,
                items: [],
                collections: [],
                //this is the $type value for Collections
                collections_type: null,
                itemTypes: [],
                itemTypes_type: null,
                collectionTypes: [],
                addSiteId: null,
                addSiteTitle: null,
                addPageId: null,
                addAfter: true,
                currentDropdownOptions: [],
                lastDropdownAction: null,
                dropdowns: {},
                /*
                 * collapseAll is tri-state 
                 * 0 = false, all are open
                 * 1 = mixed, mixed open/closed
                 * 2 = true, all are closed
                */
                collapseAll: 2,
                mixedStateLeans: true
            }
        },
        methods: {

            /**
             * Fetches and loads the data from an API call
             * */
            load() {
                var self = this;
                console.log(piranha.baseUrl + "manager/api/items");
                piranha.permissions.load(function () {
                    fetch(piranha.baseUrl + "manager/api/items")
                        .then(function (response) { return response.json(); })
                        .then(function (result) {
                            //check if result is null
                            //this may need adjusting, I think it should be at least always sending some default data
                            if (result == null) {
                                console.log("result is null", result);
                                //this still needs to trigger a change in Vue to run update() and finish loading
                                self.collections = [];
                                this.noResultsFound = true;
                            } else {
                                self.collections = result.Collections.$values;
                                self.collections_type = result.Collections.$type;
                                self.itemTypes = result.ItemTypes.$values;
                                self.itemTypes_type = result.ItemTypes.$type;
                                self.collectionTypes = result.CollectionTypes;
                                self.updateBindings = true;

                                self.dropdowns = new function () {
                                    for (let collection of self.collections) {
                                        this[collection.Id] = { isCollapsed: false }
                                    }
                                }
                            }
                            

                        })
                        .catch(function (error) { console.log("error:", error); });
                });
            },
            remove(id) {
                var self = this;

                piranha.alert.open({
                    title: piranha.resources.texts.delete,
                    body: piranha.resources.texts.deletePageConfirm,
                    confirmCss: "btn-danger",
                    confirmIcon: "fas fa-trash",
                    confirmText: piranha.resources.texts.delete,
                    onConfirm: function () {
                        fetch(piranha.baseUrl + "manager/api/page/delete/" + id)
                            .then(function (response) { return response.json(); })
                            .then(function (result) {
                                piranha.notifications.push(result);

                                self.load();
                            })
                            .catch(function (error) { console.log("error:", error); });
                    }
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
            add(siteId, pageId, after) {
                var self = this;

                self.addSiteId = siteId;
                self.addPageId = pageId;
                self.addAfter = after;

                // Get the site title
                self.sites.forEach(function (e) {
                    if (e.id === siteId) {
                        self.addSiteTitle = e.title;
                    }
                });

                // Open the modal
                $("#pageAddModal").modal("show");
            },
            selectSite(siteId) {
                var self = this;

                self.addSiteId = siteId;

                // Get the site title
                self.sites.forEach(function (e) {
                    if (e.id === siteId) {
                        self.addSiteTitle = e.title;
                    }
                });
            },

            /**
             * For adding the dropdown control to the new collection
             * */
            addDropdownCollection(collectionId) {
                this.dropdowns = new function () {
                    this[collectionId] = { collapsed: false };
				}
            },

            /**
             * For removing the dropdown control to a deleted collection
             * */
            removeDropdownCollection(collectionId) {
                delete this.dropdown[collectionId];
            },

            /**
             * Toggles the collection to either open or closed.
             * Icon for showing open/closed relies on open/closed state,
             * hence the necessity for this function. TODO still not working well on fast clicks,
             * if it can't be fixed then delete this function and just handle in template
             * 
             * @param {any} collectionId the collection's id to open/close
             */
            toggleDropdown(collectionId) {
                this.dropdowns[collectionId].isCollapsed === true ? this.dropdowns[collectionId].isCollapsed = false : this.dropdowns[collectionId].isCollapsed = true;
                this.lastDropdownAction = this.dropdowns[collectionId].isCollapsed;
                this.assessExpandOrCollapseAll();
            },

            /**
             *  Assesses whether the expand/collapse all button should be either showing
             *  as expand all, mixed state, or collapse all.
             **/
            assessExpandOrCollapseAll() {
                let collapseCount = 0;
                for (let item of Object.values(this.dropdowns)) {
                    if (!item.isCollapsed) {
                        collapseCount++;
                    }
                }

                if (collapseCount === Object.values(this.dropdowns).length) {
                    this.collapseAll = 2;
                } else if (collapseCount === 0) {
                    this.collapseAll = 0;
                } else {
                    this.collapseAll = 1;
                }

                //this part helps to figure out whether mixed state will open/close all
                let expandedCount = Object.values(this.dropdowns).length - collapseCount;

                if (collapseCount < expandedCount) {
                    //collapse all
                    this.mixedStateLeans = true;
                } else if (collapseCount > expandedCount) {
                    //expand all
                    this.mixedStateLeans = false;
                } else {
                    //equal, check most recent action
                    switch (this.lastDropdownAction) {
                        case true:
                            //expand all
                            console.log('expand');
                            this.mixedStateLeans = false;
                            break;
                        case false:
                            //collapse all
                            console.log('collapse');
                            this.mixedStateLeans = true;
                            break;
                        default:
                            //collapse all
                            this.mixedStateLeans = true;
                            break;
                    }
                }
			},

            /**
             * Expands or collapses all the collections
             **/
            collapseAllCollections() {
                for (let key of Object.keys(this.dropdowns)) {
                    this.dropdowns[key].isCollapsed = this.mixedStateLeans;
                }
                //toggle makes all dropdowns change to the opposite state, maybe useful later
                if (!this.mixedStateLeans) {
                    $('.collapse').collapse("show");
                } else {
                    $('.collapse').collapse("hide");
                }

                if (!this.mixedStateLeans) {
                    this.collapseAll = 2;
                } else {
                    this.collapseAll = 0;
				}
                
                this.mixedStateLeans = !this.mixedStateLeans;
            },

            /**
             * Calculates the items/collections to show to the user upon
             * clicking the 'Add Item' button.
             * 
             * -Only show items not in the collection (directly) already.
             * -Other collections, not including the current collection as well as
             * -Not including any collections with the current collection in it already
             * (to avoid endless nesting). TODO: this point isnt quite done as fully as it could be,
             * there are mutiple levels to check for for this...
             */
            /*
            calculateAddItemList(currentCollection, currentItems) {
                let otherCollections = [];
                let otherItems = [];

                for (let collection of this.collections) {
                    if (currentCollection !== collection && !collection.items.includes(currentCollection)) {
                        otherCollections.push(collection);
                    } else {
                        continue;
                    }

                    let difference = collection.items.filter(item => {
                        return !currentItems.includes(item);
                    });

                    //remove any items that may have appeared in multiple collections
                    //TODO: test that this works dont currently have collections with this occuring
                    let overallDifference = difference.filter(item => {
                        return !otherItems.includes(item);
                    });

                    otherItems = otherItems.concat(overallDifference);
                    //otherItems = otherItems.concat(difference);
                }

                this.currentDropdownOptions = otherItems.concat(otherCollections);
                return this.currentDropdownOptions;
            }*/
            openEditItem(itemId) {
                window.location.href = 'edit/' + itemId;
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
            this.load();
            console.log(piranha);
        },
    });
}
