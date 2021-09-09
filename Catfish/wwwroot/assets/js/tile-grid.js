Vue.component("tile-grid", {
    props: ["uid", "toolbar", "model"],
    methods: {
        onBlur: function (e) {
            this.model.keywordList.value = e.target.value;

            var content = this.model.keywordList.value;
            if (content.length > 0) {
                this.$emit('update-content', {
                    uid: this.uid,
                    keywordList: content
                });
            }

        }
    },
    template:
        `<div  class= 'block-body'>
            <h2>Tile Grid</h2>
           <div>Please list your keywords separated by a comma.</div>
         <textarea rows='4' cols='100' class='lead ' 
            v-html='model.keywordList.value' contenteditable='true' v-on:blur='onBlur' >
        </textarea>
         </div>`
});

