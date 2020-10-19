/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("controlled-vocabulary-search", {

    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {

            var elName = e.target.name;

            if (elName == "SearchPageName") {
                this.model.SearchPageName.value = e.target.value;
                var content = this.model.SearchPageName.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        SearchPageName: content
                    });
                }
            }
            else if (elName == "VocabCss") {
                this.model.VocabCss.value = e.target.value;
                var content = this.model.VocabCss.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        VocabCss: content
                    });
                }
            }
            else if (elName == "VocabList") {
                this.model.VocabList.value = e.target.value;
                var content = this.model.VocabList.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        VocabList: content
                    });
                }
            }
            else if (elName == "DesignatedSolrField") {
                this.model.DesignatedSolrField.value = e.target.value;
                var content = this.model.DesignatedSolrField.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        DesignatedSolrField: content
                    });
                }
            }
        }
    },



    computed: {
        SearchPageNameValue: {
            get: function () {
                return this.model.SearchPageName.value;
            }
        },

        VocabCssValue: {
            get: function () {
                return this.model.VocabCss.value;
            }
        },

        VocabListValue: {
            get: function () {
                return this.model.VocabList.value;
            }
        },

        DesignatedSolrField: {
            get: function () {
                return this.model.DesignatedSolrField.value;
            }
        }
    },

    template: "<div  class= 'block-body controlled-vocabulary-search-block'>" +

        "<div>" +
        "<h3> Controlled Vocabulary search: </h3>" +
        "</div>" +

        "<div  class='lead row'> <label class = 'form-label col-md-3'> Search Page Name: </label> "+
        "<input type='text' class= 'form-control col-md-4'  name='searchPageName'"+
        " v-model='model.searchPageName.value' v-on:blur='onBlur'contenteditable='true' "+
        "value= 'searchPageNameValue' /></div > " +
 

        "<div   class='lead row'> <label class = 'form-label col-md-3'> Vocab Css Class: </label>  " +
        "<input type='text' class= 'form-control col-md-4'   name='vocabCss'  " +
        " v-model='model.vocabCss.value' v-on:blur='onBlur'contenteditable='true' " +
        "value= 'vocabCssValue' /></div > "+

        "<div    class='lead row'> <label class = 'form-label col-md-3'> Vocabulary List: </label>  " +
        "<textarea name='vocabList'  v-model='model.vocabList.value' v-on:blur='onBlur'contenteditable='true'  " +
        "value= 'vocabListValue' rows='5' cols = '35' > List words seperated by commas and/ or newlines.</textarea > " +
        "</div> " +

        "<div class='lead row'> <label class = 'form-label col-md-3'> Designated Solr Field: </label>" +
        "<input type='text' class= 'form-control col-md-4' name='designatedSolrField' " +
        " v-model='model.designatedSolrField.value' v-on:blur='onBlur'contenteditable='true' " +
        "value='designatedSolrFieldValue' /></div> " +

        "</div>" 


});

