var vueApp = new Vue({
    el: '#keywords-search-block-public',
    data() {
        return {
            apiSearchUrl: "/api/search/keywords",
            searchTerms: [],
            categories: [],
            searchResults: [],
            pagesTotal: 0,
            currentPage: 1,
            cardsPerPage: 3,
            searchMade: false,
            loadingSearchResults: false,
		}
    },
    methods: {

        /**
         * Stores/removes clicked filter button to call API again
         * @param {any} event clickevent
         */
        filterByCategory(event) {
            if (!this.categories.includes(event.target.value)) {
                this.categories.push(event.target.value);
            } else {
                this.categories.splice(this.categories.indexOf(event.target.value), 1);
            }
            $('#' + event.target.id).button('toggle')

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
            this.loadingSearchResults = true;
            fetch(this.apiSearchUrl, {
                method: 'POST',
                /*headers: {
                    'Content-Type': 'application/json',
                },*/
                body: {
                    searchTerms: this.searchTerms,
                    category: this.categories
                },
            })
                .then(response => response.json())
                .then(data => {
                    this.searchResults = data;
                    console.log(this.searchResults);
                    //tmp
                    let count = 0;
                    for (let item of this.searchResults) {
                        item.id = count + Math.random();
                        count++;
					}
                    this.pagesTotal = Math.ceil(this.searchResults.length / 3);

                    //tmp
                    for (let tmp of this.searchResults) {
                        console.log(tmp.id);
					}

                    this.searchMade = true;
                    this.loadingSearchResults = false;
                    this.currentPage = 1;
                });
		}
	},
    created() {
    }
});