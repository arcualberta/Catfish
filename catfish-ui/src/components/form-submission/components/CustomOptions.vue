
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, FieldData } from '../../shared/form-models';
    import { useFormSubmissionStore } from '../store';

    const props = defineProps<{ model: Field,
                                modelData?: FieldData | null }>();
    const store = useFormSubmissionStore();

     const fieldData = computed(() => props.modelData? props.modelData :
                 store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

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
        <span class="custom-option" v-for="(_, index) in fieldData.customOptionValues" :key="index">
            <input type="text" v-model="fieldData.customOptionValues![index]" />
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteValue(index)" class="fa-icon delete" />
        </span>
    </div>
    <div class="col-sm-2">
        <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" />
    </div>
    
</template>

