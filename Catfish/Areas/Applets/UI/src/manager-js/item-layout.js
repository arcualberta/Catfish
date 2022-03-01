Vue.component("item-layout", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            layoutComponents: [],
            mainForm: [],
            allForms: [],
            validationError: '',
            groups: [],
            itemFields: [],
            selectedItemFields: [],
            fieldGroups: []
        }
    },
    methods: {

        selectItemTemplate: function (selected) {


            fetch('/applets/api/ItemTemplates/GetAllItemTemplateForms/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.allForms = data;

                });


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
        onAddComponent: function (componentLabel) {
            console.log(componentLabel);
            let selectedComponent = this.model.componentTemplates.find(ele => ele.label === componentLabel);
            if (selectedComponent) {
                let clone = JSON.parse(JSON.stringify(selectedComponent));
                clone.id = new Date().getTime()
                this.layoutComponents.push(clone);
            }
        },
        getFormsOfSelectedTemplate() {
            return this.allForms;
        }


    },
    mounted() {


        if (this.model.selectedItemTemplateId?.value) {




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
         <br/>
         <div class="alert alert-info">Please select the item fields you would like to see appear on the page </div>
         <div class="itemLayout" style="border: solid 1px lightgrey;">
           <div class='lead row'>
            <label class='form-label col-md-1'>Type:</label>
            <select class='form-control col-md-2' name="modalSize"  :value='model.selectedType.value' >
                <option value="h1">H1</option>
                <option value="h2">H2</option>
                <option value="h3">H3</option>
                <option value="h4">H4</option>
                <option value="h5">H5</option>
                <option value="div">Div</option>
                <option value="p">Paragraph</option>
                <option value="img">IMG</option>
                <option value="file">AttachmentField</option>
             </select>
             <label class='form-label col-md-2'>Class(es):</label>
             <input type="text" v-model="model.selectedClass.value" />
               <label class='form-label col-md-1'>Id:</label>
             <input type="text" v-model="model.selectedElementId.value" />
        </div>
         <div  class='lead row'>
             <label class='form-label col-md-1 '>Form: </label>
               <select v-model="model.selectedFormId.value" class="form-control" style="width:auto;" v-on:change="getFormFields(model.selectedItemTemplateId.value,model.selectedFormId.value)">
                    <option disabled value="">Please select one</option>
                    <option v-for="item in this.allForms" :value="item.value">{{item.text}}</option>
                </select>
       
           <label class='form-label col-md-2'>Select Field: </label>
           <select v-model="model.selectedField.value" class="form-control" style="width:auto;" v-on:change="selectItemField(model.selectedField.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.fieldId">{{item.fieldName}}</option>
            </select>
              <label class='form-label col-md-1'>Style:</label>
             <textarea v-model="model.selectedStyle.value" cols="30" rows="2" />
          </div>
         </div>

        <div>
            <div v-for="component in layoutComponents" :key="component.id">
              
                {{component.label}}

               <div v-if="component.label === 'Form Field'">
                   <label class='form-label col-md-1 '>Form: </label>
                    <select v-model="component.formId" class="form-control" style="width:auto;" v-on:change="getFormFields(model.selectedItemTemplateId.value,model.selectedFormId.value)">
                        <option disabled value="">Please select one</option>
                        <option v-for="form in getFormsOfSelectedTemplate()" :value="form.value">{{form.text}}</option>
                    </select>
                </div>
               <div v-if="component.label === 'Static Text'">
                    Content: <textarea />                
                </div>

                </hr />
            </div>
        </div>

        <div>
            <button v-for="component in model.componentTemplates" @click="onAddComponent(component.label)">
                + {{component.label}}
            </button>
        </div>
        <hr />
        <hr />
        <div>{{JSON.stringify(layoutComponents)}}</div>
        </div>`


});


