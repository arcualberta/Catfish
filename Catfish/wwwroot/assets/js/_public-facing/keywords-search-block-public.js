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
            searchMade: false
		}
    },
    methods: {
        filterByCategory(event) {
            if (!this.categories.includes(event.target.value)) {
                this.categories.push(event.target.value);
            } else {
                this.categories.splice(this.categories.indexOf(event.target.value), 1);
            }
            $('#' + event.target.id).button('toggle')

            this.makePostCall();
            
        },

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
                    this.pagesTotal = Math.ceil(this.searchResults.length / 3);
                    this.searchMade = true;
                });
		}
	},
    created() {
    }
});