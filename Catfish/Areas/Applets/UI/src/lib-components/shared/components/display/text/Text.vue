<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
           }
        },
        methods: {
            isUrl(text: string): boolean {
				return text?.startsWith("http://") || text?.startsWith("https://")
			}
		}
    });
</script>

<template>
    <div v-if="isUrl(model.value)"><a :href="model.value">{{model.value}}</a></div>
    <div v-else>{{model.value}}</div>
</template>

