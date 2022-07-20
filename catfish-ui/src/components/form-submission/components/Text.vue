
<script setup lang="ts">
    import { Text, TextType } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';

    const props = defineProps<{ model: Text, textType: TextType }>();
    const store = useFormSubmissionStore();

</script>

<template>
    <h6>Text</h6>
    {{model}}
    {{textType}}
</template>

