/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("advance-search", {
    props: ["uid", "toolbar", "model"],
    data: function () {
        //selectedFunctionTemplate = this.model.workflowFunction.value == null ? "" : this.model.workflowFunction.value + "|" + this.model.workflowGroup.value;
         
        return {
           // functionOptions: [],
           // selectedFunctionTemplate,
            metadatasetOptions: []
        }
    },
   
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

        //getDDLItems: function () {
        //    return this.model.entities.collectionNames;
        //},

        selectItemTemplate: function (selected) {
          
            fetch('/api/Items/GetSelectListItem/' + selected + '/true')
                .then(response => response.json())
                .then((data) => {
                  //  this.functionOptions = data;
                    this.metadatasetOptions = data;
                });
        },

        selectMetadatasetFunction: function (selected) {
            
            this.selectedMetadataset = selected;
        }

    },
    
  /*  computed: {

        isSelected: function () {
            return selectText == this.model.workflowFunction.value ? true : false;

        }

    },*/
    mounted() {
        fetch('/api/Items/GetSelectListItem/' + this.model.selectedItemTemplate.value + '/true')
            .then(response => response.json())
            .then((data) => {
                this.functionOptions = data;
                this.metadatasetOptions = data;
            });
    },
    template: `<div  class= 'block-body advance-search-block'>

      <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
           <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplate.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
            </select></div> 

     
        <div class='lead row'><label class='form-label col-md-3 required'>Metadataset: </label>
           <select v-model="model.selectedMetadataset.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetOptions" :value="item.value">{{item.text}}</option>
                 
         </select></div>

     
     
</div>` 

});

