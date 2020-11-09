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
            pagesBeforeEllipsis: 5,
            searchMade: false,
            loadingSearchResults: false,
            //for when there are a lot of pages, show '...' instead for some in the pagination
            usePaginationEllipsis: false,
            //for when there's ellipsis, need to track the position
            pageAdvancement: 0,
		}
    },
    methods: {

        clickpreviousButton() { },

        clickNextButton() { },

        /**
         * 
         **/
        clickPageNumber(index) {
            this.currentPage = index;
            if (!this.usePaginationEllipsis) {
                return;
            } else {
                if (index > this.pagesBeforeEllipsis - 2) {
                    this.pageAdvancement++;


                } else {
                    this.pageAdvancement--;
				}
			}
        },

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
                    let tmp = JSON.parse(JSON.stringify(data));
                    this.searchResults = this.searchResults.concat(tmp); //tmp
                    this.pagesTotal = Math.ceil(this.searchResults.length / 3);
                    //check to add ellipsis to pagination if pages > 5
                    if (this.pagesTotal > this.pagesBeforeEllipsis) {
                        this.usePaginationEllipsis = true;
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