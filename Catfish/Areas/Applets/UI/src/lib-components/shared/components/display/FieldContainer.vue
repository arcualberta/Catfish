<script lang="ts">
    import { defineComponent, PropType } from 'vue'

    import { Field, FieldContainer, eFieldType } from '../../models/fieldContainer'
    import TextField from './TextField.vue'
    import EmailField from './EmailField.vue'
    import OptionField from './OptionField.vue'

    export default defineComponent({
        name: "FieldContainerView",
        props: {
            model: null as PropType<FieldContainer> | null,
        },
        components: {
            TextField,
            EmailField,
            OptionField
        },
        methods: {
            getFieldType(field: Field): eFieldType {
                var typeName: string = field?.modelType.substring(0, field.modelType.indexOf(","));
                typeName = typeName?.substring(typeName.lastIndexOf(".") + 1);
                return (<any>eFieldType)[typeName];
            },
            isAttachmentField(field: Field): boolean { return this.getFieldType(field) === eFieldType.AttachmentField },
            isCheckboxField(field: Field): boolean { return this.getFieldType(field) === eFieldType.CheckboxField },
            isCompositeField(field: Field): boolean { return this.getFieldType(field) === eFieldType.CompositeField },
            isDateField(field: Field): boolean { return this.getFieldType(field) === eFieldType.DateField },
            isDecimalField(field: Field): boolean { return this.getFieldType(field) === eFieldType.DecimalField },
            isEmailField(field: Field): boolean { return this.getFieldType(field) === eFieldType.EmailField },
            isFieldContainerReference(field: Field): boolean { return this.getFieldType(field) === eFieldType.FieldContainerReference },
            isInfoSection(field: Field): boolean { return this.getFieldType(field) === eFieldType.InfoSection },
            isIntegerField(field: Field): boolean { return this.getFieldType(field) === eFieldType.IntegerField },
            isMonolingualTextField(field: Field): boolean { return this.getFieldType(field) === eFieldType.MonolingualTextField },
            isRadioField(field: Field): boolean { return this.getFieldType(field) === eFieldType.RadioField },
            isSelectField(field: Field): boolean { return this.getFieldType(field) === eFieldType.SelectField },
            isTableField(field: Field): boolean { return this.getFieldType(field) === eFieldType.TableField },
            isTextArea(field: Field): boolean { return this.getFieldType(field) === eFieldType.TextArea },
            isTextField(field: Field): boolean { return this.getFieldType(field) === eFieldType.TextField }
        }
    });
</script>

<template>
    <h4>Field Container View</h4>
    <h4>{{model.name.concatenatedContent}}</h4>

    <div v-for="field in model.fields" class="row">
        <div class="field-name col-md-3">
            {{field.name.concatenatedContent}}
        </div>

        <TextField :model="field" v-if="this.isTextField(field)" />
        <EmailField :model="field" v-if="this.isEmailField(field)" />

        <div v-if="this.isAttachmentField(model)">
            AttachmentField
        </div>
        <!--<div v-if="this.isCheckboxField(model)">
            CheckboxField
        </div>-->
        <OptionField v-if="this.isCheckboxField(field)" :model="field"  />

        <div v-if="this.isCompositeField(model)">
            CompositeField
        </div>
        <div v-if="this.isDateField(model)">
            DateField
        </div>
        <div v-if="this.isDecimalField(model)">
            DecimalField
        </div>
        <!--<div v-if="this.isEmailField(model)">
            EmailField
        </div>-->
        <div v-if="this.isFieldContainerReference(model)">
            FieldContainerReference
        </div>
        <div v-if="this.isInfoSection(model)">
            InfoSection
        </div>
        <div v-if="this.isIntegerField(model)">
            IntegerField
        </div>
        <!--<div v-if="this.isMonolingualTextField(model)">
            MonolingualTextField
        </div>-->
        <TextField :model="field" v-if="this.isMonolingualTextField(model)" />
        <div v-if="this.isRadioField(model)">
            RadioField
        </div>
        <div v-if="this.isSelectField(model)">
            SelectField
        </div>
        <div v-if="this.isTableField(model)">
            TableField
        </div>
        <!--<div v-if="this.isTextArea(model)">
        TextArea
    </div>-->
        <TextField :model="field" v-if="this.isTextArea(field)" />

    </div>
</template>