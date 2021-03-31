//import StaticItems from '../../static/string-values.json';


Vue.component('controlled-vocabulary-search', {
    props: ["uid", "model"],
    ////template: 
    ////`<div>
    ////    <div>Hello there ... </div>
    ////    <div v-if="searchResults.length > 0">Hello</div>
    ////    <div v-else>World</div>
    //// </div>`,
    data: function ()  {
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
            defaultSearchMessageLabel: '',

            //these were originally assigned in the string-values.json file, access them from there if
            //we are capable of doing that later on in development - it will require webpack
            keywordsSearchBlockPublicLabels: {
                "SEARCH_RESULTS_LABEL": "Search Results",
                "PREVIOUS_BUTTON_LABEL": "Previous",
                "NEXT_BUTTON_LABEL": "Next",
                "TOTAL_RESULTS_LABEL": "Total Results:",
                "NO_RESULTS_LABEL": "Sorry, no results.",
                "ERROR_MESSAGE_1_LABEL": "A problem has occurred completing your search.",
                "ERROR_MESSAGE_2_LABEL": "Please contact ARC for further assistance.",
                "LOADING_LABEL": "Loading...",
                "DEFAULT_SEARCH_MESSAGE_LABEL": "Select from the options above to search."
            }
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

                    ////temp, delete after
                    //this.searchResults = [
                    //    {
                    //        title: ["My Test Title 1"],
                    //        id: 0,
                    //        images: ["https://eskipaper.com/images/cat-sad-annoyed-1.jpg", "https://www.thehappycatsite.com/wp-content/uploads/2017/03/Why-Do-Cats-Stick-Their-Tongue-Out-HC-long.jpg", "https://twistedsifter.com/wp-content/uploads/2013/08/lil-bub-the-cat-sticks-tongue-out-1.jpg?w=640"],
                    //        permalink: "google.ca",
                    //        contents: ["ALDJLKADJLAJDLKADJLKADJLKADJADKJASKDJASDKJASKDJADL", "asdsadfasfasfsasafasfasfasfasfasfasfasfasfasfasfsafafsafdfa", "dajkdhsjkadhjakshdjkadhkjahdjkashjkdahasd"]

                    //    },
                    //    {
                    //        title: ["My Test Title 2"],
                    //        id: 0,
                    //        images: ["https://eskipaper.com/images/cat-sad-annoyed-1.jpg", "https://www.thehappycatsite.com/wp-content/uploads/2017/03/Why-Do-Cats-Stick-Their-Tongue-Out-HC-long.jpg", "https://twistedsifter.com/wp-content/uploads/2013/08/lil-bub-the-cat-sticks-tongue-out-1.jpg?w=640"],
                    //        permalink: "google.ca",
                    //        contents: ["ALDJLKADJLAJDLKADJLKADJLKADJADKJASKDJASDKJASKDJADL", "asdsadfasfasfsasafasfasfasfasfasfasfasfasfasfasfsafafsafdfa", "dajkdhsjkadhjakshdjkadhkjahdjkashjkdahasd"]

                    //    },
                    //    {
                    //        title: ["My Test Title 3"],
                    //        id: 0,
                    //        images: ["https://eskipaper.com/images/cat-sad-annoyed-1.jpg", "https://www.thehappycatsite.com/wp-content/uploads/2017/03/Why-Do-Cats-Stick-Their-Tongue-Out-HC-long.jpg", "https://twistedsifter.com/wp-content/uploads/2013/08/lil-bub-the-cat-sticks-tongue-out-1.jpg?w=640"],
                    //        permalink: "google.ca",
                    //        contents: ["ALDJLKADJLAJDLKADJLKADJLKADJADKJASKDJASDKJASKDJADL", "asdsadfasfasfsasafasfasfasfasfasfasfasfasfasfasfsafafsafdfa", "dajkdhsjkadhjakshdjkadhkjahdjkashjkdahasd"]

                    //    },
                    //    {
                    //        title: ["My Test Title 4"],
                    //        id: 0,
                    //        images: ["https://eskipaper.com/images/cat-sad-annoyed-1.jpg", "https://www.thehappycatsite.com/wp-content/uploads/2017/03/Why-Do-Cats-Stick-Their-Tongue-Out-HC-long.jpg", "https://twistedsifter.com/wp-content/uploads/2013/08/lil-bub-the-cat-sticks-tongue-out-1.jpg?w=640"],
                    //        permalink: "google.ca",
                    //        contents: ["ALDJLKADJLAJDLKADJLKADJLKADJADKJASKDJASDKJASKDJADL", "asdsadfasfasfsasafasfasfasfasfasfasfasfasfasfasfsafafsafdfa", "dajkdhsjkadhjakshdjkadhkjahdjkashjkdahasd"]

                    //    }
                    //];

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
        this.searchResults = [];
        //assign static values
        this.searchResultsLabel = this.keywordsSearchBlockPublicLabels.SEARCH_RESULTS_LABEL;
        this.previousButtonLabel = this.keywordsSearchBlockPublicLabels.PREVIOUS_BUTTON_LABEL;
        this.nextButtonLabel = this.keywordsSearchBlockPublicLabels.NEXT_BUTTON_LABEL;
        this.totalResultsLabel = this.keywordsSearchBlockPublicLabels.TOTAL_RESULTS_LABEL;
        this.noResultsLabel = this.keywordsSearchBlockPublicLabels.NO_RESULTS_LABEL;
        this.errorMessage1Label = this.keywordsSearchBlockPublicLabels.ERROR_MESSAGE_1_LABEL;
        this.errorMessage2Label = this.keywordsSearchBlockPublicLabels.ERROR_MESSAGE_2_LABEL;
        this.loadingLabel = this.keywordsSearchBlockPublicLabels.LOADING_LABEL;
        this.defaultSearchMessageLabel = this.keywordsSearchBlockPublicLabels.DEFAULT_SEARCH_MESSAGE_LABEL;
    }
})