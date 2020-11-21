import StaticItems from '../../static/string-values.json';

var vueApp = new Vue({
    el: '#keywords-search-block-public',
    data() {
        return {
            apiSearchUrl: "/api/solr/keywords",
            searchTerms: [],
            categories: '',
            searchResults: [],
            pagesTotal: 0,
            currentPage: 1,
            cardsPerPage: 100,
            searchMade: false,
            loadingSearchResults: false,
            //error variable for if the fetch ever fails,
            //will be used to show an error message to the user
            hasErrorOccurred: false,

            searchResultsLabel: '',
            previousButtonLabel: '', 
            nextButtonLabel: '',
            totalResultsLabel: '',
            noResultsLabel: '',
            errorMessage1Label: '', 
            errorMessage2Label: '',
            loadingLabel: '',
            defaultSearchMessageLabel: ''
		}
    },
    methods: {

        /**
         * Stores/removes clicked filter button to call API again
         * @param {any} event clickevent
         */
        filterByCategory(event) {
            if (this.categories != event.target.value) {
                $('#category-' + this.categories).button('toggle');
                this.categories = event.target.value;
            } else {
                this.categories = '';
            }
            $('#' + event.target.id).button('toggle');
            
            console.log(event.target.id);

            this.makePostCall();
            
        },

        /**
         * Stores/removes clicked filter button to call API again
         * @param {any} event clickevent
         */
        filterByCheckbox(event) {
            if (!this.searchTerms.includes(event.target.value)) {
                this.searchTerms.push(event.target.value);
            } else {
                this.searchTerms.splice(this.searchTerms.indexOf(event.target.value), 1);
            }
            
            this.makePostCall();
        },

        makePostCall() {
            console.log(event);
            let formData = new FormData();
            this.searchTerms.forEach((item, index) => formData.append("keywords[" + index + "]", item));
            //formData.append("searchTerms", this.searchTerms);

            formData.append("categories[0]", this.categories);
            //this.categories.forEach((item, index) => formData.append("categories[" + index + "]", item));
            this.loadingSearchResults = true;
            for (var pair of formData.entries()) {
                console.log(pair[0] + ', ' + pair[1]);
            }

            fetch(this.apiSearchUrl, {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (!this.hasErrorOccurred) {
                        this.hasErrorOccurred = false;
					}

                    this.searchResults = data;
                    console.log(this.searchResults);

                    this.pagesTotal = Math.ceil(this.searchResults.length / 100);

                    this.searchMade = true;
                    this.loadingSearchResults = false;
                    this.currentPage = 1;
                })
                .catch(error => {
                    console.error("Failed to fetch: ", error);
                    this.hasErrorOccurred = true;
                })
		}
	},
    created() {
        //assign static values
        this.searchResultsLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.SEARCH_RESULTS_LABEL;
        this.previousButtonLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.PREVIOUS_BUTTON_LABEL;
        this.nextButtonLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.NEXT_BUTTON_LABEL;
        this.totalResultsLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.TOTAL_RESULTS_LABEL;
        this.noResultsLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.NO_RESULTS_LABEL;
        this.errorMessage1Label = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.ERROR_MESSAGE_1_LABEL;
        this.errorMessage2Label = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.ERROR_MESSAGE_2_LABEL;
        this.loadingLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.LOADING_LABEL;
        this.defaultSearchMessageLabel = StaticItems.publicSideValues.keywordsSearchBlockPublicLabels.DEFAULT_SEARCH_MESSAGE_LABEL;
    }
});