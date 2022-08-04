
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

</script>

<template>
    <span v-for=" value in fieldData.multilingualTextValues" :key="value.id" :model="value">
        <TextCollection :model="value" :text-type="model.type" />
    </span>
    <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" style="margin-top: -45px; margin-left: 530px;" />
</template>

