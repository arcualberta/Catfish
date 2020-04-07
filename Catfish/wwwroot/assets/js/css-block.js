/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("css-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            this.model.cssVal.value = e.target.value;

            var content = this.model.cssVal.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    cssVal: content
                });
            }

        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.cssVal.value);
        }

    },
    template: "<div  class= 'block-body'>" +

        " <textarea rows='4' cols='100' class='lead ' " +
        "    v-html='model.cssVal.value' contenteditable='true' v-on:blur='onBlur' >" +
        "</textarea></div>"

});