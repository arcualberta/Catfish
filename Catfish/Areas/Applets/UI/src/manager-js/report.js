Vue.component("report", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            childForms:[],
            validationError: '',
            groups: [],
            itemFields: [],
            selectedItemFields:[]
        }
    },
     methods: {

      selectItemTemplate: function (selected) {

            fetch('/applets/api/ItemTemplates/GetItemTemplateRootForms/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.childForms = data;

                });
             fetch('/api/Items/GetItemtemplateFields/' + selected)
                 .then(response => response.json())
                 .then((data) => {
                     this.itemFields = data;

                 });
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
                    // console.log(JSON.stringify(this.model.collections))
                 });
         },

         selectItemField: function (fieldVal) {
           
             var field = this.itemFields.find(f => {
                 return f.value == fieldVal
             });
            // console.log("field found: " + JSON.stringify(field));
             this.selectedItemFields.push(field);
             //this.model.selectedFields.push(field);

             //Update this.model.selectedFields
            this.model.selectedFields = this.selectedItemFields;

            // console.log(JSON.stringify(this.model.selectedFields));
         },

         removeSelectedField: function (fvalue) {
             console.log("field val: " + fvalue);
             var filtered = this.selectedItemFields.filter(function (field, index, arr) {
                 return field.value !== fvalue;
             });
             //var filtered = this.model.selectedFields.filter(function (field, index, arr) {
             //    return field.value !== fvalue;
             //});
             //update the selectedField array
             this.selectedItemFields = filtered;
             //this.model.selectedFields = filtered;
             //Update this.model.selectedFields
             this.model.selectedFields = this.selectedItemFields;
             console.log(JSON.stringify(this.model.selectedFields));
         }
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
          fetch('/applets/api/ItemTemplates/GetItemTemplateRootForms/' + this.model.selectedItemTemplateId.value)
          .then(response => response.json())
          .then((data) => {
            this.childForms = data;

          });

          fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplateId.value)
              .then(response => response.json())
              .then((data) => {
                  this.groups = data;
              });



      }
    },
    /* computed: {
        isValid: function () {
            if (!this.model.queryParameter.value) {
                this.$data.validationError = "Specify a query parameter.";
                return false;
            }
            else if (this.model.queryParameter.value.toLowerCase() === "id") {
                this.$data.validationError = "Unacceptable query parameter value.";
                return false;
            }

            return true;
        }
    },*/
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

         <div  class='lead row'><label class='form-label col-md-3 required'>Select Field: </label>
           <select v-model="model.selectedField.value" class="form-control" style="width:auto;" v-on:change="selectItemField(model.selectedField.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.value">{{item.text}}</option>
            </select></div>

           <div  class='lead row'><label class='form-label col-md-3 required'>Selected Fields {{this.model.selectedFields?.length}}</label>
               <div   style="width:auto;" v-for="f in this.model.selectedFields" :key="f.value">
                  <div class="selectedField">{{f.text}} <span class="fas fa-times-circle" v-on:click="removeSelectedField(f.value)"> </span></div>
                </div>
           </div>

        </div>`
        

});


