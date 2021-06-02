/* global piranha component registration */
/* 1st parameter is the component name */
Vue.component("advance-search", {
    props: ["uid", "toolbar", "model"],
    data: function () {
        //selectedFunctionTemplate = this.model.workflowFunction.value == null ? "" : this.model.workflowFunction.value + "|" + this.model.workflowGroup.value;
         
        return {
           // functionOptions: [],
           // selectedFunctionTemplate,
            selectedTemplateId: '',
            renderingTypes: ['Tabular','Slip'],
            metadatasetOptions: [],
            metadataFields:[]
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
                    this.selectedTemplateId = selected;
                });
        },

        selectMetadatasetFunction: function (selected) {
           
            fetch('/api/Items/GetMetadatasetFields/' + this.selectedTemplateId + '/' + selected)
                .then(response => response.json())
                .then((data) => {
                   
                    this.metadatasetFields = data;
                });
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

        if (this.model.selectedMetadataset.value != null) {
            fetch('/api/Items/GetMetadatasetFields/' + this.model.selectedItemTemplate.value + '/' + this.model.selectedMetadataset.value)
                .then(response => response.json())
                .then((data) => {
                 
                    this.metadatasetFields = data;
                });
        }
    },
    template: `<div  class= 'block-body advance-search-block'>

      <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
           <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplate.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
            </select></div> 

     
        <div class='lead row'><label class='form-label col-md-3 required'>Metadataset: </label>
           <select v-model="model.selectedMetadataset.value" class="form-control" style="width:auto;" v-on:change="selectMetadatasetFunction(model.selectedMetadataset.value)">
                <option  value="" selected>Please select one</option>
                <option v-for="item in this.metadatasetOptions" :value="item.value">{{item.text}}</option>
                 
         </select></div>

        <div class='lead row'><label class='form-label col-md-3 required'>Result Rendering Type: </label>
           <select v-model="model.selectedRenderingType.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.renderingTypes" :value="item">{{item}}</option>
                 
         </select></div>
         <br/>
         <div class='alert alert-info'>Settings below are only needed when the RenderingType is <b>"Slip"</b> </div>
          <br/>
          <div class='lead row'><label class='form-label col-md-3 required'>Slip Title: </label>
           <select v-model="model.selectedTitleFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>

        <div class='lead row'><label class='form-label col-md-3 required'>Slip Sub Title - 1: </label>
           <select v-model="model.selectedFirstSubTitleFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>
 <div class='lead row'><label class='form-label col-md-3 required'>Slip Sub Title - 2: </label>
           <select v-model="model.selectedSecondSubTitleFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>

    <div class='lead row'><label class='form-label col-md-3 required'>Slip Body: </label>
           <select v-model="model.selectedBodyFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>

    <div class='lead row'><label class='form-label col-md-3 required'>Slip Footer: </label>
           <select v-model="model.selectedFooterFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>
    
    <div class='lead row'><label class='form-label col-md-3 required'>Slip Title Link: </label>
           <select v-model="model.selectedUrlFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>
<div class='lead row'><label class='form-label col-md-3 required'>slip Body Link: </label>
           <select v-model="model.selectedLinkFieldId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option value="Link to Item Details Page">Link to Item Details Page</option>
                <option v-for="item in this.metadatasetFields" :value="item.value">{{item.text}}</option>

         </select></div>
     
     
</div>` 

});

