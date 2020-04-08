/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("javascript-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            this.model.javascriptCode.value = e.target.value;

            var content = this.model.javascriptCode.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    javascriptCode: content
                });
            }

        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.javascriptCode.value);
        }

    },
    template: "<div  class= 'block-body'>" +

        " <textarea rows='4' cols='100' class='lead paragraph' " +
        "    v-html='model.javascriptCode.value' contenteditable='true' v-on:blur='onBlur' >" +
        "</textarea></div>"

});