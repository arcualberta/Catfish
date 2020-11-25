/**
 This is the public-facing carousel component.
 It is a slideshow-style set of images with text and a link (all optionally provided based on the manager side's input).
 */

Vue.component('vue-list', {
    props: ["model"],

    data: function () {
        return {
        }
    },
    template: `
        <ul>
            <li v-for="item in model">{{item}}</li>
        </ul>`
})