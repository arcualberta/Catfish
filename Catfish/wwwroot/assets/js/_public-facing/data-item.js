
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
        fieldNameFor(index, childPropertyName) {
            let name = "Fields[" + index + "]";

            if (childPropertyName !== null && childPropertyName !== undefined) {
                name = name + "." + childPropertyName;
            }
            return name;
        }
    },

    template: `
        <div class="container">

            <input type="hidden" name="Id" :value=this.model.Id />
            <input type="hidden" name="EntityId" :value=this.model.EntityId />
            <input type="hidden" name="TemplateId" :value=this.model.TemplateId />

            <div v-for="(field, index) in this.model.Fields" class="row field-row" :class=field.CssClass>
                <input type="hidden" :value=field.Id :name="fieldNameFor(index, 'Id')" />
                <input type="hidden" :value=field.ModelType :name="fieldNameFor(index, 'ModelType')" />

                <div class="col-md-3 control field-label" v-if="isLabelVisible(field)">
                    {{field.Name.ConcatenatedContent}}
                </div>
               <div :id=field.Id class="field-value" :class="fieldValueClass(field)" >
                    <component :is="field.VueComponent" v-bind:model=field v-bind:fieldNamePrefix="fieldNameFor(index)" />
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
    props: ["model", "fieldNamePrefix"],

    data: function () {
        return {
            dateType: this.model.IncludeTime ? "datetime-local" : "date"
        }
    },

    methods: {
        fieldNameFor(prefix, index, childPropertyName) {
            let name = prefix;

            if (index !== null && index !== undefined)
                name = name + ".Values[" + index + "]";

            if (childPropertyName !== null && childPropertyName !== undefined) {
                name = name + "." + childPropertyName;
            }
            return name;
        }
    },

    template: `
        <div>
            <div v-for="(val, index) in this.model.Values" >
                <input type="hidden" :value=val.Id :name="fieldNameFor(fieldNamePrefix, index, 'Id')" />
                <input type="hidden" :value=val.ModelType :name="fieldNameFor(fieldNamePrefix, index, 'ModelType')" />
                <input :type=dateType  placeholder="yyyy-mm-dd" :name="fieldNameFor(fieldNamePrefix, index, 'DateValue')" />
            </div>
        </div>`

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
    props: ["model", "fieldNamePrefix"],

    methods: {
        fieldNameFor(prefix, index, childPropertyName) {
            let name = prefix;

            if (index !== null && index !== undefined)
                name = name + ".Options[" + index + "]";

            if (childPropertyName !== null && childPropertyName !== undefined) {
                name = name + "." + childPropertyName;
            }
            return name;
        }
    },

    template: `
        <div>
            <span v-for="(opt, index) in this.model.Options" >
                <input type="hidden" :value=opt.Id :name="fieldNameFor(fieldNamePrefix, index, 'Id')" />
                <input type="hidden" :value=opt.ModelType :name="fieldNameFor(fieldNamePrefix, index, 'ModelType')" />
                <input type="radio" :name="fieldNameFor(fieldNamePrefix, null, null)" :value=opt.Id />
                <span class='radio-option-label'>{{opt.OptionText.ConcatenatedContent}}</span>
            </span>
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
