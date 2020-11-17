/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("vue-list", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            this.model.items.value = e.target.value;

            //var content = this.model.items.value;
            //if (content.length > 0) {
            //    this.$emit('update-content', {
            //        uid: this.uid,
            //        items: content
            //    });
            //}

        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.items.value);
        }

    },
    template: `
        <div >
            <textarea rows='4' cols='100' class='lead '
                v-html='model.items.value' contenteditable='true' v-on:blur='onBlur' >
            </textarea>
        </div>`

});