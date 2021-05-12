/*
    Javascript for the ListDisplay block.
    See ListDisplayBlock.cshtml or SingleListItem.cs/ListDisplay.cs for more info.
 */

const weekday = require("dayjs/plugin/weekday");
const weekOfYear = require("dayjs/plugin/weekOfYear");

Vue.component('vue-list-display-block', {
    props: ["uid", "model"],

    data: function () {
        return {
            
        }
    },

    computed: {
    },

    methods: {
    },

    created() {
        console.log("hi");
    },

});