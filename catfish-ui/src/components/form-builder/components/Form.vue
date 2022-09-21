
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
        <b-row>
            <b-col class="col-sm-2">
                <h6 >Name:</h6>
            </b-col>
            <b-col class="col-sm-10">
                <b-form-input v-model="model.name"></b-form-input>
            </b-col>
        </b-row>
        <br />
        <b-row>
            <b-col class="col-sm-2">
                <h6 >Description:</h6>
            </b-col>
            <b-col class="col-sm-10">
                <b-form-textarea v-model="model.description" rows="3" max-rows="6"></b-form-textarea>
            </b-col>
        </b-row>
    </div>

    <h3>Fields</h3>
    <draggable class="dragArea list-group w-full" :list="model?.fields">
        <div v-for="field in model?.fields" :key="field.id" class="form-field-border form-field">
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteField(field.id)" class="fa-icon field-delete" />
            <Field :model="field" />
        </div>
    </draggable>
</template>

