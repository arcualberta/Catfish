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
    
    template:
        `<div  class= 'block-body'>
            <h2>Item Editor</h2>
       </div>`
});