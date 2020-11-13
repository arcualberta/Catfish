piranha.processPage = new Vue({
    el: '#process-page-container',
    components: {
    },
    data() {
        return {
            postApi: "/api/solr/index",

            hasErrorOccurred: false
        }
    },
    methods: {
        /**
         * Makes a POST call to index the given site 
         * @param siteId the site's Id (GUID)
         * @param siteTypeId the site's type Id
         **/
        indexTheSite(siteId, siteTypeId) {
            let formData = new FormData();
            formData.append("siteId", siteId);
            formData.append("siteTypeId", siteTypeId);

            fetch(this.postApi, {
                method: 'POST',
                body: formData
            })
                .then(() => {
                    if (!this.hasErrorOccurred) {
                        this.hasErrorOccurred = false;
                    }

                })
                .catch(error => {
                    console.error("Failed to fetch: ", error);
                    this.hasErrorOccurred = true;
                })
        }
    }
})