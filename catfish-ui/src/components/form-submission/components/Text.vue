
<script setup lang="ts">
    import { Text, FieldType, FieldTypes } from '../../shared/form-models';

    const props = defineProps<{ model: Text, textType: FieldType, decimalPoints?: number }>();

    const decPoints = props.decimalPoints ? props.decimalPoints : 2;

</script>

<template>
    <div v-if="textType === FieldTypes.ShortAnswer">
        <b-form-input v-model="model.value"  ></b-form-input>
        <!--<input type="text" v-model="model.value" class="text-field" />-->
    </div>
    <div v-else-if="textType === FieldTypes.Paragraph">
        <b-form-textarea v-model="model.value" rows="3" max-rows="6"></b-form-textarea>
    </div>
    <div v-else-if="textType === FieldTypes.RichText">
        <!--TODO: render a proper rich-text editor here -->
        <b-form-textarea v-model="model.value" rows="3" max-rows="6"></b-form-textarea>
    </div>
    <div v-if="textType === FieldTypes.Email">
        <b-form-input v-model="model.value" type="email"></b-form-input>
    </div>
    <div v-if="textType === FieldTypes.Integer">
        <b-form-input type="number" step='1' v-model="model.value" />
    </div>
    <div v-if="textType === FieldTypes.Decimal">
        <b-form-input type="number" :step='Math.pow(10, -decPoints)' v-model="model.value" />
    </div>
    <div v-if="textType === FieldTypes.Date" >
        <b-form-input v-model="model.value" type="date"></b-form-input>
    </div>
    <div v-if="textType === FieldTypes.DateTime">
        <b-form-input type="datetime-local" v-model="model.value" />
    </div>
   
</template>

