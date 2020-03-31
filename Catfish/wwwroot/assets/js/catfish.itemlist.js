
var vm = new Vue({
    el: '#overalltest',
    data() {
        return {
            loading: true,
            updateBindings: false,
            items: [],
            collections: [],
            itemTypes: [],
            addSiteId: null,
            addSiteTitle: null,
            addPageId: null,
            addAfter: true,
            currentDropdownOptions: []
		}
    },
    methods: {
        load() {
            var self = this;
            console.log("running the code");
            console.log(piranha.baseUrl + "manager/api/items");
            piranha.permissions.load(function () {
                fetch(piranha.baseUrl + "manager/api/items")
                    .then(function (response) { return response.json(); })
                    .then(function (result) {
                        self.collections = result.collections;
                        self.itemTypes = result.itemTypes;
                        self.updateBindings = true;
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
         * Calculates the items/collections to show to the user upon
         * clicking the 'Add Item' button.
         * 
         * -Only show items not in the collection (directly) already.
         * -Other collections, not including the current collection as well as
         * -Not including any collections with the current collection in it already
         * (to avoid endless nesting).
         * 
         */
        calculateAddItemList(currentCollection, currentItems) {
            let otherCollections = [];
            let otherItems = [];

            for (let collection of this.collections) {
                if (currentCollection !== collection && !collection.items.includes(currentCollection) ) {
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
		}
    },
    updated() {
        if (this.updateBindings) {
            this.bind();
            this.updateBindings = false;
        }

        this.loading = false;
    },
    created(){
        this.load();
	}
});