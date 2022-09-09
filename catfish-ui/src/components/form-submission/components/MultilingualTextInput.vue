
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
    <div v-for="value, index in fieldData.multilingualTextValues" :key="value.id" :model="value" class="row mb-3">
        <div class="col col-sm-11" >
            <TextCollection :model="value" :text-type="model.type" />
        </div>
        <div class="col col-sm-1">
            <div v-if="fieldData.multilingualTextValues!.length > 1">
                <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteValue(index)" class="fa-icon delete" />
            </div>
        </div>
    </div>
    <div>
        <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" />
    </div>

</template>

