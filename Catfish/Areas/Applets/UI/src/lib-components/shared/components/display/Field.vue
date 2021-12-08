<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Field, eFieldType } from '../../models/fieldContainer'


    export default defineComponent({
        name: "Field",
        props: {
            model: null as PropType<Field> | null
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
    <div class="field-name col-md-3">
        {{model.name.concatenatedContent}}
    </div>
    <div class="col-md-9">
        <div v-if="this.isAttachmentField(model)">
            AttachmentField
        </div>
        <div v-if="this.isCheckboxField(model)">
            CheckboxField
        </div>
        <div v-if="this.isCompositeField(model)">
            CompositeField
        </div>
        <div v-if="this.isDateField(model)">
            DateField
        </div>
        <div v-if="this.isDecimalField(model)">
            DecimalField
        </div>
        <div v-if="this.isEmailField(model)">
            EmailField
        </div>
        <div v-if="this.isFieldContainerReference(model)">
            FieldContainerReference
        </div>
        <div v-if="this.isInfoSection(model)">
            InfoSection
        </div>
        <div v-if="this.isIntegerField(model)">
            IntegerField
        </div>
        <div v-if="this.isMonolingualTextField(model)">
            MonolingualTextField
        </div>
        <div v-if="this.isRadioField(model)">
            RadioField
        </div>
        <div v-if="this.isSelectField(model)">
            SelectField
        </div>
        <div v-if="this.isTableField(model)">
            TableField
        </div>
        <div v-if="this.isTextArea(model)">
            TextArea
        </div>
        <div v-if="this.isTextField(model)">
            TextField
        </div>
        <div>
            {{JSON.stringify(model)}}
        </div>
    </div>
</template>

