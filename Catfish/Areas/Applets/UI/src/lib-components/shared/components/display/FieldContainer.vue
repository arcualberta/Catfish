<script lang="ts">
    import { defineComponent, PropType } from 'vue'

    import { Field, FieldContainer, eFieldType } from '../../models/fieldContainer'
    import TextField from './TextField.vue'
    import EmailField from './EmailField.vue'
	import OptionsField from './OptionsField.vue'
    import DecimalField from './DecimalField.vue'
    import IntegerField from './IntegerField.vue'
    import DateField from './DateField.vue'
    import ReferenceField from './FieldContainerReference.vue'
    import AttachmentField from './AttachmentField.vue'

    export default defineComponent({
		name: "FieldContainer",
        props: {
            model: null as PropType<FieldContainer> | null,
        },
        components: {
            TextField,
            EmailField,
            OptionsField,
            DecimalField,
            IntegerField,
            DateField,
            ReferenceField,
            AttachmentField
        },
        methods: {
            getFieldType(field: Field): eFieldType {
                var typeName: string = field?.modelType.substring(0, field.modelType.indexOf(","));
                typeName = typeName?.substring(typeName.lastIndexOf(".") + 1);
                return (<any>eFieldType)[typeName];
            },
            isAttachmentField(field: Field): boolean { return this.getFieldType(field) === eFieldType.AttachmentField },
            isOptionsField(field: Field): boolean { return this.getFieldType(field) === eFieldType.CheckboxField || this.getFieldType(field) === eFieldType.RadioField || this.getFieldType(field) === eFieldType.SelectField },
            isCompositeField(field: Field): boolean { return this.getFieldType(field) === eFieldType.CompositeField },
            isDateField(field: Field): boolean { return this.getFieldType(field) === eFieldType.DateField },
            isDecimalField(field: Field): boolean { return this.getFieldType(field) === eFieldType.DecimalField },
            isEmailField(field: Field): boolean { return this.getFieldType(field) === eFieldType.EmailField },
            isFieldContainerReference(field: Field): boolean { return this.getFieldType(field) === eFieldType.FieldContainerReference },
            isInfoSection(field: Field): boolean { return this.getFieldType(field) === eFieldType.InfoSection },
            isIntegerField(field: Field): boolean { return this.getFieldType(field) === eFieldType.IntegerField },
            isMonolingualTextField(field: Field): boolean { return this.getFieldType(field) === eFieldType.MonolingualTextField },
            isTableField(field: Field): boolean { return this.getFieldType(field) === eFieldType.TableField },
            isTextArea(field: Field): boolean { return this.getFieldType(field) === eFieldType.TextArea },
            isTextField(field: Field): boolean { return this.getFieldType(field) === eFieldType.TextField },
            cssClass(field: Field): string { return field.cssClass + " " + field.fieldCssClass; }
        }
    });
</script>

<template>
    <div v-for="field in model.fields">
        <div v-if="this.isFieldContainerReference(field)" :class="cssClass(field)">
            <ReferenceField :model="field" />
        </div>
        <div v-else class="row" :class="cssClass(field)">
            <div class="field-name col-md-3">
                {{field.name.concatenatedContent}}
            </div>
            <div class="field-value col-md-9">
                <TextField :model="field" v-if="this.isTextField(field)" />
                <TextField :model="field" v-if="this.isTextArea(field)" />
                <EmailField :model="field" v-if="this.isEmailField(field)" />
                <OptionsField v-if="this.isOptionsField(field)" :model="field" />
                <DecimalField v-if="this.isDecimalField(field)" :model="field" />
                <IntegerField v-if="this.isIntegerField(field)" :model="field" />
                <DateField v-if="this.isDateField(field)" :model="field" />
                <TextField :model="field" v-if="this.isMonolingualTextField(model)" />
                <AttachmentField v-if="this.isAttachmentField(field)" :model="field" />
                
                <div v-if="this.isCompositeField(field)">
                    CompositeField
                </div>
                <div v-if="this.isInfoSection(field)">
                    InfoSection
                </div>
                <div v-if="this.isTableField(field)">
                    TableField
                </div>
            </div>
        </div>
    </div>
</template>