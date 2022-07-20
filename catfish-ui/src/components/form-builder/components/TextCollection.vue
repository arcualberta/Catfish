<script setup lang="ts">
    import { TextCollection, TextType } from '../../shared/form-models';
    import { default as Text } from './Text.vue'

    const props = defineProps<{ model: TextCollection, textType: TextType }>();

</script>

<template>
    <h5>Text Collection</h5>
    <Text v-for="text in model.values" :key="text.id" :model="text" :text-type="textType" :disp-lang="model.values?.length > 1" />
</template>

