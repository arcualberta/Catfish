
<script setup lang="ts">
    import { Text, TextType } from '../../shared/form-models';
    import { useFormEditorStore } from '../store';

    const props = defineProps<{ model: Text, textType: TextType }>();
    const store = useFormEditorStore();

</script>

<template>
    <h6>Text</h6>
    {{model}}
    {{textType}}
</template>

