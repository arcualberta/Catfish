<script lang="ts">
	import { defineComponent, PropType } from 'vue'
    import { useStore } from 'vuex';
    import { Field, eFieldType, eValidationStatus, MonolingualTextField  } from '../../models/fieldContainer'
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
	import AudioRecorderField from './AudioRecorderField.vue'
	import { FlattenedFormFiledMutations } from '../../store/flattened-form-field-mutations'
    

    export default defineComponent({
        name: "FieldBase",
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
			InfoField,
			IntegerField,
			RadioField,
			SelectField,
			MultilingualTextField,
            AudioRecorderField
		},
		setup(p) {
            const store = useStore();
            const fieldType: eFieldType = FieldContainerUtils.getFieldType(p.model);
			const cssClass: string = FieldContainerUtils.cssClass(p.model);

			var isMonolingualField = false;
			if (fieldType === eFieldType.EmailField || fieldType === eFieldType.DateField || fieldType === eFieldType.DecimalField || fieldType === eFieldType.IntegerField || fieldType === eFieldType.MonolingualTextField)
				isMonolingualField = true;
			return {
				store,
				FieldTypes: eFieldType,
				ValidationStatus: eValidationStatus,
                fieldType,
				cssClass,
				isMonolingualField
            }
		},
        methods: {
            addMonoLingualField(store: any, field: MonolingualTextField) {
                store.commit(FlattenedFormFiledMutations.APPEND_MONOLINGUAL_VALUE, field);
            },
        }
    });
</script>

<template>
	<InfoField v-if="fieldType === FieldTypes.InfoSection" :model="model" :class="cssClass" />
	<div v-else :class="cssClass + ' row'">
		<div class="col-md-3 field-name">
			{{model.name.concatenatedContent}} <span v-if="this.model.required" style="color:red">*</span>
		</div>
		<div class="col-md-9 field-value">
			<div v-if="model?.validationStatus === ValidationStatus.VALUE_REQUIRED" style="color:red">This field requires a value.</div>
			<div v-if="model?.validationStatus === ValidationStatus.INVALID" style="color:red">This field has an invalid value.</div>

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
			<AudioRecorderField v-else-if="fieldType === FieldTypes.AudioRecorderField" :model="model" />
			<div v-else-if="fieldType === FieldTypes.CompositeField"> TODO: Implement editor template for the CompositeField</div>
			<div v-else-if="fieldType === FieldTypes.TableField"> TODO: Implement editor template for the TableField</div>
			<span v-if="isMonolingualField === true" class="fa fa-plus-circle" @click="addMonoLingualField(store, model)" ></span>
		</div>
	</div>
</template>

