
<script setup lang="ts">
    import { Text, FieldType, FieldTypes } from '../../shared/form-models';

    const props = defineProps<{ model: Text, textType: FieldType, decimalPoints?: number }>();

    const decPoints = props.decimalPoints ? props.decimalPoints : 2;

</script>

<template>
    <span v-if="textType === FieldTypes.ShortAnswer">
        <input type="text" v-model="model.value" class="text-field" />
    </span>
    <span v-else-if="textType === FieldTypes.Paragraph">
        <textarea v-model="model.value" class="field-text-area" />
    </span>
    <span v-else-if="textType === FieldTypes.RichText">
        <!--TODO: render a proper rich-text editor here -->
        <textarea v-model="model.value" class="field-text-area" />
    </span>
    <span v-if="textType === FieldTypes.Email">
        <input type="email" v-model="model.value" class="text-field" />
    </span>
    <span v-if="textType === FieldTypes.Integer">
        <input type="number" step='1' v-model="model.value" class="text-field" />
    </span>
    <span v-if="textType === FieldTypes.Decimal">
        <input type="number" :step='Math.pow(10, -decPoints)' v-model="model.value" class="text-field" />
    </span>
    <span v-if="textType === FieldTypes.Date">
        <input type="date" v-model="model.value" class="text-field" />
    </span>
    <span v-if="textType === FieldTypes.DateTime">
        <input type="datetime-local" v-model="model.value" class="text-field" />
    </span>
   
</template>

