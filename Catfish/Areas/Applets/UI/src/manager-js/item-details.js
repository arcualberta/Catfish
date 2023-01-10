Vue.component("item-details", {
    props: ["uid", "toolbar", "model"],
    data() {
        return {
            validationError: ''
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
            <h2>Item Details</h2>
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
       </div>`
});