<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    import { useStore } from 'vuex'
    import { MultilingualTextField } from '../../models/fieldContainer'
    import { validateMultilingualTextField } from '../../store/form-validators'
    import TextCollection from './text/TextCollection.vue'
    import { isRichTextField, isRequiredField } from '../../store/form-submission-utils'
    import { FlattenedFormFiledMutations } from '../../store/flattened-form-field-mutations'

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
            const store = useStore();
            const type = p.model.modelType;
			//console.log("p.model: ", JSON.stringify(p.model))

			const validationStatus = computed(() => validateMultilingualTextField(p.model))

            return {
                store,
                type,
                isRichText: computed(() => isRichTextField(p.model)),
                validationStatus,
                isRequiredField: computed(() => isRequiredField(p.model)),
            }
        },
        methods: {
            addText(store: any, field: MultilingualTextField) {
                store.commit(FlattenedFormFiledMutations.APPEND_MULTILINGUAL_VALUE, field);
            },
        }
    });
</script>

<template>
    <TextCollection v-for="val in model.values?.$values" :model="val" :is-multiline="isMultiline" :is-rich-text="isRichText" :validation-status="validationStatus" />
    <span class="fa fa-plus-circle" @click="addText(store, model)"></span>
</template>

