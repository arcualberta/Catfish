/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("contact-block", {
    props: ["uid", "toolbar", "model"],

    methods: {
        onBlur: function (e) {
            var elName = e.target.name;
            if (elName == "sendTo") {
                this.model.sendTo.value = e.target.value;
                var content = this.model.sendTo.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        sendTo: content
                    });
                }
            }
            else if (elName == "subject") {
                this.model.subject.value = e.target.value;
                var content = this.model.subject.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        subject: content
                    });
                }
            }
            else if (elName == "successMessage") {
                this.model.successMessage.value = e.target.value;
                var content = this.model.successMessage.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        successMessage: content
                    });
                }
            }
        }
    },
    computed: {

        sendToValue: {
            get: function () {
                return this.model.sendTo.value;
            }
        },
        subjectValue: {
            get: function () {
                return this.model.subject.value;
            }
        },
        successMessageValue: {
            get: function () {
                return this.model.successMessage.value;
            }
        }
    },
    template: "<div  class= 'block-body contact-form-block'>" +
        "<div class='lead row'><label class='form-label col-md-3'>Send To:</label> <input type='email' class='form-control col-md-4' name='sendTo' v-model='model.sendTo.value' contenteditable='true' v-on:blur='onBlur' value='sendToValue'  /></div>" +
        "<div class='lead row'><label class='form-label col-md-3'>Subject:</label> <input type='text' class='form-control col-md-4' name='subject' v-model='model.subject.value' contenteditable='true' v-on:blur='onBlur' value='subjectValue'  /></div>" +
        "<div class='lead row'><label class='form-label col-md-3'>Success Message:</label> <input type='text' class='form-control col-md-4' name='successMessage' v-model='model.successMessage.value' contenteditable='true' v-on:blur='onBlur' value='successMessageValue'  /></div>" +

        "</div>"
}); 