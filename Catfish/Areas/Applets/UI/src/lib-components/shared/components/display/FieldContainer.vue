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
            ReferenceField
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
            isTextField(field: Field): boolean { return this.getFieldType(field) === eFieldType.TextField }
        }
    });
</script>

<template>
    <div v-for="field in model.fields" class="row">
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
            <ReferenceField v-if="this.isFieldContainerReference(field)" :model="field" />
            <div v-if="this.isAttachmentField(model)">
                AttachmentField
            </div>

            <div v-if="this.isCompositeField(model)">
                CompositeField
            </div>
            <!--<div v-if="this.isDateField(model)">
        DateField
    </div>-->
            <!--<div v-if="this.isDecimalField(model)">
        DecimalField
    </div>
    <div v-if="this.isFieldContainerReference(model)">
        FieldContainerReference
    </div>-->
            <div v-if="this.isInfoSection(model)">
                InfoSection
            </div>
            <!--<div v-if="this.isIntegerField(model)">
        IntegerField
    </div>-->
            <!--<div v-if="this.isMonolingualTextField(model)">
        MonolingualTextField
    </div>-->
            <TextField :model="field" v-if="this.isMonolingualTextField(model)" />
            <div v-if="this.isTableField(model)">
                TableField
            </div>
        </div>

    </div>
</template>