Vue.component("tile-grid", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            itemFields: []
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

            fetch('/api/Items/GetItemtemplateFields/' + selected )
                .then(response => response.json())
                .then((data) => {
                    this.itemFields = data;

                });
        },
    },
    mounted() {
        fetch('/api/Items/GetItemtemplateFields/' + this.model.selectedItemTemplate.value)
            .then(response => response.json())
            .then((data) => {
                this.itemFields = data;

        });
    },
    template:
        `<div  class= 'block-body'>
            <h2>Tile Grid</h2>
           <div>Please list your keywords separated by a comma.</div>
         <textarea rows='4' cols='100' class='lead ' 
            v-html='model.keywordList.value' contenteditable='true' v-on:blur='onBlur' name='keywordList'>
        </textarea>
           <div class='lead row'><label class='form-label col-md-3 required'>Collection: </label>
           <select v-model="model.selectedCollection.value" class="form-control" style="width:auto;">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.collections.entries" :value="item.value">{{item.text}}</option>
         </select></div>

            <div class='lead row'><label class='form-label col-md-3'>Css class for Block:</label> <input type='text' class='form-control col-md-4' name='blockCss' v-model='model.blockCss.value' contenteditable='true' v-on:blur='onBlur' value='blockCssValue'  /></div>
         <div class='lead row'><label class='form-label col-md-3'>Css class for each Tile:</label> <input type='text' class='form-control col-md-4' name='tileCss' v-model='model.tileCss.value' contenteditable='true' v-on:blur='onBlur' value='tileCssValue'  /></div>
         <br/>
        <div class="alert alert-info">Field Mapping</div>
         <div class='lead row'><label class='form-label col-md-3 required'>Item Template: </label>
           <select v-model="model.selectedItemTemplate.value" class="form-control" style="width:auto;" v-on:change="selectItemTemplate(model.selectedItemTemplate.value)">
                <option disabled value="">Please select one</option>
                <option v-for="item in model.itemTemplates.entries" :value="item.value">{{item.text}}</option>
            </select></div>

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
          
       </div>`
});

