/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("navigation-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            this.model.pageTitle.value = e.target.value;

            var content = this.model.pageTitle.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    pageTitle: content
                });
            }

        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.pageTitle.value);
        },
        pageTitleValue: {
            get: function () {
                return this.model.pageTitle.value;
            }
        }

    },
    template: 
        "<div class='lead row'><label class='form-label col-md-3'>Starting Page Title:</label>" +
        "<input type='text' class='form-control col-md-4' name='pageTitle' v-model='model.pageTitle.value' contenteditable='true' v-on:blur='onBlur' value='pageTitleValue' /></div> "
});

