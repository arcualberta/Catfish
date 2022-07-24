
<script setup lang="ts">
    import { ref } from 'vue'

    import { Option, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { useFormEditorStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Option, optionType: OptionFieldType }>();
    const store = useFormEditorStore();

    const editMode = ref(false)

</script>

<template>
    <div v-if="!editMode">
        <span v-for="value in model.optionText.values">
            <span v-if="value.value?.length > 0" class="option-values"> {{value.value}} </span>
        </span>
        <button @click="editMode = true">Edit</button>
    </div>
    <div v-else>
        <TextCollection :model="model.optionText" :text-type="FieldTypes.SingleLine" />
        <button @click="editMode = false">Done</button>
    </div>
</template>

