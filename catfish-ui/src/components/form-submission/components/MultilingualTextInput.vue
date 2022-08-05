
<script setup lang="ts">
    import { computed } from 'vue'
    import { Field, FieldData } from '../../shared/form-models'
    import { useFormSubmissionStore } from '../store'
    import { createTextCollection } from '../../shared/form-helpers'
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const addValue = () => fieldData.value.multilingualTextValues?.push(createTextCollection(store.lang))

    const deleteValue = (index: number) => {
        console.log(index)
        fieldData.value.multilingualTextValues?.splice(index, 1);
    }
</script>

<template>
    <span v-for=" value in fieldData.multilingualTextValues" :key="value.id" :model="value">
        <TextCollection :model="value" :text-type="model.type" />
        <span v-if="fieldData.multilingualTextValues.length > 1" class="multilingual-field-delete"><font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteValue(value.id)" class="fa-icon delete" /></span>
        
    </span>
    <span style="margin-top: -45px; margin-left: 530px;"><font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option"  /></span>
</template>

