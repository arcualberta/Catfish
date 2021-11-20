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
            else if (elName == "itemEditorCss") {
                this.model.tileCss.value = e.target.value;
                var content = this.model.tileCss.value;
                if (content.length > 0) {
                    this.$emit('update-content', {
                        uid: this.uid,
                        tileCss: content
                    });
                }
            }
            

        },
        //selectItemTemplate: function (selected) {

        //    fetch('/api/Items/GetItemtemplateFields/' + selected)
        //        .then(response => response.json())
        //        .then((data) => {
        //            this.itemFields = data;

        //        });

        //    fetch('/api/Items/GetItemtemplateMetadataSets/' + selected)
        //        .then(response => response.json())
        //        .then((data) => {
        //            this.metadatasets = data;

        //        });
        //},
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
        }
    },
    template:
        `<div  class= 'block-body'>
            <h2>Item Editor</h2>
       </div>`
});