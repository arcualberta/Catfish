/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("submission-form", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            ////this.model.EntityTemplateId.value = e.target.value;

            ////var content = this.model.cssVal.value;
            ////if (content.length > 0) {
            ////    this.$emit('update-content', {
            ////        uid: this.uid,
            ////        cssVal: content
            ////    });
            ////}

        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.entityTemplateId.value);
        }

    },
    template: "<div  class= 'block-body calendar-block'>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Template ID: </label><input class='form-control col-md-8' type='text' name='entityTemplateId' v-model='model.entityTemplateId.value' contenteditable='true' v-on:blur='onBlur' value='entityTemplateIdValue'  :class='{ requiredField: isEmpty }' /></div>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Collection ID: </label><input class='form-control col-md-8' type='text' name='collectionId' v-model='model.collectionId.value' contenteditable='true' v-on:blur='onBlur' value='collectionIdValue' /></div>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>CSS Class: </label><input class='form-control col-md-8' type='text' name='cssClass' v-model='model.cssClass.value' contenteditable='true' v-on:blur='onBlur' value='cssClassValue' /></div>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Submission Confirmation: </label><input class='form-control col-md-8' type='text' name='submissionConfirmation' v-model='model.submissionConfirmation.value' contenteditable='true' v-on:blur='onBlur' value='submissionConfirmationValue' /></div>" +
        "</div>"

    //template: "<div  class= 'block-body'>" +

    //    " <textarea rows='4' cols='100' class='lead ' " +
    //    "    v-html='model.EntityTemplateId.value' contenteditable='true' v-on:blur='onBlur' >" +
    //    "</textarea></div>"

});


