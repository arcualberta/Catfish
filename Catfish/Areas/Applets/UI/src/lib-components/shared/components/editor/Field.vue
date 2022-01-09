<script lang="ts">
    import { defineComponent, PropType } from 'vue'
	import { Field, eFieldType } from '../../models/fieldContainer'
    import { FieldContainerUtils } from '../../store/form-submission-utils'

	import AttachmentField from './AttachmentField.vue'
    import CheckboxField from './CheckboxField.vue'
	import DateField from './DateField.vue'
	import DecimalField from './DecimalField.vue'
	import EmailField from './EmailField.vue'
	//import FieldContainerReference from './FieldContainerReference.vue'
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
				eFieldType,
                fieldType,
                isRequired,
				cssClass
            }
        }
    });
</script>

<template>
    <InfoField v-if="fieldType === eFieldType.InfoSection" :model="model" :class="cssClass" />
    <!--<FieldContainerReference v-else-if="fieldType === eFieldType.FieldContainerReference" :model="model" :class="cssClass" />-->
	<div v-else :class="cssClass + ' row'">
		<div class="col-md-3 field-name">
			{{model.name.concatenatedContent}}
		</div>
		<div class="col-md-9 field-value">
			<AttachmentField v-if="fieldType === eFieldType.AttachmentField" :model="model" />
			<CheckboxField v-else-if="fieldType === eFieldType.CheckboxField" :model="model" />
			<DateField v-else-if="fieldType === eFieldType.DateField" :model="model" />
			<DecimalField v-else-if="fieldType === eFieldType.DecimalField" :model="model" />
			<EmailField v-else-if="fieldType === eFieldType.EmailField" :model="model" />
			<IntegerField v-else-if="fieldType === eFieldType.IntegerField" :model="model" />
			<RadioField v-else-if="fieldType === eFieldType.RadioField" :model="model" />
			<SelectField v-else-if="fieldType === eFieldType.SelectField" :model="model" />
			<TextField v-else-if="fieldType === eFieldType.TextArea" :model="model" />
			<TextField v-else-if="fieldType === eFieldType.TextField" :model="model" />

			<div v-else-if="fieldType === eFieldType.CompositeField"> TODO: CompositeField</div>
			<div v-else-if="fieldType === eFieldType.TableField"> TODO: TableField</div>
			<div v-else-if="fieldType === eFieldType.MonolingualTextField"> TODO: Check if we really need an editor template for MonolingualTextField</div>
		</div>

		<!--<div v-switch="fieldType" class="col-md-9 field-value">
		<AttachmentField v-case="eFieldType.AttachmentField" :model="field" />
		<CheckboxField v-case="eFieldType.CheckboxField" :model="field" />
		<DateField v-case="eFieldType.DateField" :model="field" />
		<DecimalField v-case="eFieldType.DecimalField" :model="field" />
		<EmailField v-case="eFieldType.EmailField" :model="field" />
		<IntegerField v-case="eFieldType.IntegerField" :model="field" />
		<RadioField v-case="eFieldType.RadioField" :model="field" />
		<SelectField v-case="eFieldType.SelectField" :model="field" />
		<TextField v-case="eFieldType.TextArea" :model="field" />
		<TextField v-case="eFieldType.TextField" :model="field" />

		<div v-case="eFieldType.CompositeField"> TODO: CompositeField</div>
		<div v-case="eFieldType.TableField"> TODO: TableField</div>
		<div v-case="eFieldType.MonolingualTextField"> TODO: Check if we really need an editor template for MonolingualTextField</div>
	</div>-->
	</div>
</template>

