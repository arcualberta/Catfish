/*
    Javascript for the ListDisplay block.
    See ListDisplayBlock.cshtml or SingleListItem.cs/ListDisplay.cs for more info.
 */

Vue.component('vue-list-display-block', {
    props: ["uid", "model"],

    data: function () {
        return {
            items: [],
            selectedItem: null,
            selectedItemIndex: null
        }
    },

    computed: {
    },

    methods: {
        selectItem(index) {
            this.selectedItem = this.model.Items[index];
            this.selectedItemIndex = index;
        }
    },

    created() {
        console.log("hi", this.model);

        this.items = this.model.Items;
    },

});