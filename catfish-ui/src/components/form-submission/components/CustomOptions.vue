
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData);

    const deleteValue = (index: number) => {
        console.log(index)
        fieldData.value.customOptionValues?.splice(index, 1);
    }

    const addValue = () => {
        if (!fieldData.value.customOptionValues)
            fieldData.value.customOptionValues = [];

        fieldData.value.customOptionValues.push("")
    }

</script>

<template>
    <div>
        <span class="custom-option" v-for="(val, index) in fieldData.customOptionValues" :key="val.id">
            <input type="text" v-model="fieldData.customOptionValues[index]" />
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteValue(index)" class="fa-icon delete" />
        </span>
    </div>
    <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" />
</template>

