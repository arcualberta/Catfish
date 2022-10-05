
<script setup lang="ts">
    import { Guid } from "guid-typescript";
    import { Entity } from "../../entity-editor/models";
    import { getFieldData } from "../../shared/entity-helpers"
    import { FieldEntry, Form } from '../../shared/form-models';
    import { default as Field } from './Field.vue'

    const props = defineProps<{
        model: Form,
        entity: Entity
    }>();
    const getFieldDataModel = (fieldId: Guid) => 
        getFieldData(props.entity, { formId: props.model.id, fieldId } as FieldEntry)
</script>

<template>
    <Field v-for="field in model?.fields" :key="field.id" :model="field" :modelData="getFieldDataModel(field.id)" />
</template>

