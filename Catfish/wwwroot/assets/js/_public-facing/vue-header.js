/* Public-facing header */

test = Vue.component('vue-header', {
    props: ["uid", "model"],
    data: function () {
        return {
            entries: []
        }
    },
    mounted() {
        this.entries = this.model.Items.Value
            .split(",")
            .filter(val => val.trim().length > 0);
    },
    template: `
        <div>
            <div>I am a test where do I show up</div>
            <ul>
                <li v-for="item in entries">{{item}}</li>
            </ul>
        </div>
        `
});//.$mount();

new test().$mount('#site-header-root');
//Vue.$mount('#site-header');