/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("embed-block", {
    props: ["uid", "toolbar", "model"],
    
    methods: {
        onBlur: function (e) {
            this.model.embed.value = e.target.value;

            var content = this.model.embed.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    embed: content
                });
            }
           
        }
    },
    computed: {
        isEmpty: function () {
            return piranha.utils.isEmptyText(this.model.embed.value);
        }

    },
    template: "<div  class= 'block-body'>" +
      
        " <textarea rows='4' cols='120' class='lead' " +
        "    v-html='model.embed.value' contenteditable='true' v-on:blur='onBlur' >Paste the iframe code here" +
        "</textarea></div>"

});