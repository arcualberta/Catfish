<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "TextArea",
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
    });
</script>

<template>
    <div>
        <textarea v-model="model" placeholder="add multiple lines" required="{isRequired ? 'required' : ''}" class="form-control"></textarea>
    </div>
</template>

