
<script setup lang="ts">
    import {default as FormTemplate } from "@/components/shared/form-models/formTemplate";
    import { Guid } from "guid-typescript";
    import { computed } from "vue";
    import { EntityData } from "../../entity-editor/models";
    import { getFieldData, getFieldDataFromFormData } from "../../shared/entity-helpers"
    import { FieldEntry, FieldData} from '../../shared/form-models';
    import { FormData } from '../../shared/form-models';
    import { default as Field } from './Field.vue'

    const props = defineProps<{
        model: FormTemplate,
        entityData?: EntityData,
        formData?: FormData
    }>();
    
    const getFieldDataModel = (fieldId: Guid): FieldData | undefined => {
        if(props.entityData)
            return getFieldData(props.entityData, { formId: props.model.id, fieldId } as FieldEntry)
        else if(props.formData)
            return getFieldDataFromFormData(props.formData, fieldId)
        else
            return undefined
    }
</script>

<template>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" :modelData="getFieldDataModel(field.id)" />
</template>

