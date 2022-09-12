
<script setup lang="ts">
    import { Text, FieldType } from '../../shared/form-models';

    const props = defineProps<{ model: Text, textType: FieldType, decimalPoints?: number }>();

    const decPoints = props.decimalPoints ? props.decimalPoints : 2;

</script>

<template>
    <div v-if="textType === FieldType.ShortAnswer">
        <b-form-input v-model="model.value"  ></b-form-input>
        <!--<input type="text" v-model="model.value" class="text-field" />-->
    </div>
    <div v-else-if="textType === FieldType.Paragraph">
        <b-form-textarea v-model="model.value" rows="3" max-rows="6"></b-form-textarea>
    </div>
    <div v-else-if="textType === FieldType.RichText">
        <!--TODO: render a proper rich-text editor here -->
        <textarea v-model="model.value" class="field-text-area" />
    </div>
    <div v-if="textType === FieldType.Email">
        <b-form-input v-model="model.value" type="email"></b-form-input>
    </div>
    <div v-if="textType === FieldType.Integer">
        <b-form-input type="number" step='1' v-model="model.value" />
    </div>
    <div v-if="textType === FieldType.Decimal">
        <b-form-input type="number" :step='Math.pow(10, -decPoints)' v-model="model.value" />
    </div>
    <div v-if="textType === FieldType.Date" >
        <b-form-input v-model="model.value" type="date"></b-form-input>
    </div>
    <div v-if="textType === FieldType.DateTime">
        <input type="datetime-local" v-model="model.value" class="col-sm-8"/>
    </div>
   
</template>

