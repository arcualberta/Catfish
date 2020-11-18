/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("submission-form", {
    props: ["uid", "toolbar", "model"],
   
    methods: {
        onBlur: function (e) {
            this.model.entityTemplateId.value = e.target.value;

            var content = this.model.entityTemplateId.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    entityTemplateId: content
                });
            }
        },

        getDDLItems: function () {
            return this.model.entities.collectionNames;
        }
    },
    
    //computed: {

    //    isEmpty: function () {
    //        return piranha.utils.isEmptyText(this.model.entityTemplateId.value);

    //    }

    //},
    //mounted() {
    //    setTimeout(() => this.ddlist = this.model.entities.collectionNames, 3000)
    //},
    template: `<div  class= 'block-body calendar-block'>
        <div class='lead row'><label class='form-label col-md-3 required'>Collection: </label>
           <select v-model="model.selectedCollection.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.collections.entries" :value="item.value">{{item.text}}</option>
         </select></div>

      <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
           <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
            </select></div> 
      <div class='lead row'><label class='form-label col-md-3 required'>CSS Class: </label><input class='form-control col-md-8' type='text' name='cssClass' v-model='model.cssClass.value' contenteditable='true' v-on:blur='onBlur' value='cssClassValue' /></div>
      <div class='lead row'><label class='form-label col-md-3 required'>Submission Confirmation: </label><input class='form-control col-md-8' type='text' name='submissionConfirmation' v-model='model.submissionConfirmation.value' contenteditable='true' v-on:blur='onBlur' value='submissionConfirmationValue' /></div>
<div class='lead row'><label class='form-label col-md-3 required'>Function: </label><input class='form-control col-md-8' type='text' name='Function' v-model='model.Function.value' contenteditable='true' v-on:blur='onBlur' value='Function' /></div>
<div class='lead row'><label class='form-label col-md-3 required'>Group: </label><input class='form-control col-md-8' type='text' name='Function' v-model='model.Function.value' contenteditable='true' v-on:blur='onBlur' value='Function' /></div>

</div>` 

});

