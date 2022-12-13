<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { SearchFieldDefinition } from '../models'
    import { computed, ref } from 'vue';
    import { eFieldType } from '../../shared/constants'

    const props = defineProps<{
        searchFields: SearchFieldDefinition[]
    }>();

    const fieldType = ref(eFieldType.Text);
    const readOnly = computed(()=>fieldType.value > 0);


    //watch(() => fieldType.value, async newValue => {
    //    if (newMessage)
    //        setTimeout(() => {
    //            props.model.message = null;
    //        }, 2000)
    //})
    </script>
<template>
    Field Selector

    {{fieldType}}
    <b-row>
        <b-col class="col-sm-4">

        </b-col>
        <b-col class="col-sm-4">
            <div v-if="fieldType === 1">
                <b-form-input type="text"></b-form-input>
            </div>
            <div v-else-if="fieldType === 2">
                <b-form-input type="date"></b-form-input>
            </div>
            <div v-else-if="fieldType === 3">
                <b-form-input type="number" step='1'></b-form-input>
            </div>
            <div v-else-if="fieldType === 4">
                <b-form-input type="number" :step='Math.pow(10, -decPoints)'></b-form-input>
            </div>
            <div v-else-if="fieldType === 5">
                <b-form-input type="email"></b-form-input>
            </div>
            <div v-else>
                <b-form-input type="text" readonly="readonly" ></b-form-input>
            </div>

            <!--<div v-else-if="textType === FieldType.Paragraph">
        <b-form-textarea v-model="model.value" rows="3" max-rows="6"></b-form-textarea>
    </div>-->
        </b-col>
        <b-col class="col-sm-4">
            <select class="form-select" v-model="fieldType">
                <option v-for="opt in searchFields" :value="opt.type">{{opt.label}}</option>
            </select>
        </b-col>
    </b-row>

</template>