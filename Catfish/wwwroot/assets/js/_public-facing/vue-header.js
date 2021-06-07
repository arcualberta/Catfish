/* Public-facing header */

test = Vue.component('vue-header', {
    props: ["uid", "model"],
    data: function () {
        return {
            entries: []
        }
    },
    mounted() {
    },
    methods: {

    }
});

window.onload = function () {
    new test().$mount('#entire-header');
}