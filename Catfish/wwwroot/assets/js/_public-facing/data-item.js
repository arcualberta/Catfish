
Vue.component('data-item', {
    props: ["model"],

    data: function () {
        return {
        }
    },

  template: `
        <div>
            <div v-for="field in this.model.Fields">
                <component :is="field.ViewTag" />
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
    props: ["model"],

    template: `
        <div>
            DateField
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

    template: `
        <div>
            InfoSection
        </div>`

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
