<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { Field, eFieldType } from '../../models/fieldContainer'
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
	import TextField from './TextField.vue'

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
			TextField,
		},
        setup(p) {
            const fieldType: eFieldType = FieldContainerUtils.getFieldType(p.model);
            const isRequired: boolean = p.model.required;
			const cssClass: string = FieldContainerUtils.cssClass(p.model);

			return {
				FieldTypes: eFieldType,
                fieldType,
                isRequired,
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
			{{model.name.concatenatedContent}}
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
			<TextField v-else-if="fieldType === FieldTypes.TextArea" :model="model" />
			<TextField v-else-if="fieldType === FieldTypes.TextField" :model="model" />

			<div v-else-if="fieldType === FieldTypes.CompositeField"> TODO: Implement editor template for the CompositeField</div>
			<div v-else-if="fieldType === FieldTypes.TableField"> TODO: Implement editor template for the TableField</div>
			<div v-else-if="fieldType === FieldTypes.MonolingualTextField"> TODO: Check if we really need an editor template for MonolingualTextField</div>
		</div>
	</div>
</template>

