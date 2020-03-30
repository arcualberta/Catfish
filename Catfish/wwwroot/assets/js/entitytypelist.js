
////piranha.entitytypelist = new Vue({
////    el: "#entityTypelist",
////    data: {
////        loading: true,
////        updateBindings: false,
////        items: [],
////        addAfter: true
////    },
////    methods: {
////        load: function () {
////            var self = this;
////            console.log(piranha.baseUrl + "manager/api/entitytypes");
////            piranha.permissions.load(function () {
////                fetch(piranha.baseUrl + "manager/api/page/list")
////                    .then(function (response) { return response.json(); })
////                    .then(function (result) {
////                        self.items = result;
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
////                    fetch(piranha.baseUrl + "manager/api/entitytypes/delete/" + id)
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


////catfish.entitytypelist = new Vue({
////    el: "#entitytypelist",
////    data: {
////        loading: !0,
////        updateBindings: !1,
////        items: [],
////        sites: [],
////        pageTypes: [],
////        addSiteId: null,
////        addSiteTitle: null,
////        addPageId: null,
////        addAfter: !0
////    },
////    methods: {
////        load: function () {
////            var e = this;
////            console.log(piranha.baseUrl + "manager/api/page/list"), piranha.permissions.load(function () {
////                fetch(piranha.baseUrl + "manager/api/page/list").then(function (e) {
////                    return e.json()
////                }).then(function (i) {
////                    e.sites = i.sites, e.pageTypes = i.pageTypes, e.updateBindings = !0
////                }).catch(function (e) {
////                    console.log("error:", e)
////                })
////            })
////        },
////        remove: function (e) {
////            var i = this;
////            piranha.alert.open({
////                title: piranha.resources.texts.delete,
////                body: piranha.resources.texts.deletePageConfirm,
////                confirmCss: "btn-danger",
////                confirmIcon: "fas fa-trash",
////                confirmText: piranha.resources.texts.delete,
////                onConfirm: function () {
////                    fetch(piranha.baseUrl + "manager/api/page/delete/" + e).then(function (e) {
////                        return e.json()
////                    }).then(function (e) {
////                        piranha.notifications.push(e), i.load()
////                    }).catch(function (e) {
////                        console.log("error:", e)
////                    })
////                }
////            })
////        },
////        bind: function () {
////            var e = this;
////            $(".sitemap-container").each(function (i, a) {
////                $(a).nestable({
////                    maxDepth: 100,
////                    group: i,
////                    callback: function (i, a) {
////                        fetch(piranha.baseUrl + "manager/api/page/move", {
////                            method: "post",
////                            headers: {
////                                "Content-Type": "application/json"
////                            },
////                            body: JSON.stringify({
////                                id: $(a).attr("data-id"),
////                                items: $(i).nestable("serialize")
////                            })
////                        }).then(function (e) {
////                            return e.json()
////                        }).then(function (i) {
////                            piranha.notifications.push(i.status), "success" === i.status.type && ($(".sitemap-container").nestable("destroy"), e.sites = [], Vue.nextTick(function () {
////                                e.sites = i.sites, Vue.nextTick(function () {
////                                    e.bind()
////                                })
////                            }))
////                        }).catch(function (e) {
////                            console.log("error:", e)
////                        })
////                    }
////                })
////            })
////        },
////        add: function (e, i, a) {
////            var t = this;
////            t.addSiteId = e, t.addPageId = i, t.addAfter = a, t.sites.forEach(function (i) {
////                i.id === e && (t.addSiteTitle = i.title)
////            }), $("#pageAddModal").modal("show")
////        },
////        selectSite: function (e) {
////            var i = this;
////            i.addSiteId = e, i.sites.forEach(function (a) {
////                a.id === e && (i.addSiteTitle = a.title)
////            })
////        }
////    },
////    created: function () { },
////    updated: function () {
////        this.updateBindings && (this.bind(), this.updateBindings = !1), this.loading = !1
////    }
////})