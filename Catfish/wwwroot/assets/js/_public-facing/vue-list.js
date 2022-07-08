Vue.component('vue-list', {
    props: ["uid", "model"],

    data: function () {
        return {
            entries: this.model.Items.Value
                .split(",")
                .filter(val => val.trim().length > 0)
        }
    },
    template: `
        <ul>
            <li v-for="item in entries">{{item}}</li>
        </ul>`
})
