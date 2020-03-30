
var vm = new Vue({
    el: '#overalltest',
    data() {
        return {
            loading: true,
            updateBindings: false,
            items: [],
            collections: [],
            pageTypes: [],
            addSiteId: null,
            addSiteTitle: null,
            addPageId: null,
            addAfter: true
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
                        self.pageTypes = result.pageTypes;
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



/*
 * This is directly pasted in from piranha.itemlist.js
 * Need to update functions for adding an item that
 * is chosen from a selection of items, coming from the
 * API call.
*/

////piranha.itemlist = new Vue({
////    el: "#itemlist",
////    data: {
////        loading: true,
////        updateBindings: false,
////        items: [],
////        sites: [],
////        pageTypes: [],
////        addSiteId: null,
////        addSiteTitle: null,
////        addPageId: null,
////        addAfter: true
////    },
////    methods: {
////        /**
////         * Loads the items in from the API call.
////         * This is used in the button 'New Item'
////         **/
////        load: function () {
////            var self = this;
////            console.log(piranha.baseUrl + "manager/api/page/list");
////            piranha.permissions.load(function () {
////                fetch(piranha.baseUrl + "manager/api/page/list")
////                    .then(function (response) { return response.json(); })
////                    .then(function (result) {
////                        self.sites = result.sites;
////                        self.pageTypes = result.pageTypes;
////                        self.updateBindings = true;
////                    })
////                    .catch(function (error) { console.log("error:", error); });
////            });
////        },
////        remove: function (id) {
////            var self = this;

////            piranha.alert.open({
////                title: piranha.resources.texts.delete,
////                body: piranha.resources.texts.deletePageConfirm,
////                confirmCss: "btn-danger",
////                confirmIcon: "fas fa-trash",
////                confirmText: piranha.resources.texts.delete,
////                onConfirm: function () {
////                    fetch(piranha.baseUrl + "manager/api/page/delete/" + id)
////                        .then(function (response) { return response.json(); })
////                        .then(function (result) {
////                            piranha.notifications.push(result);

////                            self.load();
////                        })
////                        .catch(function (error) { console.log("error:", error); });
////                }
////            });
////        },
////        bind: function () {
////            var self = this;

////            $(".sitemap-container").each(function (i, e) {
////                $(e).nestable({
////                    maxDepth: 100,
////                    group: i,
////                    callback: function (l, e) {
////                        fetch(piranha.baseUrl + "manager/api/page/move", {
////                            method: "post",
////                            headers: {
////                                "Content-Type": "application/json"
////                            },
////                            body: JSON.stringify({
////                                id: $(e).attr("data-id"),
////                                items: $(l).nestable("serialize")
////                            })
////                        })
////                            .then(function (response) { return response.json(); })
////                            .then(function (result) {
////                                piranha.notifications.push(result.status);

////                                if (result.status.type === "success") {
////                                    $('.sitemap-container').nestable('destroy');
////                                    self.sites = [];
////                                    Vue.nextTick(function () {
////                                        self.sites = result.sites;
////                                        Vue.nextTick(function () {
////                                            self.bind();
////                                        });
////                                    });
////                                }
////                            })
////                            .catch(function (error) {
////                                console.log("error:", error);
////                            });
////                    }
////                })
////            });
////        },
////        add: function (siteId, pageId, after) {
////            var self = this;

////            self.addSiteId = siteId;
////            self.addPageId = pageId;
////            self.addAfter = after;

////            // Get the site title
////            self.sites.forEach(function (e) {
////                if (e.id === siteId) {
////                    self.addSiteTitle = e.title;
////                }
////            });

////            // Open the modal
////            $("#pageAddModal").modal("show");
////        },
////        selectSite: function (siteId) {
////            var self = this;

////            self.addSiteId = siteId;

////            // Get the site title
////            self.sites.forEach(function (e) {
////                if (e.id === siteId) {
////                    self.addSiteTitle = e.title;
////                }
////            });
////        }
////    },
////    created: function () {
////    },
////    updated: function () {
////        if (this.updateBindings) {
////            this.bind();
////            this.updateBindings = false;
////        }

////        this.loading = false;
////    }
////});
