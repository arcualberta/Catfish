/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("controlled-vocabulary-search", {

    props: ["uid", "toolbar", "model"],
    data() {
        var selectTemp = this.model.selectedKeywords.value === null
            ? []
            : this.model.selectedKeywords.value
                .split(",")
                .map(x => x.trim())
                .filter(x => x.length > 0);

        var availableKeywords = this.model.vocabularySettings.vocabulary.value === null
            ? []
            : this.model.vocabularySettings.vocabulary.value
                .split(",")
                .map(x => x.trim())
                .filter(x => x.length > 0);
        var selectCatTemp = this.model.selectedCategories.value === null
            ? []
            : this.model.selectedCategories.value
                .split(",")
                .map(x => x.trim())
                .filter(x => x.length > 0);
        var availableCategories = this.model.categorySettings.vocabulary.value === null
            ? []
            : this.model.categorySettings.vocabulary.value
                .split(",")
                .map(x => x.trim())
                .filter(x => x.length > 0);
        return {
            selectedKeywords: selectTemp,
            checkOptions: availableKeywords.map(word => ({ label: word, selected: selectTemp.includes(word) })),
            selectedCategories: selectCatTemp,
            checkCatOptions: availableCategories.map(word => ({ label: word, selected: selectCatTemp.includes(word) }))
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
            else if (elName == "categoryCheckBox") {
                if (e.target.checked) {
                    this.selectedCategories.push(e.target.value);
                }
                else {
                    var index = this.selectedCategories.indexOf(e.target.value);
                    if (index >= 0) {
                        this.selectedCategories.splice(index, 1);
                    }
                }
                this.model.selectedCategories.value = this.selectedCategories.join(",");
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
        }
    },

    template: `<div  class= 'controlled-vocabulary-search-block'> 
                  <div><h3> Controlled Vocabulary Search Block </h3></div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Block Title: </label>
                    <input type='text' class= 'form-control col-md-4'  name='searchPageName'
                        v-model='model.searchPageName.value' v-on:blur='onBlur'contenteditable='true'
                        value= '' />
                  </div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'>Block CSS Class(es): </label>
                    <input type='text' class= 'form-control col-md-4'   name='blockCss'
                        v-model='model.blockCss.value' v-on:blur='onBlur'contenteditable='true'
                        value= '' />
                  </div>
                  <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Option CSS Class(es): </label>
                    <input type='text' class= 'form-control col-md-4'   name='optionCss'
                        v-model='model.optionCss.value' v-on:blur='onBlur'contenteditable='true'
                        value= '' />
                  </div>
                  <div class='lead row' > 
                    <label class = 'form-label col-md-3'> Selected Vocabulary: </label>
                    <div class='row col-md-9' id='vocabulary-check-block' >
                       
                        <div v-for='item in this.checkOptions' :key = 'item.label' class='col-md-3' >
                            <input type='checkbox' name='keywordCheckBox' :value='item.label'  v-on:blur='onBlur'  v-model="item.selected" /> {{ item.label }}
                        </div>
                    </div>
                  </div>

                 <div class='lead row'> 
                    <label class = 'form-label col-md-3'> Selected Category: </label>
                    <div class='row col-md-9' id='category-check-block'>
                       
                        <div v-for='item in this.checkCatOptions' :key = 'item.label' class='col-md-3' >
                            <input type='checkbox' name='categoryCheckBox' :value='item.label'  v-on:blur='onBlur'  v-model="item.selected" /> {{ item.label }}
                        </div>
                    </div>
                  </div>
               </div>`
});
