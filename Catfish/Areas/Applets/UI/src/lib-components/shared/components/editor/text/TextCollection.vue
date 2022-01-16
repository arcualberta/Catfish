<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { TextCollection } from '../../../models/textModels'
	import { eValidationStatus } from '../../../models/fieldContainer'
    import Text from './Text.vue'

    export default defineComponent({
        name: "TextCollection",
        props: {
            model: {
                type: null as PropType<TextCollection> | null,
                required: true
			},
			isMultiline: {
				type: Boolean,
				required: true
			},
			isRichText: {
				type: Boolean,
				required: true
			},
			validationStatus: {
				type: null as PropType<eValidationStatus> | null,
				required: true
            }
        },
        components: {
            Text
        }
    });
</script>

<template>
    <div v-for="val in model?.values?.$values">
        <span v-if="model?.values?.$values?.length > 1">{{val.language}}: </span>
        <Text :model="val" :is-multiline="isMultiline" :is-rich-text="isRichText" :validation-status="validationStatus" />
    </div>
<!--    <Text v-for="val in model?.values?.$values" :model="val" :is-multiline="isMultiline" :is-rich-text="isRichText" :validation-status="validationStatus" />-->
</template>

