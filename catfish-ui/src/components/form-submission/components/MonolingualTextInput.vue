
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, OptionFieldType, FieldData, TextType } from '../../shared/form-models';
    import { createText } from '../../shared/form-helpers'
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import { default as Text } from './Text.vue'
    const props = defineProps<{ model: Field, textType: TextType }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const addValue = () => fieldData.value.monolingualTextValues?.push(createText(null))

</script>

<template>
    <span v-for="value in fieldData.monolingualTextValues" :key="value.id" :model="value">
        <Text :model="value" :text-type="model.type"/>
        <br />
        <br />
    </span>
    
        <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" style="margin-top: -45px; margin-left: 570px;"/>
    
    
</template>

