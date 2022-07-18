<script lang="ts">
    import { defineComponent } from "vue";
    import { useFormEditorStore } from '../store';

    export default defineComponent({
        name: "Text",
    });
</script>

<script setup lang="ts">
    import { Text, TextType } from '../../shared/form-models';

    const props = defineProps<{ model: Text, textType: TextType }>();
    const store = useFormEditorStore();

</script>

<template>
    <h6>Text</h6>
    {{model}}
    {{textType}}
</template>

