
Vue.component('data-item', {
    props: ["model"],

    data: function () {
        return {
            visibleFields: []
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
            console.log("visibility check");
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
        /**
         * Receives the emit from the child field of the choice the user made.
         * @param {any} event an array of options the user chose. It is an array for single-input fields too.
         */
        optionchanged: function (event) {
            console.log("received change", event, this.model);
            //if any others for this id are Selected = true, need to set them to false bc this is a radio button
            this.model.Fields[event.indexNum].Options.forEach((option) => {
                event.optionGuid.forEach((chosenOption) => {
                    if (option.Id != chosenOption) {
                        option.Selected = false;
                    } else {
                        option.Selected = true;
                    }
                });
                
            });

            this.visibleFields = this.findVisibleFields();
        },

        findVisibleFields() {
            let visible = this.model.Fields.filter(field => this.visibleIf(field)); //this.model.Fields
            console.log("visible computed!! here you go", visible);
            return visible;
        }

    },

    mounted() {
        this.visibleFields = this.findVisibleFields();
        console.log(this.visibleFields, this.model);
    },

    template: `
        <div class="container">
            <input type="hidden" name="Id" :value=this.model.Id />
            <input type="hidden" name="EntityId" :value=this.model.EntityId />
            <input type="hidden" name="TemplateId" :value=this.model.TemplateId />

            <div v-for="(field, index) in visibleFields" class="row field-row" :class=field.CssClass >
                <input type="hidden" :value=field.Id :name="fieldNameFor(index, 'Id')" />
                <input type="hidden" :value=field.ModelType :name="fieldNameFor(index, 'ModelType')" />

                <div class="col-md-3 control field-label" v-if="isLabelVisible(field)">
                    {{field.Name.ConcatenatedContent}} 
                </div>
                <div :id=field.Id class="field-value" :class="fieldValueClass(field)" >
                    <component 
                        :is="field.VueComponent" 
                        :model="field"
                        :fieldIndex="index"
                        :fieldNamePrefix="fieldNameFor(index)" 
                        v-on:option-changed="optionchanged($event)"/>
                </div>
            </div>
        </div>`

})

Vue.component('Catfish.Core.Models.Contents.Fields.RadioField', {
    props: ["model", "fieldNamePrefix", "fieldIndex"],
    emits: ['option-changed'],

    data: function () {
        return {
        }
    },
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
        onOptionPicked: function (e) {
            this.$emit('option-changed', {
                indexNum: this.fieldIndex, //need to check if, if it skips a field due to invisibility, does it use the right id?
                optionGuid: [e.target.value] //guid of option
            });
        },

    },
    /*
     <input type="hidden" :value=opt.Id        :name="fieldNameFor(fieldNamePrefix, index, 'Id')" />
                <input type="hidden" :value=opt.ModelType :name="fieldNameFor(fieldNamePrefix, index, 'ModelType')" />
     */
    template: `
        <div>
            <span v-for="(opt, index) in model.Options" >
                <input @click="onOptionPicked" type="radio" :value="opt.Id" :name="fieldNameFor(fieldNamePrefix, null, null)" />
                <span class='radio-option-label'>{{opt.OptionText.ConcatenatedContent}}</span>
            </span>
        </div>`
    //          <input v-on:blur="$emit('options-changed')" type="radio"  :value=opt.Id        :name="fieldNameFor(fieldNamePrefix, null, null)" />
})






Vue.component('Catfish.Core.Models.Contents.Fields.CheckboxField', {
    props: ["model", "fieldNamePrefix", "fieldIndex"],
    data: function () {
        return {
            checklist: []
        }
    },
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
        onOptionPicked: function (e) {
            console.log('checklist', this.checklist);
            this.$emit('option-changed', {
                indexNum: this.fieldIndex, //need to check if, if it skips a field due to invisibility, does it use the right id?
                optionGuid: this.checklist //guids of option
            });
        },
    },

    template: `
        <div>
            <span v-for="(opt, index) in this.model.Options" >
                <input type="hidden"    :value=opt.Id          :name="fieldNameFor(fieldNamePrefix, index, 'Id')" />
                <input type="hidden"    :value=opt.ModelType   :name="fieldNameFor(fieldNamePrefix, index, 'ModelType')" />
                <div>
                    <input type="checkbox"   :value="opt.Id" v-model="checklist" @change="onOptionPicked" :name="fieldNameFor(fieldNamePrefix, index, 'Selected')" />
                    <span class='radio-option-label'>{{opt.OptionText.ConcatenatedContent}}</span>
                </div>
            </span>
        </div>`
})

Vue.component('Catfish.Core.Models.Contents.Fields.DateField', {
    props: ["model", "fieldNamePrefix", "fieldIndex"],

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
    props: ["model", "fieldNamePrefix", "fieldIndex"],

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
    props: ["model", "fieldNamePrefix", "fieldIndex"],

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
    props: ["model", "fieldNamePrefix", "fieldIndex"],

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
        onOptionPicked: function (e) {
            this.$emit('option-changed', {
                indexNum: this.fieldIndex, //need to check if, if it skips a field due to invisibility, does it use the right id?
                optionGuid: [e.target.value] //guid of option
            });
        },
    },

    template: `
        <div>
            <select :name=fieldNamePrefix :id=model.Id @change="onOptionPicked">
                <option v-for="(opt, index) in this.model.Options"
                    :value=opt.Id>
                        {{opt.OptionText.ConcatenatedContent}}
                </option>
            </select>
        </div>`
})

Vue.component('Catfish.Core.Models.Contents.Fields.TextArea', {
    props: ["model", "fieldNamePrefix", "fieldIndex"],

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
    props: ["model", "fieldNamePrefix", "fieldIndex"],

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
