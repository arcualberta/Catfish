
Vue.component('data-item', {
    props: ["model"],

    data: function () {
        return {
            visibleFields: []
            //testObject:[
            //    {
            //    AllowMultipleValues: false,
            //    Created: "0001-01-01T00:00:00",
            //    CssClass: null,
            //    Description: {
            //            ConcatenatedContent: "",
            //            ConcatenatedRichText: "",
            //            Created: "0001-01-01T00:00:00",
            //            CssClass: null,
            //            Id: "9109991d-1949-4093-80bf-b9a2e8f4d231",
            //            ModelType: "Catfish.Core.Models.Contents.MultilingualDescription, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //            Updated: null,
            //            Values: []
            //        },
            //    DisplayLabel: "Choices",
            //    FieldCssClass: "",
            //    FieldLabelCssClass: "col-md-4",
            //    FieldValueCssClass: "col-md-8",
            //    Id: "f2e43c96-45be-4b54-944a-fec020e1ad79",
            //    ModelType: "Catfish.Core.Models.Contents.Fields.RadioField, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //    Name: {
            //        ConcatenatedContent: "Is there 2m (6.5 ft) of distance between all occupants?",
            //        ConcatenatedRichText: "",
            //        Created: "0001-01-01T00:00:00",
            //        CssClass: null,
            //        Id: "bb864056-161f-44d3-862f-e63e13a9e590",
            //        ModelType: "Catfish.Core.Models.Contents.MultilingualName, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //        Updated: null,
            //        Values: [
            //            {
            //                Created: "0001-01-01T00:00:00",
            //                CssClass: null,
            //                Format: "plain",
            //                Id: "7fcc5d1a-3d7b-4630-85f5-2718f68d2b62",
            //                Language: "en",
            //                ModelType: "Catfish.Core.Models.Contents.Text, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //                Rank: 0,
            //                Updated: null,
            //                Value: "Is there 2m (6.5 ft) of distance between all occupants?"
            //            }
            //        ]
            //    },
            //    Options: ["74595a6d-dae8-469b-9934-e1b608049aae", "0e98d938-bfdf-46e4-8c02-63f0583d205d", "84852f9f-59b2-4b8e-aef7-1c4f44b7b287"],
            //    Required: true,
            //    SelectedOptionGuids: null,
            //    Updated: null,
            //    VisibleIfOptionFieldId: null,
            //    VisibleIfOptionIds: [],
            //    VueComponent: "Catfish.Core.Models.Contents.Fields.RadioField"
            //    },
            //],
            //OptionText: {
            //    "74595a6d-dae8-469b-9934-e1b608049aae": {
            //            Created: "0001-01-01T00:00:00",
            //            CssClass: null,
            //            ExtendedOption: false,
            //            //Id: "74595a6d-dae8-469b-9934-e1b608049aae",
            //            ModelType: "Catfish.Core.Models.Contents.Fields.Option, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //            OptionText: {
            //                ConcatenatedContent: "Yes",
            //                ConcatenatedRichText: "",
            //                Created: "0001-01-01T00:00:00",
            //                CssClass: null,
            //                Id: "66195f43-b578-4ca4-88da-846e7db7c589",
            //                ModelType: "Catfish.Core.Models.Contents.MultilingualName, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //                Updated: null,
            //                Values: [
            //                    {
            //                        Created: "0001-01-01T00:00:00",
            //                        CssClass: null,
            //                        Format: "plain",
            //                        Id: "4278565e-11ee-417f-9bda-b18682173e4f",
            //                        Language: "en",
            //                        ModelType: "Catfish.Core.Models.Contents.Text, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //                        Rank: 0,
            //                        Updated: null,
            //                        Value: "Yes"
            //                    }
            //                ]
            //            },
            //            Selected: false,
            //            Updated: null,
            //    },
            //    "0e98d938-bfdf-46e4-8c02-63f0583d205d": {
            //        Created: "0001-01-01T00:00:00",
            //        CssClass: null,
            //        ExtendedOption: false,
            //        Id: "0e98d938-bfdf-46e4-8c02-63f0583d205d",
            //        ModelType: "Catfish.Core.Models.Contents.Fields.Option, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //        OptionText: {
            //            ConcatenatedContent: "No",
            //            ConcatenatedRichText: "",
            //            Created: "0001-01-01T00:00:00",
            //            CssClass: null,
            //            Id: "3727cefe-13d7-401c-8d3e-5a2440632cdb",
            //            ModelType: "Catfish.Core.Models.Contents.MultilingualName, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //            Updated: null,
            //            Values: [
            //                {
            //                    Created: "0001-01-01T00:00:00",
            //                    CssClass: null,
            //                    Format: "plain",
            //                    Id: "4977ba5c-1ca9-4eff-8ea5-7a08410f9ee6",
            //                    Language: "en",
            //                    ModelType: "Catfish.Core.Models.Contents.Text, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //                    Rank: 0,
            //                    Updated: null,
            //                    Value: "No"
            //                }
            //            ]
            //        },
            //        Selected: false,
            //        Updated: null
            //    },
            //    "84852f9f-59b2-4b8e-aef7-1c4f44b7b287": {
            //        Created: "0001-01-01T00:00:00",
            //        CssClass: null,
            //        ExtendedOption: false,
            //        Id: "84852f9f-59b2-4b8e-aef7-1c4f44b7b287",
            //        ModelType: "Catfish.Core.Models.Contents.Fields.Option, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //        OptionText: {
            //            ConcatenatedContent: "N/A",
            //            ConcatenatedRichText: "",
            //            Created: "0001-01-01T00:00:00",
            //            CssClass: null,
            //            Id: "cce7f780-6497-4777-b979-e6626e97dd4b",
            //            ModelType: "Catfish.Core.Models.Contents.MultilingualName, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //            Updated: null,
            //            Values: [{
            //                Created: "0001-01-01T00:00:00",
            //                CssClass: null,
            //                Format: "plain",
            //                Id: "7d1a35dc-8f62-4edf-a146-f36f42ae6b3c",
            //                Language: "en",
            //                ModelType: "Catfish.Core.Models.Contents.Text, Catfish.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            //                Rank: 0,
            //                Updated: null,
            //                Value: "N/A"
            //            }]
            //        },
            //        Selected: false
            //    }
            //},
            //deadSimpleArray: [0, 1, 2, 3, 4, 5],
            //deadSimpleObject: [{ "a": "Ahh", "b": "Bee", "c": "Cee", "SelectedOptionGuids": "Not set"}], //, 
            //theseOptions: 
            //    {
            //        0: "Zero",
            //        1: "One",
            //        2: "Two",
            //        3: "Three",
            //        4: "Four",
            //        5: "Five"
            //    }
            
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
        optionchanged: function (event) {
            console.log("received change", event, this.model);
            //if any others for this id are Selected = true, need to set them to false bc this is a radio button
            this.model.Fields[event.indexNum].Options.forEach((option) => {
                if (option.Id != event.radioGuid) {
                    option.Selected = false;
                } else {
                    option.Selected = true;
                }
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
            radioButtonPicked: null
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
                radioGuid: e.target.value //guid of option
            });
            //console.log("optiontext", this.options);
            //this.model.cssVal.value = e.target.value;

            //var content = this.model.cssVal.value;
            //if (content.length > 0) {
            //    this.$emit('update-content', {
            //        uid: this.uid,
            //        cssVal: content
            //    });
            //}

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
