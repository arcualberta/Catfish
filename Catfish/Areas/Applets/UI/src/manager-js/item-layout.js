Vue.component("item-layout", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            mainForm: [],
            allForms:[],
            validationError: '',
            groups: [],
            itemFields: [],
            selectedItemFields: [],
          
            fieldGroups:[]
        }
    },
     methods: {
         
      selectItemTemplate: function (selected) {

            
             fetch('/applets/api/ItemTemplates/GetAllItemTemplateForms/' + selected)
                 .then(response => response.json())
                 .then((data) => {
                     this.allForms = data;

                 });
          
            fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplateId.value)
                .then(response => response.json())
                .then((data) => {
                    this.groups = data;
                });
             //fetch('/api/Items/GetCollectionList')
             //    .then(response => response.json())
             //    .then((data) => {
             //       // this.collections = data;
             //        this.model.collections = data;
                   
             //    });
         },

         selectItemField: function (fieldVal) {
           
             var field = this.itemFields.find(f => {
                 
                 return f.fieldId == fieldVal
             });
           


             this.selectedItemFields.push(field);
             
             //Update this.model.selectedFields
             this.model.selectedFieldList.value = JSON.stringify(this.selectedItemFields);

             //organize array for display
            
          
             this.fieldGroups = this.selectedItemFields.reduce(function (r, a) {
                 r[a.formName] = r[a.formName] || [];
                 r[a.formName].push(a);
                
                 return r;
             }, Object.create(null));
             
            
         },

         removeSelectedField: function (fvalue) {
             //console.log("field val: " + fvalue);
             var filtered = this.selectedItemFields.filter(function (field, index, arr) {
                
                 return field.fieldId !== fvalue;
             });
            
             //update the selectedField array
             this.selectedItemFields = filtered;
            
             //Update this.model.selectedFields
             //this.model.selectedFields = this.selectedItemFields;
             this.model.selectedFieldList.value = JSON.stringify(this.selectedItemFields);
             
             this.fieldGroups = this.selectedItemFields.reduce(function (r, a) {
                 r[a.formName] = r[a.formName] || [];
                 r[a.formName].push(a);
                
                 return r;
             }, Object.create(null));
          
         },
         getFormFields: function (itemTemplateId, formId) {
           
             fetch('/applets/api/itemtemplates/getItemtemplateFields/' + itemTemplateId + "/" + formId)
                 .then(response => response.json())
                 .then((data) => {
                     this.itemFields = data;
                   
                 });
         },

        

    },
      mounted() {
        //getCollectionList
      //fetch('/api/Items/GetCollectionList')
      //      .then(response => response.json())
      //      .then((data) => {
      //          this.collections = data;
               
      //      });


      if (this.model.selectedItemTemplateId?.value) {
          //applets/api/[controller]
          //fetch('/applets/api/ItemTemplates/GetItemTemplateRootForms/' + this.model.selectedItemTemplateId.value)
          //.then(response => response.json())
          //.then((data) => {
          //  this.childForms = data;

          //});

          fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplateId.value)
              .then(response => response.json())
              .then((data) => {
                  this.groups = data;
              });

          fetch('/applets/api/ItemTemplates/GetAllItemTemplateForms/' + this.model.selectedItemTemplateId.value)
              .then(response => response.json())
              .then((data) => {
                  this.allForms = data;

              });

          }
         
          if (this.model.selectedFieldList?.value) {
              this.selectedItemFields = JSON.parse(this.model.selectedFieldList.value);
              console.log("onmounted selectedItemFields " + JSON.stringify(this.selectedItemFields))

              this.fieldGroups = this.selectedItemFields.reduce(function (r, a) {
                  r[a.formName] = r[a.formName] || [];
                  r[a.formName].push(a);

                  return r;
              }, Object.create(null));
          }

    },
   
    template:
        `<div  class= 'block-body'>
            <h2>Item Layout</h2>
            <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
               <select v-model="model.selectedItemTemplateId.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplateId.value)">
                    <option disabled value="">Please select one</option>
                    <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
                </select>
            </div>
            
         <div  class='lead row'><label class='form-label col-md-3 required'>Group: </label>
           <select v-model="model.selectedGroupId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.groups" :value="item.value">{{item.text}}</option>
            </select></div>
          <div  class='lead row'><label class='form-label col-md-3 required'>Forms: </label>
           <select v-model="model.selectedFormId.value" class="form-control" style="width:auto;" v-on:change="getFormFields(model.selectedItemTemplateId.value,model.selectedFormId.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.allForms" :value="item.value">{{item.text}}</option>
            </select></div>
       
           <div  class='lead row'><label class='form-label col-md-3 required'>Select Field: </label>
           <select v-model="model.selectedField.value" class="form-control" style="width:auto;" v-on:change="selectItemField(model.selectedField.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.fieldId">{{item.fieldName}}</option>
            </select></div>

           <div  class='lead row'><label class='form-label col-md-3 required'>Selected Fields {{this.model.selectedFields?.length}}</label>
              <div class="col-md-9 fieldList" style="margin-left: -15px">
                  <div class="formGroup" v-for="(value, name, index) in this.fieldGroups">
                        <div class="formName">{{name}}</div>
             
                        <div   style="width:auto;display:flex" >
                            <div class="field" v-for="f in value" :key="f.fieldId">{{f.fieldName}} <span class="fas fa-times-circle" v-on:click="removeSelectedField(f.fieldId)"> </span></div>
                        </div>
                    </div>
               </div>
           </div>

        </div>`
        

});


