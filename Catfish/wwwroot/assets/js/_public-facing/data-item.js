
Vue.component('data-item', {
    props: ["model"],

    data: function () {
        return {
        }
    },

    methods: {
        isLabelVisible: function (field) {
            return field.VueComponent !== "Catfish.Core.Models.Contents.Fields.InfoSection";
        },
        fieldValueClass: function (field) {
            return this.isLabelVisible(field) ? "col-md-9" : "col-md-12";
        },
        fieldNameFor(index) {
            return "Fields[" + index + "]";
        }
    },

    template: `
        <div class="container">
            <div v-for="(field, index) in this.model.Fields" class="row" :class=field.CssClass>
                <input 
                    type="hidden" 
                    :name="'Fields[' + index + '].ModelType'" 
                    :value=field.ModelType />
                <input type="hidden" name="" :value=field.Id />
                <div class="col-md-3 control field-label" v-if="isLabelVisible(field)">
                    {{field.Name.ConcatenatedContent}}
                </div>
                <div :id=field.Id class="field-value" :class=fieldValueClass(field) >
                    <component :is="field.VueComponent" v-bind:model=field v-bind:name=fieldNameFor(index) />
                </div>
            </div>
        </div>`

})


Vue.component('Catfish.Core.Models.Contents.Fields.CheckboxField', {
    props: ["model"],

    template: `
        <div>
            CheckboxField
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.DateField', {
    props: ["model", "name"],

    data: function () {
        return {
            dateType: this.model.IncludeTime ? "datetime-local" : "date"
        }
    },

    template: `<input :type=dateType  placeholder="yyyy-mm-dd" />`

})

Vue.component('Catfish.Core.Models.Contents.Fields.DecimalField', {
    props: ["model"],

    template: `
        <div>
            DecimalField
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.InfoSection', {
    props: ["model"],

    data: function () {
        return {
            content: this.model.Content.ConcatenatedRichText
        }
    },

   template: `<div v-html="content"></div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.IntegerField', {
    props: ["model"],

    template: `
        <div>
            IntegerField
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.MonolingualTextField', {
    props: ["model"],

    template: `
        <div>
            MonolingualTextField
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.RadioField', {
    props: ["model"],

    template: `
        <div>
            RadioField
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.SelectField', {
    props: ["model"],

    template: `
        <div>
            SelectField
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.TextArea', {
    props: ["model"],

    template: `
        <div>
            Text Area
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.TextField', {
    props: ["model"],

    template: `
        <div>
            Text Field
        </div>`

})
