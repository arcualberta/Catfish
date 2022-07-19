
<script setup lang="ts">
    import { Field, FieldTypes } from '../../shared/form-models';
    import { isOptionField } from '../../shared/form-helpers'
    import { default as TextCollection } from './TextCollection.vue'
    import { default as Opt } from './Option.vue'

    const props = defineProps<{ model: Field }>();
    const isAnOptionField = isOptionField(props.model);
</script>

<template>
    <h4>{{model.type}}</h4>
    <div>
        Title:
        <TextCollection v-if="model.title" :model="model.title" :text-type="FieldTypes.SingleLine" />
        <button v-else>Set name</button>
    </div>
    <div>
        Description:
        <TextCollection v-if="model.description" :model="model.description" :text-type="FieldTypes.Paragraph" />
        <button v-else>Set description</button>
    </div>
    <div v-if="isAnOptionField">
        Options:
        <Opt v-for="option in model.options" :key="option.id" :model="option" :option-type="model.type" />
    </div>
</template>

