
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, OptionFieldType, FieldTypes } from '../../shared/form-models';
    import { createText } from '../../shared/form-helpers'
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const addValue = () => fieldData.value.monolingualTextValues?.push(createText(null))

</script>

<template>
    <div>
        {{fieldData}}
    </div>
    <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" />
</template>

