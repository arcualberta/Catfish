if (document.getElementById("process-page-container")) {
    piranha.processPage = new Vue({
        el: '#process-page-container',
        components: {
        },
        data() {
            return {
                postApi: "/api/solr/index",
                currentSite: null,

                hasErrorOccurred: false
            }
        },
        methods: {
            /**
             * Sets the current index of the site possibly being indexed
             * (possibly because there is a modal that appears next)
             * @param {any} siteIndex
             */
            setCurrentSite(siteIndex) {
                this.currentSite = siteIndex;
            },
            /**
             * Makes a POST call to index the given site 
             * @param siteId the site's Id (GUID)
             * @param siteTypeId the site's type Id
             **/
            indexTheSite(siteId, siteTypeId, buttonIndex) {
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

                        //set button to 'indexing'
                        //document.getElementById('siteIndex-' + this.currentSite).innerHTML = "Indexing";
                        document.getElementById("close-modal-button").click();

                    })
                    .catch(error => {
                        console.error("Failed to fetch: ", error);
                        this.hasErrorOccurred = true;
                    })
            }
        }
    })
}