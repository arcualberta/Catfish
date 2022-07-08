//register global Vue component
Vue.component("controlled-categories", {
    props: ["uid", "toolbar", "model", "meta"],
    data() {
        return {
            //api strings
            selectedOptions: this.model.allowedCategories
                .filter(item => item.selected)
                .map(item => item.label)
        }
    },
    methods: {
        onBlur: function (e) {
            //TODO: find the list of selected checkboxes and add their values
            //to this.model.selectedKeywords array.

            if (e.target.checked) {
                this.selectedOptions.push(e.target.value);
            }
            else {
                var index = this.selectedOptions.indexOf(e.target.value);
                if (index >= 0) {
                    this.selectedOptions.splice(index, 1);
                }
            }

            this.model.selectedCategories.Value = this.selectedOptions.join(",");
        }
    },
    template:
            `<ul class='controlledCategories listStyleTypeNone ul-flex'>
            <li v-for='item in model.allowedCategories' :key = 'item.label' class='li-flexItem'>
                <input type='checkbox' :value='item.label'  v-on:blur='onBlur'  v-model="item.selected" /> {{ item.label }}
            </li>
        </ul >`
});

