Vue.component("child-form-submission", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            childForms:[],
            validationError: ''
        }
    },
    methods: {

        selectItemTemplate: function (selected) {

            fetch('/applets/api/ItemTemplates/GetItemTemplateChildForms/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.childForms = data;

                });
        }
    },
  mounted() {
      if (this.model.selectedItemTemplate?.value) {
          //applets/api/[controller]
          fetch('/applets/api/ItemTemplates/GetItemTemplateChildForms/' + this.model.selectedItemTemplate.value)
          .then(response => response.json())
          .then((data) => {
            this.childForms = data;

          });
      }
    },
    computed: {
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
    },
    template:
        `<div  class= 'block-body'>
            <h2>Child Form Submission</h2>
          
         
            <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
               <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplate.value)">
                    <option disabled value="">Please select one</option>
                    <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
                </select>
            </div>

            <div class='lead row'><label class='form-label col-md-3 required'>Child Forms: </label>
               <select v-model="model.selectedChildForm.value" class="form-control" style="width:auto;" >
                    <option disabled value="">Please select one</option>
                    <option v-for="item in this.childForms" :value="item.value">{{item.text}}</option>
                </select>
            </div>

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

           <div class='lead row'>
                <label class='form-label col-md-3' for="display-child-list">Display Child List?</label>
                <input type="checkbox" id="display-child-list" v-model="model.displayChildList.value">
           </div>
        </div>`

});

