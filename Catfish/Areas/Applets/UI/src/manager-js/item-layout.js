Vue.component("item-layout", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            layoutComponents: [],
            allForms: [],
            itemFields: [],
        }
    },
    methods: {
        onBlur: function (e) {
            this.model.selectedComponents.value = JSON.stringify(this.layoutComponents);
        },
        selectItemTemplate: function (selected) {
            fetch('/applets/api/ItemTemplates/GetAllItemTemplateForms/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.allForms = data;

                });
        },
        removeComponent: function (componentId) {

            this.layoutComponents
            var filtered = this.layoutComponents.filter(function (field, index, arr) {

                return field.id !== componentId;
            });
            this.layoutComponents = filtered;
            this.model.selectedComponents.value = JSON.stringify(this.layoutComponents);

        },

        getFormFields: function (itemTemplateId, formTemplateId) {

            let uniqueFormIds = [...new Set(this.layoutComponents.map(com => com.formTemplateId))];

            fetch('/applets/api/itemtemplates/getMutipleFormFields/' + itemTemplateId + "/" + uniqueFormIds)
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
        updateSelectedComponents() {
            this.model.selectedComponents.value = JSON.stringify(this.layoutComponents);
        },
        getFormsOfSelectedTemplate() {
            return this.allForms;
        },
        getSelectedFormFields(formTemplateId) {

            return this.itemFields[formTemplateId];
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

        if (this.model.selectedComponents?.value) {
            this.layoutComponents = JSON.parse(this.model.selectedComponents.value);

            let uniqueFormIds = [...new Set(this.layoutComponents.map(com => com.formTemplateId))];

            fetch('/applets/api/itemtemplates/getMutipleFormFields/' + this.model.selectedItemTemplateId?.value + "/" + uniqueFormIds)
                .then(response => response.json())
                .then((data) => {
                    this.itemFields = data;

                    // console.log("multiple " + JSON.stringify(this.itemFields));
                });
        }
    },

    template:
        `<div  class= 'block-body'>
            <h2>Item Layout</h2>
            <div class='lead row'>
                <label class='form-label col-md-3 required'>Query Parameters: </label>
                <input class='form-control col-md-2'
                       type='text'
                       name='QueryParameter'
                       v-model='model.queryParameter.value'
                       contenteditable='true'
                />
                <span v-if='!isValid' style='color:red'>&nbsp;{{validationError}}</span>
            </div>
            <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
               <select v-model="model.selectedItemTemplateId.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplateId.value)">
                    <option disabled value="">Please select one</option>
                    <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
                </select>
            </div>
         <br/>
         <div class="alert alert-info">Please select the item fields you would like to see appear on the page </div>
        
        <div>
            <div v-for="component in layoutComponents" :key="component.id" class="itemLayout" >
                <div class="text-right"><span class="fas fa-times-circle text-danger" @click="removeComponent(component.id)"></span></div>
                <div v-if="component.label === 'Form Field'" class='lead row'>
                    <div class="col-md-5 row" style="margin:5px">
                        <label class='form-label' style="margin:5px">Form: </label>
                        <select v-model="component.formTemplateId" class="form-control col-md-9" @change="getFormFields(model.selectedItemTemplateId.value,component.formTemplateId)">
                            <option disabled value="">Please select one</option>
                            <option v-for="form in getFormsOfSelectedTemplate()" :value="form.value">{{form.text}}</option>
                        </select>
                    </div>
                    <div class="col-md-5 row" style="margin-left:15px;">
                    <label class='form-label' style="margin:5px;">Select Field: </label>
                    <select v-model="component.fieldId" class="form-control col-md-8" style="width:auto;" @change="updateSelectedComponents()">
                        <option disabled value="">Please select one</option>
                        <option v-for="fld in getSelectedFormFields(component.formTemplateId)" :value="fld.fieldId">{{fld.fieldName}}</option>
                    </select>
                    </div>
                </div>
                <div v-if="component.label === 'Static Text'" class='lead row'>
                   <label class='form-label col-md-2 '>Content: </label>
                   <textarea v-model="component.content" cols="50" rows="2"  class="col-md-9" v-on:blur="onBlur" />
                </div>
                <div class='lead row'>
                    <label class='form-label col-md-1'>Type:</label>
                    <select class='form-control col-md-2' name="modalSize"  v-model='component.type' @change="updateSelectedComponents()">
                        <option value="h1">H1</option>
                        <option value="h2">H2</option>
                        <option value="h3">H3</option>
                        <option value="h4">H4</option>
                        <option value="h5">H5</option>
                        <option value="div">Div</option>
                        <option value="p">Paragraph</option>
                        <option value="img">Image</option>
                        <option value="file">File</option>
                     </select>
                     <label class='form-label'  style="margin-left: 10px; margin-right: 10px">Class(es):</label>
                     <input type="text" v-model="component.cssClasses" class="col-md-2"  v-on:blur="onBlur" />
                       <label class='form-label' style="margin-left: 10px; margin-right: 10px">Id:</label>
                     <input type="text" v-model="component.elementId"  class="col-md-2" v-on:blur="onBlur" />
                      <label class='form-label' style="margin-left: 10px; margin-right: 10px">Style:</label>
                     <textarea v-model="component.cssStyle" cols="20" rows="2"  class="col-md-2" v-on:blur="onBlur" />
                </div>
            </div>
        </div>

        <div>
            <button v-for="component in model.componentTemplates" @click="onAddComponent(component.label)" class="btn btn-default btn-success" style="margin-right: 10px;">
                <span class="fas fa-plus" > {{component.label}}</span>
            </button>
        </div>
       
        </div>`


});


