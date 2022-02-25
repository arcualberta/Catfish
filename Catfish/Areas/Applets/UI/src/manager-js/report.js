Vue.component("report", {
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
         // Accepts the array and key
         //groupBy: function (array, key) {
         //    // Return the end result
         //    return array.reduce((result, currentValue) => {
         //        // If an array already present for key, push it to the array. Else create an array and push the object
         //        (result[currentValue[key]] = result[currentValue[key]] || []).push(
         //            currentValue
         //        );
         //        // Return the current iteration `result` value, this will be taken as next iteration `result` value and accumulate
         //        return result;
         //    }, {}) // empty object is the initial value for result object
         //},
      selectItemTemplate: function (selected) {

            //fetch('/applets/api/ItemTemplates/GetItemTemplateRootForms/' + selected)
            //    .then(response => response.json())
            //    .then((data) => {
            //        this.mainForm = data;

            //    });

             fetch('/applets/api/ItemTemplates/GetAllItemTemplateForms/' + selected)
                 .then(response => response.json())
                 .then((data) => {
                     this.allForms = data;

                 });
             //fetch('/api/Items/GetItemtemplateFields/' + selected)
             //    .then(response => response.json())
             //    .then((data) => {
             //        this.itemFields = data;

             //    });
            fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplateId.value)
                .then(response => response.json())
                .then((data) => {
                    this.groups = data;
                });
             fetch('/api/Items/GetCollectionList')
                 .then(response => response.json())
                 .then((data) => {
                    // this.collections = data;
                     this.model.collections = data;
                   
                 });
         },

         selectItemField: function (fieldVal) {
           
             var field = this.itemFields.find(f => {
                 
                 return f.fieldId == fieldVal
             });
           


             this.selectedItemFields.push(field);
             
             //Update this.model.selectedFields
             this.model.selectedFields = this.selectedItemFields;

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
             this.model.selectedFields = this.selectedItemFields;

             
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



      }
    },
    computed: {
        //fieldsGroups() {

        //    this.selectedForms = groupBy(this.selectedItemFields, 'formName');
        //    console.log(JSON.stringify(selectedForms));
        //    return groupBy(this.selectedItemFields, 'formName');
        //}

    },
    template:
        `<div  class= 'block-body'>
            <h2>Report</h2>
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


