
<script setup lang="ts">
    import { Text, TextType } from '../../shared/form-models';
    import { useFormEditorStore } from '../store';

    const props = defineProps<{ model: Text, textType: TextType, dispLang: boolean }>();
    const store = useFormEditorStore();

</script>

<template>
    <div v-if="dispLang">
        <div v-if="textType==='SingleLine'">
            {{model.lang}}: <span />
            <input type="text" />
        </div>
        <div v-else-if="textType==='Paragraph'">
            {{model.lang}}: <span />
            <textarea /> 
        </div>
    </div>
</template>

