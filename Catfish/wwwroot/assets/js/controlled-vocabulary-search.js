/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("controlled-vocabulary-search", {

    props: ["uid", "toolbar", "model"],
    data() {
        var selectTemp = this.model.selectedKeywords.value === null ? [] : this.model.selectedKeywords.value.split(",");
        var availableKeywords = this.model.vocabularySettings.vocabulary.value === null ? [] : this.model.vocabularySettings.vocabulary.value.split(",");
        return {
            selectedKeywords: selectTemp,
            checkOptions: availableKeywords.map(word => ({ label: word, selected: selectTemp.includes(word) }))
        }
    },
    methods: {
        onBlur: function (e) {

            var elName = e.target.name;

            if (elName === "SearchPageName") {
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
            else if (elName == "keywordCheckBox") {
                if (e.target.checked) {
                    this.selectedKeywords.push(e.target.value);
                }
                else {
                    var index = this.selectedKeywords.indexOf(e.target.value);
                    if (index >= 0) {
                        this.selectedKeywords.splice(index, 1);
                    }
                }
                this.model.selectedKeywords.value = this.selectedKeywords.join(",");
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

    template: `<div  class= 'block-body controlled-vocabulary-search-block'> 
                  <div><h3> Controlled Vocabulary search: </h3></div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Search Page Name: </label>
                    <input type='text' class= 'form-control col-md-4'  name='searchPageName'
                        v-model='model.searchPageName.value' v-on:blur='onBlur'contenteditable='true'
                        value= 'searchPageNameValue' />
                  </div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Vocab Css Class: </label>
                    <input type='text' class= 'form-control col-md-4'   name='vocabCss'
                        v-model='model.vocabCss.value' v-on:blur='onBlur'contenteditable='true'
                        value= 'vocabCssValue' />
                  </div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Vocabulary List: </label>
                    <ul id='example- 1'>
                        <li v-for='item in this.checkOptions' :key = 'item.label'>
                            <input type='checkbox' name='keywordCheckBox' :value='item.label'  v-on:blur='onBlur'  v-model="item.selected" /> {{ item.label }}
                        </li>
                    </ul>
                  </div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Designated Solr Field: </label>
                    <input type='text' class= 'form-control col-md-4' name='designatedSolrField'
                        v-model='model.designatedSolrField.value' v-on:blur='onBlur'contenteditable='true'
                        value='designatedSolrFieldValue' />
                  </div>
               </div>`
});

