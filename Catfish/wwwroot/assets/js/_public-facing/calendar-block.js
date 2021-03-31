/*
    Javascript for the Calendar block.
    Note that this is javascript solely for the visual calendar UI element,
    the javascript does not interact with the events listed.
    See CalendarBlock.cshtml or CalendarBlock.cs for more info.
 */

import dayjs from "dayjs";
const weekday = require("dayjs/plugin/weekday");
const weekOfYear = require("dayjs/plugin/weekOfYear");

Vue.component('calendar-block', {
    props: ["uid", "model"],
    ////template: 
    ////`<div>
    ////    <div>Hello there ... </div>
    ////    <div v-if="searchResults.length > 0">Hello</div>
    ////    <div v-else>World</div>
    //// </div>`,
    data: function () {
        return {}
    },

    methods: {

    },

    created () {
        console.log("this code is running ok");
        dayjs.extend(weekday);
        dayjs.extend(weekOfYear);
    }
});