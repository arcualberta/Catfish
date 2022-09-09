
<script setup lang="ts">
    import { Guid } from "guid-typescript";
    import { VueDraggableNext as draggable } from 'vue-draggable-next'

    import { Form } from '../../shared/form-models';
    import { getFieldTitle } from '../../shared/form-helpers';
    import { default as Field } from './Field.vue'
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Form }>();
    const deleteField = (optId: Guid) => {
        const idx = props.model.fields?.findIndex(opt => opt.id == optId)
        props.model.fields?.splice(idx as number, 1)
    }
</script>

<template>
    <div>
        <h4>Form properties</h4>
    </div>
    <div class="form-field-border">
        <div>
            <span class="text-field-lable">Name:</span>
            <input type="text" v-model="model.name" class="text-field" />
        </div>
        <div style="display:inline;">
            <span class="text-area-lable">Description:</span>
            <textarea v-model="model.description" class="text-area" />
        </div>
    </div>

    <h3>Fields</h3>
    <draggable class="dragArea list-group w-full" :list="model?.fields">
        <div v-for="field in model?.fields" :key="field.id" class="form-field-border form-field">
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteField(field.id)" class="fa-icon field-delete" />
            <Field :model="field" />
        </div>
    </draggable>
</template>

