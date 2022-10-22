
<script setup lang="ts">
    import {default as FormTemplate } from "@/components/shared/form-models/formTemplate";
    import { Guid } from "guid-typescript";
    import { computed } from "vue";
    import { EntityData } from "../../entity-editor/models";
    import { getFieldData } from "../../shared/entity-helpers"
    import { FieldEntry} from '../../shared/form-models';
    import { default as Field } from './Field.vue'

    const props = defineProps<{
        model: FormTemplate,
        entity: EntityData
    }>();
    //const formTemplateId: Guid= computed(()=>props.model.id);
    const getFieldDataModel = (fieldId: Guid) => 
        getFieldData(props.entity, { formId: props.model.id, fieldId } as FieldEntry)
</script>

<template>
    <Field v-for="field in model?.fields" :key="field.id" :model="field"  :modelData="getFieldDataModel(field.id)" />
</template>

