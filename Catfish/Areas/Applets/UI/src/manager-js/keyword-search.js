Vue.component("keyword-search", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            itemFields: [],
            metadatasets: [],
            statuses: [],
            selectedStatuses: [],
            groups: []
        }
    },
    methods: {
        onBlur: function (e) {

            var elName = e.target.name;
            if (elName == "keywordList") {
                this.model.keywordList.value = e.target.value;
                var content = this.model.keywordList.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        keywordList: content
                    });
                }
            }
            else if (elName == "blockCss") {
                this.model.blockCss.value = e.target.value;
                var content = this.model.blockCss.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        blockCss: content
                    });
                }
            }
            else if (elName == "tileCss") {
                this.model.tileCss.value = e.target.value;
                var content = this.model.tileCss.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        tileCss: content
                    });
                }
            }
            else if (elName == "detailedViewUrl") {
                this.model.detailedViewUrl.value = e.target.value;
                var content = this.model.detailedViewUrl.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        detailedViewUrl: content
                    });
                }
            }

        },
        selectItemTemplate: function (selected) {

            fetch('/api/Items/GetItemtemplateFields/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.itemFields = data;

                });

            fetch('/api/Items/GetItemtemplateMetadataSets/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.metadatasets = data;

                });
            fetch('/applets/api/itemtemplates/groups/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.groups = data;
                });
            fetch('/api/Items/GetItemtemplateStatuses/' + selected)
                .then(response => response.json())
                .then((data) => {
                    this.statuses = data; //statuses from template

                    if (this.model.selectedStates.value !== null) {
                        //select the checkbox if it's been selected
                        for (let i = 0; i < this.statuses.length; i++) {
                            if (this.model.selectedStates.value.includes(this.statuses[i].value)) {
                                this.statuses[i].checked = true;
                            }
                        }


                        this.selectedStatuses = (this.model.selectedStates.value.split(',')).filter(e => e !== ""); //temporary array contain selected state/status in model



                        //check if selected status/state still exist in the template
                        //1. get the guid that's no longer in the template
                        var tempSelected = [];
                        for (let j = 0; j < this.selectedStatuses.length; j++) {
                            var _temp = this.statuses.filter(e => e.value === this.selectedStatuses[j]);
                            if (_temp !== null) {
                                console.log("selected status is still remain in the template");
                                tempSelected.push(this.selectedStatuses[j]);
                            }
                            else
                                console.log("selected status has been remove from the template");
                        }

                        //console.log("tempSeelected length: " + tempSelected.length);
                        if (tempSelected.length > 0 && tempSelected.length !== this.selectedStatuses.length) {
                            //one or more selected status has been removed from the template
                            this.selectedStatuses = tempSelected;
                        }

                        // console.log("mount local selectedstatuses: " + JSON.stringify(this.selectedStatuses));
                        this.model.selectedStates.value = this.selectedStatuses.toString(); //value saved in the model
                    }

                    //this.selectedStatuses = this.model.selectedStates;
                    // console.log("mount model: " + this.model.selectedStates.value);
                });
        },
        selectStatus(id) {


            if (this.selectedStatuses?.includes(id)) {
                //console.log("deselect selected a state: " + id)
                this.selectedStatuses = this.selectedStatuses?.filter(e => e !== id);
                // console.log(JSON.stringify(this.selectedStatuses));

            } else {
                // console.log("push selected a state: " + id)
                this.selectedStatuses.push(id)
            }
            this.selectedStatuses = this.selectedStatuses?.filter(e => e !== "");

            this.model.selectedStates.value = "";
            this.model.selectedStates.value = this.selectedStatuses?.toString();

        }
    },
    mounted() {
        if (this.model.selectedItemTemplate?.value) {
            fetch('/api/Items/GetItemtemplateFields/' + this.model.selectedItemTemplate.value)
                .then(response => response.json())
                .then((data) => {
                    this.itemFields = data;

                });

            fetch('/api/Items/GetItemtemplateMetadataSets/' + this.model.selectedItemTemplate.value)
                .then(response => response.json())
                .then((data) => {
                    this.metadatasets = data;

                });

            fetch('/api/Items/GetItemtemplateStatuses/' + this.model.selectedItemTemplate.value)
                .then(response => response.json())
                .then((data) => {
                    this.statuses = data; //statuses from template

                    if (this.model.selectedStates.value !== null) {
                        //select the checkbox if it's been selected
                        for (let i = 0; i < this.statuses.length; i++) {
                            if (this.model.selectedStates.value.includes(this.statuses[i].value)) {
                                this.statuses[i].checked = true;
                            }
                        }


                        this.selectedStatuses = (this.model.selectedStates.value.split(',')).filter(e => e !== ""); //temporary array contain selected state/status in model



                        //check if selected status/state still exist in the template
                        //1. get the guid that's no longer in the template
                        var tempSelected = [];
                        for (let j = 0; j < this.selectedStatuses.length; j++) {
                            var _temp = this.statuses.filter(e => e.value === this.selectedStatuses[j]);
                            if (_temp !== null) {
                                //console.log("selected status is still remain in the template");
                                tempSelected.push(this.selectedStatuses[j]);
                            }
                            else
                                console.log("selected status has been remove from the template");
                        }

                        //console.log("tempSeelected length: " + tempSelected.length);
                        if (tempSelected.length > 0 && tempSelected.length !== this.selectedStatuses.length) {
                            //one or more selected status has been removed from the template
                            this.selectedStatuses = tempSelected;
                        }

                        // console.log("mount local selectedstatuses: " + JSON.stringify(this.selectedStatuses));
                        this.model.selectedStates.value = this.selectedStatuses.toString(); //value saved in the model
                    }

                    //this.selectedStatuses = this.model.selectedStates;
                    // console.log("mount model: " + this.model.selectedStates.value);
                });

            fetch('/applets/api/itemtemplates/groups/' + this.model.selectedItemTemplate.value)
                .then(response => response.json())
                .then((data) => {
                    this.groups = data;
                });
        }
    },
    computed: {
        getSelectedState() {
            return this.model.selectedStates.value = this.selectedStatuses ? this.selectedStatuses.toString() : "";
        }
    },
    template:
        `<div  class= 'block-body'>
            <h2>Keyword Search</h2>

               <div>Please list your keywords separated by a comma.</div>
         <textarea rows='2' cols='100' class='lead form-control '
            v-html='model.keywordList.value' contenteditable='true' v-on:blur='onBlur' name='keywordList'>
        </textarea>
        <br/>
		  <div class='lead row'><label class='form-label col-md-3'>Title: </label>
               <input type="text"  v-model="model.blockTitle.value" class="form-control col-md-9" >
            </div>
            <div class='lead row'><label class='form-label col-md-3'>Description: </label>
               <textarea  v-model="model.description.value" cols="30" rows="2" contenteditable='true' class="form-control col-md-9" ></textarea>
            </div>
        
         
           <div class='lead row'><label class='form-label col-md-3 required'>Collection: </label>
           <select v-model="model.selectedCollection.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.collections.entries" :value="item.value">{{item.text}}</option>
         </select></div>

            <div class='lead row'><label class='form-label col-md-3'>Css class for Block:</label> <input type='text' class='form-control col-md-4' name='blockCss' v-model='model.blockCss.value' contenteditable='true' v-on:blur='onBlur' value='blockCssValue'  /></div>
         <div class='lead row'><label class='form-label col-md-3'>Css class for each Tile:</label> <input type='text' class='form-control col-md-4' name='tileCss' v-model='model.tileCss.value' contenteditable='true' v-on:blur='onBlur' value='tileCssValue'  /></div>
         
        <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
           <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplate.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
            </select></div>

          <div class='lead row'><label class='form-label col-md-3 required'>Group: </label>
           <select v-model="model.selectedGroupId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.groups" :value="item.value">{{item.text}}</option>
            </select></div>
         <br/>
        <div class="alert alert-info">Metadata Set</div>
        <div class='lead row'><label class='form-label col-md-3 required'>Classification Metadata Set: </label>
           <select v-model="model.classificationMetadataSetId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.metadatasets" :value="item.value">{{item.text}}</option>
            </select></div>


          <br/>
        <div class="alert alert-info">Submission Form Field Mappings</div>
        

          <div class='lead row'><label class='form-label col-md-3 required'>Title: </label>
           <select v-model="model.selectedMapTitleId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.value">{{item.text}}</option>

         </select></div>

        <div class='lead row'><label class='form-label col-md-3 required'>Subtitle: </label>
           <select v-model="model.selectedMapSubtitleId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.value">{{item.text}}</option>

            </select></div>

           <div class='lead row'><label class='form-label col-md-3 required'>Content: </label>
           <select v-model="model.selectedMapContentId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.value">{{item.text}}</option>

            </select></div>
            <div class='lead row'><label class='form-label col-md-3 required'>Keyword Source: </label>
           <select v-model="model.keywordSourceId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.value">{{item.text}}</option>

            </select><span class="text-info" style="font-size:.8em; padding-left:5px"><b> Please select a radio, checkbox or dropdown field</b></span></div>
           <div class='lead row'><label class='form-label col-md-3 required'>Thumbnail: </label>
           <select v-model="model.selectedMapThumbnailId.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in this.itemFields" :value="item.value">{{item.text}}</option>

         </select></div>

        
         <div class='lead row'>
            <label class='form-label col-md-3'>Detailed View Url:</label>
            <input type='text'
                    class='form-control col-md-4'
                    name='detailedViewUrl'
                    v-model='model.detailedViewUrl.value'
                    contenteditable='true'
                    v-on:blur='onBlur'
                    value='detailedViewUrlValue'  />
         </div>

          <div class="alert alert-info">Filter by Statuses:</div>
          <div v-for="(item, index) in this.statuses" :key="index" v-if="item.text != ''" >
            <input type="checkbox" :id="item.value" v-model="item.checked" @click="selectStatus(item.value)" >
            <label :for="item.value">{{ item.text }}</label>
          </div>
       

         <div class="alert alert-info">Other Settings:</div>
         <div class='lead'><label >Enabled Free Text Search: </label>
            <input type="checkbox" id="enabled-free-text-search-checkbox" v-model="model.enabledFreeTextSearch.value" ><label style="padding-left:5px;">{{model.enabledFreeTextSearch.value}}</label>
            </div>

           <div class='lead row'><label class='form-label col-md-3'>Display Layout Format: </label>
           <select v-model="model.selectedDisplayFormat.value" class="form-control" style="width:auto;">
                  <option disabled value="">Please select one</option>
                <option value="List" selected>List</option>
                <option value="Directory">Directory</option>

         </select></div>
<div class='lead row'><label class='form-label col-md-3'>Info Pop-up Content: </label>
               <textarea  v-model="model.infoContent.value" cols="30" rows="5" contenteditable='true' class="form-control col-md-9" ></textarea>
            </div>
        <div  class='lead'>
               <label >List of hex color codes for 'Dictionary' layout, separated by a comma,including the # key at the beginning, (i.e: #00b19d, #5175a5, ...): </label>
               <input type="text"  v-model="model.hexColorList.value" class="form-control">
         </div>
       </div>`
});

