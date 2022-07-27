
<script setup lang="ts">
    import { Text, TextType } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';

    const props = defineProps<{ model: Text, textType: TextType, decimalPoints: number }>();
    const store = useFormSubmissionStore();

</script>

<template>
    textType{{textType}}
    <div v-if="textType===TextType.ShortAnswer">
        <input type="text" v-model="model.value" class="text-field" />
    </div>
    <div v-else-if="textType==='Paragraph'">
        <textarea v-model="model.value" class="field-text-area" />
    </div>
</template>

