
<script setup lang="ts">
    import { computed } from 'vue'

    import { Field, OptionFieldType, FieldData, TextType } from '../../shared/form-models';
    import { createText } from '../../shared/form-helpers'
    import { useFormSubmissionStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'
    import { default as Text } from './Text.vue'
    const props = defineProps<{ model: Field }>();
    const store = useFormSubmissionStore();

    const fieldData = computed(() => store.formData.fieldData?.find(fd => fd.fieldId == props.model.id) as FieldData)

    const addValue = () => fieldData.value.monolingualTextValues?.push(createText(null))
    const deleteValue = (index: number) => {
        fieldData.value.monolingualTextValues?.splice(index, 1);
    }
</script>

<template>
    <div v-for="value in fieldData.monolingualTextValues" :key="value.id" :model="value" class="col-sm-8">
        <Text :model="value" :text-type="model.type" />
        <div v-if="fieldData.monolingualTextValues.length > 1" class="col-sm-1">
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteValue(value.id)" class="fa-icon delete" />
        </div>
    </div>
    <div class="col-sm-1">
        <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addValue()" class="fa-icon plus add-option" />
    </div>
    {{fieldData}}

</template>

