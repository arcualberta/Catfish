/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("css-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            this.model.css.value = e.target.innerHTML;

            var content = this.model.embed.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    css: content
                });
            }

        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.css.value);
        }

    },
    template: "<div  class= 'block-body'>" +

        " <textarea rows='4' cols='100' class='lead ' " +
        "    v-html='model.css.value' contenteditable='true' v-on:blur='onBlur' >" +
        "</textarea></div>"

});