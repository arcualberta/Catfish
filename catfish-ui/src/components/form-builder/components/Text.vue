
<script setup lang="ts">
    import { ref } from 'vue';
    import { Text, TextType } from '../../shared/form-models';
    import { useFormEditorStore } from '../store';

    const props = defineProps<{ model: Text, textType: TextType, dispLang: boolean }>();
    const store = useFormEditorStore();

</script>

<template>
    <div v-if="dispLang">
        <div v-if="textType===TextType.ShortAnswer">
            <span class="text-field-lable">{{model.lang}}: </span>
            <input type="text" v-model="model.value" class="text-field" />
        </div>
        <div v-else-if="textType==='Paragraph'">
            <span class="text-field-lable">{{model.lang}}: </span>
            <textarea v-model="model.value" class="field-text-area" />
        </div>
    </div>
    <div v-else>
        <div v-if="textType===TextType.ShortAnswer">
            <input type="text" v-model="model.value" class="text-field" />
        </div>
        <div v-else-if="textType==='Paragraph'">
            <textarea v-model="model.value" class="field-text-area" />
        </div>
    </div>
</template>

