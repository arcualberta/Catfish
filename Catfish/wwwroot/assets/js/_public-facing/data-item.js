
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
        },
        visibleIf(field) {
            try {
                if (field.VisibleIfOptionFieldId && field.VisibleIfOptionIds) {
                    let isVisible = false;
                    this.model.Fields.map((ctrlField, idx) => {
                        if (ctrlField.Id === field.VisibleIfOptionFieldId) {
                            let matchingOptions = ctrlField.Options
                                .filter(op => field.VisibleIfOptionIds.includes(op.Id) && op.Selected === true);

                            isVisible = matchingOptions.length > 0;
                        }
                    });

                    return isVisible;
                }

                return true;
            }
            catch (err) {
                console.log(err);
            }
        },
        optionchanged: function(event) {
            this.model.Fields.forEach(field => {
                var val = event;
            })
        }
    },

    computed: {
        visibleFields: function () {
            let visible = this.model.Fields.filter(field => this.visibleIf(field));
            return visible;
        }
    },

    template: `
        <div class="container">

            <input type="hidden" name="Id" :value=this.model.Id />
            <input type="hidden" name="EntityId" :value=this.model.EntityId />
            <input type="hidden" name="TemplateId" :value=this.model.TemplateId />

            <div v-for="(field, index) in this.visibleFields" class="row field-row" :class=field.CssClass >
                <input type="hidden" :value=field.Id :name="fieldNameFor(index, 'Id')" />
                <input type="hidden" :value=field.ModelType :name="fieldNameFor(index, 'ModelType')" />

                <div class="col-md-3 control field-label" v-if="isLabelVisible(field)">
                    {{field.Name.ConcatenatedContent}}
                </div>
                <div :id=field.Id class="field-value" :class="fieldValueClass(field)" >
                    <component 
                        :is="field.VueComponent" 
                        v-bind:model=field 
                        v-bind:fieldNamePrefix="fieldNameFor(index)" 
                        @option-changed="optionchanged($event)"/>
                </div>
            </div>
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.RadioField', {
    props: ["model", "fieldNamePrefix"],
    emits: ['option-changed'],

    methods: {
        fieldNameFor(prefix, index, childPropertyName) {
            let name = prefix;

            if (index !== null && index !== undefined)
                name = name + ".Options[" + index + "]";

            if (childPropertyName !== null && childPropertyName !== undefined) {
                name = name + "." + childPropertyName;
            }
            return name;
        },
        onBlur: function (e) {
            this.$emit('option-changed', {
                name: e.target.name,
                id: e.target.value
            });

            //this.model.cssVal.value = e.target.value;

            //var content = this.model.cssVal.value;
            //if (content.length > 0) {
            //    this.$emit('update-content', {
            //        uid: this.uid,
            //        cssVal: content
            //    });
            //}

        }
    },

    template: `
        <div>
            <span v-for="(opt, index) in this.model.Options" >
                <input type="hidden" :value=opt.Id        :name="fieldNameFor(fieldNamePrefix, index, 'Id')" />
                <input type="hidden" :value=opt.ModelType :name="fieldNameFor(fieldNamePrefix, index, 'ModelType')" />
                <input v-on:blur="onBlur" type="radio"  :value=opt.Id        :name="fieldNameFor(fieldNamePrefix, null, null)" />
                <span class='radio-option-label'>{{opt.OptionText.ConcatenatedContent}}</span>
            </span>
        </div>`
    //          <input v-on:blur="$emit('options-changed')" type="radio"  :value=opt.Id        :name="fieldNameFor(fieldNamePrefix, null, null)" />
})

Vue.component('Catfish.Core.Models.Contents.Fields.CheckboxField', {
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
                <input type="hidden"    :value=opt.Id          :name="fieldNameFor(fieldNamePrefix, index, 'Id')" />
                <input type="hidden"    :value=opt.ModelType   :name="fieldNameFor(fieldNamePrefix, index, 'ModelType')" />
                <input type="checkbox"   value="true"         :name="fieldNameFor(fieldNamePrefix, index, 'Selected')" />
                <span class='radio-option-label'>{{opt.OptionText.ConcatenatedContent}}</span>
            </span>
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
    props: ["model", "fieldNamePrefix"],

    data: function () {
        return {
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
                <input type="number" step="any" :name="fieldNameFor(fieldNamePrefix, index, 'DecimalValue')" />
            </div>
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
    props: ["model", "fieldNamePrefix"],

    data: function () {
        return {
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
                <input type="number" :name="fieldNameFor(fieldNamePrefix, index, 'DecimalValue')" />
            </div>
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.MonolingualTextField', {
    props: ["model", "fieldNamePrefix"],

    data: function () {
        return {
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
                <input type="text"   :name="fieldNameFor(fieldNamePrefix, index, 'Value')" />
            </div>
        </div>`
})


Vue.component('Catfish.Core.Models.Contents.Fields.SelectField', {
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
            <select :name=fieldNamePrefix :id=model.Id >
                <option v-for="(opt, index) in this.model.Options" 
                    :value=opt.Id>
                        {{opt.OptionText.ConcatenatedContent}}
                </option>
            </select>
        </div>`
})

Vue.component('Catfish.Core.Models.Contents.Fields.TextArea', {
    props: ["model", "fieldNamePrefix"],

    methods: {
        fieldNameFor(prefix, valIndex, textIndex, childPropertyName) {
            let name = prefix;

            if (valIndex !== null && valIndex !== undefined)
                name = name + ".Values[" + valIndex + "]";

            if (textIndex !== null && textIndex !== undefined)
                name = name + ".Values[" + textIndex + "]";

            if (childPropertyName !== null && childPropertyName !== undefined) {
                name = name + "." + childPropertyName;
            }
            return name;
        }
    },

    template: `
        <div>
            <div v-for="(val, valIndex) in this.model.Values" >
                <input type="hidden" :value=val.Id :name="fieldNameFor(fieldNamePrefix, valIndex, null, 'Id')" />
                <input type="hidden" :value=val.ModelType :name="fieldNameFor(fieldNamePrefix, valIndex, null, 'ModelType')" />
                <div v-for="(txt, textIndex) in val.Values">
                    <input type="hidden" :value=txt.Id :name="fieldNameFor(fieldNamePrefix, valIndex, textIndex, 'Id')" />
                    <input type="hidden" :value=txt.ModelType :name="fieldNameFor(fieldNamePrefix, valIndex, textIndex, 'ModelType')" />
                    <textarea   :name="fieldNameFor(fieldNamePrefix, valIndex, textIndex, 'Value')">{{txt.Value}}</textarea>
                </div>
            </div>
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.TextField', {
    props: ["model", "fieldNamePrefix"],

    methods: {
        fieldNameFor(prefix, valIndex, textIndex, childPropertyName) {
            let name = prefix;

            if (valIndex !== null && valIndex !== undefined)
                name = name + ".Values[" + valIndex + "]";

            if (textIndex !== null && textIndex !== undefined)
                name = name + ".Values[" + textIndex + "]";

            if (childPropertyName !== null && childPropertyName !== undefined) {
                name = name + "." + childPropertyName;
            }
            return name;
        }
    },

    template: `
        <div>
            <div v-for="(val, valIndex) in this.model.Values" >
                <input type="hidden" :value=val.Id :name="fieldNameFor(fieldNamePrefix, valIndex, null, 'Id')" />
                <input type="hidden" :value=val.ModelType :name="fieldNameFor(fieldNamePrefix, valIndex, null, 'ModelType')" />
                <div v-for="(txt, textIndex) in val.Values">
                    <input type="hidden" :value=txt.Id :name="fieldNameFor(fieldNamePrefix, valIndex, textIndex, 'Id')" />
                    <input type="hidden" :value=txt.ModelType :name="fieldNameFor(fieldNamePrefix, valIndex, textIndex, 'ModelType')" />
                    <input type="text"   :name="fieldNameFor(fieldNamePrefix, valIndex, textIndex, 'Value')" />
                </div>
            </div>
        </div>`

})
