/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("submission-entry-point", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            this.model.EntityTemplateId.value = e.target.value;

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
            return piranha.utils.isEmptyText(this.model.EntityTemplateId.value);
        }

    },
    template: "<div  class= 'block-body calendar-block'>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Entity Template ID: </label><input class='form-control col-md-8' type='text' name='entityTemplateId' v-model='model.entityTemplateId.value' contenteditable='true' v-on:blur='onBlur' value='entityTemplateIdValue'  :class='{ requiredField: isEntityTemplateEmpty }' /></div>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Collection ID: </label><input class='form-control col-md-8' type='text' name='collectionId' v-model='model.collectionId.value' contenteditable='true' v-on:blur='onBlur' value='collectionIdValue'  :class='{ requiredField: isCollectionEmpty }' /></div>" +
        "</div>"

    //template: "<div  class= 'block-body'>" +

    //    " <textarea rows='4' cols='100' class='lead ' " +
    //    "    v-html='model.EntityTemplateId.value' contenteditable='true' v-on:blur='onBlur' >" +
    //    "</textarea></div>"

});


