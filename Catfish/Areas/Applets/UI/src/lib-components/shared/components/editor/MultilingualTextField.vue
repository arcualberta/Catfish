<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    import { MultilingualTextField } from '../../models/fieldContainer'
    import { validateMultilingualTextField } from '../../store/form-validators'
    import TextCollection from './text/TextCollection.vue'
    import { isRichTextField, isRequiredField } from '../../store/form-submission-utils'

    export default defineComponent({
		name: "MultilingualTextField",
        props: {
            model: {
                type: null as PropType<MultilingualTextField> | null,
                required: true
            },
			isMultiline: {
				type: Boolean,
				required: true
			}
        },
        components: {
            TextCollection
        },
        setup(p) {

            const type = p.model.modelType;
			//console.log("p.model: ", JSON.stringify(p.model))

			const validationStatus = computed(() => validateMultilingualTextField(p.model))

            return {
                type,
                isRichText: computed(() => isRichTextField(p.model)),
                validationStatus,
                isRequiredField: computed(() => isRequiredField(p.model)),
            }
        }
    });
</script>

<template>
    <!--  <div>IsRequired: {{isRequiredField}}</div>
      <div>{{JSON.stringify(model)}}</div>-->
    <TextCollection v-for="val in model.values?.$values" :model="val" :is-multiline="isMultiline" :is-rich-text="isRichText" :validation-status="validationStatus" />
    <!--<div>{{type
        }} : {{JSON.stringify(model)}}
    </div>-->
    <!--<label>{{model.name.concatenatedContent}} <span v-if="model.required" class="requiredField"></span></label>
    <div v-if="type.includes('Catfish.Core.Models.Contents.Fields.TextArea')">

        <RichText v-if="model.richText" v-for="val in model.values" :model="val" :key="val.id" :isRequired="model.required" />
        <TextArea v-else v-for="val in model.values" :model="val" :key="val.id" :isRequired="model.required" />
    </div>
    <div v-else>
      <TextInput  v-for="val in model.values" :model="val"  :key="val.id" :isRequired="model.required" />
    </div>-->


</template>

