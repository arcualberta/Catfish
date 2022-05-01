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
           }
        },
        components: {
            Text
        }
    });
</script>

<template>
    <div v-for="txt in model.values.$values">
        <Text :model="txt" />
    </div>
</template>

