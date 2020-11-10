var vueApp = new Vue({
    el: '#keywords-search-block-public',
    data() {
        return {
            apiSearchUrl: "/api/search/keywords",
            searchTerms: [],
            categories: '',
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
            formData.append("searchTerms", this.searchTerms);
            formData.append("category", this.categories);
            console.log("?", formData);

            for (var p of formData) {
                console.log("p", p);
			}

            this.loadingSearchResults = true;
            fetch(this.apiSearchUrl, {
                method: 'POST',
                /*headers: {
                    'Content-Type': 'application/json',
                },*/
                body: formData/*{
                    searchTerms: this.searchTerms,
                    category: this.categories
                },*/
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

                    this.searchMade = true;
                    this.loadingSearchResults = false;
                    this.currentPage = 1;
                });
		}
	},
    created() {
    }
});