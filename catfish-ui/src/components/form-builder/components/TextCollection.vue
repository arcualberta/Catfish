<script lang="ts">import { defineComponent } from "vue";

    import { default as Text } from './Text.vue'

    export default defineComponent({
        name: "TextCollection",
        components: {
            Text
        }
    });
 </script>

<script setup lang="ts">
    import { TextCollection, TextType } from '../../shared/form-models';

    const props = defineProps<{ model: TextCollection, textType: TextType }>();

</script>

<template>
    <h5>Text Collection</h5>
    {{model.id}}
    <Text v-for="text in model.values" :key="txt.id" :model="txt" :text-type="textType" />
</template>

