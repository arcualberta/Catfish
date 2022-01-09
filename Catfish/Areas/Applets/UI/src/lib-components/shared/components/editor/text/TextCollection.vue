<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { TextCollection } from '../../../models/textModels'
    import Text from './Text.vue'

    export default defineComponent({
        name: "TextCollection",
        props: {
            model: {
                type: null as PropType<TextCollection> | null,
                required: true
           },
			isRequired: {
				type: Boolean,
				required: false,
				default: false
			}
        },
        components: {
            Text
        }
    });
</script>

<template>
    <Text v-for="val in model.values" :model="val" :is-required="isRequired"/>
</template>

