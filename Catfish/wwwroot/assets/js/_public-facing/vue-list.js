Vue.component('vue-list', {
    props: ["uid", "model"],

    data: function () {
        return {
        }
    },
    template: `
        <ul>
            <li v-for="item in model">{{item}}</li>
        </ul>`
})
