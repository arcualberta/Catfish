Vue.component("item-editor", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            itemFields: [],
            metadatasets: []
        }
    },
    methods: {
        onBlur: function (e) {

            var elName = e.target.name;
            if (elName == "ItemEditor") {
                this.model.ItemId.value = e.target.value;
                var content = this.model.ItemId.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        ItemId: content
                    });
                }
            }
            
        },
        
    },
    
    template: "<div  class= 'block-body calendar-block'>" +
        "<div class='lead row'><label class='form-label col-md-3 required'>Item ID: </label><input class='form-control col-md-8' type='text' name='itemId' v-model='model.itemId.value' contenteditable='true' v-on:blur='onBlur' value='itemIdValue'  :class='{ requiredField: isItemIdEmpty }' /></div>" +
       // "<div class='lead row'><label class='form-label col-md-3 required'>Collection ID: </label><input class='form-control col-md-8' type='text' name='collectionId' v-model='model.collectionId.value' contenteditable='true' v-on:blur='onBlur' value='collectionIdValue'  :class='{ requiredField: isCollectionEmpty }' /></div>" +
        "</div>"
});