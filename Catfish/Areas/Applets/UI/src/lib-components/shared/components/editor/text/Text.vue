<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "Text",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
            },
            isRequired:
            {
                type: Boolean,
                required: false,
                default: false
            }
        }
        //methods: {
        //    isUrl(text: string): boolean {
        //        return text?.startsWith("http://") || text?.startsWith("https://")
        //    }
        //}
    });
</script>

<template>
    <input v-model="model" required="{isRequired ? 'required' : ''}" class="form-control" />
</template>

