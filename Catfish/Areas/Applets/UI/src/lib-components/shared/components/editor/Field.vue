<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { Field, eFieldType, eFieldValidationStatus } from '../../models/fieldContainer'
    import { FieldContainerUtils } from '../../store/form-submission-utils'

	import AttachmentField from './AttachmentField.vue'
    import CheckboxField from './CheckboxField.vue'
	import DateField from './DateField.vue'
	import DecimalField from './DecimalField.vue'
	import EmailField from './EmailField.vue'
	import InfoField from './InfoField.vue'
	import IntegerField from './IntegerField.vue'
	import RadioField from './RadioField.vue'
	import SelectField from './SelectField.vue'
	import MultilingualTextField from './MultilingualTextField.vue'

    export default defineComponent({
        name: "Field",
        props: {
            model: {
                type: null as PropType<Field> | null,
                required: true
			},

        },
        components: {
			AttachmentField,
            CheckboxField,
			DateField,
			DecimalField,
			EmailField,
			//FieldContainerReference,
			InfoField,
			IntegerField,
			RadioField,
			SelectField,
			MultilingualTextField
		},
        setup(p) {
            const fieldType: eFieldType = FieldContainerUtils.getFieldType(p.model);
			const cssClass: string = FieldContainerUtils.cssClass(p.model);
			return {
				FieldTypes: eFieldType,
				ValidationStatus: eFieldValidationStatus,
                fieldType,
				cssClass
            }
        }
    });
</script>

<template>
	<!--<FieldContainerReference v-else-if="fieldType === eFieldType.FieldContainerReference" :model="model" :class="cssClass" />-->

	<div v-if="fieldType === FieldTypes.FieldContainerReference">
		TODO: Implement editor template for FieldContainerReference
	</div>
	<InfoField v-else-if="fieldType === FieldTypes.InfoSection" :model="model" :class="cssClass" />
	<div v-else :class="cssClass + ' row'">
		<div class="col-md-3 field-name">
			{{model.name.concatenatedContent}} <span v-if="this.model.required" style="color:red">*</span>
		</div>
		<div class="col-md-9 field-value">
			<AttachmentField v-if="fieldType === FieldTypes.AttachmentField" :model="model" />
			<CheckboxField v-else-if="fieldType === FieldTypes.CheckboxField" :model="model" />
			<DateField v-else-if="fieldType === FieldTypes.DateField" :model="model" />
			<DecimalField v-else-if="fieldType === FieldTypes.DecimalField" :model="model" />
			<EmailField v-else-if="fieldType === FieldTypes.EmailField" :model="model" />
			<IntegerField v-else-if="fieldType === FieldTypes.IntegerField" :model="model" />
			<RadioField v-else-if="fieldType === FieldTypes.RadioField" :model="model" />
			<SelectField v-else-if="fieldType === FieldTypes.SelectField" :model="model" />
			<MultilingualTextField v-else-if="fieldType === FieldTypes.TextArea" :model="model" :is-multiline="true" />
			<MultilingualTextField v-else-if="fieldType === FieldTypes.TextField" :model="model" :is-multiline="false" />

			<div v-else-if="fieldType === FieldTypes.CompositeField"> TODO: Implement editor template for the CompositeField</div>
			<div v-else-if="fieldType === FieldTypes.TableField"> TODO: Implement editor template for the TableField</div>

			<div v-if="model?.validationStatus === ValidationStatus.VALUE_REQUIRED" style="color:red">Field value is required.</div>
			<div v-if="model?.validationStatus === ValidationStatus.VALUE_INVALID" style="color:red">Field value is invalid.</div>


		</div>
	</div>
</template>

