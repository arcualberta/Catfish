
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
    <span v-if="!editMode">
        <span v-for="value in model.optionText.values">
            <span v-if="value.value?.length > 0" class="option-values"> {{value.value}} </span>
        </span>
        <font-awesome-icon icon="fa-solid fa-pen-to-square" @click="editMode = true" class="fa-icon" />
    </span>
    <span v-else>
        <TextCollection :model="model.optionText" :text-type="FieldTypes.ShortAnswer" />
        <font-awesome-icon icon="fa-solid fa-circle-check" @click="editMode = false" class="fa-icon delete-button"/>
    </span>
</template>

