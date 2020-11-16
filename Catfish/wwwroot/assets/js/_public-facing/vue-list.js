Vue.component('vue-list', {
    props: ["uid", "model"],

    data: function () {
        return {
            count: 0
        }
    },
    template: `
        <ul>
            <li v-for="item in model">{{item}}</li>
        </ul>`
})
