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
      <div class='lead row'><label class='form-label col-md-3 required'>Workflow Function: </label><input class='form-control col-md-8' type='text' name='workflowFunction' v-model='model.workflowFunction.value' contenteditable='true' v-on:blur='onBlur' value='workflowFunction' /></div>
      <div class='lead row'><label class='form-label col-md-3 required'>Workflow Group: </label><input class='form-control col-md-8' type='text' name='workflowGroup' v-model='model.workflowGroup.value' contenteditable='true' v-on:blur='onBlur' value='workflowGroup' /></div>
      <div class='lead row'><label class='form-label col-md-3 required'>Link to Group:</label><label class='form-label required'><input class='' type='checkbox' v-model='model.linkToGroup.value' contenteditable='true' value='model.linkToGroup.value' /></label></div>
      <div v-if='model.linkToGroup.value' class='lead row'><label class='form-label col-md-3 required'>Group Selector Label: </label><input class='form-control col-md-8' type='text' v-model='model.groupSelectorLabel.value' contenteditable='true' value='model.groupSelectorLabel.value' /></div>

</div>` 

});

