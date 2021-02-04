/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("item-list", {
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
    //computed: {
    //    isEmpty: function () {
    //        return piranha.utils.isEmptyText(this.model.EntityTemplateId.value);
    //    }

    //},
    //template:` <div class='block-body calendar-block'>
    //    <div class='lead row'><label class='form-label col-md-3 required'>Collection: </label>
    //        <select v-model="model.selectedCollection.value" class="form-control" style="width:auto;">
    //            <option disabled value="">Please select one</option>
    //            <option v-for="item in model.collections.entries" : {{ item.text }}</option>
    //     </select></div>

    //    <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
    //        <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;">
    //            <option disabled value="">Please select one</option>
    //            <option v-for="item in model.itemTemplates.entries" : {{ item.text }}</option>
    //        </select></div> `

    //    +
    //    "<div class='lead row'><label class='form-label col-md-3 required'>Template ID: </label><input class='form-control col-md-8' type='text' name='entityTemplateId' v-model='model.entityTemplateId.value' contenteditable='true' v-on:blur='onBlur' value='entityTemplateIdValue'   /></div>" +
    //    "<div class='lead row'><label class='form-label col-md-3 required'>Collection ID: </label><input class='form-control col-md-8' type='text' name='collectionId' v-model='model.collectionId.value' contenteditable='true' v-on:blur='onBlur' value='collectionIdValue'   /></div>" +
    //    "</div>"

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
            </select>
      </div> 
      <div class='lead row'><label class='form-label col-md-3 required'>Authorization Failure Message: </label><input class='form-control col-md-8' type='text' name='AuthorizationFailureMessage' v-model='model.authorizationFailureMessage.value' contenteditable='true' v-on:blur='onBlur' value='authorizationFailureMessage' /></div>
</div>` 


});
