Vue.component("form-submission", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            childForms:[],
            validationError: '',
            groups: [],
            collections:[]
        }
    },
    methods: {

        selectItemTemplate: function (selected) {

            fetch('/applets/api/ItemTemplates/GetItemTemplateRootForms/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.childForms = data;

                });

            fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplate.value)
                .then(response => response.json())
                .then((data) => {
                    this.groups = data;
                });
        }
    },
    mounted() {
        //getCollectionList
        fetch('/api/Items/GetCollectionList')
            .then(response => response.json())
            .then((data) => {
                this.collections = data;

            });


      if (this.model.selectedItemTemplate?.value) {
          //applets/api/[controller]
          fetch('/applets/api/ItemTemplates/GetItemTemplateRootForms/' + this.model.selectedItemTemplate.value)
          .then(response => response.json())
          .then((data) => {
            this.childForms = data;

          });

          fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplate.value)
              .then(response => response.json())
              .then((data) => {
                  this.groups = data;
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
            <h2>Form Submission</h2>

       <div class='lead row'><label class='form-label col-md-3 required'>Collection: </label>
           <select v-model="model.selectedCollection.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.collections" :value="item.value">{{item.text}}</option>
         </select></div>
         
            <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
               <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplate.value)">
                    <option disabled value="">Please select one</option>
                    <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
                </select>
            </div>

            <div class='lead row'><label class='form-label col-md-3 required'>Form: </label>
               <select v-model="model.selectedForm.value" class="form-control" style="width:auto;" >
                    <option disabled value="">Please select one</option>
                    <option v-for="item in this.childForms" :value="item.value">{{item.text}}</option>
                </select>
            </div>
         <div class='lead row'><label class='form-label col-md-3 required'>Group: </label>
           <select v-model="model.selectedGroupId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.groups" :value="item.value">{{item.text}}</option>
            </select></div>
     
       
        </div>`

});

