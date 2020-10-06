/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("free-search", {
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
    template: "<div  class= 'block-body' style='border: solid 1px #888'>Free-text search.</div>"

});

